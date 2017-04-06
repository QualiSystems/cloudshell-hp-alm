using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TsCloudShellApi
{
    public static class StartupHelper
    {
        /// <summary>
        /// Write to log that component started
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="componentName">name of component (Agent, CustomTest)</param>
        /// <param name="assembly">Assembly of component to get the version</param>
        public static void ReportStart(Logger logger, string componentName, Assembly assembly)
        {
            var assemblyPath = assembly.Location;
            var version = FileVersionInfo.GetVersionInfo(assemblyPath).FileVersion;
            var runningFolder = Path.GetDirectoryName(assemblyPath);
            logger.Info("{0} started: {1}", componentName, runningFolder);

            UpdateDeployments(componentName, runningFolder, version);
        }

        /// <summary>
        /// The "deployments.txt" file helps tracking where is the ALM-Client running from
        /// </summary>
        private static void UpdateDeployments(string componentName, string runningFolder, string version)
        {
            var deploymentsFile = Path.Combine(Constants.TempFolder, "deployments.txt");
            var text = File.Exists(deploymentsFile) ? File.ReadAllText(deploymentsFile) : String.Empty;

            // The file contain lines, each line is a different deployment in this format:
            // key: value
            var key = string.Format("[{0}] {1}", componentName, runningFolder);
            var keyAndPrefix = key + ": ";
            var value = version;
            var line = keyAndPrefix + value;

            // check if deployment line exists
            if (!text.Contains(line))
            {
                // check if key exists
                if (text.Contains(keyAndPrefix))
                {
                    // update the value of existing deployment line
                    var start = text.IndexOf(keyAndPrefix);
                    var end = text.IndexOf(Environment.NewLine, start);
                    var oldLine = text.Substring(start, end - start);
                    text = text.Replace(oldLine, line);
                }
                else
                {
                    // add new deployment line
                    text += line + Environment.NewLine;
                }

                File.WriteAllText(deploymentsFile, text);
            }
        }
    }
}
