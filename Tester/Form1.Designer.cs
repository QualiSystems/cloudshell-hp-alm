﻿namespace Tester
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtTestPath = new System.Windows.Forms.TextBox();
            this.PanelScriptView = new System.Windows.Forms.Panel();
            this.ButtonRunTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTestRunStatus = new System.Windows.Forms.Label();
            this.btnRegisterAgent = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtTestPath
            // 
            this.txtTestPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestPath.Location = new System.Drawing.Point(93, 15);
            this.txtTestPath.Name = "txtTestPath";
            this.txtTestPath.Size = new System.Drawing.Size(521, 20);
            this.txtTestPath.TabIndex = 1;
            // 
            // PanelScriptView
            // 
            this.PanelScriptView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelScriptView.Location = new System.Drawing.Point(0, 0);
            this.PanelScriptView.Name = "PanelScriptView";
            this.PanelScriptView.Size = new System.Drawing.Size(626, 326);
            this.PanelScriptView.TabIndex = 6;
            // 
            // ButtonRunTest
            // 
            this.ButtonRunTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonRunTest.Location = new System.Drawing.Point(277, 339);
            this.ButtonRunTest.Name = "ButtonRunTest";
            this.ButtonRunTest.Size = new System.Drawing.Size(75, 54);
            this.ButtonRunTest.TabIndex = 8;
            this.ButtonRunTest.Text = "Run Test";
            this.ButtonRunTest.UseVisualStyleBackColor = true;
            this.ButtonRunTest.Click += new System.EventHandler(this.ButtonRunTest_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 396);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Test run status:";
            // 
            // lblTestRunStatus
            // 
            this.lblTestRunStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTestRunStatus.AutoSize = true;
            this.lblTestRunStatus.Location = new System.Drawing.Point(323, 396);
            this.lblTestRunStatus.Name = "lblTestRunStatus";
            this.lblTestRunStatus.Size = new System.Drawing.Size(65, 13);
            this.lblTestRunStatus.TabIndex = 10;
            this.lblTestRunStatus.Text = "<run status>";
            // 
            // btnRegisterAgent
            // 
            this.btnRegisterAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRegisterAgent.Location = new System.Drawing.Point(12, 339);
            this.btnRegisterAgent.Name = "btnRegisterAgent";
            this.btnRegisterAgent.Size = new System.Drawing.Size(75, 54);
            this.btnRegisterAgent.TabIndex = 11;
            this.btnRegisterAgent.Text = "Register Agent";
            this.btnRegisterAgent.UseVisualStyleBackColor = true;
            this.btnRegisterAgent.Click += new System.EventHandler(this.btnRegisterAgent_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(626, 435);
            this.Controls.Add(this.btnRegisterAgent);
            this.Controls.Add(this.lblTestRunStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonRunTest);
            this.Controls.Add(this.PanelScriptView);
            this.Controls.Add(this.txtTestPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTestPath;
        private System.Windows.Forms.Panel PanelScriptView;
        private System.Windows.Forms.Button ButtonRunTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTestRunStatus;
        private System.Windows.Forms.Button btnRegisterAgent;
    }
}

