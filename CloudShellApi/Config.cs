using System;
using System.Threading;

namespace QS.ALM.CloudShellApi
{
    static class Config
    {
        public static string SuiteName
        {
            get { return "ALM Suite"; }
        }

        public static string JobName
        {
            get { return "ALM Job"; }
        }

        public static TimeSpan QueueTimeout
        {
            get
            {
                var queueTimeoutMinutesStr = GetEnvVar("CloudShell.QueueTimeoutMinutes", null);

                if (queueTimeoutMinutesStr != null)
                {
                    int queueTimeoutMinutes;

                    if (int.TryParse(queueTimeoutMinutesStr, out queueTimeoutMinutes))
                        return TimeSpan.FromMinutes(queueTimeoutMinutes);
                    
                    Logger.Error("Invalid value for 'CloudShell.QueueTimeoutMinutes': {0}", queueTimeoutMinutesStr);
                }

                return TimeSpan.FromHours(1);
            }
        }

        public static string OverrideCloudShellUrl { get { return GetEnvVar("CloudShell.Url", null); } }

        public static string OverrideUsername { get { return GetEnvVar("CloudShell.Username", null); } }

        public static string OverridePassword { get { return GetEnvVar("CloudShell.Password", null); } }

        public static string LoggingProfile { get { return GetEnvVar("CloudShell.LoggingProfile", "Results"); } }

        public static string TestsRoot { get { return GetEnvVar("CloudShell.TestsRoot", null); } }

        private static string GetEnvVar(string name, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(name);
            
            Logger.Debug("Environment variable: {0}={1}", name, value ?? "EMPTY");

            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }
    }

    //~Abugov TODO: move this to a different file (I've put this here to avoid merge conflicts of project file)
    public class RunStatusManager : IDisposable
    {
        private readonly string m_RunGuid;
        private readonly ManualResetEvent m_Event = new ManualResetEvent(false);
        private bool m_RunResult;
        private Thread m_Worker;

        public RunStatusManager(string runGuid)
        {
            m_RunGuid = runGuid;
            m_Worker = new Thread(ThreadLoop);
            m_Worker.IsBackground = true;
            m_Worker.Start();
        }

        public bool WaitForRunEnd()
        {
            m_Event.WaitOne();
            return m_RunResult;
        }

        private void ThreadLoop()
        {
            var cloudShellStatus = "Running"; // Dummy value until implemented

            try
            {
                while (true)
                {
                    //cloudShellStatus = Api.GetRunStatus(m_RunGuid)

                    if (HasRunEnded(cloudShellStatus))
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

            m_RunResult = IsRunSuccess(cloudShellStatus);
            m_Event.Set();
        }

        private static bool HasRunEnded(string cloudShellStatus)
        {
            //~TODO: need to check the CloudShell status
            return false;
        }

        private static bool IsRunSuccess(string cloudShellStatus)
        {
            //~TODO: need to check the CloudShell status
            return false;
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
