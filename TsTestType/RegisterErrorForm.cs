using System;
using System.Windows.Forms;
using TsCloudShellApi;

namespace TsTestType
{
    public partial class RegisterErrorForm : Form
    {
        public RegisterErrorForm(string title, string explain, string details)
        {
            InitializeComponent();
            Text = title;
            lblExplain.Text = explain;
            txtDetails.Text = details;
            txtDetails.Visible = !string.IsNullOrEmpty(details);
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            var message = string.Format("You are about to disable {0} Integration.\n\nAre you sure?", Config.TestShell);
            var choice = MessageBox.Show(this, message, "WARNING", MessageBoxButtons.YesNo);

            if (choice == DialogResult.Yes)
                DialogResult = DialogResult.Abort;
        }
    }
}
