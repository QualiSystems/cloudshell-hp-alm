using Infragistics.Win.UltraWinTree;

namespace TsTestType.Tree
{
    public class AlmTreeNode : ITreeNode
    {
        public UltraTreeNode Node { get; private set; }

        public AlmTreeNode(UltraTreeNode node)
        {
            Node = node;
        }

        public string FullPath
        {
            get { return Node.FullPath; }
        }

        public bool Expanded
        {
            get { return Node.Expanded; }
            set { Node.Expanded = value; }
        }

        public bool Selected
        {
            get { return Node.Selected; }
            set { Node.Selected = value; }
        }
    }
}