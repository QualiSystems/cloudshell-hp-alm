using Infragistics.Win.UltraWinTree;

namespace TsTestType.Tree
{
    public class AlmTreeNode : ITreeNode
    {

        private static readonly System.Windows.Forms.ImageList m_ImageList;
        public UltraTreeNode Node { get; private set; }

        static AlmTreeNode()
        {
            m_ImageList = new System.Windows.Forms.ImageList();
            m_ImageList.Images.Add(Resource.Local);
            m_ImageList.Images.Add(Resource.Shared);
            m_ImageList.Images.Add(Resource.Folder);
            m_ImageList.Images.Add(Resource.Test);
        }

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

        public int ImageIndex { get { return 0; } set { Node.LeftImages.Add(m_ImageList.Images[value]); } }
    }
}