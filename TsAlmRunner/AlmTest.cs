using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using TDAPIOLELib;
using TsCloudShellApi;

namespace TsAlmRunner
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

        public string GetTestPath(AlmConnection almConnection,TSTest test)
        {
            string testPathUserFieldName = new TDConnectionServant(almConnection.Connection).GetTestPathFieldName();
            var theTest = (TDAPIOLELib.Test)test.Test;

            if (theTest[testPathUserFieldName] != null)
            {
                theTest.Refresh();
                return theTest[testPathUserFieldName].ToString();
            }
            throw new Exception("Test path not selected");
        }

        public TSTest FindTest(AlmConnection almConnection, AlmParameters almParameters)
        {
            TSTestFactory tsFact = (TSTestFactory)almConnection.Connection.TSTestFactory;
                List myList = tsFact.NewList("");
                foreach (TSTest tsTest in myList)
                {
                    if ((string)tsTest.ID == almParameters.TestCycleIdInteger)
                    {
                        return tsTest;
                    }
                }

           /* var testSetRunInfo = Api.GetStringFromJson(almParameters.TestSet);
            string testSetFullPath = testSetRunInfo.test_set;
            string[] testSetSplitPsth = testSetFullPath.Split('\\');
            string testSetName = testSetSplitPsth[testSetSplitPsth.Count() - 1];
            var testSetTreeManager = (TestSetTreeManager)almConnection.Connection.TestSetTreeManager;
            var testSetPath = testSetSplitPsth[0];
            var testSetParentFolder = (TestSetFolder)testSetTreeManager.NodeByPath[testSetPath];

            var theTestSet = FindTestSet(testSetParentFolder, testSetName);
            var tsTestFact = (TSTestFactory)theTestSet.TSTestFactory;
            var tsFilter = (TDFilter)tsTestFact.Filter;
            tsFilter["TC_CYCLE_ID"] = theTestSet.ID.ToString();
            var testList = tsTestFact.NewList(tsFilter.Text);

            foreach (TSTest tsTst in testList)
            {
                if (almParameters.TestCycleIdInteger == (string)tsTst.ID)
                    return tsTst;
            }*/

            throw new Exception(string.Format("Cloud not find test with name '{0}' and id '{1}' under id '{2}'", almParameters.TestName, almParameters.TestCycleIdInteger, almParameters.GetValue("testcycle_id_integer")));
        }

        private static TestSet FindTestSet(TestSetFolder testSetParentFolder, string testSetName)
        {
            TestSet testSet1 = null;

            var tsList = testSetParentFolder.FindTestSets(testSetName);
            if (tsList != null)
            {
                if (tsList.Count > 0)
                {
                    foreach (TestSet ts in tsList)
                    {
                        if (ts.Name == testSetName)
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
                    string value = GetParameterValue(element.Name, element.ActualValue);

                    if (string.IsNullOrEmpty(value))
                        value = GetParameterValue(element.Name, element.DefaultValue);

                    var item = new TestParameters(element.Name, value);
                    parameters.Add(item);
                }
            }

            return parameters;
        }


        private string GetParameterValue(string parameterName, object html)
        {

            //string tmp = Mercury.TD.Client.Library.Utilities.StripHtmlHeader((string)html);
            //string tmp1 = Mercury.TD.Client.Library.Utilities.SanitizeHtml((string)html);
            

            return Mercury.TD.Client.Library.Utilities.RichContentToText((string)html);


            /*string parameterValue = null;
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
            catch
                {
                    throw new Exception(string.Format("Parameter value must contain plain text only. Please make sure that the value of parameter '{0}' has no special formatting or other html elements such as tables, numbering, bullets, bold, italic, etc ...", parameterName));
                }*/
            //}
            //return parameterValue;
        }
    }
}
