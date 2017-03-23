using System.Windows.Forms;
using Infragistics.Win.UltraWinTree;
using Mercury.TD.Client.UI.Components.ThirdParty.QCTree;

namespace TsTestType.Tree
{
    public delegate void TreeSelectHandler(ITreeNode node);
    public delegate void TreeViewCancelHandler(ITreeNode node, ref bool cancel);

    public class AlmTreeProvider : ITreeProvider
    {
        private readonly QCTree m_Tree;

        public AlmTreeProvider()
        {
            m_Tree = new QCTree();
            m_Tree.HideSelection = false;
            m_Tree.BeforeExpand += OnBeforeExpand;
            m_Tree.AfterSelect += OnAfterSelect;
        }

        public event TreeSelectHandler AfterSelect;
        public event TreeViewCancelHandler BeforeExpand;

        public Control GetTreeControl()
        {
            return m_Tree;
        }

        private void OnAfterSelect(object sender, SelectEventArgs e)
        {
            var selectedNode = e.NewSelections.Count > 0 ? new AlmTreeNode(e.NewSelections[0]) : null;
            AfterSelect(selectedNode);
        }

        private void OnBeforeExpand(object sender, CancelableNodeEventArgs e)
        {
            var cancel = e.Cancel;
            BeforeExpand(new AlmTreeNode(e.TreeNode), ref cancel);
            e.Cancel = cancel;
        }

        public ITreeNode AddNode(string newPath, string nameNode)
        {
            var node = m_Tree.AddRow(newPath, nameNode);
            return new AlmTreeNode(node);
        }

        public ITreeNode AddNode(ITreeNode parent, string newPath, string nameNode)
        {
            var node = m_Tree.AddRow(((AlmTreeNode)parent).Node, newPath, nameNode);
            return new AlmTreeNode(node);
        }

        public ITreeNode CreateNewNode()
        {
            return new AlmTreeNode(new UltraTreeNode());
        }
    }
}