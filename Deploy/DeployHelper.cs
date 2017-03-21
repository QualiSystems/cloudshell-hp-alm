using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace QS.ALM.Deploy
{
    static class DeployHelper
    {
        public const string TestShell = "TestShell";
        public const string TestTypeDllName = "TsTestType.dll";
        public const string RunnerDllName = "TsAlmRunner.dll";

        static DeployHelper()
        {
            SolutionRoot = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\.."));
        }

        public static string SolutionRoot { get; private set; }

        public static bool DeployToTestShellFolder(string filename)
        {
            return filename.ToLower() != TestTypeDllName.ToLower();
        }

        public static List<string> HarvestFiles(string flavor)
        {
            var files = new List<string>();

            HarvestDllsFromFolder(files, Path.Combine(SolutionRoot, "Binaries", flavor));
            //HarvestDllsFromFolder(files, Path.Combine(SolutionRoot, "TsCloudShellApi", flavor));
            //HarvestDllsFromFolder(files, Path.Combine(SolutionRoot, "TsTestType", flavor));
            //HarvestDllsFromFolder(files, Path.Combine(SolutionRoot, "TsAlmRunner", flavor));

            return files;
        }

        private static void HarvestDllsFromFolder(List<string> files, string folder)
        {
            files.AddRange(Directory.GetFiles(folder, "*.dll"));
        }

        public static Version GetLastDeployVersion()
        {
            var version = File.ReadAllText(LastDeployVersionFilePath);
            return Version.Parse(version);
        }

        public static void SetLastDeployVersion(Version version)
        {
            File.WriteAllText(LastDeployVersionFilePath, version.ToString());
        }

        private static string LastDeployVersionFilePath
        {
            get { return Path.Combine(SolutionRoot, "Deploy", "LastDeployVersion.txt"); }
        }
    }
}
