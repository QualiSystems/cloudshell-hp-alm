namespace CTSAddin
{
    partial class TestShellTestsBrowserForm
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
            this.ButtonOK = new System.Windows.Forms.Button();
            this.lButtonCancel = new System.Windows.Forms.Button();
            this.TreeViewPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Enabled = false;
            this.ButtonOK.Location = new System.Drawing.Point(271, 467);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // lButtonCancel
            // 
            this.lButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lButtonCancel.Location = new System.Drawing.Point(361, 467);
            this.lButtonCancel.Name = "lButtonCancel";
            this.lButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.lButtonCancel.TabIndex = 1;
            this.lButtonCancel.Text = "Cancel";
            this.lButtonCancel.UseVisualStyleBackColor = true;
            this.lButtonCancel.Click += new System.EventHandler(this.lButtonCancel_Click);
            // 
            // TreeViewPanel
            // 
            this.TreeViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewPanel.Location = new System.Drawing.Point(50, 47);
            this.TreeViewPanel.Name = "TreeViewPanel";
            this.TreeViewPanel.Size = new System.Drawing.Size(386, 393);
            this.TreeViewPanel.TabIndex = 2;
            // 
            // TestShellTestsBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 514);
            this.Controls.Add(this.TreeViewPanel);
            this.Controls.Add(this.lButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Name = "TestShellTestsBrowserForm";
            this.Text = "TestShell Tests Browser";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button lButtonCancel;
        private System.Windows.Forms.Panel TreeViewPanel;
    }
}