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

        public TypeNode Type { get; private set; } // Test or Folder

        public TestNode(string name, TypeNode type)
        {
            Name = name;
            Type = type;
        }

        public TestNode(APIExplorerResult apiExplorerResult)
        {
            Name = apiExplorerResult.Name;
            if(apiExplorerResult.Type == "Folder")
            {
                Type = TypeNode.Folder;
            }
            else
            {
                Type = TypeNode.Test;
            }
        }
        public static TestNode[] ConvertFromArrAPIExplorerResult(ArrAPIExplorerResult arrAPIExplorerResult)
        {
            TestNode[] arrTestNode = new TestNode[arrAPIExplorerResult.Children.Length];

            for (int i = 0; i < arrAPIExplorerResult.Children.Length; ++i)
            {
                arrTestNode[i] = new TestNode(arrAPIExplorerResult.Children[i]);
            }
            return arrTestNode;
        }
    } 

    public class APIExplorerResult
    {  
        public string Name { get; private set; }
        public string Type { get; private set; } // “Test” or “Folder”

        public APIExplorerResult(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
    
    public class ArrAPIExplorerResult
    {
        public APIExplorerResult[] Children;
    }
#endregion

    #region Json

    public enum ExecutionJobResult
    {
        NotStarted,
        Completed,
        Passed,
        Failed,
        EndedWithErrors,
        EndedWithAnException,
        ManuallyStopped,
        Terminated,
        Unknown
    }
    public class ApiSuiteTemplateDetails
    {
        public ApiSuiteTemplateDetails(string testPath) { JobsDetails = new ApiJobTemplate[] { new ApiJobTemplate(testPath) }; }
        public string SuiteTemplateName { get ; set; }
        public string SuiteName { get { return Config.SuiteName; } }
        public string Description {get; set;}
        public string Type { get { return "TestShell"; } }
        public string EmailNotifications { get { return "None"; } }
        public int RemoveJobsFromQueueAfter { get { return (int)Config.QueueTimeout.TotalMinutes; } }
        public bool EndReservationOnEnd { get { return true; } }
        public ApiJobTemplate[] JobsDetails {get; private set;}
        public string ExistingReservationId { get; set; }
    }
    public class ApiJobTemplate
    {
      public ApiJobTemplate(string testPath) {Tests = new Test[]{new Test(testPath)}; }

      public ApiJobTemplate() { }
      public string Name { get { return Config.JobName; } }
      public string Description {get; set;}
      public string [] ExecutionServers { get { return new string[0]; } }
      public string LoggingProfile { get { return Config.LoggingProfile; } }
      public int EstimatedDuration { get { return 10; } }
      public bool StopOnFail { get { return false; } }
      public bool StopOnError { get { return false; } }
      public Test[] Tests { get; set; }
      public string Topology {get; set;}
      public int DurationTimeBuffer { get { return 10; } }
      public string Type { get { return "TestShell"; } }
    }
    public class Test
    {
        public Test(string testPath) { TestPath = testPath; }
        public string TestPath {get; private set;}
        public string [] Parameters { get { return new string[0]; } }
        public int EstimatedDuration { get { return 0; } }
    }

    public class ApiSuiteStatusDetails
    {
        public Guid SuiteId { get; set; }
        public string SuiteStatus { get; set; }
        public ApiJobStatusDetails[] JobsStatuses { get; set; }
    }

    public class ApiJobStatusDetails
    {
        public Guid Id { get; set; }
        public string JobState { get; set; }
    }


    public class ApiSuiteDetails  
    {
        public string SuiteId { get ; set; }
        public string SuiteName { get ; set; }
        public string SuiteTemplateName { get ; set; }
        public string Description { get ; set; }
        public string Owner { get ; set; }
        public string SuiteStatus { get ; set; }
        public string SuiteResult { get ; set; }
        public int RemainingJobs { get ; set; }
        public string StartTime { get ; set; }
        public string EndTime { get ; set; }
        public string Type { get ; set; }
        public float RemoveJobsFromQueueAfter { get ; set; }
        public bool EndReservationOnEnd { get ; set; }
        public ApiJobDetails[] JobsDetails { get; set; }       
        public string EmailNotifications { get ; set; }
    }

    public class ApiJobDetails
    {
        public string Id { get ; set; }
        public string OwnerName { get ; set; }
        public string JobState { get ; set; }
        public string JobResult { get ; set; }
        public string JobFailureDescription { get ; set; }
        public string EnqueueTime { get ; set; }
        public string StartTime { get ; set; }
        public string EndTime { get ; set; }
        public double ElapsedTime { get ; set; }
        public bool UseAnyExecutionServer { get ; set; }
        public string SelectedExecutionServer { get ; set; }
        public string SuiteId { get ; set; }
        public string ExpectedStartTime { get ; set; }
        public string Name { get ; set; }
        public string Description { get ; set; }
        public string [] ExecutionServers { get ; set; }
        public string LoggingProfile { get ; set; }
        public float EstimatedDuration { get ; set; }
        public bool StopOnFail { get ; set; }
        public bool StopOnError { get ; set; }
        public TestPaths[] Tests { get; set; }
        public string Topology { get ; set; }
        public float DurationTimeBuffer { get ; set; }
        public string EmailNotifications { get ; set; }
        public string Type { get ; set; }
    }

    public class TestPaths
    {
        public TestPaths(string testPath) { TestPath = testPath; }
        public string TestPath { get ; set; }
        public string State { get ; set; }
        public string StartTime { get ; set; }
        public string EndTime { get ; set; }
        public string Result { get ; set; }
        public string ReportId { get ; set; }
        public string ReportLink { get ; set; }
        public string [] Parameters { get ; set; }
        public string EstimatedDuration { get ; set; }
    }  
    #endregion
}


