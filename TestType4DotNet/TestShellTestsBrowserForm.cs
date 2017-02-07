using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTree;
using Mercury.TD.Client.UI.Components.Api;
using Mercury.TD.Client.UI.Components.ThirdParty.QCTree;
using QS.ALM.CloudShellApi;

namespace CTSAddin
{
    public partial class TestShellTestsBrowserForm : Form
    {
        public TestShellTestsBrowserForm()
        {
            InitializeComponent();
            AddQCTree();
        }
        public TestShellTestsBrowserForm(string path)
        {
            InitializeComponent();
            AddQCTree();
            SelectPath(path);
        }

        private void SelectPath(string path)
        {
            if (path != null && path != "")
            {
                path = path.ToLower();
                string[] arrPath = path.Split(new char[] { '/', '\\' });
                string curPath = "";
                UltraTreeNodeWithStatus tmpNode = null;
                for(int i = 0; i < arrPath.Length; ++i)
                {
                    if (curPath != "")
                    {
                        curPath += "\\";
                    }
                    AddLayerToTree(curPath += arrPath[i]);
                    m_DictonaryNodes.TryGetValue(curPath, out tmpNode);
                    if (tmpNode != null)
                    {
                        tmpNode.Node.Expanded = true;
                    }
                }

                m_DictonaryNodes.TryGetValue(curPath, out tmpNode);
                if (tmpNode != null)
                {
                    tmpNode.Node.Selected = true;
                }
            }
        }

        private Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree m_TestsBrouserQcTree;
        private Dictionary<string, UltraTreeNodeWithStatus> m_DictonaryNodes = new Dictionary<string, UltraTreeNodeWithStatus>();
        public UltraTreeNodeWithStatus SelectedNode { get; private set;}//node selecter in tree

        private void AddQCTree()
        {
            TreeViewPanel.Controls.Add(m_TestsBrouserQcTree = new Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree());
            m_TestsBrouserQcTree.Dock = System.Windows.Forms.DockStyle.Fill;
            m_TestsBrouserQcTree.HideSelection = false;
            this.m_TestsBrouserQcTree.BeforeExpand += new Infragistics.Win.UltraWinTree.BeforeNodeChangedEventHandler(this.TestsBrouserQcTree_BeforeExpand);
            this.m_TestsBrouserQcTree.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.TestsBrouserQcTree_AfterSelect);
            this.m_TestsBrouserQcTree.TabIndex = 0;
            AddLayerToTree("");//add layer root
        }

        private void TestsBrouserQcTree_AfterSelect(object sender, SelectEventArgs e)
        {
            if (e.NewSelections.Count > 0)
            {
                UltraTreeNodeWithStatus tmpNode;
                m_DictonaryNodes.TryGetValue(e.NewSelections[0].FullPath, out tmpNode);
                SelectedNode = tmpNode;
                if (SelectedNode.Status == StatusNode.Test)
                {
                    ButtonOK.Enabled = true;
                }
                else
                {
                    ButtonOK.Enabled = false;
                }
            }
            else
            {
                SelectedNode = null;
            }
        }

        private void TestsBrouserQcTree_BeforeExpand(object sender, CancelableNodeEventArgs e)
        {
            AddLayerToTree(e.TreeNode.FullPath);            
        }

        private void AddLayerToTree(string path)
        {            
            UltraTreeNodeWithStatus node = null;
            if(path == null)
            {
                path = "";
            }
            if(path != "")
            {
                path = path.ToLower();
                m_DictonaryNodes.TryGetValue(path, out node);
            }

            if (node == null || node.Status == StatusNode.NotFilled)
            {
                TestNode[] arrNodes = Api.GetNodes(path);
                if(arrNodes != null && arrNodes.Length != 0)
                {
                    if(path != "")
                    {
                        path += "\\";
                    }
                    foreach(TestNode nodeTmp in arrNodes)
                    {
                        string nameNode = nodeTmp.Name.ToLower();                       
                        string newPatn = path + nameNode;
                        UltraTreeNode ultraTreeNode;
                        if(node == null)
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(newPatn, nameNode);//add layer root
                        }
                        else
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(node.Node, newPatn, nameNode);
                        }

                        if (nodeTmp.Type == TypeNode.Folder)
                        {
                            m_DictonaryNodes.Add(newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.NotFilled));
                        }
                        else
                        {
                            m_DictonaryNodes.Add(newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.Test));
                            ultraTreeNode.Expanded = true;//for remove picture plus near node
                        }
                    }
                }
                if (node != null)
                {
                    node.Status = StatusNode.Filled;
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            string keyTest = SelectedNode.Node.FullPath;
            this.Close();
        }
    }
}
