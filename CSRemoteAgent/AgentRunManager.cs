using System;
using System.Collections.Generic;
using QS.ALM.CloudShellApi;

namespace CSRAgent
{
    class AgentRunManager
    {
        public string RunTest(Api api, string testPath, List<TestParameters> testParameters)
        {
            string contentError;
            bool isSuccess;
            //string guiId = api.RunTest(testPath, null, out contentError, out isSuccess);
            string guiId = api.RunTest(testPath, testParameters, out contentError, out isSuccess);
            string reportLink = null;

            if (isSuccess)
            {
                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(guiId, api))
                    runResult = runStatusManager.WaitForRunEnd();

                ApiSuiteDetails apiDetail = api.GetRunResult(guiId, out contentError, out isSuccess);

                reportLink = apiDetail.JobsDetails[0].Tests[0].ReportLink;
            }
            else
            {
                throw new Exception(contentError);
            }

            return reportLink;
        }

        /*public string getStatus()
        {
            return convertStatusToString(mStatus);
        }

        private string convertStatusToString(CStatus xStatus)
        {
            switch (xStatus)
            {
                case CStatus.busy:
                    return "BUSY";
                case CStatus.end_of_test:
                    return "END_OF_TEST";
                case CStatus.failed:
                    return "FAILED";
                case CStatus.init:
                    return "INIT";
                case CStatus.logical_running:
                    return "LOGICAL_RUNNING";
                case CStatus.paused:
                    return "PAUSED";
                case CStatus.ready:
                    return "READY";
                case CStatus.stopped:
                    return "STOPPED";
                case CStatus.test_passed:
                    return "TEST_PASSED";
                case CStatus.test_failed:
                    return "TEST_FAILED";
                case CStatus.retry:
                    return "RETRY";
                default:
                    return "";
            }
        }*/
    }
}
