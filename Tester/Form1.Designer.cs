namespace Tester
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
            this.PanelScriptView.Size = new System.Drawing.Size(626, 248);
            this.PanelScriptView.TabIndex = 6;
            // 
            // ButtonRunTest
            // 
            this.ButtonRunTest.Location = new System.Drawing.Point(277, 261);
            this.ButtonRunTest.Name = "ButtonRunTest";
            this.ButtonRunTest.Size = new System.Drawing.Size(75, 54);
            this.ButtonRunTest.TabIndex = 8;
            this.ButtonRunTest.Text = "Run Test";
            this.ButtonRunTest.UseVisualStyleBackColor = true;
            this.ButtonRunTest.Click += new System.EventHandler(this.ButtonRunTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 357);
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
    }
}

