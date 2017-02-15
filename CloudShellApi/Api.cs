using System;
using System.Threading;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using log4net;

namespace QS.ALM.CloudShellApi
{
    public class Api
    {
        //private static RestClient m_RestClient = null;
        //private static string m_Authorization = "";

        private readonly string m_UrlStringServer = "";
        private readonly string m_UserName = "";
        private readonly string m_UserPassword = "";
        private readonly string m_LoginContentType = "application/x-www-form-urlencoded";
        private readonly string m_Domain = "";

        public Api(string urlString, string username, string password, string domain)
        {
            m_UrlStringServer = urlString;
            m_UserName = username;
            m_UserPassword = password;
            m_Domain = domain;
        }

        private void Login(out RestClient client, out string authorization, out string contentError, out bool isSuccess)
        {
            isSuccess = false;
            string connectProperty = "url = '" + m_UrlStringServer + "', domain = '" + m_Domain +
                                        "', username = '" + m_UserName + "'." + System.Environment.NewLine;
            client = null;
            authorization = "";
            contentError = "";
            isSuccess = false;           
            string mypath = System.IO.Directory.GetCurrentDirectory();
            try
            {
                client = new RestClient(m_UrlStringServer);
            }
            catch (System.Exception e)
            {
                contentError = connectProperty + e.Message;
                LogerErrorException("Login", contentError, e);
                return;
            }

            var request = new RestRequest("/api/Auth/Login", Method.PUT);
            request.AddHeader("Content-Type", m_LoginContentType);
            request.AddJsonBody(new { username = m_UserName, password = m_UserPassword, domain = m_Domain });

            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (System.Exception e)
            {
                contentError = connectProperty + e.Message;
                LogerErrorException("Login", contentError, e);
                return;
            }

            if (IsHttpStatusCodeSuccess(res.StatusCode))
            {
                authorization = "Basic " + res.Content.Trim(new char[] { '\"' });
                isSuccess = true;
                return;
            }
            else if (res.StatusCode == 0)
            {
                contentError = connectProperty + "Connect With Server Error";
                LogerContentError("Login", contentError);
            }
            else
            {
                contentError = connectProperty + res.Content;
                LogerRestSharpError("GetNodes", contentError, res);
            }
        }

        public TestNode[] GetNodes(string parentPath, out string contentError, out bool isSuccess)
        {
            string authorization = "";
            RestClient client = null;

            Login(out client, out authorization, out contentError, out isSuccess);
            if(!isSuccess)
            {
                return null;
            }

            var request = new RestRequest("/api/Scheduling/Explorer/" + parentPath, Method.GET);
            request.AddHeader("Authorization", authorization);
            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (System.Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LogerErrorException("GetNodes", contentError, e);
                return null;
            }
            isSuccess = true;

            if (!IsHttpStatusCodeSuccess(res.StatusCode))
            {
                contentError = "Error " + ((int)res.StatusCode).ToString() + System.Environment.NewLine + res.Content;
                isSuccess = false;
                LogerRestSharpError("GetNodes", contentError, res);                
                return null;
            }

            string content = res.Content.Trim(new char[] { '[', ']' });
            if(content == "")// Empty, not collect other nodes
            {
                contentError = "";
                return null;
            }

            ArrAPIExplorerResult arrAPIExplorerResult = null;
            try
            {
                arrAPIExplorerResult = JsonConvert.DeserializeObject<ArrAPIExplorerResult>(content);
            }
            catch (System.Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LogerErrorException("GetNodes", contentError, e);
                return null;
            }

            TestNode[] arrTestNode = new TestNode[arrAPIExplorerResult.Children.Length];


            for (int i = 0; i < arrAPIExplorerResult.Children.Length; ++i )
            {
                if (arrAPIExplorerResult.Children[i].Type == "Folder")
                {
                    arrTestNode[i] = new TestNode(arrAPIExplorerResult.Children[i].Name, TypeNode.Folder);
                }
                else
                {
                    arrTestNode[i] = new TestNode(arrAPIExplorerResult.Children[i].Name, TypeNode.Test);
                }
            }
            contentError = "";
            return arrTestNode;
        }

        private void LogerErrorException(string method, string contentError, System.Exception e)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api.{0}: ContentError = '{1}'," + Environment.NewLine + "Exception = '{2}'", 
                                                                                                    method, contentError, e.ToString());
        }

        private void LogerContentError(string method, string contentError)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api{0}: ContentError = '{1}'", method, contentError);
        }

        private void LogerRestSharpError(string method, string contentError, IRestResponse res)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api.{0}: ContentError = '{1}'" + 
            Environment.NewLine + "ErrorMessage = '{2}'" +
            Environment.NewLine + "ErrorException = '{3}'" + 
            Environment.NewLine +  "StatusCode = '{4}'",
            method, contentError, res.ErrorMessage, res.ErrorException == null ? "null" : res.ErrorException.ToString(), 
            ((int)res.StatusCode).ToString());
        }

        public bool RunTest(string testPath, out string error)
        {
            Logger.Info("Run test: " + testPath);

            if (testPath.ToLower() == "root\\dummy test1" || testPath.ToLower() == "root\\dummy test2")
            {
                Thread.Sleep(1000);
                error = null;
            }
            else
            {
                Thread.Sleep(2000);
                error = "Test not found: " + testPath;
            }

            var success = error == null;

            if (success)
                Logger.Info("Run started: {0}", testPath);
            else
                Logger.Warn("Run NOT started: {0}: {1}", testPath, error);

            return success;
        }

        public TestStatus GetTestStatus(string testPath)
        {
            if (testPath.ToLower() == "root\\dummy test1")
                return TestStatus.Running;

            if (testPath.ToLower() == "root\\dummy test2")
                return TestStatus.Passed;

            return TestStatus.Failed;
        }

        private bool IsHttpStatusCodeSuccess(HttpStatusCode code)
        {
            return ((int)code >= 200 && (int)code < 300);
        }
    }
}
