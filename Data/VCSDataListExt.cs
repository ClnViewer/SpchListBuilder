using System;
using System.ComponentModel;
using SpchListBuilder.Base;

namespace SpchListBuilder.Data
{
    /* ListExt Class */
    public class ExtList : BaseNotifyPropertyChanged
    {
        private string __name;

        public string Name
        {
            get
            {
                return __name;
            }
            set
            {
                __name = value;
                OnPropertyChanged("Name");                        //MLHIDE
            }
        }

        public ExtList()
        {
            __name = String.Empty;
        }
        public ExtList(string name)
        {
            __name = name;
        }
    }
}
