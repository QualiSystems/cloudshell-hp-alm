namespace CTSAddin
{
  partial class ScriptViewerControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptViewerControl));
            this.m_TextBoxPath = new System.Windows.Forms.TextBox();
            this.labelTestPath = new System.Windows.Forms.Label();
            this.ButtonBrowse = new System.Windows.Forms.Button();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            this.LabelText1 = new System.Windows.Forms.Label();
            this.LabelText2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_TextBoxPath
            // 
            this.m_TextBoxPath.Location = new System.Drawing.Point(104, 31);
            this.m_TextBoxPath.Margin = new System.Windows.Forms.Padding(2);
            this.m_TextBoxPath.Name = "m_TextBoxPath";
            this.m_TextBoxPath.Size = new System.Drawing.Size(474, 20);
            this.m_TextBoxPath.TabIndex = 0;
            this.m_TextBoxPath.Text = "local\\folder1\\test3";
            // 
            // labelTestPath
            // 
            this.labelTestPath.AutoSize = true;
            this.labelTestPath.Location = new System.Drawing.Point(4, 34);
            this.labelTestPath.Name = "labelTestPath";
            this.labelTestPath.Size = new System.Drawing.Size(95, 13);
            this.labelTestPath.TabIndex = 1;
            this.labelTestPath.Text = "TestShell test path";
            // 
            // ButtonBrowse
            // 
            this.ButtonBrowse.Location = new System.Drawing.Point(503, 59);
            this.ButtonBrowse.Name = "ButtonBrowse";
            this.ButtonBrowse.Size = new System.Drawing.Size(75, 54);
            this.ButtonBrowse.TabIndex = 2;
            this.ButtonBrowse.Text = "Browse";
            this.ButtonBrowse.UseVisualStyleBackColor = true;
            this.ButtonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Location = new System.Drawing.Point(503, 175);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(75, 54);
            this.ButtonRefresh.TabIndex = 3;
            this.ButtonRefresh.Text = "Refresh";
            this.ButtonRefresh.UseVisualStyleBackColor = true;
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // LabelText1
            // 
            this.LabelText1.AutoSize = true;
            this.LabelText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelText1.Location = new System.Drawing.Point(104, 59);
            this.LabelText1.Name = "LabelText1";
            this.LabelText1.Size = new System.Drawing.Size(360, 80);
            this.LabelText1.TabIndex = 4;
            this.LabelText1.Text = resources.GetString("LabelText1.Text");
            // 
            // LabelText2
            // 
            this.LabelText2.AutoSize = true;
            this.LabelText2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelText2.Location = new System.Drawing.Point(104, 175);
            this.LabelText2.Name = "LabelText2";
            this.LabelText2.Size = new System.Drawing.Size(346, 48);
            this.LabelText2.TabIndex = 5;
            this.LabelText2.Text = "Clicking the refresh button allows to update the test case\r\nparameters from TestS" +
    "hell. Click the refresh if a TestShell\r\ntest has changed its interface.";
            // 
            // ScriptViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelText2);
            this.Controls.Add(this.LabelText1);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.ButtonBrowse);
            this.Controls.Add(this.labelTestPath);
            this.Controls.Add(this.m_TextBoxPath);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ScriptViewerControl";
            this.Size = new System.Drawing.Size(596, 308);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox m_TextBoxPath;
    private System.Windows.Forms.Label labelTestPath;
    private System.Windows.Forms.Button ButtonBrowse;
    private System.Windows.Forms.Button ButtonRefresh;
    private System.Windows.Forms.Label LabelText1;
    private System.Windows.Forms.Label LabelText2;
  }
}
