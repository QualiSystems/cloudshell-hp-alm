using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;
using System.Diagnostics;

namespace CTSAddin
{
  /// <summary>
  /// Handles display of test run results in the ALM user interface. This component is optional.
  /// </summary>
  public partial class ResultViewerControl : UserControl, IResultViewer
  {
    private ITDConnection m_tdc;
    public ResultViewerControl()
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
      m_tdc = (connection as Mercury.TD.Client.Ota.Core.ITDConnectedObject).TDConnection as ITDConnection;
    }

 
    /// <summary>
    /// Displays the run result of the applicable version of the specified test instance.
    /// </summary>
    /// <remarks>For more information, see the ALM Open Test Architecture Reference.</remarks>
    /// <param name="run">Output.
    /// </param>
    public void ShowResult(HP.ALM.QC.OTA.Entities.Api.IRun run)
    {
        if (run == null)
        {
            linkLabelReportResult.Text = "no run result";
            linkLabelReportResult.Enabled = false;
            return;
        }
        IRun2 legacyRun = (IRun2)((run as Mercury.TD.Client.Ota.Entities.Api.ILegacyBaseFieldProvider).LegacyBaseField);
        StepFactory stepFact = (StepFactory)legacyRun.StepFactory;
        List stepList = stepFact.NewList("");
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
