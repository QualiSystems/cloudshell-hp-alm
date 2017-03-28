using System;

namespace TsCloudShellApi
{
    public class ResultsHelper
    {
        private readonly Logger m_Logger;

        public ResultsHelper(Logger logger)
        {
            m_Logger = logger;
        }

        public ExecutionJobResult GetRunResult(ApiSuiteDetails cloudShellStatus)
        {
            if (cloudShellStatus == null)
            {
                m_Logger.Error("CloudShellStatus is null");
                return ExecutionJobResult.Unknown;
            }

            if (cloudShellStatus.JobsDetails.Length == 0)
            {
                m_Logger.Error("CloudShellStatus.JobsDetails is null");
                return ExecutionJobResult.Unknown;
            }

            var jobResult = cloudShellStatus.JobsDetails[0].JobResult;

            m_Logger.Debug("JobResult = {0}", jobResult);
            
            switch(jobResult)
            {
                case "NotStarted" :
                    return ExecutionJobResult.NotStarted;
                case "Terminated":
                    return ExecutionJobResult.Terminated;
                case "Completed":
                    return ExecutionJobResult.Completed;
                case "EndedWithErrors":
                    return ExecutionJobResult.EndedWithErrors;
                case "Passed":
                    return ExecutionJobResult.Passed;
                case "Failed":
                    return ExecutionJobResult.Failed;
                case "EndedWithAnException":
                    return ExecutionJobResult.EndedWithAnException;
                case "ManuallyStopped":
                    return ExecutionJobResult.ManuallyStopped;
                default :
                    m_Logger.Error("Invalid JobResult = {0}", jobResult);
                    return ExecutionJobResult.Unknown;
            }
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

        public static string AlmRunStatusToString(AlmRunStatus status)
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