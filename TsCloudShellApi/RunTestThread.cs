using System;
using System.Threading;

namespace TsCloudShellApi
{
    public class RunTestThread
    {
        private readonly Api m_Api;
        private readonly string m_RunGuid;
        private readonly IRunTestWaiter m_RunTestWaiter;
        private readonly Thread m_Thread;

        public RunTestThread(Api api, string runGuid, IRunTestWaiter runTestWaiter)
        {
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
            {
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error in RunTestThread: {0}", ex);

                try
                {
                    m_RunTestWaiter.OnTestRunEnded(null, ex.Message);
                }
                catch {}
            }
        }

        private static bool HasRunEnded(ApiSuiteStatusDetails apiSuiteStatusDetails)
        {
            if (apiSuiteStatusDetails == null)
            {
                Logger.Error("RunTestThread.HasRunEnded ApiSuiteStatusDetails = null");
                return true;
            }

            Logger.Debug("RunTestThread.HasRunEnded ApiSuiteStatusDetails.SuiteStatus = {0}", apiSuiteStatusDetails.SuiteStatus);

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
            return m_Api.GetRunResult(m_RunGuid, out contentError, out isSuccess);
        }

        public void Abort()
        {
            m_Thread.Abort();
        }
    }

    public interface IRunTestWaiter
    {
        void OnTestRunStatusChanged(string suiteStatus);
        void OnTestRunEnded(ApiSuiteDetails suiteDetails, string unexpectedError);
    }
}
