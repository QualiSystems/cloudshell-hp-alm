using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TsCloudShellApi;
using System.Security.Principal;

namespace TsTestType
{
    static class RegisterAgent
    {
        private static bool HasAdminPrivileges()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (UnauthorizedAccessException ex)
            {
                isAdmin = false;
            }
            catch (Exception ex)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
        public static void Register()
        {
            string output = null;
            var assemblyPath = Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunner.dll");
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd");
                processInfo.Arguments = @"/c C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegSvcs.exe " + assemblyPath + " > " + Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunnerReg.txt");
                processInfo.Verb = "runas";
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // Starts the process
                using (Process process = Process.Start(processInfo))
                {
                    // Waits for the process to exit must come *after* StandardOutput is "empty"
                    // so that we don't deadlock because the intermediate kernel pipe is full.
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                // manage errors
                throw new Exception(ex.Message);
            }
            finally
            {
                if (output != null)
                {
                    // Process your output
                }
            }
        }
    }
}
