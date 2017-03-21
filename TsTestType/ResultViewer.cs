using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;
using System.Diagnostics;

namespace TsTestType
{
  /// <summary>
  /// Handles display of test run results in the ALM user interface. This component is optional.
  /// </summary>
  public partial class ResultViewer : UserControl, IResultViewer
  {
    public ResultViewer()
    {
      InitializeComponent();
    }
    
    /// <summary>
    /// Initializes the result viewer.
    /// </summary>
    /// <remarks>Output. With the TDConnection reference, you can access all ALM Open Test Architecture components.
    /// <br />For more information, see the ALM Open Test Architecture Reference.
    /// </remarks>
    /// <param name="connection">Output. The output connection is connected to the server and authorized for the project.</param>
    public void InitViewer(Mercury.TD.Client.Ota.Api.IConnection connection)
    {
    }

 
    /// <summary>
    /// Displays the run result of the applicable version of the specified test instance.
    /// </summary>
    /// <remarks>For more information, see the ALM Open Test Architecture Reference.</remarks>
    /// <param name="run">Output.
    /// </param>
    public void ShowResult(HP.ALM.QC.OTA.Entities.Api.IRun run)
    {
        linkLabelReportResult.Enabled = run != null;

        if (run == null)
        {
            linkLabelReportResult.Text = "No run result";
            return;
        }

        var legacyBaseFieldProvider = run as Mercury.TD.Client.Ota.Entities.Api.ILegacyBaseFieldProvider;
        var legacyRun = (IRun2)legacyBaseFieldProvider.LegacyBaseField;
        var stepFact = (StepFactory)legacyRun.StepFactory;
        var stepList = stepFact.NewList("");
        if (stepList.Count == 0)
        {
            linkLabelReportResult.Enabled = false;
            linkLabelReportResult.Text = "No run result";
        }

        foreach (Step step in stepList)
        {
            linkLabelReportResult.Text = step.Name;
        }
    }

    private void linkLabelReportResult_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(linkLabelReportResult.Text);
    }
  }
}
