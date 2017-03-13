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

        public ExecutionJobResult RunTest()
        {
            string contentError;
            bool isSuccess;
            string guid = m_Api.RunTest(m_TestPath, m_TestParameters, out contentError, out isSuccess);

            if (!isSuccess) 
                throw new Exception(contentError);
            
            ExecutionJobResult runResult;

            using (var runStatusManager = new RunStatusManager(m_Api, guid))
                runResult = runStatusManager.WaitForRunEnd();

            Logger.Info("Test ended with result: " + runResult);
            return runResult;
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

        public static string ConvertAlmRunStatusToString(AlmRunStatus almRunStatus)
        {
            switch (almRunStatus)
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
                    throw new Exception("Invalid AlmRunStatus: " + almRunStatus);
            }
        }
    }
}
