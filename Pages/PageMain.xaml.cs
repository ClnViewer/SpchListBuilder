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

        public void CallVCSExport()
        {
            try
            {
                if (repo == null)
                    return;

                Task<string> t = vcsp.ExportNodeData(true);

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
                        
                        if (String.IsNullOrWhiteSpace(__ExecOut))
                        {
                            ShowMessageBox(Properties.Resources.Error0, Properties.Resources.no_files_selected_abort);
                            return;
                        }

                        switch(repo.TypeRequest)
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
                                    string __TxtOut = exec.Exec(VCSDataRepo.EnumTypeRequest.__VCS_IMPORT);
                                    exec = null;

                                    if (!String.IsNullOrWhiteSpace(__TxtOut))
                                        ShowMessageBox(Properties.Resources.VCS_return, __TxtOut);

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
                                        string __TxtOut = exec.Exec(VCSDataRepo.EnumTypeRequest.__VCS_IMPORT);
                                        exec = null;

                                        if (!String.IsNullOrWhiteSpace(__TxtOut))
                                            ShowMessageBox(Properties.Resources.VCS_return, __TxtOut);
                                    }
                                    catch (Exception e)
                                    {
                                        ShowMessageBox(Properties.Resources.Error0, e.Message);
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
                    }
                    finally
                    {
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

                        var match = Properties.Settings.Default.VCSUrlStore
                            .Cast<string>().ToList<string>()
                            .FirstOrDefault(stringToCheck => stringToCheck.Contains(uri));

                        if (match == null)
                        {
                            Properties.Settings.Default.VCSUrlStore.Insert(0, uri);
                            Properties.Settings.Default.Save();
                        }
                        VCSRepoInfoSetText(String.Format(
                            Properties.Resources._0_repository_1_contains_2_fil,
                            Settings.Default.VCSBinText[(int)repo.DataProvider],
                            repo.RepoName, vcsp.RepoItemsCount
                        ));
                    }
                    finally
                    {
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
    }
}
