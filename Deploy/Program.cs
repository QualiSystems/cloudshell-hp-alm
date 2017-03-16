using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QS.ALM.Deploy
{
    class Program
    {
        private const string TestTypeDllName = "TsTestType.dll";
        private const string RunnerDllName = "TsAlmRunner.dll";
        private const string CabAndIniName = "TestShell";
        
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

                CreateIniFile(files, Path.Combine(m_SolutionRoot, "Cab", flavor));
                CreateCabFile(Path.Combine(m_SolutionRoot, "Cab", flavor));

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

                var cabPath = Path.Combine(m_SolutionRoot, "Cab", flavor, CabAndIniName + ".CAB");

                if (!File.Exists(cabPath))
                    throw new Exception("File not found: " + cabPath);

                var ctsFolder = Path.Combine(AlmServerRoot, @"Extensions\TestShell");
                if (Directory.Exists(ctsFolder))
                {
                    Directory.Delete(ctsFolder, true);
                }
                Directory.CreateDirectory(ctsFolder);

                CopyToServerAndSign(files.ToArray(), @"Extensions\TestShell");
                CopyToServerAndSign(new[] { cabPath }, "Extensions");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.ReadLine();
                return -1;
            }

            Console.WriteLine("Success.");
            Console.WriteLine("Outputs: " + AlmServerRoot);
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

            RunExecOperation(signTool,
               string.Format("sign /v /f \"{0}\" /p qualisystems \"{1}\"", pfxPath, file),
               file, "Sign");
        }

        private static void CreateIniFile(List<string> files, string path)
        {
            if(files == null || files.Count() == 0)
                throw new Exception("List files is empty");
            HashSet<string> hashFiles = new HashSet<string>();
            foreach(string fileName in files)
            {
                hashFiles.Add(Path.GetFileName(fileName));
            }
            string contentIni = "";
            int index = 0;
            foreach(string fileName in hashFiles)
            {
                contentIni += FileNameToIni(fileName, ++index);
            }
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }            
            path = Path.Combine(path, CabAndIniName + ".ini");
            File.WriteAllText(path, contentIni);
            if(!File.Exists(path))
                throw new Exception(string.Format("Creating {0} file error", path));
        }

        private static void CreateCabFile(string path)
        {
            string fullFileName = Path.Combine(path, CabAndIniName + ".cab");
            RunExecOperation("makecab", 
                string.Format(" {0} {1}", Path.Combine(path, CabAndIniName + ".ini"), fullFileName),
                fullFileName,  "Create Cab File");

            if (!File.Exists(fullFileName))
                throw new Exception(string.Format("Creating {0} file error", fullFileName));
        }


        private static void RunExecOperation(string nameRunFile, string argument, string file, string nameOperationForExeption)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = nameRunFile;
                // signtool sign /v /f C:\Moti\sertifecat\qs.pfx /p qualisystems C:\Moti\Extensions\CTS.cab
                process.StartInfo.Arguments = argument;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("{0} failed ({1}): Exit code = {2}", nameOperationForExeption, file, process.ExitCode));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} error ({1}): {2}", nameOperationForExeption, file, ex.Message));
            }
        }

        private static string FileNameToIni(string filename, int index)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename).Substring(1);
            char[] arr = extension.ToCharArray();
            Array.Reverse(arr);
            string extensionReversed = new string (arr);
            string contentIni = "[File_" + index.ToString("D" + 4) + ']' + Environment.NewLine +
                "URLName=%URL%/Extensions/TestShell/" + name + '.' + extensionReversed + Environment.NewLine +
                "ShortName=" + QuallySubFolder(filename) + name + '.' + extension + Environment.NewLine +
                "Description=" + name + Environment.NewLine +
                DotNetRegAsm(filename) + Environment.NewLine;
            return contentIni;
        }

        private static string QuallySubFolder(string filename)
        {
            return filename.ToLower() == TestTypeDllName.ToLower() ? "" : "Quali\\";
        }

        /// <summary>
        /// see options here:
        /// http://helpfiles.intactcloud.com/ALM/11.52/hp_man_ALM11.52_Custom_TestType_Dev_zip/CustomTestTypeNET/Content/cttIniFileParams.htm
        /// </summary>
        private static string DotNetRegAsm(string filename)
        {
            // we register the interop files so we can open a connection into ALM
            return filename.Split('.').First().ToLower() == "interop" ? "DotNet=Y" + Environment.NewLine: "";
            //return filename.ToLower() == RunnerDllName.ToLower() ? "DotNet=Y" + Environment.NewLine : "";
        }
    }
}
