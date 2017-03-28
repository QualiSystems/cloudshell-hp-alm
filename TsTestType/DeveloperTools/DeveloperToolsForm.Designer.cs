namespace TsTestType.DeveloperTools
{
    partial class DeveloperToolsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProcessInfo = new System.Windows.Forms.TextBox();
            this.linkRefreshProcessInfo = new System.Windows.Forms.LinkLabel();
            this.linkRegisterAgent = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(63, 9);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(40, 13);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "0.0.0.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Folder:";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(66, 29);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(419, 20);
            this.txtFolder.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Process Information:";
            // 
            // txtProcessInfo
            // 
            this.txtProcessInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcessInfo.Location = new System.Drawing.Point(15, 88);
            this.txtProcessInfo.Multiline = true;
            this.txtProcessInfo.Name = "txtProcessInfo";
            this.txtProcessInfo.ReadOnly = true;
            this.txtProcessInfo.Size = new System.Drawing.Size(470, 129);
            this.txtProcessInfo.TabIndex = 5;
            // 
            // linkRefreshProcessInfo
            // 
            this.linkRefreshProcessInfo.AutoSize = true;
            this.linkRefreshProcessInfo.Location = new System.Drawing.Point(121, 61);
            this.linkRefreshProcessInfo.Name = "linkRefreshProcessInfo";
            this.linkRefreshProcessInfo.Size = new System.Drawing.Size(45, 13);
            this.linkRefreshProcessInfo.TabIndex = 6;
            this.linkRefreshProcessInfo.TabStop = true;
            this.linkRefreshProcessInfo.Text = "(refresh)";
            this.linkRefreshProcessInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRefreshProcessInfo_LinkClicked);
            // 
            // linkRegisterAgent
            // 
            this.linkRegisterAgent.AutoSize = true;
            this.linkRegisterAgent.Location = new System.Drawing.Point(12, 229);
            this.linkRegisterAgent.Name = "linkRegisterAgent";
            this.linkRegisterAgent.Size = new System.Drawing.Size(110, 13);
            this.linkRegisterAgent.TabIndex = 7;
            this.linkRegisterAgent.TabStop = true;
            this.linkRegisterAgent.Text = "Register Agent (force)";
            this.linkRegisterAgent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegisterAgent_LinkClicked);
            // 
            // DeveloperToolsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 414);
            this.Controls.Add(this.linkRegisterAgent);
            this.Controls.Add(this.linkRefreshProcessInfo);
            this.Controls.Add(this.txtProcessInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label1);
            this.Name = "DeveloperToolsForm";
            this.ShowIcon = false;
            this.Text = "ALM TestShell Developer Tools";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProcessInfo;
        private System.Windows.Forms.LinkLabel linkRefreshProcessInfo;
        private System.Windows.Forms.LinkLabel linkRegisterAgent;
    }
}