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
        private Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree TestsBrouserQcTree;
        private Dictionary<string, UltraTreeNodeWithStatus> DictonaryNodes = new Dictionary<string, UltraTreeNodeWithStatus>();
        private UltraTreeNodeWithStatus SelectedNode = null;//node selecter in tree
        enum StatusNode
        {
            Test,
            NotFilled,//status folder node
            Filled    //status folder node
        }

        class UltraTreeNodeWithStatus
        {
            public UltraTreeNode Node { get; set; }
            public StatusNode Status { get; set; }

            public UltraTreeNodeWithStatus(UltraTreeNode node, StatusNode status)
            {
                Node = node;
                Status = status;
            }
        }
        private void AddQCTree()
        {            
            TreeViewPanel.Controls.Add(TestsBrouserQcTree = new Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree());
            TestsBrouserQcTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TestsBrouserQcTree.BeforeExpand += new Infragistics.Win.UltraWinTree.BeforeNodeChangedEventHandler(this.TestsBrouserQcTree_BeforeExpand);
            this.TestsBrouserQcTree.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.TestsBrouserQcTree_AfterSelect);
            AddLayerToTree("");//add layer root
        }

        private void TestsBrouserQcTree_AfterSelect(object sender, SelectEventArgs e)
        {
            if (e.NewSelections.Count > 0)
            {
                DictonaryNodes.TryGetValue(e.NewSelections[0].FullPath, out SelectedNode);//TestsBrouserQcTree.Focused
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
                DictonaryNodes.TryGetValue(path, out node);
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
                           ultraTreeNode = TestsBrouserQcTree.AddRow(newPatn, nameNode);//add layer root
                        }
                        else
                        {
                            ultraTreeNode = TestsBrouserQcTree.AddRow(node.Node, newPatn, nameNode);
                        }

                        if (nodeTmp.Type == TypeNode.Folder)
                        {
                            DictonaryNodes.Add( newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.NotFilled));
                        }
                        else
                        {
                            DictonaryNodes.Add(newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.Test));
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

        private void lButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            string keyTest = SelectedNode.Node.FullPath;
            this.Close();
        }
    }
}
