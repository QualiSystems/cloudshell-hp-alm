using System;
using System.Threading;

namespace TsCloudShellApi
{
    public class RunTestThread
    {
        private readonly Logger m_Logger;
        private readonly Api m_Api;
        private readonly string m_RunGuid;
        private readonly IRunTestWaiter m_RunTestWaiter;
        private readonly Thread m_Thread;

        public RunTestThread(Logger logger, Api api, string runGuid, IRunTestWaiter runTestWaiter)
        {
            m_Logger = logger;
            m_Api = api;
            m_RunGuid = runGuid;
            m_RunTestWaiter = runTestWaiter;
            m_Thread = new Thread(ThreadLoop);
            m_Thread.IsBackground = true;
            m_Thread.Start();
        }

        private void ThreadLoop()
        {
            try
            {
                while (true)
                {
                    string contentError;
                    bool isSuccess;

                    var apiSuiteStatusDetails = m_Api.GetRunStatus(m_RunGuid, out contentError, out isSuccess);

                    if (!isSuccess)
                    {
                        throw new Exception(contentError);
                    }

                    if (HasRunEnded(apiSuiteStatusDetails))
                        break;

                    if (apiSuiteStatusDetails != null)
                        m_RunTestWaiter.OnTestRunStatusChanged(apiSuiteStatusDetails.SuiteStatus);

                    Thread.Sleep(TimeSpan.FromSeconds(int.Parse(SettingsFile.RunStatusSleepSeconds)));
                }

                var suiteDetails = GetSuiteResult();

                m_RunTestWaiter.OnTestRunEnded(suiteDetails, null);
            }
            catch (ThreadAbortException)
            {}
            catch (Exception ex)
            {
                m_Logger.Error("Unexpected error in RunTestThread: {0}", ex);

                try
                {
                    m_RunTestWaiter.OnTestRunEnded(null, ex.Message);
                }
                catch {}
            }
        }

        private bool HasRunEnded(ApiSuiteStatusDetails apiSuiteStatusDetails)
        {
            if (apiSuiteStatusDetails == null)
            {
                m_Logger.Error("RunTestThread.HasRunEnded ApiSuiteStatusDetails = null");
                //throw new Exception
                return true;
            }

            m_Logger.Debug("RunTestThread.HasRunEnded ApiSuiteStatusDetails.SuiteStatus = {0}", apiSuiteStatusDetails.SuiteStatus);

            if (apiSuiteStatusDetails.SuiteStatus == "Ended")
            {
                return true;
            }
            return false;
        }
        
        private ApiSuiteDetails GetSuiteResult()
        {
            string contentError;
            bool isSuccess;
            ApiSuiteDetails apiSuiteDetails = m_Api.GetRunResult(m_RunGuid, out contentError, out isSuccess);
            if (!isSuccess)
            {
                throw new Exception(contentError);
            }
            return apiSuiteDetails;
        }

        public void Abort()
        {
            try
            {
                m_Thread.Abort();
            }
            catch {}

            try
            {
                string contentError;
                bool isSuccess;
                m_Api.StopTest(m_RunGuid, out contentError, out isSuccess);
                if (!isSuccess)
                {
                    m_Logger.Error(contentError);
                }
            }
            catch //In case trying abort thread in case when it yet ended
            {
            }

        }
    }

    public interface IRunTestWaiter
    {
        void OnTestRunStatusChanged(string suiteStatus);
        void OnTestRunEnded(ApiSuiteDetails suiteDetails, string unexpectedError);
    }
}
