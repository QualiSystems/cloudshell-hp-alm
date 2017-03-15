using System;
using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;
using QS.ALM.CloudShellApi;
using Mercury.TD.Client.Ota.Api;
using Mercury.TD.Client.Ota.Core;


namespace TestType
{
    /// <summary>
    /// Handles displaying test scripts from the project repository in the ALM user interface.
    /// </summary>
    public partial class ScriptViewerControl : UserControl, IScriptViewer
    {
        private Api m_Api;
        private HP.ALM.QC.OTA.Entities.Api.ITest m_CurrentTest;
        private string m_TestPathUserFieldName;//"TS_USER_01"
        public ScriptViewerControl()
        {
            InitializeComponent();
        }

        public ScriptViewerControl(Api api)
        {
            m_Api = api;
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the script viewer.
        /// </summary>
        /// <remarks>With the TDConnection reference, you can access all ALM Open Test Architecture components.
        /// <br />For more information, see the ALM Open Test Architecture Reference.</remarks>
        /// <param name="connection">Output. The connection is connected to the server and authorized for the project.</param>
        public void InitViewer(IConnection connection)
        {
            try
            {
                var tdConnectedObject = (ITDConnectedObject)connection;
                var tdc = (ITDConnection)tdConnectedObject.TDConnection;
                m_Api = new Api(tdc);
                m_TestPathUserFieldName = new TDConnectionServant(tdc).GetQualiTestPathFieldName();
            }
            catch (Exception ex)
            {
                m_Api = null;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                Enabled = false;
            }
        }

        /// <summary>
        /// "true" sets script to read-only.
        /// </summary>
        public bool ReadOnly
        {
            set { }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <remarks>Optional. If implemented, ALM calls SaveScript on moving to a different tab or another object.</remarks>
        public void SaveScript()
        {
            //MessageBox.Show("Saving script.");
        }

        public string TestPath { get { return TextBoxPath.Text; } }

        /// <summary>
        /// Displays the active version of the test script in the Script Viewer control.
        /// </summary>
        /// <param name="test">Output. An ALM Open Test Architecture ITest object.</param>
        public void ShowTest(HP.ALM.QC.OTA.Entities.Api.ITest test)
        {
            if (string.IsNullOrWhiteSpace(m_TestPathUserFieldName))
            {
                Enabled = false;
                MessageBox.Show("TestPath field name is unknown", "Error", MessageBoxButtons.OK);
                return;
            }

            m_CurrentTest = test;
            TextBoxPath.Text = m_CurrentTest[m_TestPathUserFieldName] == null ? "" : m_CurrentTest[m_TestPathUserFieldName].ToString();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            var browserForm = new TestShellTestsBrowserForm(m_Api);
            var path = browserForm.TryShowDialog(TextBoxPath.Text);

            if (path != null && m_CurrentTest == null)//for tester
            {
                TextBoxPath.Text = path;
                return;
            }
            if (path != null)
            {
                m_CurrentTest[m_TestPathUserFieldName] = TextBoxPath.Text = path;
                RefreshTestParameter(path);
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you would like to refresh the" + Environment.NewLine + "test parameters ?";
            string caption = "Refresh";

            var result = MessageBox.Show(message, caption, MessageBoxButtons.OKCancel);

            if (result == DialogResult.OK)
            {
                RefreshTestParameter(TextBoxPath.Text);
            }
        }

        private void RefreshTestParameter(string path)
        {
            string contentError;
            bool isSuccess;
            var testParameter = m_Api.GetTestParameter(path, out contentError, out isSuccess);
            if (!isSuccess)
            {
                MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);
                return;
            }
            RewriteTestParameters(testParameter.Parameters);
        }

        private void RewriteTestParameters(APITestParameterInfo[] arrParameters)
        {
            var paramFactory = (Factory<HP.ALM.QC.OTA.Entities.Api.ITestParameter, HP.ALM.QC.OTA.Entities.Api.ITest>)m_CurrentTest.ParameterFactory;
            var baseFactory = (IBaseFactory)paramFactory.LegacyFactory;

            var baseList = baseFactory.NewList("");

            if (baseList != null)
            {
                foreach (var element in baseList)
                {
                    baseFactory.RemoveItem(element);
                }
            }

            if (arrParameters != null)
            {
                for (int i = 0; i < arrParameters.Length; ++i)
                {
                    HP.ALM.QC.OTA.Entities.Api.ITestParameter newEnt = paramFactory.NewEntity();
                    newEnt.Name = arrParameters[i].Name;
                    newEnt.Description = arrParameters[i].Description;
                    newEnt.Post();
                }
            }
        }
    }
}
