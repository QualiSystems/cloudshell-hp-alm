using System;
using System.IO;

namespace TsCloudShellApi
{
    public class Constants
    {
        public static string TempFolder { get { return CreateFolder(Environment.ExpandEnvironmentVariables(@"%temp%\ALMTestShell")); } }
        public static string LogsFolder { get { return CreateFolder(Path.Combine(TempFolder, "Logs")); } }

        private static string CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
            return path;
        }
    }
}
