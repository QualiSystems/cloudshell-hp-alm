using System;
using System.Threading;

namespace QS.ALM.CloudShellApi
{
    public class RunStatusManager : IDisposable
    {
        private readonly string m_RunGuid;
        private readonly ManualResetEvent m_Event = new ManualResetEvent(false);
        private Thread m_Worker;
        private readonly Api m_Api;

        public RunStatusManager(Api api, string runGuid)
        {
            m_RunGuid = runGuid;
            m_Worker = new Thread(ThreadLoop);
            m_Worker.IsBackground = true;
            m_Worker.Start();
            m_Api = api;
            Logger.Debug("QS.ALM.CloudShellApi.RunStatusManager(string runGuid, Api api), runGuid = {0}", m_RunGuid);
        }

        public void WaitForRunEnd()
        {
            m_Event.WaitOne();
        }

        private void ThreadLoop()
        {
            try
            {
                while (true)
                {
                    string contentError;
                    bool isSuccess;

                    if (HasRunEnded(m_Api.GetRunStatus(m_RunGuid, out contentError, out isSuccess)))
                        break;

                    Thread.Sleep(TimeSpan.FromSeconds(int.Parse(SettingsFile.RunStatusSleepSeconds)));
                }

            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("Unexpected error in RunStatusManager: {0}", ex);
            }

            m_Event.Set();
        }

        private static bool HasRunEnded(ApiSuiteStatusDetails apiSuiteStatusDetails)
        {
            if (apiSuiteStatusDetails == null)
            {
                Logger.Error("Method QS.ALM.CloudShellApi.RunStatusManager.HasRunEnded ApiSuiteStatusDetails = null");
                return true;
            }

            Logger.Debug("Method QS.ALM.CloudShellApi.RunStatusManager.HasRunEnded ApiSuiteStatusDetails.SuiteStatus = {0}", apiSuiteStatusDetails.SuiteStatus);
            
            if (apiSuiteStatusDetails.SuiteStatus == "Ended")
            {
                return true;
            }
            return false;
        }

        public static ExecutionJobResult GetRunResult(ApiSuiteDetails cloudShellStatus)
        {
            if (cloudShellStatus == null)
            {
                Logger.Error("cloudShellStatus is null");
                return ExecutionJobResult.Unknown;
            }

            if (cloudShellStatus.JobsDetails.Length == 0)
            {
                Logger.Error("cloudShellStatus.JobsDetails is null");
                return ExecutionJobResult.Unknown;
            }

            var jobResult = cloudShellStatus.JobsDetails[0].JobResult;

            Logger.Debug("Method QS.ALM.CloudShellApi.RunStatusManager.GetRunResult ApiSuiteStatusDetails.SuiteStatus = {0}", jobResult);
            
            switch(jobResult)
            {
                case "NotStarted" :
                    return ExecutionJobResult.NotStarted;
                case "Terminated":
                    return ExecutionJobResult.Terminated;
                case "Completed":
                    return ExecutionJobResult.Completed;
                case "EndedWithErrors":
                    return ExecutionJobResult.EndedWithErrors;
                case "Passed":
                    return ExecutionJobResult.Passed;
                case "Failed":
                    return ExecutionJobResult.Failed;
                case "EndedWithAnException":
                    return ExecutionJobResult.EndedWithAnException;
                case "ManuallyStopped":
                    return ExecutionJobResult.ManuallyStopped;
                default :
                    Logger.Error("Method QS.ALM.CloudShellApi.RunStatusManager.GetRunResult ApiSuiteStatusDetails.SuiteStatus = {0}", 
                        jobResult);
                    return ExecutionJobResult.Unknown;
            }
        }

        public void Dispose()
        {
            if (m_Worker != null)
            {
                try
                {
                    m_Worker.Abort();
                }
                catch
                {
                }

                m_Worker = null;
            }
        }
    }
}