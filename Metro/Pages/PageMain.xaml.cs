/*
    MIT License

    Copyright (c) 2018 PS
    GitHub SPCH: https://github.com/ClnViewer/Split-post-commit-Hook---SVN-GIT-HG
    GitHub SpchListBuilder: https://github.com/ClnViewer/SpchListBuilder

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sub license, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SpchListBuilder.Data;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Pages
{
    public partial class PageMain : BasePage
    {
        public PageMainViewModel vcsp { get; set; }
        public VCSDataRepo repo { get; set; }

        public PageMain()
        {
            this.DataContext = vcsp = new PageMainViewModel();
            this.repo = null;

            InitializeComponent();

            vcsp.EventError += EventErrorPrint;
        }

        #region Controls update

        private void VCSInfoSetText(string Src)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                (this.DataContext as PageMainViewModel).BlockInfo = Src;
                VCSInfo.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }));
        }

        private void VCSRepoInfoSetText(string Src)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                (this.DataContext as PageMainViewModel).RepoInfo = Src;
                VCSRepoInfo.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
            }));
        }

        private void SelectorExtension(RoutedEventArgs e, bool isSet)
        {
            if ((e == null) || (e.OriginalSource == null))
                return;

            string __ext = (e.OriginalSource as MenuItem).CommandParameter as String;
            if (String.IsNullOrWhiteSpace(__ext))
                return;

            (this.DataContext as PageMainViewModel).TvNodes.SelectNodeByExt(__ext, isSet);
        }

        private void VCSTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if ((e == null) || (e.NewValue == null))
                return;

            Node node = e.NewValue as Node;
            if ((node == null) || (node.Desc == String.Empty))
                return;

            VCSInfoSetText(node.Desc);
        }

        private void VCSTreeView_ContextMenu_Select(object sender, RoutedEventArgs e)
        {
            SelectorExtension(e, true);
        }

        private void VCSTreeView_ContextMenu_UnSelect(object sender, RoutedEventArgs e)
        {
            SelectorExtension(e, false);
        }

        #endregion
        #region VCS Export

        public void CallVCSExport()
        {
            try
            {
                if (repo == null)
                    return;

                Task<string> t = vcsp.SaveNodeDataAll(repo, true);

                if (t == null)
                    return;

                t.ContinueWith(x =>
                {
                    try
                    {
                        if (!TaskCheckReturn<string>(x, Properties.Resources.GetData))
                            return;

                        string __fileName = String.Empty;
                        string __ExecOut = x.Result as string;
                        string __TxtOut = String.Empty;

                        if (String.IsNullOrWhiteSpace(__ExecOut))
                        {
                            ShowMessageBox(Properties.Resources.Error0, Properties.Resources.no_files_selected_abort);
                            return;
                        }

                        switch (repo.TypeRequest)
                        {
                            case VCSDataRepo.EnumTypeRequest.__VCS_STAT:
                                {
                                    __fileName = String.Format(
                                        "{0}{1}{2}",                               //MLHIDE
                                        repo.UriOrigin,
                                        Path.DirectorySeparatorChar,
                                        Properties.Settings.Default.VCSOutListFileName
                                    );
                                    File.WriteAllText(__fileName, __ExecOut);

                                    string __exepath =
                                        Properties.Settings.Default.VCSBinPath[(int)repo.DataProvider];
                                    VCSExec exec = new VCSExec(__exepath, repo, __fileName);
                                    __TxtOut = exec.Exec(VCSDataRepo.EnumTypeRequest.__VCS_IMPORT);
                                    exec = null;

                                    break;
                                }
                            case VCSDataRepo.EnumTypeRequest.__VCS_LIST:
                                {
                                    if (
                                        (Properties.Settings.Default.VCSBinSelect < 0) ||
                                        (Properties.Settings.Default.VCSBinSelect > 2)
                                        )
                                    {
                                        ShowMessageBox(Properties.Resources.Error0, Properties.Resources.VCS_type_not_selected_abort);
                                        return;
                                    }
                                    try
                                    {
                                        string __exepath =
                                            Properties.Settings.Default.VCSBinPath[(int)repo.DataProvider];
                                        __fileName = Path.GetTempPath()
                                                   + Guid.NewGuid().ToString() + ".tmp";             //MLHIDE

                                        File.WriteAllText(__fileName, __ExecOut);

                                        VCSExec exec = new VCSExec(__exepath, repo, __fileName);
                                        __TxtOut = exec.Exec(VCSDataRepo.EnumTypeRequest.__VCS_IMPORT);
                                        exec = null;
                                    }
                                    catch (Exception e)
                                    {
                                        ShowMessageBox(Properties.Resources.Error0, e.Message);
                                        return;
                                    }
                                    finally
                                    {
                                        File.Delete(__fileName);
                                    }
                                    break;
                                }
                            default:
                                {
                                    ShowMessageBox(Properties.Resources.Error0, Properties.Resources.not_supported_VCS_operation);
                                    return;
                                }
                        }

                        if (!String.IsNullOrWhiteSpace(__TxtOut))
                            ShowMessageBox(
                                String.Format(
                                    Properties.Resources.VCS_return,
                                    Properties.Settings.Default.VCSBinText[(int)repo.DataProvider]
                                ),
                                __TxtOut
                            );
                    }
                    finally
                    {
                        if (x != null)
                            x.Dispose();
                    }
                });
            }
            catch (Exception ex)
            {
                VCSInfoSetText(String.Empty);
                ShowMessageBox(Properties.Resources.Error0, ex.ToString());
            }
        }

        #endregion
        #region VCS Get

        private void CallVCSData(string uri)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(uri))
                {
                    VCSInfoSetText(Properties.Resources.Uri_is_empty);
                    return;
                }

                repo = new VCSDataRepo(uri);

                if (!repo.DataOK)
                {
                    VCSInfoSetText(Properties.Resources.Type_Uri_not_validate_use_path);
                    return;
                }

                Properties.Settings.Default.VCSBinSelect = (int)repo.DataProvider;
                CBoxButton.GetBindingExpression(ListBox.SelectedIndexProperty).UpdateTarget();
                string __exec = Properties.Settings.Default.VCSBinPath[(int)repo.DataProvider];
                string __file = Path.GetFileNameWithoutExtension(
                    Properties.Settings.Default.VCSOutListFileName
                );
                Properties.Settings.Default.VCSOutListFileName =
                    String.Format(
                        "{0}.{1}",                                              //MLHIDE
                        __file,
                        ((Properties.Settings.Default.XmlListOutputFormat) ?
                            Properties.Settings.Default.XmlListOutputExtension[1] :
                            Properties.Settings.Default.XmlListOutputExtension[0]
                        )
                    );

                VCSInfoSetText(String.Format(Properties.Resources.Load_repo_0, repo.RepoName));
                VCSRepoInfoSetText(String.Empty);

                Task<bool> t = vcsp.GetData(__exec, repo);

                if (t == null)
                    return;

                t.ContinueWith(x =>
                {
                    try
                    {
                        if (!TaskCheckReturn<bool>(x, Properties.Resources.GetData))
                            return;

                        if (x.Result)
                        {
                            VCSInfoSetText(String.Format(Properties.Resources.Load_0_complete, repo.RepoName));
                        }
                        else
                            return;

                        bool __isAddStore = false;

                        if (Properties.Settings.Default.VCSUrlStore.Count == 0)
                        {
                            __isAddStore = true;
                        }
                        else
                        {
                            var match = Properties.Settings.Default.VCSUrlStore
                                .Cast<string>().ToList<string>()
                                .FirstOrDefault(stringToCheck => stringToCheck.Contains(uri));

                            if (match == null)
                                __isAddStore = true;
                        }
                        if (__isAddStore)
                        {
                            Properties.Settings.Default.VCSUrlStore.Insert(0, uri);
                            Properties.Settings.Default.Save();
                        }
                        VCSRepoInfoSetText(String.Format(
                            Properties.Resources._0_repository_1_contains_2_fil,
                            Settings.Default.VCSBinText[(int)repo.DataProvider],
                            repo.RepoName, vcsp.RepoItemsCount
                        ));
                        vcsp.ServiceMenuIsEnable = true;
                    }
                    finally
                    {
                        if (x != null)
                            x.Dispose();
                    }
                });
            }
            catch (Exception ex)
            {
                VCSInfoSetText(String.Empty);
                ShowMessageBox(Properties.Resources.Error0, ex.ToString());
            }
        }

        private void CBoxUri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
                return;

            if (e.Key == Key.Enter)
            {
                CallVCSData(CBoxUri.Text);
            }
        }

        private void BtnGetUri_Click(object sender, RoutedEventArgs e)
        {
            if ((e == null) || (e.OriginalSource == null))
                return;

            string __uri = (e.OriginalSource as Button).CommandParameter as String;
            CallVCSData(__uri);
        }

        #endregion
        #region ServiceList Load

        private void ServiceListLoad_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;

            try
            {
                int sel = ((Properties.Settings.Default.XmlListOutputFormat) ? 1 : 0);
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Multiselect = false;
                dlg.RestoreDirectory = true;
                dlg.DefaultExt = Properties.Settings.Default.XmlListOutputExtension[sel];
                dlg.Filter =
                    Properties.Resources.SPCH_list_files
                    + "*." + Properties.Settings.Default.XmlListOutputExtension[sel] + ")|*." //MLHIDE
                    + Properties.Settings.Default.XmlListOutputExtension[sel];

                if (
                    (dlg.ShowDialog() != true) ||
                    (String.IsNullOrWhiteSpace(dlg.FileName))
                   )
                    return;

                Task<bool> t = vcsp.LoadNodeDataAll(dlg.FileName);

                if (t == null)
                    return;

                t.ContinueWith(x =>
                {
                    try
                    {
                        TaskCheckReturn<bool>(x, Properties.Resources.GetData);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(Properties.Resources.Error0, ex.ToString());
                    }
                    finally
                    {
                        if (x != null)
                            x.Dispose();
                    }
                });
            }
            catch (Exception ex)
            {
                ShowMessageBox(Properties.Resources.Error0, ex.ToString());
            }
        }

        #endregion
        #region ServiceList Save

        private void ServiceListSave(bool ischeck)
        {
            try
            {
                int sel = ((Properties.Settings.Default.XmlListOutputFormat) ? 1 : 0);
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.RestoreDirectory = true;
                dlg.DefaultExt = Properties.Settings.Default.XmlListOutputExtension[sel];
                dlg.Filter =
                    Properties.Resources.SPCH_list_files
                    + "*." + Properties.Settings.Default.XmlListOutputExtension[sel] + ")|*." //MLHIDE
                    + Properties.Settings.Default.XmlListOutputExtension[sel];
                dlg.FileName = String.Format(
                    "{0}-{1}-{2}.{3}",                                                        //MLHIDE
                    repo.RepoName,
                    DateTime.Now.ToShortDateString(),
                    ((ischeck) ? "selected" : "full"),                                        //MLHIDE
                    Properties.Settings.Default.XmlListOutputExtension[sel]
                );

                if (
                    (dlg.ShowDialog() != true) ||
                    (String.IsNullOrWhiteSpace(dlg.FileName))
                   )
                    return;

                Task<string> t = vcsp.SaveNodeDataAll(repo, ischeck);

                if (t == null)
                    return;

                t.ContinueWith(x =>
                {
                    try
                    {
                        if (!TaskCheckReturn<string>(x, Properties.Resources.GetData))
                            return;
                        
                        string __body = x.Result as String;
                        if (String.IsNullOrWhiteSpace(__body))
                            return;

                        File.WriteAllText(dlg.FileName, __body);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(Properties.Resources.Error0, ex.ToString());
                    }
                    finally
                    {
                        if (x != null)
                            x.Dispose();
                    }
                });
            }
            catch (Exception ex)
            {
                ShowMessageBox(Properties.Resources.Error0, ex.ToString());
            }
        }

        private void SaveSelectedXmlList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;

            ServiceListSave(true);
        }

        private void SaveFullXmlList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount < 2)
                return;

            ServiceListSave(false);
        }
        #endregion
    }
}
