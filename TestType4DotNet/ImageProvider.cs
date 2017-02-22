using HP.ALM.QC.UI.Modules.Shared.Api;
using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
namespace CTSAddin
{
    /// <summary>
    /// Gets the test type's icons for display in the user interface.
    /// </summary>
  public class ImageProvider : ITestTypeImageProvider
  {
      static ImageProvider()
      {
          AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
      }

      private static string QualiSubFolder
      {
          get { return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Quali"); }
      }

      static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
      {
          var filename = args.Name.Split(',').First().Trim();

          if (filename.ToLower().EndsWith(".resources"))
              return null;

          if (!filename.ToLower().EndsWith(".dll"))
              filename += ".dll";

          var path = Path.Combine(QualiSubFolder, filename);

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

    /// <summary>
    /// Returns an icon to display near the tests, test instances, and runs of the test type.
    /// </summary>
    /// <remarks>The icon size is 16x16 pixels.</remarks>
    /// <param name="status">
    /// <para>The test status that the bitmap represents. One of:</para>
    /// <ul>
    /// <li>0 - General status.</li>
    /// <li>1 - For a test containing design steps.</li>
    /// </ul>
    /// </param>
    /// <returns></returns>
      public System.Drawing.Image TestTypeIcon(int status)
      {
          var path = Path.Combine(QualiSubFolder, "RemoteAgent.dll");
          RegAgent(path);
          return Resource.TestTypeImage;
      }

      private static void RegAgent(string assemblyPath)
      {
          var process = new Process();

          try
          {
              process.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegSvcs.exe";
              process.StartInfo.Arguments = assemblyPath;
              process.StartInfo.UseShellExecute = false;
              process.StartInfo.CreateNoWindow = true;
              process.StartInfo.RedirectStandardOutput = true;
              process.StartInfo.RedirectStandardError = true;
              process.Start();
              process.WaitForExit();

              var standardOutput = process.StandardOutput.ReadToEnd();
              var standardError = process.StandardError.ReadToEnd();

              if (process.ExitCode != 0)
                  throw new Exception(string.Format("Exit code: {0}\n\n{1}", process.ExitCode, standardOutput + Environment.NewLine + standardError));
          }
          catch (Exception ex)
          {
              MessageBox.Show(string.Format("RemoteAgent registration failed.\n\n{0}\n\nCommand was:\n{1} {2}", ex.Message, process.StartInfo.FileName, process.StartInfo.Arguments));
          }
      }
  }
}
