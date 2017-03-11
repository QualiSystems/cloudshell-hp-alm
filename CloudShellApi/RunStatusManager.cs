using System;
using System.Threading;

namespace QS.ALM.CloudShellApi
{
    public class RunStatusManager : IDisposable
    {
        private readonly string m_RunGuid;
        private readonly ManualResetEvent m_Event = new ManualResetEvent(false);
        private ExecutionJobResult m_RunResult;
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

        public ExecutionJobResult WaitForRunEnd()
        {
            m_Event.WaitOne();
            return m_RunResult;
        }

        private void ThreadLoop()
        {
            string contentError = null;
            bool isSuccess = false;

            try
            {
                while (true)
                {
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

            m_RunResult = GetRunResult(m_Api.GetRunResult(m_RunGuid, out contentError, out isSuccess));
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

        private static ExecutionJobResult GetRunResult(ApiSuiteDetails cloudShellStatus)
        {
            if (cloudShellStatus == null)
            {
                Logger.Error("Method QS.ALM.CloudShellApi.RunStatusManager.GetRunResult object ApiSuiteDetails is null");
                return ExecutionJobResult.Unknown;
            }
            Logger.Debug("Method QS.ALM.CloudShellApi.RunStatusManager.GetRunResult ApiSuiteStatusDetails.SuiteStatus = {0}", cloudShellStatus.JobsDetails[0].JobResult);
            
            switch(cloudShellStatus.JobsDetails[0].JobResult)
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
                        cloudShellStatus.JobsDetails[0].JobResult);
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