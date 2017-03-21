using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QS.ALM.Deploy
{
    static class DeployServer
    {
        private const string AlmServerRoot = @"C:\ProgramData\HP\ALM\webapps\qcbin";

        public static void Deploy(List<string> files, string flavor)
        {
            VerifyAlmNotRunning();

            var cabFolder = Path.Combine(DeployHelper.SolutionRoot, "Binaries", flavor, "Cab");
            Directory.CreateDirectory(cabFolder);
            var cabPath = Path.Combine(cabFolder, DeployHelper.TestShell + ".cab");
            var iniPath = Path.Combine(cabFolder, DeployHelper.TestShell + ".ini");
            CreateIniFile(files, iniPath);
            CreateCabFile(cabPath, iniPath);

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

            var testShellFolder = Path.Combine(AlmServerRoot, @"Extensions\" + DeployHelper.TestShell);
            
            if (Directory.Exists(testShellFolder))
                Directory.Delete(testShellFolder, true);
            
            Directory.CreateDirectory(testShellFolder);

            CopyToServerAndSign(files.ToArray(), @"Extensions\" + DeployHelper.TestShell);
            CopyToServerAndSign(new[] { cabPath }, "Extensions");

            Console.WriteLine("Outputs: " + AlmServerRoot);
        }

        private static void VerifyAlmNotRunning()
        {
            if (Process.GetProcessesByName("ALM").Any())
                throw new Exception("Please close ALM.exe");
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

            var pfxPath = Path.Combine(DeployHelper.SolutionRoot, "pfx", "qs.pfx");

            if (!File.Exists(pfxPath))
                throw new Exception("Pfx not found: " + pfxPath);

            RunExecOperation(signTool, string.Format("sign /v /f \"{0}\" /p qualisystems \"{1}\"", pfxPath, file), "Sign");
        }

        private static void CreateIniFile(List<string> files, string iniPath)
        {
            if(files == null || files.Count == 0)
                throw new Exception("List files is empty");

            var contentIni = "";
            var index = 0;

            foreach (var file in files)
                contentIni += AddFileToIni(file, ++index);

            File.WriteAllText(iniPath, contentIni);
        }

        private static void CreateCabFile(string cabPath, string iniPath)
        {            
            RunExecOperation("makecab", string.Format(" {0} {1}", iniPath, cabPath), "Create Cab File");

            if (!File.Exists(cabPath))
                throw new Exception(string.Format("Creating {0} file error", cabPath));
        }


        private static void RunExecOperation(string nameRunFile, string arguments, string nameOperationForExeption)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = nameRunFile;
                // signtool sign /v /f C:\Moti\sertifecat\qs.pfx /p qualisystems C:\Moti\Extensions\CTS.cab
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("{0} failed ({1}): Exit code = {2}", nameOperationForExeption, nameRunFile, process.ExitCode));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} error ({1}): {2}", nameOperationForExeption, nameRunFile, ex.Message));
            }
        }

        private static string AddFileToIni(string path, int index)
        {
            var filename = Path.GetFileName(path);
            var name = Path.GetFileNameWithoutExtension(filename);
            var extension = Path.GetExtension(filename).Substring(1);
            var arr = extension.ToCharArray();
            Array.Reverse(arr);
            var extensionReversed = new string (arr);
            var subFolder = DeployHelper.DeployToTestShellFolder(filename) ? DeployHelper.TestShell + "\\" : "";

            var versionString = string.Empty;

            if (extension.ToLower() == "dll" || extension.ToLower() == "exe")
            {
                var version = FileVersionInfo.GetVersionInfo(path).FileVersion;
                versionString = "version=" + version + Environment.NewLine;
            }

            // add version here for every file you want to update
            var contentIni =
                "[File_" + index.ToString("D" + 4) + ']' + Environment.NewLine +
                "URLName=%URL%/Extensions/TestShell/" + name + '.' + extensionReversed + Environment.NewLine +
                "ShortName=" + subFolder + name + '.' + extension + Environment.NewLine +
                "Description=" + name + Environment.NewLine +
                versionString +
                DotNetRegAsm(filename) + Environment.NewLine;

            return contentIni;
        }
        
        /// <summary>
        /// see options here:
        /// http://helpfiles.intactcloud.com/ALM/11.52/hp_man_ALM11.52_Custom_TestType_Dev_zip/CustomTestTypeNET/Content/cttIniFileParams.htm
        /// </summary>
        private static string DotNetRegAsm(string filename)
        {
            return string.Empty;
            // we register the interop files so we can open a connection into ALM
            //return filename.Split('.').First().ToLower() == "interop" ? "DotNet=Y" + Environment.NewLine: "";
            //return filename.ToLower() == RunnerDllName.ToLower() ? "DotNet=Y" + Environment.NewLine : "";
        }
    }
}
