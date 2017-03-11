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
            StatusDescription = AlmRunStatus.Ready;
        }

        private string StatusDescription { get; set; }

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
            descr = StatusDescription;
            status = "ERROR 97";
            return 0;
        }

        public int is_host_ready(ref string descr)
        {
            StatusDescription = AlmRunStatus.Ready;
            descr = StatusDescription;
            MessageBox.Show("Host is ready!");
            return 0;
        }

        public int run()
        {
            StatusDescription = AlmRunStatus.Init;
            var almConnection = new AlmConnection(m_AlmParameters);

            var almTestHelper = new AlmTest();
            var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
            var testPath = almTestHelper.GetTestPath(test);
            var testParameters = almTestHelper.GetTestParameters(test);

            StatusDescription = AlmRunStatus.LogicalRunning;
            Exception runException = null;

            try
            {
                var api = new Api(almConnection.Connection);
                var agentRunManager = new AgentRunManager();
                StatusDescription = agentRunManager.RunTest(api, testPath, testParameters);
            }
            catch (Exception ex)
            {
                StatusDescription = AlmRunStatus.Failed;
                runException = ex;
            }

            // Renew connection (not sure needed):
            almConnection = new AlmConnection(m_AlmParameters);
            var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
            var almResults = new AlmResults(m_AlmParameters, testSetFactory);
            almResults.SaveRunResults(StatusDescription);

            if (runException != null)
                throw runException;

            return 0;
        }

        public int stop()
        {
            Process.GetCurrentProcess().Kill();
            return 0;
        }
    }
}
