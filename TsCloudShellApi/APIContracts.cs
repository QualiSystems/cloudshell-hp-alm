using System;
using System.Collections.Generic;
using System.Linq;

namespace TsCloudShellApi
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
            Logger.Debug("Method QS.ALM.CloudShellApi.TestNode.TestNode(APIExplorerResult) apiExplorerResult.Type = {0}", apiExplorerResult.Type);
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
        public ApiSuiteTemplateDetails(string suiteName, string jobName, string testPath, int estimatedDuration, TestParameters[] parameters)
        {
            SuiteName = suiteName;
            JobsDetails = new[] { new ApiJobTemplate(jobName, testPath, estimatedDuration, parameters) };
        }

        public string SuiteTemplateName { get; set; }
        public string SuiteName { get; set; }
        public string Description {get; set;}
        public string Type { get { return Config.TestShell; } }
        public string EmailNotifications { get { return "None"; } }
        public int RemoveJobsFromQueueAfter { get { return (int)Config.QueueTimeout.TotalMinutes; } }
        public bool EndReservationOnEnd { get { return true; } }
        public ApiJobTemplate[] JobsDetails {get; private set;}
        public string ExistingReservationId { get; set; }
    }

    public class ApiJobTemplate
    {
        public ApiJobTemplate(string jobName, string testPath, int estimatedDuration, TestParameters[] parameters)
        {
            Name = jobName;
            EstimatedDuration = estimatedDuration;
            Tests = new Test[] {new Test(testPath, parameters)};
        }

        public ApiJobTemplate()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public string[] ExecutionServers
        {
            get { return new string[0]; }
        }

        public string LoggingProfile
        {
            get { return Config.LoggingProfile; }
        }

        public int EstimatedDuration { get; set; }

        public bool StopOnFail
        {
            get { return false; }
        }

        public bool StopOnError
        {
            get { return false; }
        }

        public Test[] Tests { get; set; }
        public string Topology { get; set; }

        public int DurationTimeBuffer
        {
            get { return 10; }
        }

        public string Type
        {
            get { return Config.TestShell; }
        }
    }


    public class Test
    {
        public Test(string testPath, TestParameters[] parameters) 
        { 
            TestPath = testPath;
            if (parameters == null)
            {
                Parameters = new List<TestParameters>();
            }
            else
            {
                Parameters = parameters.ToList();
            }
        }
        public string TestPath {get; private set;}
        public List<TestParameters> Parameters { get; private set; }
        public int EstimatedDuration { get { return 0; } }
    }

    public class TestParameters
    {
        public TestParameters(string name, string value) {ParameterName = name; ParameterValue = value;}
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
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
        public TestParameters[] Parameters { get; set; }
        public string EstimatedDuration { get ; set; }
    }

    public class APITestExplorerTestInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? Duration { get; set; }
        public APITestParameterInfo[] Parameters { get; set; }
    }

    public class APITestParameterInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Dimension { get; set; }
        public string Direction { get; set; }
        public string[] PossibleValues { get; set; }
    }

    #endregion
}


