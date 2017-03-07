using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Forms;
using TDAPIOLELib;

[assembly: ApplicationName("Scotty CS Remote Agent")]
[assembly: ApplicationActivation(ActivationOption.Server)]
[assembly: System.EnterpriseServices.Description("a C# version remote agent for alm 11.52")]
[assembly: ApplicationAccessControl(false)]

namespace CSRAgent
{
  [Guid("479DFA08-CF6D-4890-AAAF-7CAFC39B6974"), ComVisible(true), ProgId("AlmCsRemoteAgent1152")]
  public class CSRAgent : ServicedComponent, IRAgent
  {
    ALMCon mALMCon = new ALMCon();
    private string mDescr;
    //private TDConnection conn;
   

    public int get_status(ref string descr, ref string status)
    {
      descr = mDescr;
      status = mALMCon.getStatus();
      return 0;
    }

    public int is_host_ready(ref string descr)
    {
      descr = "Ready";
      MessageBox.Show("is host ready!!!");
      return 0;
    }

    public int run()
    {
     /* MessageBox.Show(string.Format(
        "Running test: id-{0}, name-{1}, user-{2}, server-{3}, domain-{4}, project-{5}, host-{6}",
        mALMCon.GetValue("test_id"),  mALMCon.GetValue("test_name"),  mALMCon.GetValue("user_name"), 
        mALMCon.GetValue("TDAPI_host_name"),  mALMCon.GetValue("domain_name"),mALMCon.GetValue("project_name"),  mALMCon.GetValue("host_name")));*/
      mALMCon.OpenCon();
     
      // post the run
     // mALMCon.getTestParameters();
      mALMCon.getTestPath();
      mALMCon.RunQSheel();
      mALMCon.WaitTestComplit();
      PostRun(mALMCon.getStatus());//"Passed");
      //Process.GetCurrentProcess().Kill();
      mALMCon.mStatus = CStatus.end_of_test; //"END_OF_TEST";
      mDescr = "Completed";
      return 0;
    }

    private void PostRun(string runStatus)
    {
      mALMCon.OpenCon();

      Run run = (((((((mALMCon.conn.TestSetFactory as TestSetFactory)[ mALMCon.GetValue("test_set_id")]) as TestSet).TSTestFactory as TSTestFactory)[ mALMCon.GetValue("testcycle_id_integer")]) as TSTest).RunFactory as RunFactory).AddItem("scottyRun") as Run;
      run["RN_TESTER_NAME"] =  mALMCon.GetValue("user_name");
      run["RN_HOST"] =  mALMCon.GetValue("host_name");
      //run.Status = runStatus;
      run.Post();
    }

    public int get_value(string prmName, ref string prmValue)
    {
        return mALMCon.GetValue(prmName,ref prmValue);
    }

    public int set_value(string prmName, string prmValue)
    {
      return mALMCon.SetValue(prmName, prmValue);
    }

    public int stop()
    {
      MessageBox.Show("stopping the run");
      Process.GetCurrentProcess().Kill();
      return 0;
    }
  }
}
