using System;
using System.Collections.Generic;
using QS.ALM.CloudShellApi;

namespace TsAlmRunner
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
    }
}
