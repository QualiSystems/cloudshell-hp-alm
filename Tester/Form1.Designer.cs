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
            this.btnRunTest = new System.Windows.Forms.Button();
            this.txtTestPath = new System.Windows.Forms.TextBox();
            this.BrouseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRunTest
            // 
            this.btnRunTest.Location = new System.Drawing.Point(12, 12);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(75, 23);
            this.btnRunTest.TabIndex = 0;
            this.btnRunTest.Text = "Run Test";
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // txtTestPath
            // 
            this.txtTestPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestPath.Location = new System.Drawing.Point(93, 15);
            this.txtTestPath.Name = "txtTestPath";
            this.txtTestPath.Size = new System.Drawing.Size(179, 20);
            this.txtTestPath.TabIndex = 1;
            // 
            // BrouseButton
            // 
            this.BrouseButton.Location = new System.Drawing.Point(93, 120);
            this.BrouseButton.Name = "BrouseButton";
            this.BrouseButton.Size = new System.Drawing.Size(75, 23);
            this.BrouseButton.TabIndex = 2;
            this.BrouseButton.Text = "Brouse";
            this.BrouseButton.UseVisualStyleBackColor = true;
            this.BrouseButton.Click += new System.EventHandler(this.BrouseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.BrouseButton);
            this.Controls.Add(this.txtTestPath);
            this.Controls.Add(this.btnRunTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunTest;
        private System.Windows.Forms.TextBox txtTestPath;
        private System.Windows.Forms.Button BrouseButton;
    }
}

