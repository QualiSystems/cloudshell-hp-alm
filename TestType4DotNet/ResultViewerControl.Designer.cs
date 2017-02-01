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
      this.m_textBox = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // m_textBox
      // 
      this.m_textBox.Location = new System.Drawing.Point(71, 84);
      this.m_textBox.Name = "m_textBox";
      this.m_textBox.Size = new System.Drawing.Size(456, 22);
      this.m_textBox.TabIndex = 0;
      // 
      // ResultViewerControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.m_textBox);
      this.Name = "ResultViewerControl";
      this.Size = new System.Drawing.Size(605, 216);
      this.ResumeLayout(false);
      this.PerformLayout();

    }
//comment
    #endregion

    private System.Windows.Forms.TextBox m_textBox;
  }
}
