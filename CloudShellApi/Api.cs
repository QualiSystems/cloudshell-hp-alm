using System.Threading;

namespace QS.ALM.CloudShellApi
{
    public class Api
    {
        public TestNode GetTestsRoot()
        {
            var root = new TestNode("Root");
            root.Children.AddRange(new[] {new TestNode("Dummy test1"), new TestNode("Dummy test2")});
            return root;
        }

        public bool RunTest(string testPath, out string error)
        {
            Logger.Info("Run test: " + testPath);

            if (testPath.ToLower() == "root\\dummy test1" || testPath.ToLower() == "root\\dummy test2")
            {
                Thread.Sleep(1000);
                error = null;
            }
            else
            {
                Thread.Sleep(2000);
                error = "Test not found: " + testPath;
            }

            var success = error == null;

            if (success)
                Logger.Info("Run started: {0}", testPath);
            else
                Logger.Warn("Run NOT started: {0}: {1}", testPath, error);

            return success;
        }

        public TestStatus GetTestStatus(string testPath)
        {
            if (testPath.ToLower() == "root\\dummy test1")
                return TestStatus.Running;

            if (testPath.ToLower() == "root\\dummy test2")
                return TestStatus.Passed;

            return TestStatus.Failed;
        }
    }
}
