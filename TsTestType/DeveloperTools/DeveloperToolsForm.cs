using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TsCloudShellApi;

namespace TsTestType.DeveloperTools
{
    public partial class DeveloperToolsForm : Form
    {
        private readonly Logger m_Logger;

        public DeveloperToolsForm(Logger logger)
        {
            m_Logger = logger;
            InitializeComponent();

            var assemblyPath = Assembly.GetExecutingAssembly().Location;
            lblVersion.Text = FileVersionInfo.GetVersionInfo(assemblyPath).FileVersion;
            txtFolder.Text = Path.GetDirectoryName(assemblyPath);
            RefreshProcessInfo();
        }

        private void linkRefreshProcessInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshProcessInfo();
        }

        private void RefreshProcessInfo()
        {
            txtProcessInfo.Clear();

            string[] AlmProcesses = { "ALM-Client".ToLower(), "iexplore".ToLower(), "dllhost".ToLower() };

            var processes = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process process in processes)
            {
                if (AlmProcesses.Contains(process.ProcessName.ToLower()))
                {
                    try
                    {
                        txtProcessInfo.Text += process.ProcessName + Environment.NewLine;
                    }
                    catch { }
                }
            }
        }

        private void linkRegisterAgent_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new RegisterAgent(m_Logger).ForceRegister();
        }
    }
}
