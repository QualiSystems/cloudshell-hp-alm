using System;
using System.Runtime.InteropServices;

/// <summary>
/// Handles communication between ALM and the testing tool.
/// </summary>
namespace CSRAgent
{
    /// <summary>
    /// Handles communication between ALM and the testing tool.
    /// </summary>
    /// <remarks> 
    /// The remote agent is responsible for control of the testing tool and returning status and results to ALM.
    /// The remote agent resides on the host machine with the testing tool and uses the DCOM protocol to 
    /// communicate over the network with the host machine on which ALM resides.
    /// <br />Your remote agent class implements the methods of this interface to enable ALM to use your testing tool.
    /// <br />Your implementation can use the ALM Open Test Architecture API 
    /// to write results and other data to ALM.
    /// </remarks> 
  [Guid("A33D1851-2BFA-4844-B42C-15A3AC202E8B"), ComVisible(true)]
  public interface IRAgent
  {

    /// <summary>
    /// Gets the current status of the testing tool.
    /// </summary>
   /// <remarks>During test execution, ALM checks the status of the testing tool and displays this information. This enables the tester to monitor the test’s progress at each stage of the run.</remarks>
    /// <param name="descr">The testing tool’s current status.</param>
    /// <param name="status">One of:
      /// <dl>
      /// <dt>busy</dt><dd>The testing tool is currently running another test.</dd>
      /// <dt>end_of_test</dt><dd>The testing tool has reached the end of the current test.</dd>
      /// <dt>failed</dt><dd>The testing tool has failed.</dd>
      /// <dt>init</dt><dd>The testing tool is in its initialization stage.</dd>
      /// <dt>logical_running</dt><dd>The testing tool is running the test.</dd>
      /// <dt>paused</dt><dd>The testing tool has paused execution of the current test.</dd>
      /// <dt>ready</dt><dd>The testing tool is ready to run the test.</dd>
      /// <dt>stopped</dt><dd>The testing tool has stopped execution of the current test.</dd>
      /// <dt>test_passed</dt><dd>The test has been successfully completed.</dd>
      /// <dt>test_failed</dt><dd>The test failed.</dd>
      /// <dt>retry</dt><dd>You cannot execute the test on the current host. Try to execute the test on another host from the attached host group.</dd>
      /// </dl>    
    /// </param>
    /// <returns>0 if the testing tool and host are ready to run tests, otherwise an error code.</returns>
    int get_status(ref string descr, ref string status);

    /// <summary>
    /// Checks if the testing tool application can be run.
    /// </summary>
    /// <remarks>Before test execution, the ALM client uses this method to ask the remote agent to ensure that the designated testing host is available and ready to run the test.</remarks>
    /// <param name="descr">The reason the host is not ready.</param>
    /// <returns>0 if the testing tool and host are ready to run tests, otherwise an error code.</returns>
    int is_host_ready(ref string descr);

    /// <summary>
    /// Starts the test run.
    /// </summary>
    /// <remarks>
    /// The run method instructs the testing tool to load and run the test.
    /// This function launches the testing tool if it is not already running.
    /// </remarks>
    /// <returns>0 if the call succeeds, otherwise an error code.</returns>
    int run();

    /// <summary>
    /// Sets the value of a run parameter.
    /// </summary>
    /// <param name="prm_name">The name of the parameter to be set. Not case-sensitive. One of:
    /// <dl>
    /// <dt>database_name</dt><dd>The name of the active ALM database.</dd>
    /// <dt>domain_name</dt><dd>The name of the ALM domain in which test and result information is stored.</dd>
    /// <dt>host_name</dt><dd>The name of the host on which the remote agent test type is run.</dd>
    /// <dt>password</dt><dd>The user’s password.</dd>
    /// <dt>pinned_baseline_id</dt><dd>The ID of the baseline to which the current test set is pinned.</dd>
    /// <dt>plann_status</dt><dd>The planning mode status of the test.</dd>
    /// <dt>project_name</dt><dd>The name of the ALM project in which test and result information is stored.</dd>
    /// <dt>responsible</dt><dd>The name of the user responsible for the project.</dd>
    /// <dt>runner_result_dir</dt><dd>The name of the test run results directory.</dd>
    /// <dt>scheduler_version</dt><dd>The software version number of the scheduler.</dd>
    /// <dt>subject</dt><dd>The subject folder to which the test belongs in the test plan tree.</dd>
    /// <dt>sys_computer_name</dt><dd>The name of the PC on which the ALM client is running.</dd>
    /// <dt>sys_user_name</dt><dd>The login user name for the user logged in on the ALM client PC.</dd>
    /// <dt>TDAPI_host_name</dt><dd>The name of the host on which the ALM Platform is running.</dd>
    /// <dt>testcycle_id_integer</dt><dd>The test instance ID.</dd>
    /// <dt>test_id</dt><dd>The ID of the test to be run.</dd>
    /// <dt>test_name</dt><dd>The name of the test to be run.</dd>
    /// <dt>test_path</dt><dd>The full path of the test to be run.</dd>
    /// <dt>test_set</dt><dd>A string of form: {test_set: "" testcycle_id: "148~1"}</dd>
    /// <dt>test_set_id</dt><dd>The ID of the test set to which the test belongs.</dd>
    /// <dt>test_set_end</dt><dd>A value of Y indicates that the last test in the test set run finished. This does not indicate success or failure of tests, only the end of the run. See also, parameter test_set_start.</dd>
    /// <dt>test_set_start</dt><dd>A value of Y indicates that a test set run has started. To enable remote agents to receive test_set_start and test_set_end information, add the SUPPORT_TESTSET_END configuration parameter in the Site Administration UI and set it to Y. Respond with a value of Y to get_value calls for both parameters.</dd>
    /// <dt>test_set_user1...99</dt><dd>The value of a user field in the Test in the Test Set table.</dd>
    /// <dt>test_type</dt><dd>The custom test type.</dd>
    /// <dt>test_user1...99</dt><dd>The value of a user field in the Tests table. </dd>
    /// <dt>testcycle_id</dt><dd>The ID of the test.</dd>
    /// <dt>tester_name</dt><dd>The name of the tester assigned to run the test.</dd>
    /// <dt>tstest_name</dt><dd>The name of the test to be run with a "[1]" instance prefix.</dd>
    /// <dt>user_name</dt><dd>The name of the user running the test.</dd>
    /// </dl>
    /// </param>
    /// <param name="prm_value">The value to be set. Not case-sensitive.</param>
    /// <returns>0 if the call succeeds, otherwise an error code.</returns>
    int set_value(string prm_name, string prm_value);

