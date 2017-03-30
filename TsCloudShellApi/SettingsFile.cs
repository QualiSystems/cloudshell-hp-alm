using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TsCloudShellApi
{
    public static class SettingsFile
    {
        public static bool DebugMode
        {
            get
            {
                if (FileExists("Debug"))
                    return true;

                return bool.Parse(ReadAppSetting("DebugMode", "false"));
            }
        }

        public static string Verbosity
        {
            get
            {
                if (FileExists("MaxLog"))
                    return "Debug";

                return ReadAppSetting("Verbosity", "Info");
            }
        }

        public static string RunStatusSleepSeconds
        {
            get { return ReadAppSetting("RunStatusSleepSeconds", "3"); }
        }

        /// <summary>
        /// AppSettings are too complicated :)
        /// *In addition* of controlling the settings using the settings.config file,
        /// you can also place files that will act as on/off switch.
        /// e.g.: if the "MaxLog.*" file exists (any extension) then we set Verbosity=Debug
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static bool FileExists(string filename)
        {
            return Directory.GetFiles(Constants.TempFolder, filename + "*", SearchOption.TopDirectoryOnly).Any();
        }

        private static string ReadAppSetting(string key, string defaultValue)
        {
            var appSettings = OpenAppSettings();

            if (appSettings[key] != null)
                return appSettings[key].Value;

            return defaultValue;
        }

        private static KeyValueConfigurationCollection OpenAppSettings()
        {
            try
            {
                // Try to open the optional settings file (if missing, an empty "AppSettings" is returned)
                var configFileMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(Constants.TempFolder, "settings.config")
                };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                return config.AppSettings.Settings; // this is empty, if file not exists
            }
            catch (Exception ex)
            {
                Logger.EventLogError("Failed to open configuration: {0}", ex.Message);
                return new KeyValueConfigurationCollection();
            }
        }
    }
}
