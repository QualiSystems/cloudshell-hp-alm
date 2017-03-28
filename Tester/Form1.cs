using System;
using System.Reflection;
using System.Windows.Forms;
using TsCloudShellApi;
using TsTestType;

namespace Tester
{
    public partial class Form1 : Form, IRunTestWaiter
    {
        private ScriptViewer m_Script = null;
        private readonly Api m_Api;
        private readonly Logger m_Logger = new Logger("Tester");

        public Form1()
        {   
            InitializeComponent();

            StartupHelper.ReportStart(m_Logger, "Tester", Assembly.GetExecutingAssembly());

            try
            {
                m_Api = new Api(m_Logger, "http://192.168.42.35:9000", null, null, "admin", "admin", AuthenticationMode.User, "Global");
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
            string runGuid = m_Api.RunTest(m_Script.TestPath, new TestParameters[0], out contentError, out isSuccess);

            if (isSuccess)
            {
                MessageBox.Show("Result Test = \"" + runGuid + '\"', "Returned Key", MessageBoxButtons.OK);

                new RunTestThread(m_Logger, m_Api, runGuid, this);
                ButtonRunTest.Enabled = false;
            }
            else
            {
                MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
            }
        }

        public void OnTestRunStatusChanged(string suiteStatus)
        {
            BeginInvoke(new Action(() => { lblTestRunStatus.Text = suiteStatus + " " + DateTime.Now.ToString("T"); }));
        }

        public void OnTestRunEnded(ApiSuiteDetails suiteDetails, string unexpectedError)
        {
            BeginInvoke(new Action(() =>
            {
                ButtonRunTest.Enabled = true;
                lblTestRunStatus.Text = "Not running";

                if (!string.IsNullOrEmpty(unexpectedError))
                {
                    MessageBox.Show("Test ended with error: " + unexpectedError);
                    return;
                }

                try
                {

                    var runResultStatus = new ResultsHelper(m_Logger).GetRunResult(suiteDetails);
                    var almRunStatus = ResultsHelper.ConvertTestShellResultToAlmRunStatus(runResultStatus);
                    var reportLink = suiteDetails.JobsDetails[0].Tests[0].ReportLink;
                    MessageBox.Show(string.Format("Test ended.\n\nResult: {0}\nLink: {1}", almRunStatus, reportLink));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error getting test result: " + ex.Message));
                }
            }));
        }

        private void btnRegisterAgent_Click(object sender, EventArgs e)
        {
            new RegisterAgent(m_Logger).RegisterIfNeeded();
        }
    }
}
