using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SpchListBuilder.Data
{
    [Serializable()]
    [XmlRoot(ElementName = "options")]                   //MLHIDE
    public class SpchOptions : INotifyPropertyChanged
    {
        [XmlElement(ElementName = "split")]              //MLHIDE
        public string SplitRepo
        {
            get { return this.__SplitRepo; }
            set
            {
                if (__SplitRepo == value)
                    return;

                __SplitRepo = value;
                OnPropertyChanged("SplitRepo");          //MLHIDE
            }
        }
        [XmlElement(ElementName = "deploy")]             //MLHIDE
        public string Deploy
        {
            get { return this.__Deploy; }
            set
            {
                if (__Deploy == value)
                    return;

                __Deploy = value;
                OnPropertyChanged("Deploy");             //MLHIDE
            }
        }
        [XmlElement(ElementName = "backup")]             //MLHIDE
        public string Backup
        {
            get { return this.__Backup; }
            set
            {
                if (__Backup == value)
                    return;

                __Backup = value;
                OnPropertyChanged("Backup");             //MLHIDE
            }
        }
        [XmlElement(ElementName = "log")]                //MLHIDE
        public string Log
        {
            get { return this.__Log; }
            set
            {
                if (__Log == value)
                    return;

                __Log = value;
                OnPropertyChanged("Log");                //MLHIDE
            }
        }
        [XmlElement(ElementName = "execdir")]            //MLHIDE
        public string ExecPath
        {
            get { return this.__ExecPath; }
            set
            {
                if (__ExecPath == value)
                    return;

                __ExecPath = value;
                OnPropertyChanged("ExecPath");           //MLHIDE
            }
        }
        [XmlElement(ElementName = "uid")]                //MLHIDE
        public string Uuid
        {
            get { return this.__Uuid; }
            set
            {
                if (__Uuid == value)
                    return;

                __Uuid = value;
                OnPropertyChanged("Uuid");               //MLHIDE
            }
        }
        [XmlElement(ElementName = "rename")]             //MLHIDE
        public string Prefix
        {
            get { return this.__Prefix; }
            set
            {
                if (__Prefix == value)
                    return;

                __Prefix = value;
                __GetPrefix();
                OnPropertyChanged("Prefix");             //MLHIDE
            }
        }
        [XmlElement(ElementName = "chnglog")]            //MLHIDE
        public string ChangeLog
        {
            get { return this.__ChangeLog; }
            set
            {
                if (__ChangeLog == value)
                    return;

                __ChangeLog = value;
                __InChangeLog();
                OnPropertyChanged("ChangeLog");          //MLHIDE
            }
        }
        [XmlElement(ElementName = "check")]              //MLHIDE
        public string FileMaskCheck
        {
            get { return this.__FileMaskCheck; }
            set
            {
                if (__FileMaskCheck == value)
                    return;

                __FileMaskCheck = value;
                __InFileMaskCheck();
                OnPropertyChanged("FileMaskCheck");      //MLHIDE
            }
        }
        [XmlElement(ElementName = "vcs")]                //MLHIDE
        public string VcsType
        {
            get { return this.__VcsType; }
            set
            {
                if (__VcsType == value)
                    return;

                __VcsType = value;
                OnPropertyChanged("VcsType");            //MLHIDE
            }
        }
        [XmlElement(ElementName = "yaml")]               //MLHIDE
        public bool isYaml
        {
            get { return this.__isYaml; }
            set
            {
                if (__isYaml == value)
                    return;

                __isYaml = value;
                OnPropertyChanged("isYaml");             //MLHIDE
            }
        }
        [XmlElement(ElementName = "force")]              //MLHIDE
        public bool isForce
        {
            get { return this.__isForce; }
            set
            {
                if (__isForce == value)
                    return;

                __isForce = value;
                OnPropertyChanged("isForce");            //MLHIDE
            }
        }
        [XmlElement(ElementName = "nonloop")]            //MLHIDE
        public bool isDaemon
        {
            get { return __isDaemon; }
            set
            {
                if (__isDaemon == value)
                    return;

                __isDaemon = value;
                OnPropertyChanged("isDaemon");           //MLHIDE
            }
        }
        [XmlElement(ElementName = "quiet")]              //MLHIDE
        public bool isQuiet
        {
            get { return __isQuiet; }
            set
            {
                if (__isQuiet == value)
                    return;

                __isQuiet = value;
                OnPropertyChanged("isQuiet");            //MLHIDE
            }
        }

        private bool __isDaemon = true;
        private bool __isQuiet = true;
        private bool __isForce = false;
        private bool __isYaml = false;
        private int __idxChangeLog = -1;
        private int __idxFileMaskCheck = -1;
        private string __Prefix1 = String.Empty;
        private string __Prefix2 = String.Empty;
        private string __SplitRepo = String.Empty;
        private string __Deploy = String.Empty;
        private string __Backup = String.Empty;
        private string __Log = String.Empty;
        private string __ExecPath = String.Empty;
        private string __Uuid = String.Empty;
        private string __Prefix = String.Empty;
        private string __ChangeLog = String.Empty;
        private string __FileMaskCheck = String.Empty;
        private string __VcsType = String.Empty;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        [XmlIgnore]
        public string Prefix1
        {
            get { return __Prefix1; }
            set
            {
                if (__Prefix1 == value)
                    return;

                __Prefix1 = value;
                Prefix = __SetPrefix();
                OnPropertyChanged("Prefix1");                           //MLHIDE
            }
        }
        [XmlIgnore]
        public string Prefix2
        {
            get { return __Prefix2; }
            set
            {
                if (__Prefix2 == value)
                    return;

                __Prefix2 = ((String.IsNullOrWhiteSpace(__Prefix1)) ? String.Empty : value);
                Prefix = __SetPrefix();
                OnPropertyChanged("Prefix2");                             //MLHIDE
            }
        }
        [XmlIgnore]
        public int IdxChangeLog
        {
            get { return __idxChangeLog; }
            set
            {
                if (__idxChangeLog == value)
                    return;

                __idxChangeLog = value;
                switch (__idxChangeLog)
                {
                    case 0:
                        ChangeLog = "md"; break;                //MLHIDE
                    case 1:
                        ChangeLog = "gnu"; break;               //MLHIDE
                    default:
                        ChangeLog = String.Empty; break;
                }
                OnPropertyChanged("IdxChangeLog");              //MLHIDE
            }
        }
        [XmlIgnore]
        public int IdxFileMaskCheck
        {
            get { return __idxFileMaskCheck; }
            set
            {
                if (__idxFileMaskCheck == value)
                    return;

                __idxFileMaskCheck = value;
                switch (__idxFileMaskCheck)
                {
                    case 0:
                        FileMaskCheck = "m"; break;                 //MLHIDE
                    case 1:
                        FileMaskCheck = "c"; break;                 //MLHIDE
                    case 2:
                        FileMaskCheck = "s"; break;                 //MLHIDE
                    case 3:
                        FileMaskCheck = "ms"; break;                //MLHIDE
                    case 4:
                        FileMaskCheck = "cs"; break;                //MLHIDE
                    case 5:
                        FileMaskCheck = "mcs"; break;               //MLHIDE
                    default:
                        FileMaskCheck = String.Empty; break;
                }
                OnPropertyChanged("IdxFileMaskCheck");                   //MLHIDE
            }
        }

        private void __InChangeLog()
        {
            if (String.IsNullOrWhiteSpace(__ChangeLog))
            {
                IdxChangeLog = -1;
                return;
            }
            switch (__ChangeLog)
            {
                case "md":
                    IdxChangeLog = 0; break;               //MLHIDE
                case "gnu":
                    IdxChangeLog = 1; break;               //MLHIDE
                default:
                    IdxFileMaskCheck = -1; break;
            }
        }

        private void __InFileMaskCheck()
        {
            if (String.IsNullOrWhiteSpace(__FileMaskCheck))
            {
                IdxFileMaskCheck = -1;
                return;
            }
            switch (__FileMaskCheck)
            {
                case "m":
                    IdxFileMaskCheck = 0; break;               //MLHIDE
                case "c":
                    IdxFileMaskCheck = 1; break;               //MLHIDE
                case "s":
                    IdxFileMaskCheck = 2; break;               //MLHIDE
                case "ms":
                    IdxFileMaskCheck = 3; break;               //MLHIDE
                case "cs":
                    IdxFileMaskCheck = 4; break;               //MLHIDE
                case "mcs":
                    IdxFileMaskCheck = 5; break;               //MLHIDE
                default:
                    IdxFileMaskCheck = -1; break;
            }
        }

        private string __SetPrefix()
        {
            return ((String.IsNullOrWhiteSpace(__Prefix2)) ?
                __Prefix1 :
                ((String.IsNullOrWhiteSpace(__Prefix1)) ?
                    String.Empty :
                    String.Format("{0}={1}", __Prefix1, __Prefix2)       //MLHIDE
                )
            );
        }

        private void __GetPrefix()
        {

            if (String.IsNullOrWhiteSpace(__Prefix))
                return;

            if (__Prefix.Contains("="))                                  //MLHIDE
            {
                var arr = __Prefix.Split('=');                           //MLHIDE
                switch (arr.Length)
                {
                    case 1:
                        {
                            Prefix1 = arr[0]; break;
                        }
                    case 2:
                        {
                            Prefix1 = arr[0];
                            Prefix2 = arr[1]; break;
                        }
                }
                return;
            }
            Prefix1 = __Prefix;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler pc = PropertyChanged;
            if (pc != null)
            {
                pc(this, new PropertyChangedEventArgs(name));
            }
        }

        public void Clear()
        {
            SplitRepo = Deploy = Backup = Log = ExecPath = Uuid = Prefix =
            ChangeLog = FileMaskCheck = VcsType = Prefix1 = Prefix2 = String.Empty;

            isYaml = isForce = false;
            isDaemon = isQuiet = true;

            IdxChangeLog = IdxFileMaskCheck = -1;

        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "files")]                     //MLHIDE
    public class SpchFiles
    {
        [XmlElement(ElementName = "file")]               //MLHIDE
        public List<string> Files;
    }

    [Serializable()]
    [XmlRoot(ElementName = "settings")]                  //MLHIDE
    public class SpchSettings
    {
        [XmlElement(ElementName = "repo")]               //MLHIDE
        public string RepoName;
        [XmlElement(ElementName = "date")]               //MLHIDE
        public Int32 Date;
        [XmlElement(ElementName = "options")]            //MLHIDE
        public SpchOptions Options;
    }

    [Serializable()]
    [XmlRoot(ElementName = "spchlist")]                  //MLHIDE
    public class SpchListData
    {
        [XmlElement(ElementName = "settings")]            //MLHIDE
        public SpchSettings Setting;
        [XmlElement(ElementName = "files")]               //MLHIDE
        public SpchFiles Files;
    }
}
