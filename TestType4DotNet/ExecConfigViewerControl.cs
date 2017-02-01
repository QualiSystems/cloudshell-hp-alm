using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;

namespace CTSAddin
{
    /// <summary>
    /// Configures the testing tool for execution of a single test instance. This component is optional.
    /// </summary>
  public partial class ExecConfigViewerControl : UserControl, IExecutionConfigurationViewer
  {
    private ITDConnection m_tdc;

    public ExecConfigViewerControl()
    {
      InitializeComponent();
    }

    /// <summary>
    /// The test instance configuration string in the internal format of the test type.
    /// </summary>
    public string ExecutionConfiguration
    {
      get
      {
        return "Example execution configuration";
      }
      set
      {
        MessageBox.Show("Setting execution configuration = " + value);
      }
    }

    /// <summary>
    /// Initializes the viewer.
    /// </summary>
    /// <remarks>
    /// With the TDConnection reference, you can access all ALM Open Test Architecture components.
    /// For more information, see the ALM Open Test Architecture Reference.
    /// </remarks>
    /// <param name="connection">Output. The connection is connected to the server and authorized for the project.
    /// </param>
    public void InitViewer(Mercury.TD.Client.Ota.Api.IConnection connection)
    {
      m_tdc = (ITDConnection) ((connection as Mercury.TD.Client.Ota.Core.ITDConnectedObject).TDConnection);
    }

    /// <summary>
    /// Displays the configuration of the test instance in the control.
    /// </summary>
    /// <param name="testInstance">Output.
    /// For more information, see the ALM Open Test Architecture Reference</param>
    public void ShowExecutionConfiguration(HP.ALM.QC.OTA.Entities.Api.ITestInstance testInstance)
    {
      m_textBox.Text = "Showing test instance" + testInstance.Name + " in project " + m_tdc.ProjectName;
    }
  }
}
