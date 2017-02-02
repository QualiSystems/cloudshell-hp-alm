﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;

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
        m_textBox.Text = "no run result";
        return;
      }
      m_textBox.Text = string.Format("showing run ({0}) in project {1}", run.Id, m_tdc.ProjectName);
    }
  }
}