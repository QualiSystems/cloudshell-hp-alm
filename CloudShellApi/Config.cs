using System;

namespace QS.ALM.CloudShellApi
{
    public static class Config
    {
        public const string TestShell = "TestShell";
        
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
}
