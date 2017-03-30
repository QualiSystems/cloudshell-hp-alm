namespace TsTestType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestShellTestsBrowserForm));
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.TreeViewPanel = new System.Windows.Forms.Panel();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.panelSeparator1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Enabled = false;
            this.ButtonOK.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ButtonOK.FlatAppearance.BorderSize = 0;
            this.ButtonOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.ButtonOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.ButtonOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("ButtonOK.Image")));
            this.ButtonOK.Location = new System.Drawing.Point(406, 495);
            this.ButtonOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(142, 42);
            this.ButtonOK.TabIndex = 2;
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ButtonCancel.FlatAppearance.BorderSize = 0;
            this.ButtonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.ButtonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.Image")));
            this.ButtonCancel.Location = new System.Drawing.Point(562, 495);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(142, 42);
            this.ButtonCancel.TabIndex = 3;
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // TreeViewPanel
            // 
            this.TreeViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewPanel.Location = new System.Drawing.Point(10, 10);
            this.TreeViewPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TreeViewPanel.Name = "TreeViewPanel";
            this.TreeViewPanel.Size = new System.Drawing.Size(730, 461);
            this.TreeViewPanel.TabIndex = 1;
            // 
            // panelSeparator
            // 
            this.panelSeparator.BackColor = System.Drawing.Color.LightGray;
            this.panelSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelSeparator.Location = new System.Drawing.Point(0, 470);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(740, 1);
            this.panelSeparator.TabIndex = 8;
            // 
            // panelSeparator1
            // 
            this.panelSeparator1.BackColor = System.Drawing.Color.LightGray;
            this.panelSeparator1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panelSeparator1.Location = new System.Drawing.Point(0, 0);
            this.panelSeparator1.Name = "panelSeparator1";
            this.panelSeparator1.Size = new System.Drawing.Size(740, 1);
            this.panelSeparator1.TabIndex = 9;
            // 
            // TestShellTestsBrowserForm
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(740, 561);
            this.Controls.Add(this.panelSeparator1);
            this.Controls.Add(this.panelSeparator);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.TreeViewPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TestShellTestsBrowserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestShell Tests Browser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestShellTestsBrowserForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Panel TreeViewPanel;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.Panel panelSeparator1;
    }
}