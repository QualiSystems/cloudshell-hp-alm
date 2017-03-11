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
            string guiId = api.RunTest(testPath, testParameters, out contentError, out isSuccess);

            if (isSuccess)
            {
                ExecutionJobResult runResult;

                using (var runStatusManager = new RunStatusManager(guiId, api))
                    runResult = runStatusManager.WaitForRunEnd();

                Logger.Info("Test ended with result: " + runResult);

                switch (runResult)
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
        
            throw new Exception(contentError);
        }
    }
}
