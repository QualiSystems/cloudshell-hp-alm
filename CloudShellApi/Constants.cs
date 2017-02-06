using System;
using System.IO;

namespace QS.ALM.CloudShellApi
{
    public class Constants
    {
        public static string TempFolder { get { return Environment.ExpandEnvironmentVariables(@"%temp%\ALMCS"); } }
        public static string LogsFolder { get { return Path.Combine(TempFolder, "Logs"); } }
    }
}
