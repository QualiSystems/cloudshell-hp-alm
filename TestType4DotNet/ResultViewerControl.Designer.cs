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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_textBox
            // 
            this.m_textBox.Location = new System.Drawing.Point(2, 68);
            this.m_textBox.Margin = new System.Windows.Forms.Padding(2);
            this.m_textBox.Name = "m_textBox";
            this.m_textBox.Size = new System.Drawing.Size(697, 20);
            this.m_textBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Result";
            // 
            // ResultViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_textBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ResultViewerControl";
            this.Size = new System.Drawing.Size(701, 176);
            this.ResumeLayout(false);
            this.PerformLayout();

    }
//comment
    #endregion

    private System.Windows.Forms.TextBox m_textBox;
    private System.Windows.Forms.Label label1;
  }
}
