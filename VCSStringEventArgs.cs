using SpchListBuilder.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpchListBuilder
{
    public class StringEventArgs : EventArgs
    {
        public string Message { get; internal set; }
        public Exception Exception { get; internal set; }

        public StringEventArgs(Exception x, string s)
        {
            Message = s;
            Exception = x;
        }
        public override string ToString()
        {
            return ((String.IsNullOrWhiteSpace(Message)) ?
                ((Exception == null) ? Resources._unknown_error : Exception.ToString()) :
                Resources.Error0 + "[" + Message + "]" + ((Exception == null) ? "" : Exception.ToString())  //MLHIDE
            );
        }
    }
}
