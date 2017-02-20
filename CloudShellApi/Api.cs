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

        private string CurrentUrl
        {
            get 
            {
                string url = Config.OverrideCloudShellUrl;
                return url == null ? m_UrlStringServer : url;
            }
        }

        private string CurrentUsername 
        { 
            get 
            { 
                string username = Config.OverrideUsername;
                return username == null ? m_UserName : username;
            } 
        }

        private string CurrentPassword
        {
            get
            {
                string password = Config.OverridePassword;
                return password == null ? m_UserPassword : password;
            }
        }

        private void Login(out RestClient client, out string authorization, out string contentError, out bool isSuccess)
        {
            isSuccess = false;
            string connectProperty = "url = '" + CurrentUrl + "', domain = '" + m_Domain +
                                        "', username = '" + CurrentUsername + "'." + System.Environment.NewLine;
            client = null;
            authorization = "";
            contentError = "";
            isSuccess = false;           
            string mypath = System.IO.Directory.GetCurrentDirectory();
            try
            {
                client = new RestClient(CurrentUrl);
            }
            catch (System.Exception e)
            {
                contentError = connectProperty + e.Message;
                LogerErrorException("Login", contentError, e);
                return;
            }

            var request = new RestRequest("/api/Auth/Login", Method.PUT);
            request.AddHeader("Content-Type", m_LoginContentType);
            request.AddJsonBody(new { username = CurrentUsername, password = CurrentPassword, domain = m_Domain });

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
            if (string.IsNullOrEmpty(parentPath))
            {
                string root = Config.TestsRoot;
                if (!string.IsNullOrEmpty(root))
                {
                    root = root.Replace('\\', '/');
                    isSuccess = true;
                    contentError = "";
                    return new TestNode[] { new TestNode(root, TypeNode.Folder) };
                }
            }
            ArrAPIExplorerResult arrAPIExplorerResult = GetServerObject<ArrAPIExplorerResult>("/api/Scheduling/Explorer/" + parentPath,
                                                                                            "GetNodes", out contentError, out isSuccess);
            if(arrAPIExplorerResult == null)
            {
                return null;
            }

            TestNode[] arrTestNode = TestNode.ConvertFromArrAPIExplorerResult(arrAPIExplorerResult);

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

        public string RunTest(string testPath, out string contentError, out bool isSuccess)
        {
            string authorization = "";
            RestClient client = null;
            isSuccess = true;
            contentError = "";

            if (string.IsNullOrEmpty(testPath))
            {
                isSuccess = false;
                contentError = "Path to Test is Empty";
                return null;
            }
            testPath = testPath.Replace('\\', '/');
            Login(out client, out authorization, out contentError, out isSuccess);
            if (!isSuccess)
            {
                return null;
            }

            var request = new RestRequest("/api/Scheduling/Suites/", Method.POST);
            request.AddHeader("Authorization", authorization);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new ApiSuiteTemplateDetails("TestShell\\Tests\\" + testPath.Replace('/', '\\')));
            string content = ExecuteServerRequest(client, request, "RunTest", out contentError, out isSuccess);
            if(content == null)
            {
                return null;
            }
            return content.Trim(new char[]{'\"'});
        }

        public string ExecuteServerRequest(RestClient client, RestRequest request, string nameCallingMethod, out string contentError, out bool isSuccess)
        {
            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (System.Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LogerErrorException(nameCallingMethod, contentError, e);
                return null;
            }

            if (!IsHttpStatusCodeSuccess(res.StatusCode))
            {
                contentError = "Error " + ((int)res.StatusCode).ToString() + System.Environment.NewLine + res.Content;
                isSuccess = false;
                LogerRestSharpError(nameCallingMethod, contentError, res);
                return null;
            }

            if (res.Content == "")
            {
                isSuccess = false;
                contentError = "Uknown Error";
                return null;
            }
            isSuccess = true;
            contentError = "";
            return res.Content;
        }

        public ApiSuiteStatusDetails GetRunStatus(string m_RunGuid, out string contentError, out bool isSuccess)
        {
            return GetServerObject<ApiSuiteStatusDetails>("/api/Scheduling/Suites/Status/" + m_RunGuid,
                                                                    "GetRunStatus",  out contentError, out isSuccess);           
        }

        public ApiSuiteDetails IsRunSuccess(string m_RunGuid, out string contentError, out bool isSuccess)
        {
            return GetServerObject<ApiSuiteDetails>("/api/Scheduling/Suites/" + m_RunGuid, "IsRunSuccess", out contentError, out isSuccess);            
        }

        public T GetServerObject<T>(string nameFunctionOnServer, string nameCallingMethod, out string contentError, out bool isSuccess)
        {
            contentError = null;
            isSuccess = true;

            string authorization = "";
            RestClient client = null;
            isSuccess = true;
            contentError = "";

            Login(out client, out authorization, out contentError, out isSuccess);
            if (!isSuccess)
            {
                return default(T);//null;
            }

            var request = new RestRequest(nameFunctionOnServer, Method.GET);
            request.AddHeader("Authorization", authorization);
            request.AddHeader("Content-Type", "application/json");
            string content = ExecuteServerRequest(client, request, nameCallingMethod, out contentError, out isSuccess);
            if (content == null)
            {
                return default(T);//null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (System.Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LogerErrorException(nameCallingMethod, contentError, e);
                return default(T);//null;
            }
        }

        private bool IsHttpStatusCodeSuccess(HttpStatusCode code)
        {
            return ((int)code >= 200 && (int)code < 300);
        }
    }
}
