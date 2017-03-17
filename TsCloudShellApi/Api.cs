using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using TDAPIOLELib;

namespace TsCloudShellApi
{
    public class Api
    {
        private const string LoginContentType = "application/x-www-form-urlencoded";
        private string m_UrlStringServer;
        private string m_UserName;
        private string m_UserPassword;
        private string m_Domain;
        private readonly string m_SuiteName;
        private readonly string m_JobName;
        private readonly int m_EstimatedDuration;

        public Api(string urlString, string almUsername, string almPassword, string cloudShellUsername, string cloudShellPassword, AuthenticationMode authenticationMode, string domain)
        {
            Init(urlString, almUsername, almPassword, cloudShellUsername, cloudShellPassword, authenticationMode, domain);
        }

        public Api(ITDConnection tdConnection, string cloudShellUsername = null, string cloudShellPassword = null)
        {
            var conectionServant = new TDConnectionServant(tdConnection);
            var url = conectionServant.GetTdParam("CLOUDSHELL_SERVER_URL");
            var almUsername = conectionServant.GetTdParam("CLOUDSHELL_USERNAME");
            var almPassword = conectionServant.GetTdParam("CLOUDSHELL_PASSWORD");
            var domain = conectionServant.GetTdParam("CLOUDSHELL_DOMAIN");
            var mode = conectionServant.GetAlmMode();

            m_SuiteName = conectionServant.GetTdParam("CLOUDSHELL_SUITE_NAME", "ALM Suite"); // Changing suite name is undocumented
            m_JobName = conectionServant.GetTdParam("CLOUDSHELL_JOB_NAME", "ALM Job"); // Changing job name is undocumented

            // Changing estimated duration is undocumented
            if (!int.TryParse(conectionServant.GetTdParam("CLOUDSHELL_ESTIMATED_DURATION", "10"), out m_EstimatedDuration))
                throw new Exception("CLOUDSHELL_ESTIMATED_DURATION must have a numeric value.");

            Init(url, almUsername, almPassword, cloudShellUsername, cloudShellPassword, mode, domain);
        }

        private void Init(string urlString, string almUsername, string almPassword, string cloudShellUsername, string cloudShellPassword, AuthenticationMode authenticationMode, string domain)
        {
            m_UrlStringServer = urlString;
            m_Domain = domain;

            // Ignore AuthenticationMode in design mode. Only in run mode we have the password of the logged-in user
            if (string.IsNullOrEmpty(cloudShellPassword))
            {
                m_UserName = almUsername;
                m_UserPassword = almPassword;
            }
            else
            {
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
                        throw new Exception("Invalid authentication mode: " +  authenticationMode);
                }
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
            var connectionDetails = Environment.NewLine + string.Format("(url: {0}, domain: {1}, username: {2})", CurrentUrl, m_Domain, CurrentUsername);
            client = null;
            authorization = "";
            contentError = "";

            try
            {
                client = new RestClient(CurrentUrl);
            }
            catch (Exception e)
            {
                contentError = e.Message + connectionDetails;
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
                contentError = e.Message + connectionDetails;
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
                contentError ="Server Connection Error." + connectionDetails;
                LoggerContentError("Login", contentError);
            }
            else
            {
                contentError = res.Content + connectionDetails;
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
            testPath = "TestShell\\Tests\\" + testPath.Replace('/', '\\');
            request.AddJsonBody(new ApiSuiteTemplateDetails(m_SuiteName, m_JobName, testPath, m_EstimatedDuration, parameters));
            string content = ExecuteServerRequest(client, request, "RunTest", out contentError, out isSuccess);
            if (content == null)
            {
                return null;
            }
            return content.Trim('\"');
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
                contentError = res.Content + Environment.NewLine + string.Format("(error code {0})", (int)res.StatusCode);
                isSuccess = false;
                LoggerRestSharpError(nameCallingMethod, contentError, res);
                return null;
            }

            if (res.Content == "")
            {
                isSuccess = false;
                contentError = "Unknown Error";
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
