using System;

namespace QS.ALM.CloudShellApi
{
    public static class Logger
    {
        private enum LogVerbosity
        {
            Debug,
            Info,
            Warn,
            Error
        }

        public static void Debug(string format, params object[] args)
        {
            Log(LogVerbosity.Debug, format, args);
        }

        public static void Info(string format, params object[] args)
        {
            Log(LogVerbosity.Info, format, args);
        }

        public static void Warn(string format, params object[] args)
        {
            Log(LogVerbosity.Warn, format, args);
        }

        public static void Error(string format, params object[] args)
        {
            Log(LogVerbosity.Error, format, args);
        }

        private static void Log(LogVerbosity verbosity, string format, object[] args)
        {
            if (verbosity < Verbosity)
                return;

            try
            {
                format = string.Format(format, args);
            }
            catch {}
        }

        private static LogVerbosity Verbosity
        {
            get {  return LogVerbosity.Debug; }
        }
    }
}
