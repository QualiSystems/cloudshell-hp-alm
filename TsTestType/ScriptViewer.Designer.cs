﻿namespace TsTestType
{
  partial class ScriptViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptViewer));
            this.TextBoxPath = new System.Windows.Forms.TextBox();
            this.labelTestPath = new System.Windows.Forms.Label();
            this.ButtonBrowse = new System.Windows.Forms.Button();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            this.LabelText1 = new System.Windows.Forms.Label();
            this.LabelText2 = new System.Windows.Forms.Label();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBoxPath
            // 
            this.TextBoxPath.BackColor = System.Drawing.Color.White;
            this.TextBoxPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxPath.Location = new System.Drawing.Point(2, 7);
            this.TextBoxPath.Margin = new System.Windows.Forms.Padding(2);
            this.TextBoxPath.Name = "TextBoxPath";
            this.TextBoxPath.ReadOnly = true;
            this.TextBoxPath.Size = new System.Drawing.Size(582, 13);
            this.TextBoxPath.TabIndex = 0;
            // 
            // labelTestPath
            // 
            this.labelTestPath.AutoSize = true;
            this.labelTestPath.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelTestPath.Location = new System.Drawing.Point(4, 34);
            this.labelTestPath.Name = "labelTestPath";
            this.labelTestPath.Size = new System.Drawing.Size(95, 13);
            this.labelTestPath.TabIndex = 1;
            this.labelTestPath.Text = "TestShell test path";
            // 
            // ButtonBrowse
            // 
            this.ButtonBrowse.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonBrowse.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ButtonBrowse.FlatAppearance.BorderSize = 0;
            this.ButtonBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.ButtonBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.ButtonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonBrowse.Image = ((System.Drawing.Image)(resources.GetObject("ButtonBrowse.Image")));
            this.ButtonBrowse.Location = new System.Drawing.Point(453, 101);
            this.ButtonBrowse.Name = "ButtonBrowse";
            this.ButtonBrowse.Size = new System.Drawing.Size(142, 42);
            this.ButtonBrowse.TabIndex = 2;
            this.ButtonBrowse.UseVisualStyleBackColor = false;
            this.ButtonBrowse.Click += new System.EventHandler(this.ButtonBrowse_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ButtonRefresh.FlatAppearance.BorderSize = 0;
            this.ButtonRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.ButtonRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.ButtonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonRefresh.Image = global::TsTestType.Resource.RefreshButton;
            this.ButtonRefresh.Location = new System.Drawing.Point(453, 234);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(142, 42);
            this.ButtonRefresh.TabIndex = 3;
            this.ButtonRefresh.UseVisualStyleBackColor = true;
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // LabelText1
            // 
            this.LabelText1.AutoSize = true;
            this.LabelText1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelText1.Location = new System.Drawing.Point(7, 101);
            this.LabelText1.Name = "LabelText1";
            this.LabelText1.Size = new System.Drawing.Size(360, 80);
            this.LabelText1.TabIndex = 4;
            this.LabelText1.Text = resources.GetString("LabelText1.Text");
            // 
            // LabelText2
            // 
            this.LabelText2.AutoSize = true;
            this.LabelText2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelText2.Location = new System.Drawing.Point(7, 234);
            this.LabelText2.Name = "LabelText2";
            this.LabelText2.Size = new System.Drawing.Size(346, 48);
            this.LabelText2.TabIndex = 5;
            this.LabelText2.Text = "Clicking the refresh button allows to update the test case\r\nparameters from TestS" +
    "hell. Click the refresh if a TestShell\r\ntest has changed its interface.";
            // 
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.Color.LightGray;
            this.panelSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelSeparator.Location = new System.Drawing.Point(7, 194);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(586, 1);
            this.panelSeparator.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.TextBoxPath);
            this.panel1.Location = new System.Drawing.Point(7, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 30);
            this.panel1.TabIndex = 10;
            // 
            // ScriptViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelSeparator);
            this.Controls.Add(this.LabelText2);
            this.Controls.Add(this.LabelText1);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.ButtonBrowse);
            this.Controls.Add(this.labelTestPath);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ScriptViewer";
            this.Size = new System.Drawing.Size(596, 308);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox TextBoxPath;
    private System.Windows.Forms.Label labelTestPath;
    private System.Windows.Forms.Button ButtonBrowse;
    private System.Windows.Forms.Button ButtonRefresh;
    private System.Windows.Forms.Label LabelText1;
    private System.Windows.Forms.Label LabelText2;
    private System.Windows.Forms.Panel panelSeparator;
    private System.Windows.Forms.Panel panel1;
  }
}
