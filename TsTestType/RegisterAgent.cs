using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using TsCloudShellApi;

namespace TsTestType
{
    public static class RegisterAgent
    {
        private enum RegisterResult
        {
            Success,
            Error,
            Canceled
        }

        //private static bool HasAdminPrivileges()
        //{
        //    bool isAdmin;
        //    try
        //    {
        //        WindowsIdentity user = WindowsIdentity.GetCurrent();
        //        WindowsPrincipal principal = new WindowsPrincipal(user);
        //        isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        isAdmin = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        isAdmin = false;
        //    }
        //    return isAdmin;
        //}

        private static string DllPath
        {
            get { return Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunner.dll"); }
        }

        public static void RegisterIfNeeded()
        {
            var tlbPath = Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunner.tlb");

            if (File.Exists(tlbPath))
                return;

            RegisterInLoop();
        }

        private static void RegisterInLoop()
        {
            while (true)
            {
                string error;

                var result = Register(out error);

                switch (result)
                {
                    case RegisterResult.Success:
                        return;
                    case RegisterResult.Error:
                        if (ShowErrorForm("Registeration Error", string.Format("There was an error during {0} agent registration:", Config.TestShell), error))
                            return;
                        break;
                    case RegisterResult.Canceled:
                        if (ShowErrorForm("Registeration Canceled", "User canceled registration.", string.Format("You must complete {0} agent registration in order to enable {0} integration.", Config.TestShell)))
                            return;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static bool ShowErrorForm(string title, string explain, string details)
        {
            var form = new RegisterErrorForm(Config.TestShell + " " + title, explain, details);
            var result = form.ShowDialog();

            // Return true to abort register loop
            if (result == DialogResult.Abort)
                return true;

            return false;
        }

        private static RegisterResult Register(out string error)
        {
            if (!File.Exists(DllPath))
            {
                error = "File not found: " + DllPath;
                return RegisterResult.Error;
            }

            try
            {
                var processInfo = new ProcessStartInfo("cmd");

                const string RegSvcsExe = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegSvcs.exe";
                var outputFile = Path.Combine(SubFolderResovler.TestShellSubFolder, "TsAlmRunnerReg.txt");
                processInfo.Arguments = string.Format(@"/c {0} {1} > {2}", RegSvcsExe, DllPath, outputFile);
                processInfo.Verb = "runas"; // require administrator
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;


                using (var process = Process.Start(processInfo))
                {
                    process.WaitForExit();
                    error = GetRegisterError(process, outputFile);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("RegisterAgent: " + ex);

                if (UserCanceldUacDialog(ex))
                {
                    error = null;
                    return RegisterResult.Canceled;
                }

                error = ex.Message;
            }

            return error == null ? RegisterResult.Success : RegisterResult.Error;
        }

        private static bool UserCanceldUacDialog( Exception exception)
        {
            if (exception.Message.ToUpper() == "THE OPERATION WAS CANCELED BY THE USER")
                return true;
            return false;
        }

        private static string GetRegisterError(Process process, string outputFile)
        {
            if (process.ExitCode != 0 && File.Exists(outputFile))
                return File.ReadAllText(outputFile);

            return null;
        }
    }
}
