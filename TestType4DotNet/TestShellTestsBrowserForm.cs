using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTree;
using QS.ALM.CloudShellApi;

namespace TestType
{
    /// <summary>
    /// Class dialog Form for present tree control for selecting test.
    /// </summary>
    public partial class TestShellTestsBrowserForm : Form
    {
        private string m_StartTestPath;
        private readonly Api m_Api;
        private Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree m_TestsBrouserQcTree;
        private readonly Dictionary<string, UltraTreeNodeWithStatus> m_DictonaryNodes = new Dictionary<string, UltraTreeNodeWithStatus>();
        
        /// <summary>
        /// Current node, selected in tree.
        /// </summary>
        private UltraTreeNodeWithStatus m_SelectedNode { get; set; }

        public TestShellTestsBrowserForm(Api api)
        {
            InitializeComponent();
            m_Api = api;
        }

        /// <summary>
        /// Show Dialog Form if path root for tree control from server reading success. Return choosen test's path.
        /// </summary>
        /// <returns></returns>
        public string TryShowDialog(string path)
        {
            if (AddQcTree() && SelectPath(path))
            {
                m_StartTestPath = path.Replace('\\', '/');
                CheckButtunOkEnabledStatus();
                ShowDialog();
                return m_SelectedNode == null ? null : m_SelectedNode.Node.FullPath.Replace('\\', '/');
            }
            
            return null;
        }

        private bool AddQcTree()
        {
            m_TestsBrouserQcTree = new Mercury.TD.Client.UI.Components.ThirdParty.QCTree.QCTree();
            TreeViewPanel.Controls.Add(m_TestsBrouserQcTree);
            m_TestsBrouserQcTree.Dock = DockStyle.Fill;
            m_TestsBrouserQcTree.HideSelection = false;
            m_TestsBrouserQcTree.BeforeExpand += TestsBrowserQcTree_BeforeExpand;
            m_TestsBrouserQcTree.AfterSelect += TestsBrowsTestsBrouserQcTreeAfterSelect;
            m_TestsBrouserQcTree.TabIndex = 0;

            //add root node
            return AddLayerToTree("");
        }

        private bool SelectPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace('\\', '/');
                string[] arrPath = path.Split('\\', '/');
                var curPath = string.Empty;
                UltraTreeNodeWithStatus tmpNode;

                for(int i = 0; i < arrPath.Length; ++i)
                {
                    if (curPath != "")
                    {
                        curPath += "/";
                    }

                    //Check if need, added layer to tree control from server
                    if (!AddLayerToTree(curPath += arrPath[i]))
                    {
                        return true;
                    }
                    
                    m_DictonaryNodes.TryGetValue(curPath, out tmpNode);

                    if (tmpNode != null)
                    {
                        //open current node
                        tmpNode.Node.Expanded = true;
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

        private void TestsBrowsTestsBrouserQcTreeAfterSelect(object sender, SelectEventArgs e)
        {
            if (e.NewSelections.Count > 0)
            {
                UltraTreeNodeWithStatus tmpNode;
                m_DictonaryNodes.TryGetValue(e.NewSelections[0].FullPath.Replace('\\', '/'), out tmpNode);
                m_SelectedNode = tmpNode;
                CheckButtunOkEnabledStatus();                
            }
            else
            {
                m_SelectedNode = null;
            }
        }

        private void TestsBrowserQcTree_BeforeExpand(object sender, CancelableNodeEventArgs e)
        {
            if (!AddLayerToTree(e.TreeNode.FullPath.Replace('\\', '/')))
            {
                e.Cancel = true;
            }
        }

        private bool AddLayerToTree(string path)
        {
            UltraTreeNodeWithStatus node = null;

            if(path == null)
                path = "";

            if(path != "")
            {
                path = path.Replace('\\', '/');
                m_DictonaryNodes.TryGetValue(path, out node);
                if(node == null)
                {
                    MessageBox.Show("Path '" + path + "' not exist.", "Error", MessageBoxButtons.OK);
                    return false;
                }
            }
            ////////////////////////////////FOR TEST CANCELED EXPAND
            /*if (path == "Shared" && node.Status == StatusNode.NotFilled)
            {
                
                UltraTreeNode node1 = m_TestsBrouserQcTree.AddRow(node.Node, "Shared/NodeTestCollaps", "NodeTestCollaps");
                m_DictonaryNodes.Add("Shared/NodeTestCollaps", new UltraTreeNodeWithStatus(node1, StatusNode.NotFilled));
            }*/
            if (node == null || node.Status == StatusNode.NotFilled)//Or root or data about it layer yet not read from server.
            {
                string contentError;
                bool isStatusServerOk;
                TestNode[] arrNodes = m_Api.GetNodes(path, out contentError, out isStatusServerOk);
                if (path == "")// for root
                {
                    if (!isStatusServerOk)
                    {
                        MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
                        return false;
                    }
                    if (arrNodes != null && arrNodes.Length == 1)
                    {
                        AddRootToDictonaryNodes(arrNodes[0]);
                    }
                }
                else if (!isStatusServerOk)
                {
                    if (contentError == "")
                    {
                        MessageBox.Show("Unknown error 1", "Error", MessageBoxButtons.OK);
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
                        path += "/";
                    }
                    foreach(TestNode nodeTmp in arrNodes)
                    {
                        string nameNode = nodeTmp.Name;                       
                        string newPath = path + nameNode;
                        UltraTreeNode ultraTreeNode;
                        if(node == null)
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(newPath, nameNode);//add layer root
                        }
                        else
                        {
                            ultraTreeNode = m_TestsBrouserQcTree.AddRow(node.Node, newPath, nameNode);//Insert node to tree control under node.Node.
                        }

                        if (nodeTmp.Type == TypeNode.Folder)
                        {
                            m_DictonaryNodes.Add(newPath, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.NotFilled));
                        }
                        else
                        {
                            m_DictonaryNodes.Add(newPath, new UltraTreeNodeWithStatus(ultraTreeNode, StatusNode.Test));
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
        /// <summary>
        /// All nodes out of range for current substring as root adding to dictinary with StatusNode.Filled
        /// </summary>
        /// <param name="root"></param>
        private void AddRootToDictonaryNodes(TestNode root)
        {
            string[] arrPath = root.Name.Split(new char[] { '/', '\\' });
            string curPath = "";
            if (arrPath.Length > 1)
            {
                for (int i = 0; i < arrPath.Length-1; ++i)
                {
                    if (curPath != "")
                    {
                        curPath += "/";
                    }
                    curPath = curPath + arrPath[i];
                    m_DictonaryNodes.Add(curPath, new UltraTreeNodeWithStatus(new UltraTreeNode(), StatusNode.Filled));
                }                
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            UltraTreeNodeWithStatus tmp = m_SelectedNode;
            this.Close();
            m_SelectedNode = tmp;
        }

        void CheckButtunOkEnabledStatus()
        {
            if (m_SelectedNode != null && m_SelectedNode.Status == StatusNode.Test && m_StartTestPath != m_SelectedNode.Node.FullPath.Replace('\\', '/'))
            {
                ButtonOK.Enabled = true;
            }
            else
            {
                ButtonOK.Enabled = false;
            }
        }
        private void TestShellTestsBrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_SelectedNode = null;
        }
    }
}
