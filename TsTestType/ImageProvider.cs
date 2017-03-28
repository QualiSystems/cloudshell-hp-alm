using HP.ALM.QC.UI.Modules.Shared.Api;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TsCloudShellApi;
using TsTestType.DeveloperTools;

namespace TsTestType
{
    /// <summary>
    /// Gets the test type's icons for display in the user interface.
    /// </summary>
  public class ImageProvider : ITestTypeImageProvider
  {
      static ImageProvider()
      {
          // The "ImageProvider" ctor is called in a very early stage of the integration, just before the user is logged into ALM project.
          // We use this point to hook the sub folder resolver
          SubFolderResovler.Hook();

          // DON'T PUT ANY CODE HERE!
          // WHEN RUNNING INSIDE ALM ALL THE REFERENCES ARE IN A SUB FOLDER. SO TYPES FROM THESE REFERENCES CANNOT BE USED IN THIS METHOD.
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
            // WARNING: don't move this code:
            // The "SettingsFile" class is loaded from the "Quai sub folder (when running in ALM), so the "Hook" must be called first
            if (SettingsFile.DebugMode)
                Debugger.Launch();

            StartupHelper.ReportStart(TestTypeLogger.Instance, "CustomTest", Assembly.GetExecutingAssembly());

            HookDeveloperWindow.HookOnce();

            new RegisterAgent(TestTypeLogger.Instance).RegisterIfNeeded();

            return Resource.TestTypeImage;
        }
  }
}
