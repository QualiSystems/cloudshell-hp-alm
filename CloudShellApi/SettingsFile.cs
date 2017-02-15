using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace QS.ALM.CloudShellApi
{
    public static class SettingsFile
    {
        public static string Verbosity
        {
            get { return Read("Verbosity", "Info"); }
        }

        private static string Read(string key, string defaultValue)
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
                var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = Path.Combine(Constants.TempFolder, "settings.config") };
                var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
                return config.AppSettings.Settings; // this is empty if file not exists
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("AlmCloudShell", "Failed to open configuration: " + ex.Message, EventLogEntryType.Error);
                return new KeyValueConfigurationCollection();
            }
        }
    }
}
