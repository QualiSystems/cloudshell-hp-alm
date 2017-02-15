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
    /// <summary>
    /// Class dialog Form for present tree control for selecting test.
    /// </summary>
    public partial class TestShellTestsBrowserForm : Form
    {
        //private string m_CurrentTestPath = null;
        Api m_Api;
        private Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree m_TestsBrouserQcTree;
        private Dictionary<string, UltraTreeNodeWithStatus> m_DictonaryNodes = new Dictionary<string, UltraTreeNodeWithStatus>();
        /// <summary>
        /// Current node, selected in tree.
        /// </summary>
        public UltraTreeNodeWithStatus m_SelectedNode { get; private set; }

        /// <summary>
        /// Create form and try to select node in tree control by incomming path.
        /// </summary>
        /// <param name="path"></param>
        public TestShellTestsBrowserForm()
        {
            InitializeComponent();            
        }
        /// <summary>
        /// Show Dialog Form if path root for tree control from server reading success. Return choosen test's path.
        /// </summary>
        /// <returns></returns>
        public string TryShowDialog(string path)
        {
            if (AddQCTree() && SelectPath(path))
            {
                ShowDialog();
                return m_SelectedNode == null ? null : m_SelectedNode.Node.FullPath; ;// m_CurrentTestPath;
            }
            else
            {
                return null;
            }
        }

        private bool SelectPath(string path)
        {
            if (path != null && path != "")
            {
                //path = path.ToLower();
                string[] arrPath = path.Split(new char[] { '/', '\\' });
                string curPath = "";
                UltraTreeNodeWithStatus tmpNode = null;
                for(int i = 0; i < arrPath.Length; ++i)
                {
                    if (curPath != "")
                    {
                        curPath += "\\";
                    }
                    if (!AddLayerToTree(curPath += arrPath[i]))//Check if need, added layer to tree control from server
                    {
                        return true;
                    }
                    m_DictonaryNodes.TryGetValue(curPath, out tmpNode);
                    if (tmpNode != null)
                    {
                        tmpNode.Node.Expanded = true;//open current node
                    }
                    else
                    {
                        return false;
                    }
                }
                m_DictonaryNodes.TryGetValue(curPath, out tmpNode);
                if (tmpNode != null)
                {
                    tmpNode.Node.Selected = true;
                }
            }
            return true;
        }      

        private bool AddQCTree()
        {

            m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", "Global");
            TreeViewPanel.Controls.Add(m_TestsBrouserQcTree = new Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree());
            m_TestsBrouserQcTree.Dock = System.Windows.Forms.DockStyle.Fill;
            m_TestsBrouserQcTree.HideSelection = false;
            m_TestsBrouserQcTree.BeforeExpand += new BeforeNodeChangedEventHandler(TestsBrouserQcTree_BeforeExpand);
            m_TestsBrouserQcTree.AfterSelect += new AfterNodeSelectEventHandler(TestsBrouserQcTree_AfterSelect);
            m_TestsBrouserQcTree.TabIndex = 0;
            return AddLayerToTree("");//add layer root
        }

        private void TestsBrouserQcTree_AfterSelect(object sender, SelectEventArgs e)
        {
            if (e.NewSelections.Count > 0)
            {
                UltraTreeNodeWithStatus tmpNode;
                m_DictonaryNodes.TryGetValue(e.NewSelections[0].FullPath, out tmpNode);
                m_SelectedNode = tmpNode;
                if (m_SelectedNode.Status == StatusNode.Test)
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
                m_SelectedNode = null;
            }
        }

        private void TestsBrouserQcTree_BeforeExpand(object sender, CancelableNodeEventArgs e)
        {
            AddLayerToTree(e.TreeNode.FullPath);            
        }

        private bool AddLayerToTree(string path)
        {            
            UltraTreeNodeWithStatus node = null;
            string contentError = "";
            bool IsStatusServerOk = false;
            if(path == null)
            {
                path = "";
            }
            if(path != "")
            {
                m_DictonaryNodes.TryGetValue(path, out node);
                if(node == null)
                {
                    MessageBox.Show("Path '" + path + "' not exist.", "Error", MessageBoxButtons.OK);
                    return false;
                }
            }
            if (node == null || node.Status == StatusNode.NotFilled)//Or root or data about it layer yet not read from server.
            {
                TestNode[] arrNodes = m_Api.GetNodes(path, out contentError, out IsStatusServerOk);
                if (path == "")// for root
                {
                    if (!IsStatusServerOk)
                    {
                        MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
                        return false;
                    }
                }
                else if (!IsStatusServerOk)
                {
                    if (contentError == "")
                    {
                        MessageBox.Show("Some unknown error !!!", "Error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
                    }
                    return false;
                }
                if(arrNodes != null && arrNodes.Length > 0)
                {
                    if(path != "")
                    {
                        path += "\\";
                    }
                    foreach(TestNode nodeTmp in arrNodes)
                    {
                        string nameNode = nodeTmp.Name;                       
                        string newPatn = path + nameNode;
                        UltraTreeNode ultraTreeNode;
                        if(node == null)
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(newPatn, nameNode);//add layer root
                        }
                        else
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(node.Node, newPatn, nameNode);//Insert node to tree control under node.Node.
                        }

                        if (nodeTmp.Type == TypeNode.Folder)
                        {
                            m_DictonaryNodes.Add(newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.NotFilled));
                        }
                        else
                        {
                            m_DictonaryNodes.Add(newPatn, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.Test));
                            ultraTreeNode.Expanded = true;//For remove picture plus near node in tree control.
                        }
                    }
                }
                if (node != null)
                {
                    node.Status = StatusNode.Filled;
                }
            }
            return true;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            UltraTreeNodeWithStatus tmp = m_SelectedNode;
            this.Close();
            m_SelectedNode = tmp;
        }

        private void TestShellTestsBrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_SelectedNode = null;
        }
    }
}
