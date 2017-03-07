﻿using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using TDAPIOLELib;

namespace QS.ALM.CloudShellApi
{
    public class Api
    {
        private const string LoginContentType = "application/x-www-form-urlencoded";
        private string m_UrlStringServer;
        private string m_UserName;
        private string m_UserPassword;
        private string m_Domain;
        //private ITDConnection m_tdc;

        public Api(string urlString, string almUsername, string almPassword, string cloudShellUsername, string cloudShellPassword, AuthenticationMode authenticationMode, string domain)
        {
            Init(urlString, almUsername, almPassword, cloudShellUsername, cloudShellPassword, authenticationMode, domain);
        }

        public Api(ITDConnection tdConnection)
        {
            TDConnectionServant conectionServant = TDConnectionServant.GetInstance(tdConnection);
            string url = conectionServant.GetTdParam("QS_SERVER_URL");
            string almUsername = conectionServant.GetTdParam("QS_USERNAME");
            string almPassword = conectionServant.GetTdParam("QS_PASSWORD");            
            string domain = conectionServant.GetTdParam("QS_DOMAIN");
            AuthenticationMode mode = conectionServant.GetAlmMode();
                                 
            Init(url, almUsername, almPassword, null, null, mode, domain);

            //string localUsername = tdConnection.UserName;
            //string localUserPas = tdConnection.Password;
        }

        private void Init(string urlString, string almUsername, string almPassword, string cloudShellUsername, string cloudShellPassword, AuthenticationMode authenticationMode, string domain)
        {
            m_UrlStringServer = urlString;
            m_Domain = domain;

            switch (authenticationMode)
            {
                case AuthenticationMode.Alm:
                    m_UserName = almUsername;
                    m_UserPassword = almPassword;
                    break;
                case AuthenticationMode.CloudShell:
                    m_UserName = cloudShellUsername;
                    m_UserPassword = cloudShellPassword;
                    break;
                default:
                    throw new Exception("Invalid AuthenticationMode: " +  authenticationMode);
            }

            if (string.IsNullOrEmpty(m_UserName) || string.IsNullOrEmpty(m_UserPassword))
                throw new Exception(string.Format("Authentication mode is '{0}' but username or password are empty.", authenticationMode));
        }

        private string CurrentUrl
        {
            get
            {
                string url = Config.OverrideCloudShellUrl;
                return url ?? m_UrlStringServer;
            }
        }

        private string CurrentUsername
        {
            get
            {
                string username = Config.OverrideUsername;
                return username ?? m_UserName;
            }
        }

        private string CurrentPassword
        {
            get
            {
                string password = Config.OverridePassword;
                return password ?? m_UserPassword;
            }
        }

        private void Login(out RestClient client, out string authorization, out string contentError, out bool isSuccess)
        {
            isSuccess = false;
            string connectProperty = "url = '" + CurrentUrl + "', domain = '" + m_Domain + "', username = '" + CurrentUsername + "'." + Environment.NewLine;
            client = null;
            authorization = "";
            contentError = "";

            try
            {
                client = new RestClient(CurrentUrl);
            }
            catch (Exception e)
            {
                contentError = connectProperty + e.Message;
                LoggerErrorException("Login", contentError, e);
                return;
            }

            var request = new RestRequest("/api/Auth/Login", Method.PUT);
            request.AddHeader("Content-Type", LoginContentType);
            request.AddJsonBody(new {username = CurrentUsername, password = CurrentPassword, domain = m_Domain});

            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (Exception e)
            {
                contentError = connectProperty + e.Message;
                LoggerErrorException("Login", contentError, e);
                return;
            }

            if (IsHttpStatusCodeSuccess(res.StatusCode))
            {
                authorization = "Basic " + res.Content.Trim('\"');
                isSuccess = true;
            }
            else if (res.StatusCode == 0)
            {
                contentError ="Connect With Server Error" + Environment.NewLine + "Details :" + Environment.NewLine +  connectProperty;
                LoggerContentError("Login", contentError);
            }
            else
            {
                contentError = connectProperty + res.Content;
                LoggerRestSharpError("GetNodes", contentError, res);
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
                    return new[] {new TestNode(root, TypeNode.Folder)};
                }
            }
            ArrAPIExplorerResult arrApiExplorerResult = GetServerObject<ArrAPIExplorerResult>("/api/Scheduling/Explorer/" + parentPath, "GetNodes", out contentError, out isSuccess);
            if (arrApiExplorerResult == null)
            {
                return null;
            }

