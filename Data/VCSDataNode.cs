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
using System.ComponentModel;
using SpchListBuilder.Base;

namespace SpchListBuilder.Data
{
    /* Node + Nodes Class */
    public class Node : BaseNotifyPropertyChanged
    {
        private string __name;
        private string __desc;
        private bool __check;
        private ObservableCollectionExtension<Node> __nodes { get; set; }
        public ObservableCollectionExtension<Node> Nodes
        {
            get
            {
                return __nodes;
            }
            set
            {
                __nodes = value;
                OnPropertyChanged("Nodes");              //MLHIDE
            }
        }
        public string Name
        {
            get
            {
                return __name;
            }
            set
            {
                __name = value;
                OnPropertyChanged("Name");                //MLHIDE
            }
        }
        public string Desc
        {
            get
            {
                return __desc;
            }
            set
            {
                __desc = value;
                OnPropertyChanged("Desc");                //MLHIDE
            }
        }
        public string DescNoNotify
        {
            get
            {
                return __desc;
            }
            set
            {
                __desc = value;
            }
        }
        public bool Check
        {
            get
            {
                return __check;
            }
            set
            {
                __check = value;
                OnCheckChange(this.Nodes, __check);
                OnPropertyChanged("Check");               //MLHIDE
            }
        }

        public Node()
        {
            __nodes = new ObservableCollectionExtension<Node>();
            __name = String.Empty;
            __desc = String.Empty;
            __check = false;
        }
        public Node(string name, string desc)
        {
            __nodes = new ObservableCollectionExtension<Node>();
            __name = name;
            __check = false;

            if (!String.IsNullOrWhiteSpace(desc))
                __desc = desc;
            else
                __desc = String.Empty;
        }
        private void OnCheckChange(ObservableCollectionExtension<Node> Nodes, bool isCheck)
        {
            if (Nodes.Count == 0)
                return;

            foreach (Node node in Nodes)
            {
                if (node.Check != isCheck)
                    node.Check = isCheck;

                if (node.Nodes.Count > 0)
                    OnCheckChange(node.Nodes, isCheck);
            }
        }
    }
}
