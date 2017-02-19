using System;
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

    public string TestPath {get { return TextBoxPath.Text; }}

    /// <summary>
    /// Displays the active version of the test script in the Script Viewer control.
    /// </summary>
    /// <param name="test">Output. An ALM Open Test Architecture ITest object.</param>
    public void ShowTest(HP.ALM.QC.OTA.Entities.Api.ITest test)
    {
      TextBoxPath.Text = "Showing test " + test.Name + " in project " + m_tdc.ProjectName;
    }

    private void ButtonBrowse_Click(object sender, System.EventArgs e)
    {
        TestShellTestsBrowserForm BrouseForm = new TestShellTestsBrowserForm("http://192.168.42.35:9000", "admin", "admin", "Global");
        string path;
        path = BrouseForm.TryShowDialog(TextBoxPath.Text);
        if(path != null)
        {
            TextBoxPath.Text = path;
        }
    }

    private void ButtonRefresh_Click(object sender, System.EventArgs e)
    {
        string message = "Are you sure you would like to refresh the" + Environment.NewLine + "test parameters ?";
        string caption = "Refresh";
        MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
        DialogResult result;

        

        // Displays the MessageBox.

        result = MessageBox.Show(message, caption, buttons);

        if (result == System.Windows.Forms.DialogResult.Yes)
        {

        }
    }
  }
}
