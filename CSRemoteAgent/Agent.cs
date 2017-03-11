using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QS.ALM.CloudShellApi;
using TDAPIOLELib;

[assembly: ApplicationName("Scotty CS Remote Agent")]
[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]

namespace CSRAgent
{
    [Guid("479DFA08-CF6D-4890-AAAF-7CAFC39B6974"), ComVisible(true), ProgId("AlmCsRemoteAgent1152")]
    public class CSRAgent : ServicedComponent, IRAgent
    {
        private readonly AlmParameters m_AlmParameters = new AlmParameters();

        public CSRAgent()
        {
            m_Status = AlmRunStatus.Init;
            MessageBox.Show("Run Agent is up!");
        }

        private AlmRunStatus m_Status;
        private string m_StatusDesc;

        private void SetStatus(AlmRunStatus almRunStatus, string statusDesc)
        {
            m_Status = almRunStatus;
            m_StatusDesc = statusDesc;
        }

        public int get_value(string paramName, ref string paramValue)
        {
            if (m_AlmParameters.ContainsParameter(paramName))
                paramValue = m_AlmParameters.GetValue(paramName);

            return 0;
        }

        public int set_value(string paramName, string paramValue)
        {
            m_AlmParameters.SetValue(paramName, paramValue);
            return 0;
        }

        public int get_status(ref string descr, ref string status)
        {
            descr = m_StatusDesc;
            status = AgentRunManager.ConvertAlmRunStatusToString(m_Status);
            return 0;
        }

        public int is_host_ready(ref string descr)
        {
            SetStatus(AlmRunStatus.Init, "Host initializing");

            try
            {
                var almConnection = new AlmConnection(m_AlmParameters);
                var api = new Api(almConnection.Connection);
                api.VerifyLogin();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                SetStatus(AlmRunStatus.Failed, ex.Message);
                descr = ex.Message;
                return -1;
            }

            descr = "Host is ready";
            SetStatus(AlmRunStatus.Ready, descr);
            return 0;
        }

        public int run()
        {
            SetStatus(AlmRunStatus.LogicalRunning, "Running test");
            
            Exception runException = null;
            AlmConnection almConnection;

            try
            {
                almConnection = new AlmConnection(m_AlmParameters);
                var almTestHelper = new AlmTest();
                var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
                var testPath = almTestHelper.GetTestPath(test);
                var testParameters = almTestHelper.GetTestParameters(test);

                var api = new Api(almConnection.Connection);
                var agentRunManager = new AgentRunManager(api, testPath, testParameters);
                var runResultStatus = agentRunManager.RunTest();

                SetStatus(AgentRunManager.ConvertTestShellResultToAlmRunStatus(runResultStatus), "Test run: " + runResultStatus);
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, "Test run failed: " + ex.Message);
                runException = ex;
            }

            // Renew connection (not sure needed):
            almConnection = new AlmConnection(m_AlmParameters);
            var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
            var almResults = new AlmResults(m_AlmParameters, testSetFactory);
            almResults.SaveRunResults(m_Status);

            return runException != null ? 1 : 0;
        }

        public int stop()
        {
            Process.GetCurrentProcess().Kill();
            return 0;
        }
    }
}