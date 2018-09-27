using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SpchListBuilder.Data
{
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
        public string Options;
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
