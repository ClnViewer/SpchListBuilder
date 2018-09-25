using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SpchListBuilder.Base;
using System.Diagnostics;
using System.Windows;

namespace SpchListBuilder.Pages
{
    public class PageSetupViewModel : BaseViewModel, IDisposable
    {
        public string SetupPathSet
        {
            get { return __SetupPathSet; }
            set
            {
                __SetupPathSet = value;
                OnPropertyChanged("SetupPathSet");         //MLHIDE
            }
        }
        public bool SetupProcess
        {
            get { return __setupProcess; }
            set
            {
                __setupProcess = value;
                OnPropertyChanged("SetupUIisEnable");         //MLHIDE
                OnPropertyChanged("SetupUIisReadOnly");       //MLHIDE
            }
        }
        public bool SetupUIisEnable
        {
            get { return (!SetupProcess); }
        }
        public bool SetupUIisReadOnly
        {
            get { return SetupProcess; }
        }
        public string AppVersion  // Application Assembly Version
        {
            get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); }
        }
        public string AppInfo  // Application Assembly Description
        {
            get { return FileVersionInfo.GetVersionInfo(Application.ResourceAssembly.Location).ProductVersion; }
        }

        private bool __setupProcess { get; set; }
        public string __SetupPathSet { get; set; }

        CancellationTokenSource cts = null;
        CancellationToken ctoken;

        public PageSetupViewModel()
        {
            SetupPathSet = String.Empty;
            __setupProcess = false;
        }
        ~PageSetupViewModel()
        {
            Dispose(false);
        }

        private void __ExploreFiles(DirectoryInfo d, string Name, int idx)
        {
            try
            {
                foreach (FileInfo f in d.GetFiles())
                {
                    
                    if (f.Extension == ".exe")                                   //MLHIDE
                    {
                        string item = f.ToString();
                        for (int i = 0; i < Properties.Settings.Default.VCSBinExe.Count; i++)
                        {
                            if (
                                ((idx == -1) || (idx == i)) &&
                                (item.Equals(Properties.Settings.Default.VCSBinExe[i]))
                                )
                            {
                                Properties.Settings.Default.VCSBinPath[i] =
                                    String.Format("{0}\\{1}", Name, item);       //MLHIDE
                                Fire_EventRefresh(new StringEventArgs(null,i.ToString()));

                                if (idx != -1)
                                {
                                    SearchVCSBinaryStop();
                                    break;
                                }
                            }
                        }
                        item = null;
                    }
                }

                SetupPathSet = Name;
                foreach (DirectoryInfo sd in d.GetDirectories())
                {
                    if (ctoken.IsCancellationRequested)
                        break;

                    __ExploreFiles(sd, String.Format("{0}\\{1}", Name, sd.ToString()), idx);   //MLHIDE
                }
                d = null;
            }
            catch (Exception) {}
        }

        private void __ExploreDirs(DriveInfo d, string Name, int idx)
        {
            try
            {
                SetupPathSet = Name;
                foreach (DirectoryInfo sd in d.RootDirectory.GetDirectories())
                {
                    if (ctoken.IsCancellationRequested)
                        break;

                    __ExploreFiles(sd, String.Format("{0}\\{1}", Name, sd.ToString()), idx);   //MLHIDE
                }
                d = null;
            }
            catch (Exception) { }
        }

        public void SearchVCSBinaryStop()
        {
            if (!__setupProcess)
                return;

            if (cts != null)
                cts.Cancel();
        }

        public Task SearchVCSBinaryStart(int idx = -1)
        {
            if (__setupProcess)
                return null;

            if (cts != null)
                cts.Dispose();
                
            cts = new CancellationTokenSource();
            ctoken = cts.Token;

            return Task.Factory.StartNew(() =>
            {
                SetupProcess = true;

                try
                {
                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        if (ctoken.IsCancellationRequested)
                            break;

                        __ExploreDirs(drive, drive.Name.Substring(0,2), idx);
                    }
                    GC.Collect(0, GCCollectionMode.Forced);
                }
                catch (Exception) { }
            });
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool x = false)
        {
            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }

            if (x)
                GC.SuppressFinalize(this);
        }

        #endregion
    }
}
