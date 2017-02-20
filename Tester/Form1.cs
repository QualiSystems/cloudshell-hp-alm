using System;
using System.Windows.Forms;
using QS.ALM.CloudShellApi;
using CTSAddin;

namespace Tester
{
    public partial class Form1 : Form
    {
        private ScriptViewerControl m_ScriptControl = null;
        private Api m_Api;
        public Form1()
        {   
            InitializeComponent();

            m_ScriptControl = new ScriptViewerControl();
            PanelScriptView.Controls.Add(m_ScriptControl);
            m_ScriptControl.Dock = DockStyle.Fill;
        }

        private void ButtonRunTest_Click(object sender, EventArgs e)
        {
            m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", "Global");
            string contentError;
            bool isSuccess;
            string GuiId = m_Api.RunTest(m_ScriptControl.TestPath, out contentError, out isSuccess);

            if (isSuccess)
            {
                MessageBox.Show("Result Test = \"" + GuiId + '\"', "Returned Key", MessageBoxButtons.OK);

                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(GuiId, m_Api))
                    runResult = runStatusManager.WaitForRunEnd();

                MessageBox.Show("Run Result = " + runResult.ToString());

                //~TODO: write the runResult to ALM ...
            }
            else
            {
                MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
            }
        }
    }
}
