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
