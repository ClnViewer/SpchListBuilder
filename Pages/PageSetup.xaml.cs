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
                ShowMessageBox("Open GitHub Url", ex.Message);
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
