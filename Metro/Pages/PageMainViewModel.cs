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
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using SpchListBuilder.Base;
using SpchListBuilder.Data;
using SpchListBuilder.Extension;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Pages
{
    public class PageMainViewModel : BaseViewModel
    {
        private bool __isProcess { get; set; }
        private bool __isServiceMenu { get; set; }
        private int __repoItemsCount { get; set; }
        private string __blockInfo { get; set; }      /* Top InfoBox */
        string __repoInfo { get; set; }               /* Bottom InfoBox */

        private ObservableCollectionExtension<ExtList> __ListExt { get; set; }
        private ObservableCollectionExtension<Node> __TvNodes { get; set; }
        private VCSDataRepo __repo { get; set; }
        private SpchOptions __RepoOptions { get; set; }

        public ObservableCollectionExtension<ExtList> ListExt
        {
            get { return __ListExt; }
            private set { __ListExt = value; OnPropertyChanged("ListExt"); }  //MLHIDE
        }
        public ObservableCollectionExtension<Node> TvNodes
        {
            get { return __TvNodes; }
            private set { __TvNodes = value; OnPropertyChanged("TvNodes"); }  //MLHIDE
        }
        public SpchOptions RepoOptions
        {
            get { return __RepoOptions; }
            private set { __RepoOptions = value; OnPropertyChanged("RepoOptions"); }  //MLHIDE
        }
        public string BlockInfo // Top InfoBox
        {
            get { return __blockInfo; }
            set { __blockInfo = value; OnPropertyChanged("BlockInfo"); }      //MLHIDE
        }
        public string RepoInfo  // Bottom InfoBox
        {
            get { return __repoInfo; }
            set { __repoInfo = value; OnPropertyChanged("RepoInfo"); }        //MLHIDE
        }
        public bool IsProcess  // Spinner-Busy enable
        {
            get { return __isProcess; }
            set { __isProcess = value; OnPropertyChanged("IsProcess"); }      //MLHIDE
        }
        public bool ServiceMenuIsEnable  // Spinner-Busy enable
        {
            get { return ((__isProcess) ? false : __isServiceMenu); }
            set { __isServiceMenu = value; OnPropertyChanged("ServiceMenuIsEnable"); }      //MLHIDE
        }
        public int RepoItemsCount
        {
            get { return __repoItemsCount; }
        }

        public PageMainViewModel()
        {
            __isProcess = false;
            __isServiceMenu = false;
            __repo = null;
            this.TvNodes = new ObservableCollectionExtension<Node>();
            this.ListExt = new ObservableCollectionExtension<ExtList>();
            this.RepoOptions = new SpchOptions();
        }

        public Task<bool> GetData(string exepath, VCSDataRepo repo)
        {
            try
            {
                __repo = repo;

                return Task<bool>.Factory.StartNew(() =>
                {
                    if (
                        (String.IsNullOrWhiteSpace(exepath)) ||
                        (String.IsNullOrWhiteSpace(__repo.UriOrigin))
                        )
                    {
                        Fire_EventError(new StringEventArgs(null, Resources.GetData_input_variable_is_empt));
                        return false;
                    }
                    if (__isProcess)
                    {
                        Fire_EventError(new StringEventArgs(null, Resources.another_process_is_running));
                        return false;
                    }

                    try
                    {
                        string __XmlSrc;
                        IsProcess = true;

                        VCSExec exec = new VCSExec(exepath, __repo);
                        __XmlSrc = exec.Exec(__repo.TypeRequest);
                        exec = null;

                        if (String.IsNullOrWhiteSpace(__XmlSrc))
                        {
                            Fire_EventError(
                                new StringEventArgs(
                                    null, Resources.XML + Resources.GetData_input_variable_is_empt
                                )
                            );
                            return false;
                        }

                        ObservableCollectionExtension<Node> t__TvNodes =
                            new ObservableCollectionExtension<Node>();

                        ObservableCollectionExtension<ExtList> t__ListExt =
                            new ObservableCollectionExtension<ExtList>();

                        switch (__repo.TypeRequest)
                        {
                            case VCSDataRepo.EnumTypeRequest.__VCS_STAT:
                                {
                                    XmlSerializer s = new XmlSerializer(typeof(VCSStatRoot));
                                    using (TextReader t = new StringReader(__XmlSrc))
                                    {
                                        var __XmlData = (VCSStatRoot)s.Deserialize(t);
                                        t__TvNodes.AddNodeRange<VCSStatRoot, VCSStatEntry>(__XmlData);
                                        t__ListExt.AddNodeExt<VCSStatRoot>(__XmlData);
                                        __repoItemsCount = __XmlData.Root.Entry.Count;
                                    }
                                    break;
                                }
                            case VCSDataRepo.EnumTypeRequest.__VCS_LIST:
                                {
                                    XmlSerializer s = new XmlSerializer(typeof(VCSListRoot));
                                    using (TextReader t = new StringReader(__XmlSrc))
                                    {
                                        var __XmlData = (VCSListRoot)s.Deserialize(t);
                                        t__TvNodes.AddNodeRange<VCSListRoot, VCSListEntry>(__XmlData);
                                        t__ListExt.AddNodeExt<VCSListRoot>(__XmlData);
                                        __repoItemsCount = __XmlData.Root.Entry.Count;
                                    }
                                    break;
                                }
                        }

                        if (t__TvNodes.Count == 0)
                        {
                            Fire_EventError(
                                new StringEventArgs(
                                    null, Resources.XML + Resources.no_items_from_repo_list
                                )
                            );
                            return false;
                        }
                        else
                            App.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                __TvNodes.NotifyOff().Clear();
                                TvNodes = t__TvNodes;
                                __TvNodes.NotifyOn();
                            }));

                        if (t__ListExt.Count == 0)
                            Fire_EventError(
                                new StringEventArgs(
                                    null, Resources.XML + Resources.extension_list_is_empty
                                )
                            );
                        else
                            App.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                __ListExt.NotifyOff().Clear();
                                ListExt = t__ListExt;
                                __ListExt.NotifyOn();
                            }));

                        App.Current.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            RepoOptions.Clear();
                        }));

                        return true;
                    }
                    catch (Exception ex)
                    {
                        Fire_EventError(new StringEventArgs(ex, Resources.GetData));
                        return false;
                    }
                    finally
                    {
                        IsProcess = false;
                    }
                });
            }
            catch (Exception e)
            {
                Fire_EventError(new StringEventArgs(e, Resources.Data_Task));
                return null;
            }
        }

        public Task<string> SaveNodeDataAll(VCSDataRepo repo, bool isSelected = false)
        {
            if (repo == null)
                return null;

            try
            {
                return Task<string>.Factory.StartNew(() =>
                {
                    if (__isProcess)
                    {
                        Fire_EventError(new StringEventArgs(null, Resources.another_process_is_running));
                        return String.Empty;
                    }
                    try
                    {
                        if (Properties.Settings.Default.XmlListOutputFormat)
                        {
                            RepoOptions.VcsType = 
                                Properties.Settings.Default.VCSBinText[Properties.Settings.Default.VCSBinSelect].ToLower();

                            SpchListData __data = new SpchListData()
                            {
                                Setting = new SpchSettings
                                {
                                    Options = RepoOptions,
                                    RepoName = repo.RepoName,
                                    Date = DateTime.Now.GetUnixTimeStamp()
                                },
                                Files = new SpchFiles
                                {
                                    Files = this.TvNodes.ExportSelectedNode(isSelected)
                                }
                            };
                            if ((__data.Files.Files == null) || (__data.Files.Files.Count == 0))
                            {
                                Fire_EventError(new StringEventArgs(null, Resources.Split_list_empty_select_files_));
                                return String.Empty;
                            }

                            XmlSerializer s = new XmlSerializer(typeof(SpchListData));
                            using (StringWriter sw = new StringWriter())
                            {
                                using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings()
                                {
                                    Encoding = new UTF8Encoding(false),
                                    Indent = true,
                                    NewLineOnAttributes = true,
                                })
                                )
                                {
                                    s.Serialize(xw, __data);
                                    s = null;
                                    return sw.ToString();
                                }
                            }
                        }
                        else
                        {
                            List<String> __list = this.TvNodes.ExportSelectedNode(isSelected);
                            return String.Join("\n", __list.ToArray());                            //MLHIDE
                        }
                    }
                    catch (Exception e)
                    {
                        Fire_EventError(new StringEventArgs(e, Resources.Export_Task));
                        return String.Empty;
                    }
                });
            }
            catch (Exception e)
            {
                Fire_EventError(new StringEventArgs(e, Resources.Export_Task));
                return null;
            }
        }

        public Task<bool> LoadNodeDataAll(string inFpath = null)
        {
            if (
                (inFpath == null) ||
                (!File.Exists(inFpath))
               )
                return null;

            try
            {
                return Task<bool>.Factory.StartNew(() =>
                {
                    if (__isProcess)
                    {
                        Fire_EventError(new StringEventArgs(null, Resources.another_process_is_running));
                        return false;
                    }
                    try
                    {
                        SpchListData __XmlData = null;
                        string __StrSrc = File.ReadAllText(inFpath);
                        string __fileDate = String.Empty;

                        if (String.IsNullOrWhiteSpace(__StrSrc))
                        {
                            Fire_EventError(new StringEventArgs(null, Resources.Xml_file_empty_or_corrupt));
                            return false;
                        }

                        if (Properties.Settings.Default.XmlListOutputFormat)
                        {
                            XmlSerializer s = new XmlSerializer(typeof(SpchListData));
                            using (TextReader t = new StringReader(__StrSrc))
                            {
                                __XmlData = (SpchListData)s.Deserialize(t);
                                s = null;
                            }
                            if ((__XmlData == null) || (__XmlData.Files.Files.Count == 0))
                            {
                                Fire_EventError(new StringEventArgs(null, Resources.Split_list_empty_load_another));
                                return false;
                            }
                            foreach (string f in __XmlData.Files.Files)
                            {
                                __TvNodes.SelectNodeByList(f.SplitToList());
                            }
                            RepoOptions = __XmlData.Setting.Options;
                            __fileDate =
                                __XmlData.Setting.Date.GetDateTimeFromUnixTimeStamp().ToShortDateString();
                        }
                        else
                        {
                            FileInfo fi = new FileInfo(inFpath);
                            List<String> __list = __StrSrc.Split('\n').ToList<String>();

                            foreach (string f in __list)
                            {
                                __TvNodes.SelectNodeByList(f.SplitToList());
                            }
                            __fileDate =
                                fi.LastWriteTime.ToShortDateString();
                            fi = null;
                        }
                        RepoInfo = String.Format(
                            Resources.Load_0_last_edit_date_1,
                             Path.GetFileName(inFpath),
                             __fileDate
                        );
                        return true;
                    }
                    catch (Exception e)
                    {
                        Fire_EventError(new StringEventArgs(e, Resources.Export_Task));
                        return false;
                    }
                });
            }
            catch (Exception e)
            {
                Fire_EventError(new StringEventArgs(e, Resources.Export_Task));
                return null;
            }
        }
    }
}
