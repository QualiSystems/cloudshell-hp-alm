using System.Diagnostics;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace TsCloudShellApi
{
    public static class Logger
    {
        static Logger()
        {
            Setup();
        }

        public static void Debug(string format, params object[] args)
        {
            InternalLogger.Debug(FormatMessage(format, args));
        }

        public static void Info(string format, params object[] args)
        {
            InternalLogger.Info(FormatMessage(format, args));
        }

        public static void Warn(string format, params object[] args)
        {
            InternalLogger.Warn(FormatMessage(format, args));
        }

        public static void Error(string format, params object[] args)
        {
            InternalLogger.Error(FormatMessage(format, args));
        }
        
        private static string FormatMessage(string format, object[] args)
        {
            try
            {
                format = string.Format(format, args);
            }
            catch { }

            return format;
        }

        private static ILog InternalLogger
        {
            get { return LogManager.GetLogger("AlmCloudShell"); } 
        }

        private static void Setup()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();

            var patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "[%date] [%thread] %-5level - %message%newline";
            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender
            {
                AppendToFile = true,
                File = Path.Combine(Constants.LogsFolder, "AlmCloudShell.log"),
                Layout = patternLayout,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "1MB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };

            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            var memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = ReadVerbosity();
            hierarchy.Configured = true;
        }

        private static Level ReadVerbosity()
        {
            var verbosity = SettingsFile.Verbosity;

            switch (verbosity.ToLower())
            {
                case "debug":
                    return Level.Debug;
                case "info":
                    return Level.Info;
                case "warn":
                    return Level.Warn;
                case "error":
                    return Level.Error;
                default:
                    EventLog.WriteEntry("AlmCloudShell", "Invalid verbosity: " + verbosity, EventLogEntryType.Error);
                    return Level.Info;
            }
        }
    }
}
