using System;
using System.Linq;
using TDAPIOLELib;

namespace CSRAgent
{
    class AlmTest
    {
        public string GetTestPath(TSTest test)
        {
            var theTest = (Test)test.Test;
            if (theTest["TS_USER_01"] != null)
                return theTest["TS_USER_01"].ToString();
            
            throw new Exception("Test path not selected");
        }

        public TSTest FindTest(AlmConnection almConnection, AlmParameters almParameters)
        {
            string[] strParmsArr;
            string testF = "";
            string testn = "";
            //int TestInstID = Convert.ToInt32(almParameters.Params["testcycle_id_integer"]);
            if (almParameters.TestSet.Split(',')[0].Split('\"')[0].IndexOfAny("ntest_set".ToCharArray()) > -1)
            {
                strParmsArr = almParameters.TestSet.Split(',')[0].Split('\"')[1].Split('\\');
                testF = strParmsArr[2];
                testn = strParmsArr[strParmsArr.Count() - 1];

            }
            else
            {
                throw new Exception("ERROR 99"); //TODO
            }

            //TreeManager treeMgr = (TreeManager)almConnection.Connection.TreeManager;
            //SubjectNode subjectNode = (SubjectNode)treeMgr.get_NodeByPath("Subject\\" + testF);
            TestSetTreeManager testSetFolderF = (TestSetTreeManager)almConnection.Connection.TestSetTreeManager;
            TestSetFolder tstSetFolder = (TestSetFolder)testSetFolderF.NodeByPath["Root"];

            var theTestSet = FindTestSet(tstSetFolder, testn);
            string testName = almParameters.TestName;
            var TSTestFact = (TSTestFactory)theTestSet.TSTestFactory;
            var tsFilter = (TDFilter)TSTestFact.Filter;
            tsFilter["TC_CYCLE_ID"] = theTestSet.ID.ToString();
            var testList = TSTestFact.NewList(tsFilter.Text);

            foreach (TSTest TSTst in testList)
            {
                if (testName == TSTst.TestName)
                    return TSTst;;
            }

            throw new Exception("ERROR 98"); //TODO
        }

        private static TestSet FindTestSet(TestSetFolder testSetF, string tsName)
        {
            TestSet testSet1 = null;

            var tsList = testSetF.FindTestSets(tsName);
            if (tsList != null)
            {
                if (tsList.Count > 0)
                {
                    foreach (TestSet ts in tsList)
                    {
                        if (ts.Name == tsName)
                        {
                            testSet1 = ts;
                            break;
                        }
                    }
                }
            }

            if (testSet1 == null)
                throw new Exception("testSet not found");

            return testSet1;
        }

        public object GetTestParameters(TSTest tsTst)
        {
            var supportParameterValues = (ISupportParameterValues)tsTst;
            var testParametersVList = supportParameterValues.ParameterValueFactory.NewList("");

            if (testParametersVList != null)
            {
                foreach (ParameterValue element in testParametersVList)
                {
                    string value = GetParameterValue(element.ActualValue);
                    string Ls = element.Name;
                }
            }

            return null;
        }

        private string GetParameterValue(object p)
        {
            return "";
        }
    }
}
