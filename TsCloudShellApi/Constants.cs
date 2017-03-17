using System;
using System.IO;

namespace TsCloudShellApi
{
    public class Constants
    {
        public static string TempFolder { get { return Environment.ExpandEnvironmentVariables(@"%temp%\ALMCS"); } }
        public static string LogsFolder { get { return Path.Combine(TempFolder, "Logs"); } }
    }
}
