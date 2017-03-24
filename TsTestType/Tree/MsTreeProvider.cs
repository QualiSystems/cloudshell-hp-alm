using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TsTestType.Tree
{
    class MsTreeProvider : ITreeProvider
    {
        private readonly TreeView m_Tree;
        public event TreeSelectHandler AfterSelect;
        public event TreeViewCancelHandler BeforeExpand;

        public MsTreeProvider()
        {
            m_Tree = new TreeView();
            m_Tree.HideSelection = false;
            ImageList imageList = new ImageList();
            imageList.Images.Add(Resource.Local);
            imageList.Images.Add(Resource.Shared);
            imageList.Images.Add(Resource.Folder);
            imageList.Images.Add(Resource.Test);
            m_Tree.ImageList = imageList;
            m_Tree.BeforeExpand += OnBeforeExpand;
            m_Tree.AfterSelect += OnAfterSelect;
        }

        public Control GetTreeControl()
        {
            return m_Tree;
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
        }

        public ITreeNode AddNode(string newPath, string nameNode)
        {
            TreeNode node = new TreeNode(nameNode, new TreeNode[]{new TreeNode()});
            node.Name = nameNode;
            if (nameNode.ToLower() == "local")
            {
                node.ImageIndex = 0;
            }
            else if(nameNode.ToLower() == "shared")
            {
                node.ImageIndex = 1;
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
       
    }
}
