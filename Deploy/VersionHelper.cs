﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace QS.ALM.Deploy
{
    static class VersionHelper
    {
        private const string VersionCsFileneme = "AlmCloudShellVersion.cs";

        public static Version VerifyVersion(List<string> files)
        {
            var qualiFiles = new List<string>();

            foreach (var file in files)
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(file);

                if (versionInfo.CompanyName == "Quali")
                    qualiFiles.Add(file);
            }
            
            var qualiVersions = new HashSet<Version>();

            foreach (var file in qualiFiles)
                qualiVersions.Add(VerifyVersionIncremented(file));

            if (!qualiVersions.Any())
                throw new Exception("None of the files for deploy was created by Quali");
            
            if (qualiVersions.Count > 1)
                throw new Exception("Quali files must have the same version. Found: " + string.Join(", ", qualiVersions));

            return qualiVersions.First();
        }

        private static Version VerifyVersionIncremented(string file)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(file);
            var fileVersion = Version.Parse(versionInfo.FileVersion);
            var lastDeployedVersion = GetLastDeployVersion();

            var versionFile = Path.Combine(DeployHelper.SolutionRoot, VersionCsFileneme);
            var regex = new Regex("AssemblyFileVersion\\(.*\\)");
            var text = File.ReadAllText(versionFile);
            string match = regex.Match(text).Value;
            var codeVersion = match.Substring(match.IndexOf('\"')+1, match.LastIndexOf('\"') - match.IndexOf('\"')-1);

            if (fileVersion.ToString() != codeVersion)
                throw new Exception("You forgot to build the solution (version in .dll doesn't match version in .cs).\nPLEASE BUILD THE SOLUTION.");

            if (fileVersion <= lastDeployedVersion)
            {
                Console.WriteLine("Version was not incremented.");
                Console.WriteLine();
                Console.Write("Do you want to increment the version? [y/n] ");
                var answer = Console.ReadLine();

                Console.WriteLine();

                if (answer.Trim().ToLower() == "y")
                {
                    IncrementVersionInQualiAssemblies();
                    Console.WriteLine("Version incremented.");
                    Console.WriteLine("PLEASE REBUILD THE SOLUTION and run Deploy again.");
                }

                throw new Exception(string.Empty);
            }

            return fileVersion;
        }

        private static void IncrementVersionInQualiAssemblies()
        {
            var versionFile = Path.Combine(DeployHelper.SolutionRoot, VersionCsFileneme);
            var reg2 = new Regex("AssemblyFileVersion\\(.*\\)");
            var text2 = File.ReadAllText(versionFile);
            var match2 = reg2.Match(text2);

            var newVersion = IncrementVersion(GetLastDeployVersion()).ToString();
            var newText2 = string.Format("AssemblyFileVersion(\"{0}\")", newVersion);

            if (match2.Success)
            {
                var fixedText2 = text2.Replace(match2.Value, newText2);
                File.WriteAllText(versionFile, fixedText2);
            }
            else
            {
                throw new Exception("Could not find 'AssemblyFileVersion' string in file: " + versionFile);
            }
        }

        public static void IncrementLastDeployVersion()
        {
            var version = IncrementVersion(GetLastDeployVersion());
            File.WriteAllText(LastDeployVersionFilePath, version.ToString());
        }
        
        private static Version IncrementVersion(Version version)
        {
            version = new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
            return version;
        }


        public static Version GetLastDeployVersion()
        {
            var version = File.ReadAllText(LastDeployVersionFilePath);
            return Version.Parse(version);
        }

        private static string LastDeployVersionFilePath
        {
            get { return Path.Combine(DeployHelper.SolutionRoot, "Deploy", "LastDeployVersion.txt"); }
        }
    }
}
