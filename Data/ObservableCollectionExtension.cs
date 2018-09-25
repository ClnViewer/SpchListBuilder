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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using SpchListBuilder.Properties;

namespace SpchListBuilder.Data
{

    public class ObservableCollectionExtension<T> : ObservableCollection<T>
    {
        private bool __isNotify = true;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (__isNotify)
                base.OnCollectionChanged(e);
        }

        public ObservableCollectionExtension<T> NotifyOff()
        {
            __isNotify = false;
            return this;
        }

        public ObservableCollectionExtension<T> NotifyOn()
        {
            __isNotify = true;
            base.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return this;
        }

        public ObservableCollectionExtension<T> ClearNoNotify()
        {
            __isNotify = false;
            ClearItems();
            __isNotify = true;
            return this;
        }
    }

    public static class ObservableCollectionExtensionUtil
    {
        #region Public methods
        #region SelectNodeByExt methods

        public static void SelectNodeByExt(this ObservableCollectionExtension<Node> Nodes, string ExtTag, bool isCheck)
        {
            if ((Nodes == null) || (Nodes.Count == 0))
                return;

            foreach (Node node in Nodes)
            {
                if (node.Name.EndsWith(ExtTag))
                    node.Check = isCheck;

                if (node.Nodes.Count > 0)
                    node.Nodes.SelectNodeByExt(ExtTag, isCheck);
            }
        }

        #endregion
        #region ExportSelectedNode methods

        public static List<String> ExportSelectedNode(this ObservableCollectionExtension<Node> Nodes, bool isSelected = false)
        {
            if (Nodes.Count == 0)
                throw new VCSDataException(Resources.GetData_input_variable_is_empt);

            List<String> __list = new List<string>();

            try
            {
                Nodes.__NodesToList(__list, String.Empty, isSelected);
                return __list;
            }
            catch (Exception e)
            {
                throw new VCSDataException(null, e);
            }
        }

        #endregion
        #region AddNodeExt methods

        public static void AddNodeExt<T>(this ObservableCollectionExtension<ExtList> Dst, T Src)
            where T : class
        {
            if (Src == null)
                throw new VCSDataException(Resources.XML + Resources.data_serialization_error_a);

            List<String> __ls = null;

            if (typeof(T) == typeof(VCSStatRoot))
            {
                __ls = (from x in (Src as VCSStatRoot).Root.Entry
                        where x.Paths.Count > 0 &&
                              x.Paths.Last().Contains('.') &&
                             !x.Paths.Last().StartsWith(".")                 //MLHIDE
                        select Path.GetExtension(x.Paths.Last())).ToList();
            }
            else if (typeof(T) == typeof(VCSListRoot))
            {
                __ls = (from x in (Src as VCSListRoot).Root.Entry
                        where x.Paths.Count > 0 &&
                              x.Paths.Last().Contains('.') &&
                             !x.Paths.Last().StartsWith(".")                 //MLHIDE
                        select Path.GetExtension(x.Paths.Last())).ToList();
            }
            if ((__ls == null) || (__ls.Count == 0))
                return;

            List<String> __lsuniq = (from x in __ls
                                     orderby x
                                     select x).Distinct().ToList();

            __lsuniq.ForEach(x => Dst.Add(new ExtList(x)));
        }

        #endregion
        #region AddNodeRange methods

        public static void AddNodeRange<Tin, Tentry>(this ObservableCollectionExtension<Node> Dst, Tin Src)
            where Tin : class
            where Tentry : class
        {
            if (Src == null)
                throw new VCSDataException(Resources.XML + Resources.data_serialization_error_a);

            try
            {
                Type t = typeof(Tentry);
                IOrderedEnumerable<Tentry> __Entry = Src.__AddRangeFilter<Tin, Tentry>();

                Dst.NotifyOff();

                foreach (Tentry entry in __Entry)
                {
                    if (t == typeof(VCSStatEntry))
                        Dst.__AddRangeStat(entry as VCSStatEntry);
                    else if (t == typeof(VCSListEntry))
                        Dst.__AddRangeList(entry as VCSListEntry);
                    else
                    {
                        Dst.NotifyOn();
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Dst.ClearNoNotify();
                throw;
            }
            finally
            {
                Dst.NotifyOn();
            }
        }

        #endregion
        #endregion
        #region Private methods

        private static IOrderedEnumerable<Tentry> __AddRangeFilter<Tin, Tentry>(this Tin Src)
            where Tin : class
            where Tentry : class
        {
            if (typeof(Tin) == typeof(VCSStatRoot))
            {
                return (IOrderedEnumerable<Tentry>)
                       (from x in (Src as VCSStatRoot).Root.Entry
                        where x.Paths.Count > 0
                        orderby x.Paths.Count descending, x.Paths[0]
                        select x);
            }
            else if (typeof(Tin) == typeof(VCSListRoot))
            {
                return (IOrderedEnumerable<Tentry>)
                       (from x in (Src as VCSListRoot).Root.Entry
                        where x.Paths.Count > 0
                        orderby x.Paths.Count descending, x.Paths[0]
                        select x);
            }
            return default(IOrderedEnumerable<Tentry>);
        }

        private static void __AddRangeList(this ObservableCollectionExtension<Node> Dst, VCSListEntry Entry)
        {
            ObservableCollectionExtension<Node> NodeRootChild = Dst;

            for (int i = 0; i < Entry.Paths.Count; i++)
            {
                try
                {
                    string desc = String.Empty;

                    if (i == (Entry.Paths.Count - 1))
                    {
                        if (Entry.Commit == null)
                        {
                            desc = ((!String.IsNullOrWhiteSpace(Entry.Kind)) ? Entry.Kind : String.Empty);
                        }
                        else
                        {
                            desc = __GetDescriptions(
                                ((!String.IsNullOrWhiteSpace(Entry.Commit.Revision)) ?
                                    Entry.Commit.Revision : String.Empty
                                ),
                                String.Empty,
                                ((!String.IsNullOrWhiteSpace(Entry.Commit.Author)) ?
                                    Entry.Commit.Author : String.Empty
                                ),
                                ((!String.IsNullOrWhiteSpace(Entry.Commit.Date)) ?
                                    Entry.Commit.Date : String.Empty
                                ),
                                ((!String.IsNullOrWhiteSpace(Entry.Size)) ?
                                    Entry.Size : String.Empty
                                )
                            );
                        }
                    }

                    Node searcNode = NodeRootChild.ToList().Find(x => x.Name.Equals(Entry.Paths[i]));

                    if (searcNode != null)
                    {
                        searcNode.DescNoNotify = ((!String.IsNullOrWhiteSpace(desc)) ? desc : searcNode.Desc);
                        NodeRootChild = searcNode.Nodes;
                        continue;
                    }
                    Node rootNode = new Node(Entry.Paths[i], desc);
                    NodeRootChild.Add(rootNode);
                    NodeRootChild = rootNode.Nodes;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private static void __AddRangeStat(this ObservableCollectionExtension<Node> Dst, VCSStatEntry Entry)
        {
            ObservableCollectionExtension<Node> NodeRootChild = Dst;

            for (int i = 0; i < Entry.Paths.Count; i++)
            {
                try
                {
                    string desc = String.Empty;

                    if ((i == (Entry.Paths.Count - 1)) && (Entry.Wcstatus != null))
                    {
                        if ((!String.IsNullOrWhiteSpace(Entry.Wcstatus.Item)) && (Entry.Wcstatus.Item.StartsWith("unver")))   //MLHIDE
                        {
                            desc = Entry.Wcstatus.Item;
                        }
                        else
                        {
                            desc = __GetDescriptions(
                                ((!String.IsNullOrWhiteSpace(Entry.Wcstatus.Revision)) ?
                                    Entry.Wcstatus.Revision : String.Empty
                                ),
                                (((Entry.Wcstatus.Commit != null) && (!String.IsNullOrWhiteSpace(Entry.Wcstatus.Commit.Revision))) ?
                                    Entry.Wcstatus.Commit.Revision : String.Empty
                                ),
                                (((Entry.Wcstatus.Commit != null) && (!String.IsNullOrWhiteSpace(Entry.Wcstatus.Commit.Author))) ?
                                    Entry.Wcstatus.Commit.Author : String.Empty
                                ),
                                (((Entry.Wcstatus.Commit != null) && (!String.IsNullOrWhiteSpace(Entry.Wcstatus.Commit.Date))) ?
                                    Entry.Wcstatus.Commit.Date : String.Empty
                                ),
                                String.Empty
                            );
                        }
                    }

                    Node searcNode = NodeRootChild.ToList().Find(x => x.Name.Equals(Entry.Paths[i]));

                    if (searcNode != null)
                    {
                        searcNode.DescNoNotify = ((!String.IsNullOrWhiteSpace(desc)) ? desc : searcNode.Desc);
                        NodeRootChild = searcNode.Nodes;
                        continue;
                    }
                    Node rootNode = new Node(Entry.Paths[i], desc);
                    NodeRootChild.Add(rootNode);
                    NodeRootChild = rootNode.Nodes;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private static string __GetDescriptions(
            string __rev, string __crev, string __autor, string __date, string __size)
        {
            if (__date != String.Empty)
            {
                DateTime dt = DateTime.Parse(__date, null, System.Globalization.DateTimeStyles.RoundtripKind);
                __date = dt.Date.ToUniversalTime().ToString("dd-MM-yyyy");   //MLHIDE
            }
            return String.Format(Resources.commit_0_1_2_3,
                ((__rev != String.Empty) ? __rev : "0"),                     //MLHIDE
                ((__crev != String.Empty) ? __crev : "0"),                   //MLHIDE
                ((__date != String.Empty) ? __date : "-|-"),                 //MLHIDE
                ((__autor != String.Empty) ? __autor : "-"),                 //MLHIDE
                ((__size != String.Empty) ? "[" + __size + "]" : "")         //MLHIDE
            );
        }

        private static void __NodesToList(this ObservableCollectionExtension<Node> Nodes, List<String> DstList, string PathRoot, bool isSelected)
        {
            if (Nodes.Count == 0)
                return;

            foreach (Node node in Nodes)
            {
                string __dir = string.Format(
                    "{0}{1}{2}",   //MLHIDE
                    ((String.IsNullOrWhiteSpace(PathRoot)) ? "" : PathRoot),
                    ((String.IsNullOrWhiteSpace(PathRoot)) ? "" : Path.DirectorySeparatorChar.ToString()),
                    node.Name
                );

                if (node.Nodes.Count > 0)
                {
                    node.Nodes.__NodesToList(DstList, __dir, isSelected);
                }
                else if ((!isSelected) || (node.Check))
                {
                    DstList.Add(__dir);
                }
            }
        }

        #endregion
    }
}
