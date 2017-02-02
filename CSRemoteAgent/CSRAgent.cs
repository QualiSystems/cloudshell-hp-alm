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
    ALMCon m_ALMCon = new ALMCon();
    private string m_status ;
    private string m_descr;
   

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
        m_ALMCon.getValue("test_id"),  m_ALMCon.getValue("test_name"),  m_ALMCon.getValue("user_name"), 
        m_ALMCon.getValue("TDAPI_host_name"),  m_ALMCon.getValue("domain_name"),m_ALMCon.getValue("project_name"),  m_ALMCon.getValue("host_name")));
      m_ALMCon.OpenCon();
      List LTestData = new List();
      LTestData = m_ALMCon.conn.get_Fields(m_ALMCon.getValue("test_set_id"));
      // post the run
      PostRun("Passed");
      //Process.GetCurrentProcess().Kill();
      m_status = "END_OF_TEST";
      m_descr = "Completed";
      return 0;
    }

    private void PostRun(string runStatus)
    {
      /*TDConnection conn = new TDConnectionClass();
      conn.InitConnectionEx( m_ALMCon.getValue("TDAPI_host_name"));
      conn.ConnectProjectEx( m_ALMCon.getValue("domain_name"),  m_ALMCon.getValue("project_name"),  m_ALMCon.getValue("user_name"),  m_ALMCon.getValue("password"));
*/
      m_ALMCon.OpenCon();

      Run run = (((((((m_ALMCon.conn.TestSetFactory as TestSetFactory)[ m_ALMCon.getValue("test_set_id")]) as TestSet).TSTestFactory as TSTestFactory)[ m_ALMCon.getValue("testcycle_id_integer")]) as TSTest).RunFactory as RunFactory).AddItem("scottyRun") as Run;
      run["RN_TESTER_NAME"] =  m_ALMCon.getValue("user_name");
      run["RN_HOST"] =  m_ALMCon.getValue("host_name");
      run.Status = runStatus;
      run.Post();
    }

    public int get_value(string prm_name, ref string prm_value)
    {
        return m_ALMCon.getValue(prm_name,ref prm_value);
    }

    public int set_value(string prm_name, string prm_value)
    {
      return m_ALMCon.setValue(prm_name, prm_value);
    }

    public int stop()
    {
      MessageBox.Show("stopping the run");
      Process.GetCurrentProcess().Kill();
      return 0;
    }
  }
}