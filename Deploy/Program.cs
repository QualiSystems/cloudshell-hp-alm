using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QS.ALM.Deploy
{
    class Program
    {
        private const string AlmServerRoot = @"C:\ProgramData\HP\ALM\webapps\qcbin";
        static string m_SolutionRoot;

        static int Main(string[] args)
        {
            try
            {
                VerifyAlmNotRunning();

                const string flavor = @"bin\Debug";
                m_SolutionRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\.."));

                var files = new List<string>();

                AddFolder(files, (Path.Combine(m_SolutionRoot, "CloudShellApi", flavor)));
                AddFolder(files, (Path.Combine(m_SolutionRoot, "CSRemoteAgent", flavor)));
                AddFolder(files, (Path.Combine(m_SolutionRoot, "TestType4DotNet", flavor)));
                //files.Add(Path.Combine(solutionRoot, "CloudShellApi", flavor, "Newtonsoft.Json.dll"));
                //files.Add(Path.Combine(solutionRoot, "CloudShellApi", flavor, "QS.ALM.CloudShellApi.dll"));
                //files.Add(Path.Combine(solutionRoot, "CloudShellApi", flavor, "RestSharp.dll"));

                //files.Add(Path.Combine(solutionRoot, "CSRemoteAgent", flavor, "Interop.TDAPIOLELib.dll"));
                //files.Add(Path.Combine(solutionRoot, "CSRemoteAgent", flavor, "RemoteAgent.dll"));

                //files.Add(Path.Combine(solutionRoot, "TestType4DotNet", flavor, "CustomTestType.dll"));

                var missingFiles = false;

                // Verify all files exists
                foreach (var file in files)
                {
                    if (!File.Exists(file))
                    {
                        missingFiles = true;
                        Console.WriteLine("File not found: " + file);
                    }
                }

                if (missingFiles)
                    throw new Exception("Missing files");

                var cabPath = Path.Combine(m_SolutionRoot, "Cab", "Debug", "QSALM.CAB");

                if (!File.Exists(cabPath))
                    throw new Exception("File not found: " + cabPath);

                var ctsFolder = Path.Combine(AlmServerRoot, @"Extensions\CTS");
                Directory.Delete(ctsFolder, true);
                Directory.CreateDirectory(ctsFolder);

                CopyToServerAndSign(files.ToArray(), @"Extensions\CTS");
                CopyToServerAndSign(new[] { cabPath }, "Extensions");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
                return -1;
            }

            Console.WriteLine("Success.");
            Console.ReadLine();
            return 0;
        }

        private static void VerifyAlmNotRunning()
        {
            if (Process.GetProcessesByName("ALM").Any())
                throw new Exception("Please close ALM.exe");
        }

        private static void AddFolder(List<string> files, string folder)
        {
            files.AddRange(Directory.GetFiles(folder, "*.dll"));
        }

        private static void CopyToServerAndSign(string[] files, string targetFolder)
        {
            foreach (var file in files)
            {
                var targetFile = Path.Combine(AlmServerRoot, targetFolder);

                if (Path.GetExtension(file).ToLower() == ".dll")
                    targetFile = Path.Combine(targetFile, Path.GetFileNameWithoutExtension(file) + ".lld");
                else
                    targetFile = Path.Combine(targetFile, Path.GetFileName(file));

                Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
                File.Copy(file, targetFile, true);
                Sign(targetFile);
            }
        }

        private static void Sign(string file)
        {
            var signTool = @"C:\Program Files (x86)\Windows Kits\8.1\bin\x86\signtool.exe";
            
            if (!File.Exists(signTool))
                signTool = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Bin\signtool.exe";

            if (!File.Exists(signTool))
                throw new Exception("Sign tool not found: " + signTool);

            var pfxPath = Path.Combine(m_SolutionRoot, "pfx", "qs.pfx");

            if (!File.Exists(pfxPath))
                throw new Exception("Pfx not found: " + pfxPath);

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = signTool;
                // signtool sign /v /f C:\Moti\sertifecat\qs.pfx /p qualisystems C:\Moti\Extensions\CTS.cab
                process.StartInfo.Arguments = string.Format("sign /v /f \"{0}\" /p qualisystems \"{1}\"", pfxPath, file);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("Sign failed ({0}): Exit code = {1}", file, process.ExitCode));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Sign error ({0}): {1}", file, ex.Message));
            }
        }
    }
}
