using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using SpchListBuilder.Extension;

namespace SpchListBuilder.Data
{
    /* svn stat */

    [Serializable()]
    [XmlRoot(ElementName = "commit")]               //MLHIDE
    public class VCSStatCommit
    {
        [XmlElement(ElementName = "author")]        //MLHIDE
        public string Author { get; set; }
        [XmlElement(ElementName = "date")]          //MLHIDE
        public string Date { get; set; }
        [XmlAttribute(AttributeName = "revision")]  //MLHIDE
        public string Revision { get; set; }

        public VCSStatCommit()
        {
            Author = String.Empty;
            Date = String.Empty;
            Revision = String.Empty;
        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "wc-status")]            //MLHIDE
    public class VCSStatWcstatus
    {
        [XmlElement(ElementName = "commit")]        //MLHIDE
        public VCSStatCommit Commit { get; set; }
        [XmlAttribute(AttributeName = "item")]      //MLHIDE
        public string Item { get; set; }
        [XmlAttribute(AttributeName = "revision")]  //MLHIDE
        public string Revision { get; set; }
        [XmlAttribute(AttributeName = "props")]     //MLHIDE
        public string Props { get; set; }

        public VCSStatWcstatus()
        {
            Item = String.Empty;
            Revision = String.Empty;
            Props = String.Empty;
        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "entry")]               //MLHIDE
    public class VCSStatEntry
    {
        [XmlIgnore]
        private List<string> __Paths;
        [XmlElement(ElementName = "wc-status")]    //MLHIDE
        public VCSStatWcstatus Wcstatus { get; set; }
        [XmlAttribute(AttributeName = "path")]     //MLHIDE
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
        public VCSStatEntry()
        {
            __Paths = null;
        }
    }

    [Serializable()]
    [XmlRoot(ElementName = "target")]                 //MLHIDE
    public class VCSStatMain
    {
        [XmlElement(ElementName = "entry")]           //MLHIDE
        public List<VCSStatEntry> Entry { get; set; }
        [XmlAttribute(AttributeName = "path")]        //MLHIDE
        public string Path { get; set; }
    }

    [Serializable()]
    [XmlRoot(ElementName = "status")]                 //MLHIDE
    public class VCSStatRoot
    {
        [XmlElement(ElementName = "target")]          //MLHIDE
        public VCSStatMain Root { get; set; }
    }
}
