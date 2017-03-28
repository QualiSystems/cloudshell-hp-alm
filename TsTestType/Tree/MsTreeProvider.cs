using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TsTestType.Tree
{
    class MsTreeProvider : ITreeProvider
    {
        private readonly TreeView m_Tree;
        public event TreeSelectHandler AfterSelect;
        public event TreeViewCancelHandler BeforeExpand;
        private string m_SelectedPath = "";
        private List<TreeNode> m_ListSelectedNodes = new List<TreeNode>();
        private TreeNode m_SelectedNode = null;
        private bool m_TreeViewWasNewlyFocused = false;
        private readonly int m_SizeFontRootNode = 14;
        private readonly int m_SizeFontSimpleNode = 12;
        private readonly string m_Shared = "shared";

        public MsTreeProvider()
        {
            m_Tree = new TreeView();
            m_Tree.HideSelection = false;
            ImageList imageList = new ImageList();
            imageList.Images.Add(Resource.TreeNodeLocal);
            imageList.Images.Add(Resource.TreeNodeShared);
            imageList.Images.Add(Resource.TreeNodeFolder);
            imageList.Images.Add(Resource.TreeNodeTest);
            m_Tree.ImageList = imageList;
            m_Tree.BeforeExpand += OnBeforeExpand;
            m_Tree.AfterSelect += OnAfterSelect;
            m_Tree.NodeMouseClick += OnNodeMouseClick;
            m_Tree.LostFocus += OnLostFocus;
            m_Tree.BeforeSelect += OnBeforeSelect;
            m_Tree.ShowPlusMinus = false;
            m_Tree.PathSeparator = "\\";
            m_Tree.Font = new Font(m_Tree.Font.Name, m_SizeFontRootNode, FontStyle.Bold);
        }

        public Control GetTreeControl()
        {
            return m_Tree;
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            m_TreeViewWasNewlyFocused = true;
        }

        private void OnBeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // if the tree has lost focus, when the focus is back it will paint the first node as selected. We want to hide the blue selection background
            if (m_TreeViewWasNewlyFocused && m_SelectedNode != e.Node)
            {
                e.Cancel = true;
                m_TreeViewWasNewlyFocused = false;
            }            
        }

        private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)//Because by default expand/collapse working by double click.
        {
            if(e.Node.IsExpanded == false)
            {
                e.Node.Expand();
            }
            else
            {
                e.Node.Collapse();
            }
        }

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var cancel = e.Cancel;
            BeforeExpand(new MsTreeNode(e.Node), ref cancel);
            e.Cancel = cancel;
            if(cancel == false)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    if(string.IsNullOrEmpty(node.Name))
                    {
                        e.Node.Nodes.Remove(node);
                        return;
                    }
                }
            }
        }
        private void OnAfterSelect(object sender, TreeViewEventArgs e)
        {
            AfterSelect(new MsTreeNode(e.Node));
            MarkNewTreePath();
        }

        public ITreeNode SelectedNode
        {
            get { return new MsTreeNode(m_SelectedNode); }
            set 
            {
                if (!m_Tree.Focused)
                {
                    m_TreeViewWasNewlyFocused = true;
                }
                m_Tree.SelectedNode = ((MsTreeNode)value).Node;
                m_SelectedNode = ((MsTreeNode)value).Node;
            }
        }

        public ITreeNode AddNode(string newPath, string nameNode)
        {            
            TreeNode node = new TreeNode(nameNode, new TreeNode[]{new TreeNode()});
            node.Name = nameNode;
            if (nameNode.ToLower() == m_Shared)
            {
                SetNodeImage(new MsTreeNode(node), TreeImage.Shared);
            }
            else //For Local and any other case
            {
                SetNodeImage(new MsTreeNode(node), TreeImage.Local);
            }
            m_Tree.Nodes.Add(node);
            return new MsTreeNode(node);
        }

       public ITreeNode AddNode(ITreeNode parent, string newPath, string nameNode)
        {
            TreeNode node = new TreeNode(nameNode, new TreeNode[] { new TreeNode() });
            node.Name = nameNode;
            ((MsTreeNode)parent).Node.Nodes.Add(node);
            return new MsTreeNode(node);
        }

        public ITreeNode CreateNewNode()
        {
            return new MsTreeNode(new TreeNode());
        }

        public void SetNodeImage(ITreeNode node, TreeImage image) 
        {            
            TreeNode msNode = ((MsTreeNode)node).Node;
            msNode.ImageIndex = (int)image;
            msNode.SelectedImageIndex = (int)image;
            msNode.ForeColor = Color.Gray;
            if (image == TreeImage.Folder || image == TreeImage.Test)
            {
                msNode.NodeFont = new Font(m_Tree.Font.Name, m_SizeFontSimpleNode, FontStyle.Regular);
            }
        }

        private void MarkNewTreePath()
        {
            string[] arrSelectedPath = m_SelectedPath.Split('\\');
            string newSelectedPath = m_Tree.SelectedNode.FullPath;//e.Node.FullPath;
            string[] arrNewPath = newSelectedPath.Split('\\');
            int index = 0;
            for (; index < arrNewPath.Length && index < arrSelectedPath.Length; ++index)//Find the same start path
            {
                if (arrSelectedPath[index] != arrNewPath[index])
                {
                    break;
                }
            }
            while (index < m_ListSelectedNodes.Count)//Old marked path return to gray
            {
                m_ListSelectedNodes[index].ForeColor = Color.Gray;
                m_ListSelectedNodes.Remove(m_ListSelectedNodes[index]);
            }
            for (int i = index; i < arrNewPath.Length; ++i)//marked to Color.DodgerBlue new selected path
            {
                TreeNodeCollection collect;
                if (i == 0)
                {
                    collect = m_Tree.Nodes;
                }
                else
                {
                    collect = m_ListSelectedNodes[i - 1].Nodes;
                }
                foreach (TreeNode node in collect)
                {
                    if (node.Text.ToLower() == arrNewPath[i].ToLower())
                    {
                        node.ForeColor = Color.DodgerBlue;//.FromArgb(0, 150, 214);
                        m_ListSelectedNodes.Add(node);
                        break;
                    }
                }

            }
            m_SelectedPath = newSelectedPath;
            m_SelectedNode = m_Tree.SelectedNode;
            m_Tree.AfterSelect -= OnAfterSelect;
            m_Tree.SelectedNode = null;// we need to set the SelectedNode to null in order to hide the selection color (we don't want the blue background). Forthis we need to cancel the AfterSelect event in order to prevent infinite loop
            m_Tree.AfterSelect += OnAfterSelect;
        }
       
    }
}
