using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QS.ALM.Deploy
{
    static class DeployClient
    {
        public static void Deploy(List<string> files)
        {
            var userProfile = Environment.ExpandEnvironmentVariables("%USERPROFILE%");
            var destRoot = Path.Combine(userProfile, "AppData\\Local\\HP\\ALM-Client\\localhost");

            if (!Directory.Exists(destRoot))
                throw new Exception("Folder not found: " + destRoot);

            var processes = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process process in processes)
            {
                if (new[] { "ALM-Client".ToLower(), "iexplore".ToLower(), "dllhost".ToLower() }.Contains(process.ProcessName.ToLower()))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch {}
                }
            }

            System.Threading.Thread.Sleep(1000);

            foreach (var source in files)
            {
                var filename = Path.GetFileName(source);
                var subFolder = DeployHelper.DeployToTestShellFolder(filename) ? DeployHelper.TestShell + "\\" : "";
                var target = Path.Combine(destRoot, subFolder + filename);
                File.Copy(source, target, true);
            }
        }
    }
}
