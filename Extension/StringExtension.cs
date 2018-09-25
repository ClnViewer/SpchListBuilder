using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpchListBuilder.Extension
{
    public static class StringExtension
    {
        public static char GetSeparator(this string Src)
        {
            return ((Src.Contains('\\')) ? '\\' :
                    ((Src.Contains('/')) ? '/' : '\0')
            );
        }
        public static List<String> SplitToList(this string Src)
        {
            char __sep = Src.GetSeparator();
            if (__sep == '\0')
                return new List<string> { Src };
            return Src.Split(__sep).ToList<String>();
        }
    }
}
