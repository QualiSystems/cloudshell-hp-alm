using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TsTestType
{
    /// <summary>
    /// This class helps ALM to load the required assemblies for this test type from a sub-folder under the client folder.
    /// e.g.: 
    /// The client is downloaded into this folder: 
    /// C:\Users\alex.a\AppData\Local\HP\ALM-Client\192
    /// And the assemblies used by this test type are located at:
    /// C:\Users\alex.a\AppData\Local\HP\ALM-Client\192\Quali
    /// </summary>
    static class SubFolderResovler
    {
        public static string TestShellSubFolder
        {
            get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestShell"); }
        }

        public static void Hook()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
        }

        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var filename = args.Name.Split(',').First().Trim();

            if (filename.ToLower().EndsWith(".resources"))
                return null;

            if (!filename.ToLower().EndsWith(".dll"))
                filename += ".dll";

            var path = Path.Combine(TestShellSubFolder, filename);

            if (File.Exists(path))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(path);
                    return assembly;
                }
                catch (Exception ex)
                {

                    WriteToEventLog(string.Format("Error loading ({0}): {1}" + path, ex), EventLogEntryType.Error);
                    return null;
                }
            }

            WriteToEventLog("Missing: " + args.Name, EventLogEntryType.Information);
            return null;
        }

        private static void WriteToEventLog(string message, EventLogEntryType type)
        {
            EventLog.WriteEntry("QS.ALM", message, type);
        }
    }
}
