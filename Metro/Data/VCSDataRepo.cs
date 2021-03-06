﻿/*
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
using SpchListBuilder.Extension;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Data
{
    public class VCSDataRepo
    {
        public enum EnumDataProvider : int
        {
            __SVN = 0,
            __GIT = 1,
            __HG = 2,
            __DEFAULT = 3
        }
        public enum EnumTypeRequest : int
        {
            __VCS_STAT = 0,
            __VCS_LIST = 1,
            __VCS_IMPORT = 2,
            __DEFAULT = 3
        }

        public bool DataOK { get; private set; }
        public EnumDataProvider DataProvider { get; private set; }
        public EnumTypeRequest TypeRequest { get; private set; }
        public string RepoName { get; private set; }
        public string UriOrigin { get; private set; }
        public string UriDirectory { get; private set; }
        public char UriSeparator { get; private set; }

        public VCSDataRepo(string uri)
        {
            DataOK = false;
            UriSeparator = '\0';
            UriOrigin = String.Empty;
            UriDirectory = String.Empty;
            RepoName = String.Empty;
            TypeRequest = EnumTypeRequest.__DEFAULT;
            DataProvider = EnumDataProvider.__DEFAULT;
            __InitUri(uri);
        }

        private void __InitUri(string uri)
        {
            if (String.IsNullOrWhiteSpace(uri))
                return;

            UriOrigin = uri;
            bool __isSheme = false;

            if (uri.StartsWith(Resources.svn))
            {
                TypeRequest = EnumTypeRequest.__VCS_LIST;
                DataProvider = EnumDataProvider.__SVN;
                __isSheme = true;
            }
            else if (uri.StartsWith(Resources.git))
            {
                TypeRequest = EnumTypeRequest.__VCS_LIST;
                DataProvider = EnumDataProvider.__GIT;
                __isSheme = true;
            }
            else if (uri.StartsWith(Resources.hg))
            {
                TypeRequest = EnumTypeRequest.__VCS_LIST;
                DataProvider = EnumDataProvider.__HG;
                __isSheme = true;
            }
            else
            {
                DataProvider = (EnumDataProvider)Properties.Settings.Default.VCSBinSelect;
                TypeRequest = EnumTypeRequest.__VCS_STAT;
            }
            if (!__isSheme)
            {
                if (DataProvider == EnumDataProvider.__DEFAULT)
                    return;

                if (uri.StartsWith(Resources.file))
                    UriDirectory = uri.Substring(7);
                else
                    UriDirectory = uri;
            }
            else
                UriDirectory = String.Empty;

            UriSeparator = uri.GetSeparator();
            RepoName = ((UriSeparator == '\0') ?                        //MLHIDE
                uri : uri.Split(UriSeparator).Last<String>().ToUpper()
            );
            UriSeparator = ((UriSeparator == '\0') ?                    //MLHIDE
                Path.DirectorySeparatorChar : UriSeparator
            );

            DataOK = true;
        }

    }
}
