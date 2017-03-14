using System;
using System.Collections.Generic;
using QS.ALM.CloudShellApi;

namespace CSRAgent
{
    class AgentRunManager
    {
        private readonly Api m_Api;
        private readonly string m_TestPath;
        private readonly List<TestParameters> m_TestParameters;

        public AgentRunManager(Api api, string testPath, List<TestParameters> testParameters)
        {
            m_Api = api;
            m_TestPath = testPath;
            m_TestParameters = testParameters;
        }
        
        public string RunTest()
        {
            string contentError;
            bool isSuccess;
            var runGuid = m_Api.RunTest(m_TestPath, m_TestParameters, out contentError, out isSuccess);

            if (!isSuccess)
                throw new Exception(contentError);
            
            using (var runStatusManager = new RunStatusManager(m_Api, runGuid))
                runStatusManager.WaitForRunEnd();
            
            return runGuid;
        }


        public static AlmRunStatus ConvertTestShellResultToAlmRunStatus(ExecutionJobResult executionJobResult)
        {
            switch (executionJobResult)
            {
                case ExecutionJobResult.Completed:
                case ExecutionJobResult.Passed:
                    return AlmRunStatus.TestPassed;
                case ExecutionJobResult.Failed:
                    return AlmRunStatus.TestFailed;
                default:
                    return AlmRunStatus.Failed;
            }
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

        public static string ConvertAlmRunStatusToString(AlmRunStatus status)
        {
            switch (status)
            {
                case AlmRunStatus.Busy:
                    return "BUSY"; //The testing tool is currently running another test.
                case AlmRunStatus.EndOfTest:
                    return "END_OF_TEST"; //The testing tool has reached the end of the current test.
                case AlmRunStatus.Failed:
                    return "FAILED"; //The testing tool has failed.
                case AlmRunStatus.Init:
                    return "INIT"; //The testing tool is in its initialization stage.
                case AlmRunStatus.LogicalRunning:
                    return "LOGICAL_RUNNING"; //The testing tool is running the test.
                case AlmRunStatus.Paused:
                    return "PAUSED"; //The testing tool has paused execution of the current test.
                case AlmRunStatus.Ready:
                    return "READY"; //The testing tool is ready to run the test.
                case AlmRunStatus.Stopped:
                    return "STOPPED"; //The testing tool has stopped execution of the current test.
                case AlmRunStatus.TestPassed:
                    return "TEST_PASSED"; //The test has been successfully completed.
                case AlmRunStatus.TestFailed:
                    return "TEST_FAILED"; //The test failed.
                case AlmRunStatus.Retry:
                    return "RETRY"; //You cannot execute the test on the current host. Try to execute the test on another host from the attached host group
                default:
                    throw new Exception("Invalid AlmRunStatus: " + status);
            }
        }
    }
}
