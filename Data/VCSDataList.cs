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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SpchListBuilder.Extension;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Data
{
    /* svn list */

    [Serializable()]
    [XmlRoot(ElementName = "commit")]                 //MLHIDE
    public class VCSListCommit
    {
        [XmlElement(ElementName = "author")]          //MLHIDE
        public string Author { get; set; }
        [XmlElement(ElementName = "date")]            //MLHIDE
        public string Date { get; set; }
        [XmlAttribute(AttributeName = "revision")]    //MLHIDE
        public string Revision { get; set; }
    }

    [Serializable()]
    [XmlRoot(ElementName = "entry")]                  //MLHIDE
    public class VCSListEntry
    {
        [XmlIgnore]
        private List<string> __Paths;
        [XmlElement(ElementName = "size")]            //MLHIDE
        public string Size { get; set; }
        [XmlElement(ElementName = "commit")]          //MLHIDE
        public VCSListCommit Commit { get; set; }
        [XmlAttribute(AttributeName = "kind")]        //MLHIDE
        public string Kind { get; set; }
        [XmlElement(ElementName = "name")]            //MLHIDE
        public string PathEntry
        {
            set
            {
                if ((!String.IsNullOrWhiteSpace(value)) && (value != "."))   //MLHIDE
                {
                    __Paths = value.SplitToList();
                }
                else
                {
                    __Paths = new List<string>();
                }
            }
            get
            {
                return String.Empty;
            }
        }
        [XmlIgnore]
        public List<string> Paths
        {
            get
            {
                if (__Paths == null)
                    __Paths = new List<string>();

                return __Paths;
            }
        }
        public VCSListEntry()
        {
            __Paths = null;
        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "list")]                   //MLHIDE
    public class VCSListMain
    {
        [XmlElement(ElementName = "entry")]           //MLHIDE
        public List<VCSListEntry> Entry { get; set; }
        [XmlAttribute(AttributeName = "path")]        //MLHIDE
        public string Path { get; set; }
    }

    [Serializable()]
    [XmlRoot(ElementName = "lists")]                  //MLHIDE
    public class VCSListRoot
    {
        [XmlElement(ElementName = "list")]            //MLHIDE
        public VCSListMain Root { get; set; }
    }
}