            TestNode[] arrTestNode = TestNode.ConvertFromArrAPIExplorerResult(arrApiExplorerResult);

            contentError = "";
            return arrTestNode;
        }

        private static void LoggerErrorException(string method, string contentError, Exception e)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api.{0}: ContentError = '{1}'," + Environment.NewLine + "Exception = '{2}'", method, contentError, e.ToString());
        }

        private static void LoggerContentError(string method, string contentError)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api{0}: ContentError = '{1}'", method, contentError);
        }

        private static void LoggerRestSharpError(string method, string contentError, IRestResponse res)
        {
            Logger.Error("QS.ALM.CloudShellApi.Api.{0}: ContentError = '{1}'" + Environment.NewLine + "ErrorMessage = '{2}'" + Environment.NewLine + "ErrorException = '{3}'" + Environment.NewLine + "StatusCode = '{4}'", method, contentError, res.ErrorMessage, res.ErrorException == null ? "null" : res.ErrorException.ToString(), ((int) res.StatusCode).ToString());
        }

        public string RunTest(string testPath, List<TestParameters> parameters, out string contentError, out bool isSuccess)
        {
            string authorization;
            RestClient client;

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
            request.AddJsonBody(new ApiSuiteTemplateDetails("TestShell\\Tests\\" + testPath.Replace('/', '\\'), parameters));
            string content = ExecuteServerRequest(client, request, "RunTest", out contentError, out isSuccess);
            if (content == null)
            {
                return null;
            }
            return content.Trim(new char[] {'\"'});
        }

        private string ExecuteServerRequest(RestClient client, RestRequest request, string nameCallingMethod, out string contentError, out bool isSuccess)
        {
            IRestResponse res;
            try
            {
                res = client.Execute(request);
            }
            catch (Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LoggerErrorException(nameCallingMethod, contentError, e);
                return null;
            }

            if (!IsHttpStatusCodeSuccess(res.StatusCode))
            {
                contentError = "Error " + ((int) res.StatusCode) + Environment.NewLine + res.Content;
                isSuccess = false;
                LoggerRestSharpError(nameCallingMethod, contentError, res);
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

        public ApiSuiteStatusDetails GetRunStatus(string runGuid, out string contentError, out bool isSuccess)
        {
            return GetServerObject<ApiSuiteStatusDetails>("/api/Scheduling/Suites/Status/" + runGuid, "GetRunStatus", out contentError, out isSuccess);
        }

        public ApiSuiteDetails GetRunResult(string runGuid, out string contentError, out bool isSuccess)
        {
            return GetServerObject<ApiSuiteDetails>("/api/Scheduling/Suites/" + runGuid, "GetRunResult", out contentError, out isSuccess);
        }

        public APITestExplorerTestInfo GetTestParameter(string path, out string contentError, out bool isSuccess)
        {
            return GetServerObject<APITestExplorerTestInfo>("/api/Scheduling/TestInfo/" + path, "GetTestParameter", out contentError, out isSuccess);
        }

        private T GetServerObject<T>(string nameFunctionOnServer, string nameCallingMethod, out string contentError, out bool isSuccess)
        {
            string authorization;
            RestClient client;

            Login(out client, out authorization, out contentError, out isSuccess);
            if (!isSuccess)
            {
                return default(T); //null;
            }

            var request = new RestRequest(nameFunctionOnServer, Method.GET);
            request.AddHeader("Authorization", authorization);
            request.AddHeader("Content-Type", "application/json");
            string content = ExecuteServerRequest(client, request, nameCallingMethod, out contentError, out isSuccess);
            if (content == null)
            {
                return default(T); //null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                contentError = e.Message;
                isSuccess = false;
                LoggerErrorException(nameCallingMethod, contentError, e);
                return default(T); //null;
            }
        }

        private static bool IsHttpStatusCodeSuccess(HttpStatusCode code)
        {
            return ((int) code >= 200 && (int) code < 300);
        }
    }
}
