using System.Windows.Forms;

namespace TsTestType.Tree
{
    public interface ITreeProvider
    {
        ITreeNode AddNode(string newPath, string nameNode);
        ITreeNode AddNode(ITreeNode parent, string newPath, string nameNode);
        ITreeNode CreateNewNode();
        event TreeSelectHandler AfterSelect;
        event TreeViewCancelHandler BeforeExpand;
        Control GetTreeControl();
    }
}