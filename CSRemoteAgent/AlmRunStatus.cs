namespace CSRAgent
{
    static class AlmRunStatus
    {
        public const string Busy = "BUSY"; //The testing tool is currently running another test.
        public const string EndOfTest = "END_OF_TEST"; //The testing tool has reached the end of the current test.
        public const string Failed = "FAILED"; //The testing tool has failed.
        public const string Init = "INIT"; //The testing tool is in its initialization stage.
        public const string LogicalRunning = "LOGICAL_RUNNING"; //The testing tool is running the test.
        public const string Paused = "PAUSED"; //The testing tool has paused execution of the current test.
        public const string Ready = "READY"; //The testing tool is ready to run the test.
        public const string Stopped = "STOPPED"; //The testing tool has stopped execution of the current test.
        public const string TestPassed = "TEST_PASSED"; //The test has been successfully completed.
        public const string TestFailed = "TEST_FAILED"; //The test failed.
        public const string Retry = "RETRY"; //You cannot execute the test on the current host. Try to execute the test on another host from the attached host group
    }
}