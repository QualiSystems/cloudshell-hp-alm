using System;
using System.Threading;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Authenticators;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace QS.ALM.CloudShellApi
{
    public static class Api
    {
        private static RestClient m_RestClient = null;
        private static string m_Authorization = "";

        private static string m_UrlStringServer = "";
        private static string m_UserName = "";
        private static string m_UserPassword = "";
        private static string m_LoginContentType = "";
        private static string m_Domain = "";

        public static void Login ( string urlString, string contentType, string username, string password,
                                  string domain, out string contentError, out bool isSuccess)
        {
            m_UrlStringServer = urlString;
            m_UserName = username;
            m_UserPassword = password;
            m_Domain = domain;
            m_LoginContentType = contentType;
            RestClient client;
            isSuccess = false;
            string connectProperty = "url = '" + urlString + "', domain = '" + domain +
                                        "', username = '" + username + "'." + System.Environment.NewLine;
            try
            {
                client = new RestClient(urlString);
            }
            catch (System.Exception e)
            {
                contentError = connectProperty + e.Message;                
                return;
            }

            var request = new RestRequest("/api/Auth/Login", Method.PUT);
            request.AddHeader("Content-Type", contentType);
            request.AddJsonBody(new { username = username, password = password, domain = domain});

            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (System.Exception e)
            {
                contentError = connectProperty + e.Message;
                return;
            }

            if ((int)res.StatusCode >= 200 && (int)res.StatusCode < 300)
            {
                m_RestClient = client;
                m_Authorization = "Basic " + res.Content.Trim(new char[] { '\"' });
                isSuccess = true;
                contentError = "";
                return;
            }
            else if (res.StatusCode == 0)
            {
                contentError = connectProperty + "Connect With Server Error";
            }
            else
            {
                contentError = connectProperty + res.Content;
            }
            m_RestClient = null;
            m_Authorization = "";
        }

        public static TestNode[] GetNodes(string parentPath, out string contentError, out bool isSuccess)
        {
            if(m_RestClient == null || m_Authorization == "")
            {
                Login(m_UrlStringServer, m_LoginContentType, m_UserName, m_UserPassword,
                                            m_Domain, out contentError, out isSuccess);
                if(m_RestClient == null || m_Authorization == "")
                {
                    return null;
                }
            }

            var request = new RestRequest("/api/Scheduling/Explorer/" + parentPath, Method.GET);
            request.AddHeader("Authorization", m_Authorization);
            IRestResponse res;
            try
            {
                res = m_RestClient.Execute(request);
            }
            catch (System.Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                return null;
            }
            isSuccess = true;            
            
            if ((int)res.StatusCode < 200 || (int)res.StatusCode >= 300)
            {
                contentError = "Error " + ((int)res.StatusCode).ToString() + System.Environment.NewLine + res.Content;
                isSuccess = false;
                return null;
            }

            string content = res.Content.Trim(new char[] { '[', ']' });
            if(content == "")
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


        public static TestNode GetTestsRoot()
        {
            var root = new TestNode("Root", TypeNode.Folder);
            root.Children.AddRange(new[] {new TestNode("Dummy test1", TypeNode.Test), new TestNode("Dummy test2", TypeNode.Test)});
            return root;
        }

        public static bool RunTest(string testPath, out string error)
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

        public static TestStatus GetTestStatus(string testPath)
        {
            if (testPath.ToLower() == "root\\dummy test1")
                return TestStatus.Running;

            if (testPath.ToLower() == "root\\dummy test2")
                return TestStatus.Passed;

            return TestStatus.Failed;
        }
    }
}
