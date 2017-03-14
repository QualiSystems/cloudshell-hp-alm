namespace  CTSAddin
{
  partial class ResultViewerControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelReportResult = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Click the link to open Report :";
            // 
            // linkLabelReportResult
            // 
            this.linkLabelReportResult.AutoSize = true;
            this.linkLabelReportResult.Location = new System.Drawing.Point(3, 51);
            this.linkLabelReportResult.Name = "linkLabelReportResult";
            this.linkLabelReportResult.Size = new System.Drawing.Size(55, 13);
            this.linkLabelReportResult.TabIndex = 2;
            this.linkLabelReportResult.TabStop = true;
            this.linkLabelReportResult.Text = "linkLabel1";
            this.linkLabelReportResult.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReportResult_LinkClicked);
            // 
            // ResultViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelReportResult);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ResultViewerControl";
            this.Size = new System.Drawing.Size(701, 176);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
//comment
    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel linkLabelReportResult;
  }
}
