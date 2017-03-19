using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TsCloudShellApi;

namespace TsTestType
{
    static class RegisterAgent
    {
        public static void Register()
        {
            var assemblyPath = Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunner.dll");

            var process = new Process();

            try
            {
                process.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegSvcs.exe";
                process.StartInfo.Arguments = assemblyPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();
                process.WaitForExit();

                var standardOutput = process.StandardOutput.ReadToEnd();
                var standardError = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("Exit code: {0}\n\n{1}", process.ExitCode, standardOutput + Environment.NewLine + standardError));
            }
            catch (Exception ex)
            {
                Logger.Error("RegisterAgent Failed: " + ex);

                var message = ex.Message;

                // System.Transactions.TransactionException - The Transaction Manager is not available.
                if (ex.Message.Contains("System.Transactions.TransactionException"))
                    message = "The 'Distributed Transaction Coordinator' service is disabled. Please start this service under Windows Services.";

                MessageBox.Show(string.Format("RemoteAgent registration failed.\n\n{0}\n\nCommand was:\n{1} {2}", message, process.StartInfo.FileName, process.StartInfo.Arguments));
            }
        }
    }
}