    /// <summary>
    /// Instructs the testing tool to terminate the test that is currently running.
    /// </summary>
    /// <returns>0 if the call succeeds, otherwise an error code.</returns>
    int stop();

    /// <summary>
    /// Gets the value of a run parameter.
    /// </summary>
    /// <param name="prm_name">The name of the parameter to retrieve. Not case-sensitive. One of:
    /// <dl>
    /// <dt>database_name</dt><dd>The name of the active ALM database.</dd>
    /// <dt>domain_name</dt><dd>The name of the ALM domain in which test and result information is stored.</dd>
    /// <dt>host_name</dt><dd>The name of the host on which the remote agent test type is run.</dd>
    /// <dt>password</dt><dd>The user’s password.</dd>
    /// <dt>pinned_baseline_id</dt><dd>The ID of the baseline to which the current test set is pinned.</dd>
    /// <dt>plann_status</dt><dd>The planning mode status of the test.</dd>
    /// <dt>project_name</dt><dd>The name of the ALM project in which test and result information is stored.</dd>
    /// <dt>responsible</dt><dd>The name of the user responsible for the project.</dd>
    /// <dt>runner_result_dir</dt><dd>The name of the test run results directory.</dd>
    /// <dt>scheduler_version</dt><dd>The software version number of the scheduler.</dd>
    /// <dt>subject</dt><dd>The subject folder to which the test belongs in the test plan tree.</dd>
    /// <dt>sys_computer_name</dt><dd>The name of the PC on which the ALM client is running.</dd>
    /// <dt>sys_user_name</dt><dd>The login user name for the user logged in on the ALM client PC.</dd>
    /// <dt>TDAPI_host_name</dt><dd>The name of the host on which the ALM Platform is running.</dd>
    /// <dt>testcycle_id_integer</dt><dd>The test instance ID.</dd>
    /// <dt>test_id</dt><dd>The ID of the test to be run.</dd>
    /// <dt>test_name</dt><dd>The name of the test to be run.</dd>
    /// <dt>test_path</dt><dd>The full path of the test to be run.</dd>
    /// <dt>test_set</dt><dd>A string of form: {test_set: "" testcycle_id: "148~1"}</dd>
    /// <dt>test_set_id</dt><dd>The ID of the test set to which the test belongs.</dd>
    /// <dt>test_set_end</dt><dd>A value of Y indicates that the last test in the test set run finished. This does not indicate success or failure of tests, only the end of the run. See also, parameter test_set_start.</dd>
    /// <dt>test_set_start</dt><dd>A value of Y indicates that a test set run has started. To enable remote agents to receive test_set_start and test_set_end information, add the SUPPORT_TESTSET_END configuration parameter in the Site Administration UI and set it to Y. Respond with a value of Y to get_value calls for both parameters.</dd>
    /// <dt>test_set_user1...99</dt><dd>The value of a user field in the Test in the Test Set table.</dd>
    /// <dt>test_type</dt><dd>The custom test type.</dd>
    /// <dt>test_user1...99</dt><dd>The value of a user field in the Tests table. </dd>
    /// <dt>testcycle_id</dt><dd>The ID of the test.</dd>
    /// <dt>tester_name</dt><dd>The name of the tester assigned to run the test.</dd>
    /// <dt>tstest_name</dt><dd>The name of the test to be run with a "[1]" instance prefix.</dd>
    /// <dt>user_name</dt><dd>The name of the user running the test.</dd>
    /// </dl>
    /// </param>
    /// <param name="prm_value">Output. The value.</param>
    /// <returns>0 if the call succeeds, otherwise an error code.</returns>
    int get_value(string prm_name, ref string prm_value);
  }
}

/*
 * From old documentation
get_value Method
Gets the value of a run parameter.

Syntax

Visual Basic
Public Function get_value( _
   ByVal prm_name As String, _
   ByRef prm_value As String _
) As Long
Parameters

prm_name
The name of the parameter. For a list of parameters, see set_value.
prm_value
The value of the parameter.
Return Type

S_OK if the call succeeds. Any other HRESULT on error.
Remarks

For parameter names test_set_end and test_set_start, return YES if the agent requires this information. If YES is returned, set_value will be called with these parameters when their values change.

 */
