using System;
using System.Windows.Forms;
using QS.ALM.CloudShellApi;
using TsTestType;

namespace Tester
{
    public partial class Form1 : Form
    {
        private ScriptViewer m_Script = null;
        private readonly Api m_Api;
        public Form1()
        {   
            InitializeComponent();
            try
            {
                m_Api = new Api("http://192.168.42.35:9000", null, null, "admin", "admin", AuthenticationMode.CloudShell, "Global");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                Enabled = false;
            }
            m_Script = new ScriptViewer(m_Api);
            PanelScriptView.Controls.Add(m_Script);
            m_Script.Dock = DockStyle.Fill;
        }

        private void ButtonRunTest_Click(object sender, EventArgs e)
        {
            string contentError;
            bool isSuccess;

            //List<TestParameters> parameters = new List<TestParameters>(); //for testing test parameters "Shared/Folder 1/Test A empty test"
            //parameters.Add(new TestParameters("Param1", "")); 
            //parameters.Add(new TestParameters("Param2_int", "0"));
            string guiId = m_Api.RunTest(m_Script.TestPath, null/*parameters*/, out contentError, out isSuccess);

            if (isSuccess)
            {
                MessageBox.Show("Result Test = \"" + guiId + '\"', "Returned Key", MessageBoxButtons.OK);

                using (var runStatusManager = new RunStatusManager(m_Api, guiId))
                    runStatusManager.WaitForRunEnd();


                //~TODO: write the runResult to ALM ...
            }
            else
            {
                MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
            }
        }
    }
}
