using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TsTestType.Tree
{
    class MsTreeNode : ITreeNode
    {
        public TreeNode Node { get; private set; }

        public MsTreeNode(TreeNode node)
        {
            Node = node;
        }

        public string FullPath
        {
            get { return Node.FullPath; }
        }

        public bool Expanded
        {
            get { return Node.IsExpanded; }
            set
            {
                if (value == true)
                {
                    Node.Expand();
                }
                else
                {
                    Node.Collapse();
                }
            }
        }

        public bool Selected
        {
            get { return Node.IsSelected; }
            set { if (value == true) { Node.TreeView.SelectedNode = Node; } }
        }

        public int ImageIndex { get { return Node.ImageIndex; } set { Node.ImageIndex = value;} }
    }
}
