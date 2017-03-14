using System;

namespace CSRAgent
{
    enum AlmRunStatus
    {
        Busy,
        EndOfTest,
        Failed,
        Init,
        LogicalRunning,
        Paused,
        Ready,
        Stopped,
        TestPassed,
        TestFailed,
        Retry
    }

    static class AlmRunStatusStrings
    {
        public static string ToString(AlmRunStatus status)
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