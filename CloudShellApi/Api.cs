using System.Threading;

namespace QS.ALM.CloudShellApi
{
    public static class Api
    {
        public static TestNode[] GetNodes(string parentPath)
        {
            if (parentPath == null || parentPath == "")
            return new[] { new TestNode("local", TypeNode.Folder), new TestNode("shared", TypeNode.Folder) };

            if (parentPath == "local")
                return new[] { new TestNode("Folder1", TypeNode.Folder), new TestNode("Folder2", TypeNode.Folder), new TestNode("test1", TypeNode.Test) };

            if (parentPath == "local\\folder1")
                return new[] { new TestNode("test3", TypeNode.Test) };

            return null;

        }


        public static TestNode GetTestsRoot()
        {
            var root = new TestNode("Root", TypeNode.Folder);
            root.Children.AddRange(new[] {new TestNode("Dummy test1", TypeNode.Test), new TestNode("Dummy test2", TypeNode.Test)});
            return root;
        }

        public static bool RunTest(string testPath, out string error)
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

        public static TestStatus GetTestStatus(string testPath)
        {
            if (testPath.ToLower() == "root\\dummy test1")
                return TestStatus.Running;

            if (testPath.ToLower() == "root\\dummy test2")
                return TestStatus.Passed;

            return TestStatus.Failed;
        }
    }
}
