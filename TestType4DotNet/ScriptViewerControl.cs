using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;

namespace CTSAddin
{
   /// <summary>
    /// Handles displaying test scripts from the project repository in the ALM user interface.
   /// </summary>
    /// <remarks>Implementation is optional.</remarks>
  public partial class ScriptViewerControl : UserControl, IScriptViewer
  {
    private ITDConnection m_tdc;
    public ScriptViewerControl()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes the script viewer.
    /// </summary>
    /// <remarks>With the TDConnection reference, you can access all ALM Open Test Architecture components.
    /// <br />For more information, see the ALM Open Test Architecture Reference.</remarks>
    /// <param name="connection">Output. The connection is connected to the server and authorized for the project.</param>
    public void InitViewer(Mercury.TD.Client.Ota.Api.IConnection connection)
    {
      m_tdc = (connection as Mercury.TD.Client.Ota.Core.ITDConnectedObject).TDConnection as ITDConnection;
    }

    /// <summary>
    /// "true" sets script to read-only.
    /// </summary>
    public bool ReadOnly
    {
      set { MessageBox.Show("Setting read only = " + value); }
    }

    /// <summary>
    /// Saves the script.
    /// </summary>
    /// <remarks>Optional. If implemented, ALM calls SaveScript on moving to a different tab or another object.</remarks>
    public void SaveScript()
    {
      MessageBox.Show("Saving script.");
    }

    /// <summary>
    /// Displays the active version of the test script in the Script Viewer control.
    /// </summary>
    /// <param name="test">Output. An ALM Open Test Architecture ITest object.</param>
    public void ShowTest(HP.ALM.QC.OTA.Entities.Api.ITest test)
    {
      m_textBox.Text = "Showing test " + test.Name + " in project " + m_tdc.ProjectName;
    }
  }
}
