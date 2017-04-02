using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Reflection;
using System.Runtime.InteropServices;
using TDAPIOLELib;
using TsCloudShellApi;

[assembly: ApplicationName("TestShell Remote Agent")]
[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: ApplicationAccessControl(false)]

namespace TsAlmRunner
{
    [Guid("E8A238DD-6D7E-4085-969D-700273173385"), ComVisible(true), ProgId("TestShellRemoteAgent1221")]
    public class Agent : ServicedComponent, IRAgent, IRunTestWaiter
    {
        private readonly Logger m_Logger = new Logger("Agent");
        private readonly AlmParameters m_AlmParameters = new AlmParameters();
        private AlmRunStatus m_Status;
        private string m_StatusDesc;
        private RunTestThread m_RunTestThread;
        private string m_ToServer;
        private string m_OnServer;

        public Agent()
        {
            if (SettingsFile.DebugMode)
                Debugger.Launch();

            m_Status = AlmRunStatus.Init;

            StartupHelper.ReportStart(m_Logger, "Agent", Assembly.GetExecutingAssembly());
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
            status = ResultsHelper.AlmRunStatusToString(m_Status);
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
            if (m_RunTestThread != null)
            {
                SetStatus(AlmRunStatus.Failed, "Previous test is still running");
                return 0;
            }

            try
            {
                var almConnection = new AlmConnection(m_Logger, m_AlmParameters);
                var almTestHelper = new AlmTest();
                var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
                var testPath = almTestHelper.GetTestPath(almConnection,test);
                var testParameters = almTestHelper.GetTestParameters(test);                
               
                string[] executionServers = SetExecutionServers(almConnection.Connection);
                var api = new Api(m_Logger, almConnection.Connection, m_AlmParameters.UserName, m_AlmParameters.Password, m_AlmParameters.HostName);

                string contentError;
                bool isSuccess;
                string runGuId = api.RunTest(testPath, testParameters.ToArray(), executionServers, out contentError, out isSuccess);

                if (!isSuccess)
                    throw new Exception(contentError);

                // Start the Run Test Thread
                m_RunTestThread = new RunTestThread(m_Logger, api, runGuId, this);
                
               SetStatus(AlmRunStatus.LogicalRunning, "Sending Test" + m_ToServer);               
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
            }

            return 0;
        }
        
        public int stop()
        {
            if (m_RunTestThread != null)
            {
                m_RunTestThread.Abort();
                m_RunTestThread = null;
            }

            Process.GetCurrentProcess().Kill();
            return 0;
        }

        public void OnTestRunStatusChanged(string suiteStatus)
        {
            SetStatus(AlmRunStatus.LogicalRunning, suiteStatus + m_OnServer + DateTime.Now.ToString("T"));
        }

        public void OnTestRunEnded(ApiSuiteDetails suiteDetails, string unexpectedError)
        {
            if (!string.IsNullOrEmpty(unexpectedError))
            {
                SetStatus(AlmRunStatus.Failed, "Test ended unexpectedly:" + m_OnServer + unexpectedError);
                return;
            }

            // get test result from Quali
            SetStatus(AlmRunStatus.LogicalRunning, "Getting test results");

            string reportLink;
            AlmRunStatus almRunStatus;

            try
            {

                var runResultStatus = new ResultsHelper(m_Logger).GetRunResult(suiteDetails);
                reportLink = suiteDetails.JobsDetails[0].Tests[0].ReportLink;
                almRunStatus = ResultsHelper.ConvertTestShellResultToAlmRunStatus(runResultStatus);

                if (!string.IsNullOrWhiteSpace(suiteDetails.JobsDetails[0].JobFailureDescription))
                {
                    SetStatus(almRunStatus, suiteDetails.JobsDetails[0].JobFailureDescription);
                }
                else
                {
                    SetStatus(almRunStatus, runResultStatus.ToString());
                }
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
                return;
            }

            // Save test result to ALM
            try
            {
                var almConnection = new AlmConnection(m_Logger, m_AlmParameters);
                var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
                var almResults = new AlmResults(m_AlmParameters, testSetFactory);
                almResults.SaveRunResults(almRunStatus, reportLink);
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
            }
        }

        private string[] SetExecutionServers(ITDConnection tdConnection)
        {
            TDConnectionServant conectionServant = new TDConnectionServant(tdConnection);
            string isExecutionServerLocal = conectionServant.GetTdParam("CLOUDSHELL_EXECUTION_SERVER", "SpecificHost");
            string executionServerName = Config.ExecutionServerName;
            string[] executionServers = null;
            if (string.IsNullOrWhiteSpace(executionServerName) && isExecutionServerLocal.ToLower() =="any")
            {
                executionServers = new string[0];
            }
            else
            {
                executionServers = new string[1];
                if (!string.IsNullOrWhiteSpace(executionServerName))
                {
                    executionServers[0] = executionServerName;
                }
                else
                {
                    executionServers[0] = m_AlmParameters.HostName;
                }
            }
            if (executionServers.Length == 1)
            {
                m_ToServer = " to '" + executionServers[0] + "' ";
                m_OnServer = " on '" + executionServers[0] + "' ";
            }
            else
            {
                m_ToServer = " ";
                m_OnServer = " ";
            }
            return executionServers;
        }
    }
}
