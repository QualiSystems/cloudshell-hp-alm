using System;
using System.Windows.Forms;
using HP.ALM.QC.UI.Modules.Shared.Api;
using TDAPIOLELib;
using QS.ALM.CloudShellApi;
using Mercury.TD.Client.Ota.Api;
using Mercury.TD.Client.Ota.Core;


namespace CTSAddin
{
    /// <summary>
    /// Handles displaying test scripts from the project repository in the ALM user interface.
    /// </summary>
    public partial class ScriptViewerControl : UserControl, IScriptViewer
    {
        private Api m_Api;
        private HP.ALM.QC.OTA.Entities.Api.ITest m_CurrentTest;
        private string m_TestPathUserFieldName; //"TS_USER_01"

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
                var tdConnection = (ITDConnection)tdConnectedObject.TDConnection;
                m_Api = new Api(tdConnection);
                m_TestPathUserFieldName = new TdConnectionServant(tdConnection).GetQualiTestPathFieldName();
            }
            catch (Exception ex)
            {
                m_Api = null;
                ReadOnly = true;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }

            //m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", null, null, AuthenticationMode.Alm, "Global");
            //var list = tdc.get_Fields("SYSTEM_FIELD");
            /*var command = m_tdc.Command;
      command.CommandText = "Select SF_COLUMN_NAME from SYSTEM_FIELD WHERE SF_USER_LABEL = 'QUALI_TEST_PATH'";
      var result = command.Execute();
      string userFieldName = result["SF_COLUMN_NAME"].ToString();*/
            //var xxx = m_tdc.get_Fields("TP_NAME");
            //var xxx1 = m_tdc.get_Fields("PathForOurTestQuali");
        }

        /// <summary>
        /// "true" sets script to read-only.
        /// </summary>
        public bool ReadOnly
        {
            set
            {
                Enabled = !value;
            }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <remarks>Optional. If implemented, ALM calls SaveScript on moving to a different tab or another object.</remarks>
        public void SaveScript()
        {
            //MessageBox.Show("Saving script.");
        }

        public string TestPath
        {
            get { return TextBoxPath.Text; }
        }

        /// <summary>
        /// Displays the active version of the test script in the Script Viewer control.
        /// </summary>
        /// <param name="test">Output. An ALM Open Test Architecture ITest object.</param>
        public void ShowTest(HP.ALM.QC.OTA.Entities.Api.ITest test)
        {
            if (string.IsNullOrWhiteSpace(m_TestPathUserFieldName))
                return;
            
            m_CurrentTest = test;
            TextBoxPath.Text = m_CurrentTest[m_TestPathUserFieldName] == null ? "" : m_CurrentTest[m_TestPathUserFieldName].ToString();
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            var browserForm = new TestShellTestsBrowserForm(m_Api);
            var path = browserForm.TryShowDialog(TextBoxPath.Text);
            
            if (path != null && m_CurrentTest == null) //for tester
            {
                TextBoxPath.Text = path;
                return;
            }

            if (path != null)
            {
                m_CurrentTest[m_TestPathUserFieldName] = TextBoxPath.Text = path;
                RefreshTestParameter(path);
            }

            /*m_CurrentTest

            var fact = (Mercury.TD.Client.Ota.Core.Factory<HP.ALM.QC.OTA.Entities.Api.ITest, HP.ALM.QC.OTA.Entities.Api.ITestFolder>)m_CurrentTest.Factory;
            var legFact = fact.LegacyFactory;
            var tFilter = legFact.Filter;
            //var qTName = "\"T1\"";
            tFilter["TS_NAME"] = "";
            var tList = tFilter.NewList();
            foreach (var item in tList)
            {
                var tmp = item.ToString();
            }*/

            //m_CurrentTest.NewHistoryFilter
            //object tmp = m_CurrentTest["QUALI_TEST_PATH"];
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            var message = "Are you sure you would like to refresh the" + Environment.NewLine + "test parameters ?";
            var result = MessageBox.Show(message, "Refresh", MessageBoxButtons.OKCancel);

            if (result == DialogResult.Yes)
            {
                RefreshTestParameter(TextBoxPath.Text);
            }
        }

        private void RefreshTestParameter(string path)
        {
            string contentError;
            bool isSuccess;
            APITestExplorerTestInfo testParameter = m_Api.GetTestParameter(path, out contentError, out isSuccess);

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
            var baseFactory = (IBaseFactory) paramFactory.LegacyFactory;

            List baseList = baseFactory.NewList("");
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
