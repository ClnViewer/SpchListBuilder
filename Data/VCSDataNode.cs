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
