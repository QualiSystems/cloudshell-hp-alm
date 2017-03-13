using mshtml;
using QS.ALM.CloudShellApi;
using System;
using System.Collections.Generic;
using System.Linq;
using TDAPIOLELib;

namespace CSRAgent
{
    class AlmTest
    {
        private enum mHtml
        {
            HTML,
            HEAD,
            TITLE,
            BODY,
            DIV,
            FONT,
            SPAN
        }
        public string GetTestPath(TSTest test)
        {
            var theTest = (TDAPIOLELib.Test)test.Test;
            if (theTest["TS_USER_01"] != null)
                return theTest["TS_USER_01"].ToString();
            
            throw new Exception("Test path not selected");
        }

        public TSTest FindTest(AlmConnection almConnection, AlmParameters almParameters)
        {
            string[] strParmsArr;
            string testF = "";
            string testn = "";
            if (almParameters.TestSet.Split(',')[0].Split('\"')[0].IndexOfAny("ntest_set".ToCharArray()) > -1)
            {
                strParmsArr = almParameters.TestSet.Split(',')[0].Split('\"')[1].Split('\\');
                testF = strParmsArr[2];
                testn = strParmsArr[strParmsArr.Count() - 1];

            }
            else
            {
                throw new Exception("ERROR 99"); 
            }

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

        public List<TestParameters> GetTestParameters(TSTest tsTst)
        {
            List<TestParameters> parameters = new List<TestParameters>();
            var supportParameterValues = (ISupportParameterValues)tsTst;
            var testParametersVList = supportParameterValues.ParameterValueFactory.NewList("");

            if (testParametersVList != null)
            {
                foreach (ParameterValue element in testParametersVList)
                {
                    string str =  GetParameterValue(element.ActualValue);
                    if(str == null || str == "")
                    {
                        str = GetParameterValue(element.DefaultValue);
                    }
                    TestParameters item = new TestParameters(element.Name,str);
                    parameters.Add(item);
                }
            }

            return parameters;
        }


        private string GetParameterValue(object html)
        {
            string str = "";
            IHTMLDocument2 doc = (IHTMLDocument2)new HTMLDocument();
            doc.write((string)html);
            int count = 0;
            int count7 = 0;
            foreach (IHTMLElement el in doc.all)
            {
                switch ((mHtml)count)
                {
                    case mHtml.HTML:
                        if (el.tagName == "HTML")
                            count += 1;
                        break;
                    case mHtml.HEAD:
                        if (el.tagName == "HEAD")
                            count += 1;
                        break;
                    case mHtml.TITLE:
                        if (el.tagName == "TITLE")
                            count += 1;
                        break;
                    case mHtml.BODY:
                        if (el.tagName == "BODY")
                            count += 1;
                        break;
                    case mHtml.DIV:
                        if (el.tagName == "DIV")
                            count += 1;
                        break;
                    case mHtml.FONT:
                        if (el.tagName == "FONT")
                            count += 1;
                        break;
                    case mHtml.SPAN:
                        if (el.tagName == "SPAN")
                            count += 1;
                        break;
                }
                count7 += 1;
                if (count == 7 && count7 == 7)
                {
                    //str1 = el.tagName;
                    
                    str = el.getAttribute("outerText").ToString();
                }
                else if (count7 > count)
                {
                    return str;
                }
            }
            return str;
        }
    }
}
