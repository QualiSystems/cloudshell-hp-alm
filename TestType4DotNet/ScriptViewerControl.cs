using System;
using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;
using QS.ALM.CloudShellApi;
using Mercury.TD.Client.Ota.Api;

namespace CTSAddin
{
   /// <summary>
    /// Handles displaying test scripts from the project repository in the ALM user interface.
   /// </summary>
    /// <remarks>Implementation is optional.</remarks>
  public partial class ScriptViewerControl : UserControl, IScriptViewer
  {
    private readonly Api m_Api;
    private ITDConnection m_tdc;
    private HP.ALM.QC.OTA.Entities.Api.ITest m_CurrentTest;
    public ScriptViewerControl()
    {
        InitializeComponent();

        try
        {
            m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", null, null, AuthenticationMode.Alm, "Global");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            Enabled = false;
        }
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
      //var xxx = m_tdc.get_Fields("TP_NAME");
      //var xxx1 = m_tdc.get_Fields("PathForOurTestQuali");
    }

    /// <summary>
    /// "true" sets script to read-only.
    /// </summary>
    public bool ReadOnly
    {
      set { /*MessageBox.Show("Setting read only = " + value);*/ }
    }

    /// <summary>
    /// Saves the script.
    /// </summary>
    /// <remarks>Optional. If implemented, ALM calls SaveScript on moving to a different tab or another object.</remarks>
    public void SaveScript()
    {
      //MessageBox.Show("Saving script.");
    }

    public string TestPath {get { return TextBoxPath.Text; }}

    /// <summary>
    /// Displays the active version of the test script in the Script Viewer control.
    /// </summary>
    /// <param name="test">Output. An ALM Open Test Architecture ITest object.</param>
    public void ShowTest(HP.ALM.QC.OTA.Entities.Api.ITest test)
    {
        m_CurrentTest = test;

        TextBoxPath.Text = test["TS_USER_01"] == null ? "" : test["TS_USER_01"].ToString();// "Showing test " + test.Name + "path = '" + test.Path + "' in project " + m_tdc.ProjectName;

        //iFact.PostList(newList);      
    }

    private void ButtonBrowse_Click(object sender, System.EventArgs e)
    {
        TestShellTestsBrowserForm BrouseForm = new TestShellTestsBrowserForm(m_Api);
        string path;
        path = BrouseForm.TryShowDialog(TextBoxPath.Text);
        if (path != null)
        {
            m_CurrentTest["TS_USER_01"] = TextBoxPath.Text = path;
            string contentError;
            bool isSuccess = false;
            RewriteTestParameters(m_Api.GetTestParameter(path, out contentError, out isSuccess).Parameters);
            if(!isSuccess)
            {
                MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
            }
        }
    }

    private void ButtonRefresh_Click(object sender, System.EventArgs e)
    {
        string message = "Are you sure you would like to refresh the" + Environment.NewLine + "test parameters ?";
        string caption = "Refresh";
        MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
        DialogResult result;

        result = MessageBox.Show(message, caption, buttons);

        if (result == System.Windows.Forms.DialogResult.Yes)
        {
            string contentError;
            bool isSuccess = false;
            RewriteTestParameters(m_Api.GetTestParameter(TextBoxPath.Text, out contentError, out isSuccess).Parameters);
        }
    }

    void RewriteTestParameters(APITestParameterInfo[] arrParameters)
    {
        if(m_CurrentTest == null)
        {
            return;
        }
        var paramFactory = m_CurrentTest.ParameterFactory as Mercury.TD.Client.Ota.Core.Factory<HP.ALM.QC.OTA.Entities.Api.ITestParameter, HP.ALM.QC.OTA.Entities.Api.ITest>;
        var baseFactory = paramFactory.LegacyFactory as TDAPIOLELib.IBaseFactory;

        List baseList = baseFactory.NewList("");
        if (baseList != null)
        {
            foreach (var element in baseList)
            {
                baseFactory.RemoveItem(element);
            }
        }
        if (arrParameters != null)
        {
            for (int i = 0; i < arrParameters.Length; ++i)
            {
                HP.ALM.QC.OTA.Entities.Api.ITestParameter newEnt = paramFactory.NewEntity();
                newEnt.Name = arrParameters[i].Name;
                newEnt.Description = arrParameters[i].Description;
                newEnt.Post();                
            }
        }
    }
  }
}
