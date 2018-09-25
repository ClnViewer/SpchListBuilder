using System;
using System.Windows;
using MahApps.Metro.Controls;
using SpchListBuilder.Pages;

namespace SpchListBuilder
{
    public partial class MainWindow : MetroWindow
    {
        PageMain pageMain = null;
        PageSetup pageSetup = null;

        public MainWindow()
        {
            InitializeComponent();

            pageMain = new PageMain();
            ContentFrame.NavigationService.Navigate(pageMain);

            this.Closing += (o, e) =>
                {
                    e.Cancel = false;
                    Properties.Settings.Default.Save();
                };
        }

        private void BtnSetup_Click(object sender, RoutedEventArgs e)
        {
            Type t = ContentFrame.NavigationService.Content.GetType();

            if (t == typeof(PageMain))
                pageSetup = ((pageSetup == null) ? new PageSetup() : pageSetup);
            else
                return;

            ContentFrame.NavigationService.Navigate(pageSetup);
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Type t = ContentFrame.NavigationService.Content.GetType();

            if (t == typeof(PageSetup))
                pageMain = ((pageMain == null) ? new PageMain() : pageMain);
            else
                return;

            ContentFrame.NavigationService.Navigate(pageMain);
        }

        private void BtnExportList_Click(object sender, RoutedEventArgs e)
        {
            Type t = ContentFrame.NavigationService.Content.GetType();

            if (t == typeof(PageSetup))
            {
                pageMain = ((pageMain == null) ? new PageMain() : pageMain);
                ContentFrame.NavigationService.Navigate(pageMain);
            }

            pageMain.CallVCSExport();
        }

    }
}
