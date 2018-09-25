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
