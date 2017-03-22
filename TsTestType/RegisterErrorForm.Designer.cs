namespace TsTestType
{
    partial class RegisterErrorForm
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
            this.lblError = new System.Windows.Forms.Label();
            this.lblExplain = new System.Windows.Forms.Label();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(12, 84);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(54, 13);
            this.lblError.TabIndex = 0;
            this.lblError.Text = "label Error";
            // 
            // lblExplain
            // 
            this.lblExplain.AutoSize = true;
            this.lblExplain.Location = new System.Drawing.Point(12, 22);
            this.lblExplain.Name = "lblExplain";
            this.lblExplain.Size = new System.Drawing.Size(62, 13);
            this.lblExplain.TabIndex = 1;
            this.lblExplain.Text = "Explenation";
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(217, 219);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(112, 35);
            this.ButtonCancel.TabIndex = 5;
            this.ButtonCancel.Text = "Abort";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Enabled = false;
            this.ButtonOK.Location = new System.Drawing.Point(81, 219);
            this.ButtonOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(112, 35);
            this.ButtonOK.TabIndex = 4;
            this.ButtonOK.Text = "Retry";
            this.ButtonOK.UseVisualStyleBackColor = true;
            // 
            // RegisterErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 268);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.lblExplain);
            this.Controls.Add(this.lblError);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RegisterErrorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Title";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblExplain;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonOK;
    }
}