﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace QS.ALM.Deploy
{
    static class DeployServer
    {
        private const string AlmServerRoot = @"C:\ProgramData\HP\ALM\webapps\qcbin";

        public static void Deploy(List<string> files, string flavor)
        {
            var version = VersionHelper.VerifyVersion(files);
            VerifyAlmNotRunning();

            var cabPath = CreateCabFile(files, flavor, version);

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

            CopyTestShellTestTypeXml();

            VersionHelper.IncrementLastDeployVersion();

            Console.WriteLine("Outputs can be found under: " + AlmServerRoot);
        }

        private static void CopyTestShellTestTypeXml()
        {
            var source = Path.Combine(DeployHelper.SolutionRoot, @"Deploy\TestShellTestType.xml");
            var target = Path.Combine(AlmServerRoot, @"CustomTestTypes\TestShellTestType.xml");
            Directory.CreateDirectory(Path.GetDirectoryName(target));
            File.Copy(source, target);
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

        private static string CreateCabFile(List<string> files, string flavor, Version version)
        {
            var cabFolder = Path.Combine(DeployHelper.SolutionRoot, "Binaries", flavor, "Cab");
            Directory.CreateDirectory(cabFolder);

            var cabPath = Path.Combine(cabFolder, DeployHelper.TestShell + ".cab");
            var iniPath = Path.Combine(cabFolder, DeployHelper.TestShell + ".ini");
            var versionFilePath = Path.Combine(cabFolder, version + ".txt");
            var ddfPath = Path.Combine(cabFolder, "cab.ddf");

            CreateIniFile(files, iniPath);
            CreateVersionFile(versionFilePath, version);
            
            CreateCabDdfFile(ddfPath, cabFolder, iniPath, versionFilePath);

            if (File.Exists(cabPath))
                File.Delete(cabPath);

            RunExecOperation("makecab", string.Format("/F \"{0}\"", ddfPath), "Create Cab File");

            if (!File.Exists(cabPath))
                throw new Exception(string.Format("Creating {0} file error", cabPath));

            return cabPath;
        }

        private static void CreateVersionFile(string versionFilePath, Version version)
        {
            var text =
                "Version: " + version + Environment.NewLine +
                "Create Date: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            File.WriteAllText(versionFilePath, text);
        }

        private static void CreateCabDdfFile(string ddfPath, string cabFolder, string iniPath, string versionFilePath)
        {
            var ddfTemplatePath = Path.Combine(DeployHelper.SolutionRoot, @"Deploy\CabTemplate.ddf");
            var ddfText = File.ReadAllText(ddfTemplatePath);

            ddfText += 
                Environment.NewLine +
                ".set DiskDirectory1=" + "\"" + cabFolder + "\"" + Environment.NewLine + 
                "\"" + iniPath + "\"" + Environment.NewLine + 
                "\"" + versionFilePath + "\"";

            File.WriteAllText(ddfPath, ddfText);
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
