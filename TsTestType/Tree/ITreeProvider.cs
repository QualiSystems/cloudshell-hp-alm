using System.Windows.Forms;

namespace TsTestType.Tree
{
    public enum TreeImage
    {
        Local,
        Shared,
        Folder,
        Test
    }

    public delegate void TreeSelectHandler(ITreeNode node);
    public delegate void TreeViewCancelHandler(ITreeNode node, ref bool cancel);
    public interface ITreeProvider
    {
        ITreeNode AddNode(string newPath, string nameNode);
        ITreeNode AddNode(ITreeNode parent, string newPath, string nameNode);
        ITreeNode CreateNewNode();
        ITreeNode SelectedNode{ get; set; }
        void SetNodeImage(ITreeNode node, TreeImage image);
        event TreeSelectHandler AfterSelect;
        event TreeViewCancelHandler BeforeExpand;
        Control GetTreeControl();
    }
}