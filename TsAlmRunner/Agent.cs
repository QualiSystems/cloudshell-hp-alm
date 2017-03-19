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
    public class Agent : ServicedComponent, IRAgent, IRunTestWaiter
    {
        private readonly AlmParameters m_AlmParameters = new AlmParameters();
        private AlmRunStatus m_Status;
        private string m_StatusDesc;
        private RunTestThread m_RunTestThread;

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
                var almConnection = new AlmConnection(m_AlmParameters);
                var almTestHelper = new AlmTest();
                var test = almTestHelper.FindTest(almConnection, m_AlmParameters);
                var testPath = almTestHelper.GetTestPath(almConnection,test);
                var testParameters = almTestHelper.GetTestParameters(test);

                var api = new Api(almConnection.Connection, m_AlmParameters.UserName, m_AlmParameters.Password);

                string contentError;
                bool isSuccess;
                var runGuid = api.RunTest(testPath, testParameters.ToArray(), out contentError, out isSuccess);

                if (!isSuccess)
                    throw new Exception(contentError);

                // Start the Run Test Thread
                m_RunTestThread = new RunTestThread(api, runGuid, this);

                SetStatus(AlmRunStatus.LogicalRunning, "Started");
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
            SetStatus(AlmRunStatus.LogicalRunning, suiteStatus + " " + DateTime.Now.ToString("T"));
        }

        public void OnTestRunEnded(ApiSuiteDetails suiteDetails, string unexpectedError)
        {
            if (!string.IsNullOrEmpty(unexpectedError))
            {
                SetStatus(AlmRunStatus.Failed, "Test ended unexpectedly: " + unexpectedError);
                return;
            }

            // get test result from Quali
            SetStatus(AlmRunStatus.LogicalRunning, "Getting test results");

            string reportLink;
            AlmRunStatus almRunStatus;

            try
            {

                var runResultStatus = ResultsHelper.GetRunResult(suiteDetails);
                reportLink = suiteDetails.JobsDetails[0].Tests[0].ReportLink;
                almRunStatus = ResultsHelper.ConvertTestShellResultToAlmRunStatus(runResultStatus);

                SetStatus(almRunStatus, runResultStatus.ToString());
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
                return;
            }

            // Save test result to ALM
            try
            {
                var almConnection = new AlmConnection(m_AlmParameters);
                var testSetFactory = (TestSetFactory)almConnection.Connection.TestSetFactory;
                var almResults = new AlmResults(m_AlmParameters, testSetFactory);
                almResults.SaveRunResults(almRunStatus, reportLink);
            }
            catch (Exception ex)
            {
                SetStatus(AlmRunStatus.Failed, ex.Message);
            }
        }
    }
}
