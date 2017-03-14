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
        private enum ParameterHtmlElements
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
            string testn;
            var testSet = almParameters.TestSet;
            var testSetSplit = testSet.Split(',');
            var first = testSetSplit[0];
            var firstSplit = first.Split('\"');

            if (firstSplit[0].IndexOfAny("ntest_set".ToCharArray()) > -1)
            {
                var strParmsArr = firstSplit[1].Split('\\');
                //testF = strParmsArr[2];
                testn = strParmsArr[strParmsArr.Count() - 1];

            }
            else
            {
                throw new Exception("ERROR 99");
            }

            var testSetFolderF = (TestSetTreeManager)almConnection.Connection.TestSetTreeManager;
            var tstSetFolder = (TestSetFolder)testSetFolderF.NodeByPath["Root"];

            var theTestSet = FindTestSet(tstSetFolder, testn);
            var testName = almParameters.TestName;
            var testId = almParameters.TestCycleIdInteger;
            var tsTestFact = (TSTestFactory)theTestSet.TSTestFactory;
            var tsFilter = (TDFilter)tsTestFact.Filter;
            tsFilter["TC_CYCLE_ID"] = theTestSet.ID.ToString();
            var testList = tsTestFact.NewList(tsFilter.Text);

            foreach (TSTest tsTst in testList)
            {
                if (testName == tsTst.TestName && testId == (string)tsTst.ID)
                    return tsTst;;
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
            var parameters = new List<TestParameters>();
            var supportParameterValues = (ISupportParameterValues)tsTst;
            var testParametersVList = supportParameterValues.ParameterValueFactory.NewList("");

            if (testParametersVList != null)
            {
                foreach (ParameterValue element in testParametersVList)
                {
                    string str = GetParameterValue(element.Name, element.ActualValue);
                    
                    if(string.IsNullOrEmpty(str))
                        str = GetParameterValue(element.Name, element.DefaultValue);
                    
                    var item = new TestParameters(element.Name, str);
                    parameters.Add(item);
                }
            }

            return parameters;
        }


        private string GetParameterValue(string parameterName, object html)
        {
            string parameterValue = null;
            IHTMLDocument2 doc = (IHTMLDocument2)new HTMLDocument();
            doc.write((string)html);

            // Verify that the html got exactly 7 elements.
            // We do this since the user is using a html editor to edit the paraemter value
            int count = 0;
            int count7 = 0;
            foreach (IHTMLElement el in doc.all)
            {
                switch ((ParameterHtmlElements)count)
                {
                    case ParameterHtmlElements.HTML:
                        if (el.tagName == "HTML")
                            count += 1;
                        break;
                    case ParameterHtmlElements.HEAD:
                        if (el.tagName == "HEAD")
                            count += 1;
                        break;
                    case ParameterHtmlElements.TITLE:
                        if (el.tagName == "TITLE")
                            count += 1;
                        break;
                    case ParameterHtmlElements.BODY:
                        if (el.tagName == "BODY")
                            count += 1;
                        break;
                    case ParameterHtmlElements.DIV:
                        if (el.tagName == "DIV")
                            count += 1;
                        break;
                    case ParameterHtmlElements.FONT:
                        if (el.tagName == "FONT")
                            count += 1;
                        break;
                    case ParameterHtmlElements.SPAN:
                        if (el.tagName == "SPAN")
                            count += 1;
                        break;
                }
                count7 += 1;

                if (count == 7 && count7 == 7)
                {
                    parameterValue = el.getAttribute("outerText").ToString();
                    break;
                }
                
                if (count7 > count)
                {
                    throw new Exception(string.Format("Parameter value must contain plain text only. Please make sure that the value of parameter '{0}' has no special formatting or other html elements such as tables, numbering, bullets, bold, italic, etc ...", parameterName));
                }
            }
            return parameterValue;
        }
    }
}
