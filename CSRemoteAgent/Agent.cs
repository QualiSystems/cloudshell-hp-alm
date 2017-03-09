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
            descr = "Ready";
            MessageBox.Show("Host is ready!");
            return 0;
        }

        public int run()
        {
            var almConnection = new AlmConnection(m_AlmParameters);

            var almTestHelper = new AlmTest();
            var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
            var testPath = almTestHelper.GetTestPath(test);
            var testParameters = almTestHelper.GetTestParameters(test);

            Api api;

            try
            {
                api = new Api(almConnection.Connection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var agentRunManager = new AgentRunManager();
            agentRunManager.RunTest(api, testPath);

            //m_AlmParameters.mStatus = CStatus.end_of_test; //"END_OF_TEST";
            StatusDescription = "Completed";

            // Renew connection (not sure needed):
            almConnection = new AlmConnection(m_AlmParameters);
            var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
            var almResults = new AlmResults(m_AlmParameters, testSetFactory);
            almResults.SaveRunResults("TODO 96");

            //Process.GetCurrentProcess().Kill();
            return 0;
        }

        public int stop()
        {
            Process.GetCurrentProcess().Kill();
            return 0;
        }
    }
}
