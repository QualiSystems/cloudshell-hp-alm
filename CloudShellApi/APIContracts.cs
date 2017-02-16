using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QS.ALM.CloudShellApi
{
#region treeNodes
    public enum TypeNode
    {
        Folder,
        Test
    }

    public enum TestStatus
    {
        NotRunning,
        Running,
        Passed,
        Failed
    }

    public class TestNode
    {
        public string Name { get; private set; }

        public TypeNode Type { get; set; } // Test or Folder

        public TestNode(string name, TypeNode type)
        {
            Name = name;
            Type = type;
        }
    } 

    class APIExplorerResult
    {  
        public string Name { get; private set; }
        public string Type { get; private set; } // “Test” or “Folder”

        public APIExplorerResult(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
    
    class ArrAPIExplorerResult
    {
        public APIExplorerResult[] Children;
    }
#endregion

#region Json
    public class SuiteDetails
    {
        public SuiteDetails(string testPath) { JobsDetails = new JobDetails[] { new JobDetails("TestShell\\Tests\\" + testPath.Replace('/', '\\')) }; }
        public string SuiteTemplateName { get ; set; }
        public string SuiteName { get { return Config.SuiteName; } }
        public string Description {get; set;}
        public string Type { get { return "TestShell"; } }
        public string EmailNotifications { get { return "None"; } }
        public int RemoveJobsFromQueueAfter { get { return (int)Config.QueueTimeout.TotalMinutes; } }
        public bool EndReservationOnEnd { get { return true; } }
        public JobDetails[] JobsDetails {get; set;}
        public string ExistingReservationId { get; set; }
    }

    public class JobDetails
    {
      public JobDetails(string testPath) {Tests = new Test[]{new Test(testPath)}; }
      public string Name { get { return Config.JobName; } }
      public string Description {get; set;}
      public string [] ExecutionServers { get { return new string[0]; } }
      public string LoggingProfile { get { return Config.LoggingProfile; } }
      public int EstimatedDuration { get { return 10; } }
      public bool StopOnFail { get { return false; } }
      public bool StopOnError { get { return false; } }
      public Test [] Tests {get; set;}
      public string Topology {get; set;}
      public int DurationTimeBuffer { get { return 10; } }
      public string Type { get { return "TestShell"; } }
    }

    public class Test
    {
        public Test(string testPath) { TestPath = testPath; }
        public string TestPath {get; set;}
        public string [] Parameters { get { return new string[0]; } }
        public int EstimatedDuration { get { return 0; } }
    }
#endregion
}


