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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Pages
{
    public partial class PageSetup : BasePage
    {
        PageSetupViewModel psvm { get; set; }

        public PageSetup()
        {
            this.DataContext = psvm = new PageSetupViewModel();
            InitializeComponent();
            psvm.EventError += EventErrorPrint;
            psvm.EventRefresh += (o, e) =>
                {
                    CollectionViewSource.GetDefaultView(SetupVCSBinList.ItemsSource).Refresh();
                };
        }

        private void SetupVCSBinFileDialog(int idx)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.Multiselect = false;
                dlg.RestoreDirectory = true;
                dlg.DefaultExt =
                    Properties.Settings.Default.VCSBinExe[idx];
                dlg.Filter =
                    Properties.Resources.VCS_executable_files
                    + Properties.Settings.Default.VCSBinExe[idx] + ")|" //MLHIDE
                    + Properties.Settings.Default.VCSBinExe[idx];

                if (dlg.ShowDialog() == true)
                {
                    Settings.Default.VCSBinPath[idx] = dlg.FileName;
                    CollectionViewSource.GetDefaultView(SetupVCSBinList.ItemsSource).Refresh();
                }
            }
            catch (Exception e)
            {
                ShowMessageBox(Properties.Resources.Select_executable_files, e.Message);
            }
        }

        private void SetupVCSBinList_Selection(object sender, MouseButtonEventArgs e)
        {
            if (
                (psvm.SetupProcess) ||
                (e.ClickCount < 2) ||
                (SetupVCSBinList.SelectedIndex < 0) ||
                (SetupVCSBinList.SelectedIndex > 2)
               )
                return;

            SetupVCSBinFileDialog(SetupVCSBinList.SelectedIndex);
        }

        private void MenuVCSBinAdd_Click(object sender, RoutedEventArgs e)
        {
            if (
                (psvm.SetupProcess) ||
                (SetupVCSBinList.SelectedIndex < 0) ||
                (SetupVCSBinList.SelectedIndex > 2)
               )
                return;

            SetupVCSBinFileDialog(SetupVCSBinList.SelectedIndex);
        }

        private void MenuVCSBinRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (
                    (psvm.SetupProcess) ||
                    (SetupVCSBinList.SelectedIndex < 0) ||
                    (SetupVCSBinList.SelectedIndex > 2)
                   )
                    return;

                Settings.Default.VCSBinPath[SetupVCSBinList.SelectedIndex] = String.Empty;
                CollectionViewSource.GetDefaultView(SetupVCSBinList.ItemsSource).Refresh();
            }
            catch (Exception ex)
            {
                ShowMessageBox(Properties.Resources.Select_executable_files, ex.Message);
            }
        }

        private void MenuVCSBinSearch_Click(object sender, RoutedEventArgs e)
        {
            if (
                (psvm.SetupProcess) ||
                (SetupVCSBinList.SelectedIndex < 0) ||
                (SetupVCSBinList.SelectedIndex > 2)
               )
                return;

            __SearchVCS(SetupVCSBinList.SelectedIndex);

        }

        private void ButtonSearchVCS_Click(object sender, RoutedEventArgs e)
        {
            __SearchVCS(-1);
        }

        private void Hyperlink_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            try
            {
                Hyperlink hl = (Hyperlink)sender;
                System.Diagnostics.Process.Start(hl.NavigateUri.ToString());
            }
            catch (Exception ex)
            {
                ShowMessageBox(Properties.Resources.Open_GitHub_Url, ex.Message);
            }
        }

        private void __SearchVCS(int idx)
        {
            if (psvm.SetupProcess)
            {
                psvm.SearchVCSBinaryStop();
                return;
            }

            Task t = psvm.SearchVCSBinaryStart(idx);

            if (t == null)
                return;

            t.ContinueWith(x =>
            {
                if (x == null)
                {
                    psvm.SetupPathSet = Properties.Resources.Operation_fault;
                }
                else if (x.IsCanceled)
                {
                    psvm.SetupPathSet = Properties.Resources.Operation_Cancelled;
                }
                else if (x.IsFaulted)
                {
                    psvm.SetupPathSet = (((x.Exception != null) && (x.Exception.Message != null)) ?
                        x.Exception.Message : Properties.Resources.Operation_fault
                    );
                }
                else
                    psvm.SetupPathSet = String.Empty;

                psvm.SetupProcess = false;
                CollectionViewSource.GetDefaultView(SetupVCSBinList.ItemsSource).Refresh();

                if (x != null)
                    x.Dispose();
            });
        }
    }
}
