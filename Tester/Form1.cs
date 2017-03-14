using System;
using System.Windows.Forms;
using System.Collections.Generic;
using QS.ALM.CloudShellApi;
using CTSAddin;

namespace Tester
{
    public partial class Form1 : Form
    {
        private ScriptViewerControl m_ScriptControl = null;
        private readonly Api m_Api;
        public Form1()
        {   
            InitializeComponent();
            try
            {
                m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", null, null, AuthenticationMode.Alm, "Global");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                Enabled = false;
            }
            m_ScriptControl = new ScriptViewerControl(m_Api);
            PanelScriptView.Controls.Add(m_ScriptControl);
            m_ScriptControl.Dock = DockStyle.Fill;
        }

        private void ButtonRunTest_Click(object sender, EventArgs e)
        {
            string contentError;
            bool isSuccess;

            //List<TestParameters> parameters = new List<TestParameters>(); //for testing test parameters "Shared/Folder 1/Test A empty test"
            //parameters.Add(new TestParameters("Param1", "")); 
            //parameters.Add(new TestParameters("Param2_int", "0"));
            string guiId = m_Api.RunTest(m_ScriptControl.TestPath, null/*parameters*/, out contentError, out isSuccess);

            if (isSuccess)
            {
                MessageBox.Show("Result Test = \"" + guiId + '\"', "Returned Key", MessageBoxButtons.OK);

                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(m_Api, guiId))
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
