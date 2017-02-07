using System;
using System.Windows.Forms;
using QS.ALM.CloudShellApi;
using CTSAddin;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {   
            InitializeComponent();

            ScriptViewerControl scriptControl = new ScriptViewerControl();
            PanelScriptView.Controls.Add(scriptControl);
            scriptControl.Dock = DockStyle.Fill;
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            string error;

            if (!Api.RunTest(txtTestPath.Text, out error))
                MessageBox.Show(error);
        }
    }
}
