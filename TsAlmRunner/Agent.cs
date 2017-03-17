using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using TDAPIOLELib;
using TsCloudShellApi;

[assembly: ApplicationName("TestShell Remote Agent")]
[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]

namespace TsAlmRunner
{
    [Guid("E8A238DD-6D7E-4085-969D-700273173385"), ComVisible(true), ProgId("TestShellRemoteAgent1221")]
    public class Agent : ServicedComponent, IRAgent
    {
        private readonly AlmParameters m_AlmParameters = new AlmParameters();
        private AlmRunStatus m_Status;
        private string m_StatusDesc;

        public Agent()
        {
            if (SettingsFile.DebugMode)
                Debugger.Launch();

            m_Status = AlmRunStatus.Init;
        }

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
            status = AlmRunStatusStrings.ToString(m_Status);
            return 0;
        }

        public int is_host_ready(ref string descr)
        {
            descr = "Host is ready";
            SetStatus(AlmRunStatus.Ready, descr);
            return 0;
        }

        public int run()
        {
            SetStatus(AlmRunStatus.LogicalRunning, "Running test");

            Exception runException = null;
            AlmConnection almConnection;
            string reportLink = null;
            AlmRunStatus almRunStatus;

            try
            {
                almConnection = new AlmConnection(m_AlmParameters);
                var almTestHelper = new AlmTest();
                var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
                var testPath = almTestHelper.GetTestPath(almConnection,test);
                var testParameters = almTestHelper.GetTestParameters(test);

                // Run the test
                var api = new Api(almConnection.Connection, m_AlmParameters.UserName, m_AlmParameters.Password);
                var agentRunManager = new AgentRunManager(api, testPath, testParameters);
                var runGuid = agentRunManager.RunTest();

                var apiDetail = GetSuiteResult(api, runGuid);
                var runResultStatus = RunStatusManager.GetRunResult(apiDetail);
                reportLink = apiDetail.JobsDetails[0].Tests[0].ReportLink;
                almRunStatus = AgentRunManager.ConvertTestShellResultToAlmRunStatus(runResultStatus);

                SetStatus(almRunStatus, runResultStatus.ToString());
            }
            catch (Exception ex)
            {
                almRunStatus = AlmRunStatus.Failed;
                SetStatus(almRunStatus, ex.Message);
                runException = ex;
            }

            try
            {
                // Renew connection (not sure needed):
                almConnection = new AlmConnection(m_AlmParameters);
                var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
                var almResults = new AlmResults(m_AlmParameters, testSetFactory);
                almResults.SaveRunResults(almRunStatus, reportLink);
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
                runException = ex;
            }
            return runException != null ? 1 : 0;
        }

        private static ApiSuiteDetails GetSuiteResult(Api api, string runGuid)
        {
            string contentError;
            bool isSuccess;
            return api.GetRunResult(runGuid, out contentError, out isSuccess);
        }

        public int stop()
        {
            Process.GetCurrentProcess().Kill();
            return 0;
        }
    }
}