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
    private Dictionary<string, string> Params = new Dictionary<string, string>();
    private string m_status ;
    private string m_descr;
   /* private string m_serverName;
    private string m_projectName;
    private string m_domainName;
    private string m_userName;
    private string m_password;
    private string m_hostName;
    private string m_testPath;
    private string m_testName;
    private string m_testID;
    private string m_testSetID;
    private string m_testSetInfo;
    private string m_testInstID;
    private string m_testInstName;
    private string m_test;*/

    public int get_status(ref string descr, ref string status)
    {
      descr = m_descr;
      status = m_status;
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
      MessageBox.Show(string.Format(
        "Running test: id-{0}, name-{1}, user-{2}, server-{3}, domain-{4}, project-{5}, host-{6}",
        Params["test_id"], Params["test_name"], Params["user_name"], Params["TDAPI_host_name"], Params["domain_name"], Params["project_name"], Params["host_name"]));

      // post the run
      PostRun("Passed");
      //Process.GetCurrentProcess().Kill();
      m_status = "END_OF_TEST";
      m_descr = "Completed";
      return 0;
    }

    private void PostRun(string runStatus)
    {
      TDConnection conn = new TDConnectionClass();
      conn.InitConnectionEx(Params["TDAPI_host_name"]);
      conn.ConnectProjectEx(Params["domain_name"], Params["project_name"], Params["user_name"], Params["password"]);

      Run run = (((((((conn.TestSetFactory as TestSetFactory)[Params["test_set_id"]]) as TestSet).TSTestFactory as TSTestFactory)[Params["testcycle_id_integer"]]) as TSTest).RunFactory as RunFactory).AddItem("scottyRun") as Run;
      run["RN_TESTER_NAME"] = Params["user_name"];
      run["RN_HOST"] = Params["host_name"];
      run.Status = runStatus;
      run.Post();
    }

    public int get_value(string prm_name, string prm_value)
    {
        return 0;
    }

    public int set_value(string prm_name, string prm_value)
    {
      //MessageBox.Show("set value param:" + prm_name + " value: " + "prm_value");
      Params.Add(prm_name, prm_value);
      /*switch (prm_name)
      {
        case "host_name":
          m_hostName = prm_value;
          break;
        case "TDAPI_host_name":
          m_serverName = prm_value;
          break;
        case "project_name":
          m_projectName = prm_value;
          break;
        case "domain_name":
          m_domainName = prm_value;
          break;
        case "user_name":
          m_userName = prm_value;
          break;
        case "password":
          m_password = prm_value;
          break;
        case "test_path":
          m_testPath = prm_value;
          break;
        case "test_name":
          m_testName = prm_value;
          break;
        case "test_id":
          m_testID = prm_value;
          break;
        case "test_set_id":
          m_testSetID = prm_value;
          break;
        case "test_set":
          m_testSetInfo = prm_value;
          break;
        case "testcycle_id_integer":
          m_testInstID = prm_value;
          break;
        case "tstest_name":
          m_testInstName = prm_value;
          break;
      }*/
      return 0;
    }

    public int stop()
    {
      MessageBox.Show("stopping the run");
      Process.GetCurrentProcess().Kill();
      return 0;
    }
  }
}