using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using TDAPIOLELib;
using QS.ALM.CloudShellApi;


namespace CSRAgent
{
    class ALMCon
    {
        private  Api m_Api;
        public TDConnection conn;
        static private Dictionary<string, string> Params;
        public CStatus mStatus;
        private string m_TestPath;
        TDAPIOLELib.List lst;
        TestFactory testConfigTestFact;
        TSTestFactory TSTestFact;
        public ALMCon()
        {
            Params = new Dictionary<string,string>();
        }

        public int OpenCon()
        {
            try
            {
                conn = new TDConnectionClass();
                conn.InitConnectionEx(this.GetValue("TDAPI_host_name"));
                conn.ConnectProjectEx(this.GetValue("domain_name"), this.GetValue("project_name"), this.GetValue("user_name"), this.GetValue("password"));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return -1;
            }
            return 0;
        }
        public int CloseCon()
        {
            if(conn.Connected)
                conn.DisconnectProject();
            return 0;
        }

        public string GetValue(string prmName)
        {
            if (Params.ContainsKey(prmName))
            {
                return  Params[prmName];
            }
            return "";
        }
        public int GetValue(string prmName, ref string prmValue)
        {
            if (Params.ContainsKey(prmName))
            {
                prmValue = Params[prmName];
            }
            return 0;
        }
        public int SetValue(string prmName, string prmValue)
        {
            if (!Params.ContainsKey(prmName))
            {
                Params.Add(prmName, prmValue);
            }
            else
            {
                Params[prmName] = prmValue;
            }
            return 0;
        }

        public int RunQSheel()
        {
            MessageBox.Show("Start QSheel");
            try
            {
                m_Api = new Api(conn); //"http://192.168.42.35:9000", "admin", "admin", null, null, AuthenticationMode.Alm, "Global");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                throw new System.InvalidOperationException(ex.Message);
            }
            return 0;
        }
        public int WaitTestComplit()
        {
            string contentError;
            bool isSuccess;
            string guiId = m_Api.RunTest(m_TestPath,null,  out contentError, out isSuccess);

            if (isSuccess)
            {
                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(guiId, m_Api))
                    runResult = runStatusManager.WaitForRunEnd();
            }
            else
            {
                throw new System.InvalidOperationException(contentError);
            }   

            return 0;
        }

        public string getStatus()
        {
            return convertStatusToString(mStatus);
        }
        public int getTestParameters(TDAPIOLELib.TestSet theTestSet, TestSetFolder tsFolder, string tSetName , string testName)//TDAPIOLELib.Test test)//, 
        {
            String tempStrt = String.Empty;
            TSTestFactory TSTestFact;
            List testList;
            TDFilter tsFilter;
            TSTestFact = (TSTestFactory)theTestSet.TSTestFactory;
            tsFilter = (TDFilter)TSTestFact.Filter;
            tsFilter["TC_CYCLE_ID"] = theTestSet.ID.ToString();
            testList = TSTestFact.NewList(tsFilter.Text);
            Debug.Print("Test instances and planned hosts:");
            foreach (TSTest TSTst in testList)
            {
                if (testName == TSTst.TestName)
                {
                    TDAPIOLELib.Test theTest = (TDAPIOLELib.Test)TSTst.Test;
                    if (theTest["TS_USER_01"] != null)
                        m_TestPath = theTest["TS_USER_01"].ToString();
                    else
                        m_TestPath = "";
                    getTheParams(TSTst);
                    break;
                }
            }

           
            return 0;
        }

        private void getTheParams(TDAPIOLELib.TSTest TSTst)
        {
            TDAPIOLELib.ParameterValueFactory paramValueFact;

            List testParametersVList = ((ISupportParameterValues)TSTst).ParameterValueFactory.NewList("");


            if (testParametersVList != null)
            {
                foreach (ParameterValue element in testParametersVList)
                {
                    string value = GetParameterValue(element.ActualValue);
                    string Ls = element.Name;
                }
            }
        }

        private string GetParameterValue(object p)
        {
            return "";
        }
        private int getCinfigortion()
        {
            return 0;
        }


        public TestSet GetTestSet(TestSetFolder testSetF, String tsName, String tsStatus)
        {
            try
            {
                TestSet testSet1;
                List tsList;
                bool found;

                tsList = null;
                testSet1 = null;
                found = false;

                try
                {

                    tsList = testSetF.FindTestSets(tsName);
                    if (tsList != null)
                    {
                        if (tsList.Count > 0)
                        {
                            foreach (TestSet ts in tsList)
                            {
                                if (ts.Name.ToString() == tsName)
                                {
                                    testSet1 = ts;
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                }
                finally
                {
                    if (!found)
                    {
                        TestSetFactory tsFact;
                        tsFact = (TestSetFactory)testSetF.TestSetFactory;

                        testSet1 = (TestSet)tsFact.AddItem(DBNull.Value);
                        testSet1.Name = tsName;
                        testSet1.Status = tsStatus;
                        testSet1.Post();
                    }
                }
                return testSet1;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public int getTestPath()
        {
            string[] strParmsArr;
            string testF = "";
            string testn = "";
            int TestInstID = Convert.ToInt32(Params["testcycle_id_integer"]);
            if (Params["test_set"].Split(',')[0].Split('\"')[0].IndexOfAny("ntest_set".ToCharArray()) > -1)
            {
                strParmsArr = Params["test_set"].Split(',')[0].Split('\"')[1].Split('\\');
                testF = strParmsArr[2].ToString();
                testn = strParmsArr[strParmsArr.Count() - 1];

            }
            else
            {
                return -1;
            }
           
                TreeManager treeMgr = (TreeManager)conn.TreeManager;
                SubjectNode subjectNode = (SubjectNode)treeMgr.get_NodeByPath("Subject\\" + testF);
                TestSetTreeManager testSetFolderF = (TestSetTreeManager)conn.TestSetTreeManager;
                TestSetFolder tstSetFolder = (TestSetFolder)testSetFolderF.NodeByPath["Root"];
                
                TDAPIOLELib.TestSet theTestSet = GetTestSet(tstSetFolder, testn, mStatus.ToString());
                getTestParameters(theTestSet, tstSetFolder, testn , Params["test_name"]);//theTest);//, 


                return 0;
        }
        public string convertStatusToString(CStatus xStatus)
        {
            switch (xStatus)
            {
                case CStatus.busy:
                    return "BUSY";
                    break;
                case CStatus.end_of_test:
                    return "END_OF_TEST";
                    break;
                case CStatus.failed:
                    return "FAILED";
                    break;
                case CStatus.init:
                    return "INIT";
                    break;
                case CStatus.logical_running:
                    return "LOGICAL_RUNNING";
                    break;
                case CStatus.paused:
                    return "PAUSED";
                    break;
                case CStatus.ready:
                    return "READY";
                    break;
                case CStatus.stopped:
                    return "STOPPED";
                    break;
                case CStatus.test_passed:
                    return "TEST_PASSED";
                    break;
                case CStatus.test_failed:
                    return "TEST_FAILED";
                    break;
                case CStatus.retry:
                    return "RETRY";
                    break;
                default:
                    return "";
            }
        }


        public object TestInstID { get; set; }
    }
}
