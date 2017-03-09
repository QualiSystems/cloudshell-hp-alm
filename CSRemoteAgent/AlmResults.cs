using TDAPIOLELib;

namespace CSRAgent
{
    class AlmResults
    {
        private readonly AlmParameters m_AlmParameters;
        private readonly TestSetFactory m_TestSetFactory;

        public AlmResults(AlmParameters almParameters, TestSetFactory testSetFactory)
        {
            m_AlmParameters = almParameters;
            m_TestSetFactory = testSetFactory;
        }

        public void SaveRunResults(string runStatus)
        {
            var testSetId = m_AlmParameters.TestSetId;
            var testSet = (TestSet)m_TestSetFactory[testSetId];
            var tsTestFactory = (TSTestFactory)testSet.TSTestFactory;
            var testCycleId = m_AlmParameters.TestCycleIdInteger;
            var tsTest = (TSTest)tsTestFactory[testCycleId];
            var runFactory = (RunFactory)tsTest.RunFactory;
            var run = (Run)runFactory.AddItem("TestShell Run");
            run["RN_TESTER_NAME"] = m_AlmParameters.UserName;
            run["RN_HOST"] = m_AlmParameters.HostName;
            run.Status = runStatus;
            run.Post();
        }
    }
}
