using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using TDAPIOLELib;
using QS.ALM.CloudShellApi;
using HP.ALM.QC.OTA.Entities.Api;


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
                m_Api = new Api("http://192.168.42.35:9000", "admin", "admin", null, null, AuthenticationMode.Alm, "Global");
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
               // MessageBox.Show("Result Test = \"" + guiId + '\"', "Returned Key", MessageBoxButtons.OK);

                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(guiId, m_Api))
                    runResult = runStatusManager.WaitForRunEnd();

                //MessageBox.Show("Run Result = " + runResult.ToString());

                //~TODO: write the runResult to ALM ...
            }
            else
            {
                //MessageBox.Show(contentError, "Error", MessageBoxButtons.OK);

                throw new System.InvalidOperationException(contentError);
            }   

            return 0;
        }

        public string getStatus()
        {
            return convertStatusToString(mStatus);
        }
        public int getTestParameters(TDAPIOLELib.Test testSet)
        {

            /*TDAPIOLELib.ISupportTestParameters supportParamTest;
            supportParamTest = conn.TestFactory.Item(5);*/
            
            string str = conn.get_TDParams("Name");
            string str0 = conn.get_TDParams("Parame Name");
            string str1 = conn.get_TDParams("Parame_Name");
            string str2 = conn.get_TDParams("Parameters");
           // supportParamTest = conn.get_TDParams("Parame Name");//( TestFactory TDConnection.TestFactory.Item(5)
            
           // var paramFactory = (Mercury.TD.Client.Ota.QC9.FactoryListClass<HP.ALM.QC.OTA.Entities.Api.ITestParameter, HP.ALM.QC.OTA.Entities.Api.ITest>)testSet.Params; //.ParameterFactory;
            //var baseFactory = (TDAPIOLELib.IBaseFactory)paramFactory.LegacyFactory;

           /* List baseList = baseFactory.NewList("");
            if (baseList != null)
            {
                foreach (var element in baseList)
                {
                    baseFactory.RemoveItem(element);
                }
            }
            if (arrParameters != null)
            {
                for (int i = 0; i < arrParameters.Length; ++i)*/
              //  {
                   /* HP.ALM.QC.OTA.Entities.Api.ITestParameter newEnt = paramFactory.NewEntity();
                    newEnt.Name = arrParameters[i].Name;
                    newEnt.Description = arrParameters[i].Description;
                    newEnt.Post();*/
              //  }
            //}

           /* TestConfigFactory testConfigFact = (TestConfigFactory)testSet.TestConfigFactory;
            TestFactory testFact = (TestFactory)testSet.TestCriterionFactory;

            List testList = (List)testFact.NewList("");



            for (int i = 0; i < testList.Count; i++)
            {

                Object testConfig = testList[i];



                //TDAPIOLELib.Test testConfigTest = (TDAPIOLELib.Test)testConfigTestFact[testConfig.ID];
            }*/
            return 0;
           /* TSTestFactory tsf1;
            tsf1 = (TSTestFactory)testSet.TestCriterionFactory;//.TSTestFactory;
            List testlist1;
            testlist1 = tsf1.NewList("");
            foreach (TSTest test in testlist1)
            {
                ISupportParameterValues aParam;
                IBaseFactory paramValueFct;
                ParameterValue ramava;
                TSTest aTS;
                List aList;
                aTS = test;
                aParam = (
                ISupportParameterValues)aTS;
                paramValueFct = aParam.ParameterValueFactory;
                TDAPIOLELib.List lst = paramValueFct.NewList("");
                ramava = (ParameterValue)lst[1];
                string test3355 = ramava.ActualValue.ToString();
            }*/

          /*  TestFactory testFactory = (TestFactory)conn.TestFactory;
            TDFilter tffilter = (TDFilter)testFactory.Filter;
            tffilter["TS_NAME"] = Params["test_name"].ToString();
            List testList = (List)testFactory.NewList(tffilter.Text);
            //trace through each test cases to get the status and parms
            foreach (TDAPIOLELib.Test test in testList)
            {
                ISupportParameterValues aParam;
                IBaseFactory paramValueFct;
                ParameterValue ramava;
                TDAPIOLELib.Test aTS;
                List aList;
                aTS = test;
                aParam = (ISupportParameterValues)aTS;
                paramValueFct = aParam.ParameterValueFactory;
                TDAPIOLELib.List lst = paramValueFct.NewList(""); // throws exception {"Failed to Get Simple Key Entity"} System.Exception {System.Runtime.InteropServices.COMException}

                ramava = (ParameterValue)lst[1];
                string test3355 = ramava.ActualValue.ToString();

                Console.WriteLine("Name:" + test.Name + "hasParms" + test.HasParam + "Parms:" + test.Params);

            }*/
            return 0;
        }
        public int getTestPath()
        {
            
                TreeManager treeMgr = (TreeManager)conn.TreeManager;
                SubjectNode subjectNode = (SubjectNode)treeMgr.get_NodeByPath("Subject\\" + Params["test_set"].Split('\\')[2].ToString());// f1");

                TDAPIOLELib.Test theTest;
                List tList =  subjectNode.FindTests(Params["test_name"].ToString());
                
               /* if (tList.Count > 1 )
                {
                    MessageBox.Show("FindTests found more than one test: refine search");
                }
                else */if (tList.Count < 1 )
                {
                   // MessageBox.Show("FindTests: test not found");

                    throw new System.InvalidOperationException("FindTests: test not found");
                }

                theTest = (TDAPIOLELib.Test)tList[1];

                m_TestPath = theTest["TS_USER_01"].ToString();

                getTestParameters(theTest);


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

    }
}
