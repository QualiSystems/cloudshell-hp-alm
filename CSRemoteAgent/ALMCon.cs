using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDAPIOLELib;
using System.Threading;
using System.Windows.Forms;

namespace CSRAgent
{
    class ALMCon
    {
        public TDConnection conn;
        static private Dictionary<string, string> Params;
        public CStatus mStatus;
        public ALMCon()
        {
            Params = new Dictionary<string,string>();
        }

        public int OpenCon()
        {
            if(conn == null)
                conn = new TDConnectionClass();
            if (!conn.Connected)
            {
                conn.InitConnectionEx(this.GetValue("TDAPI_host_name"));
                conn.ConnectProjectEx(this.GetValue("domain_name"), this.GetValue("project_name"), this.GetValue("user_name"), this.GetValue("password"));
            }
            return 0;
        }
        public int CloseCon()
        {
            if(conn.Connected)
                conn.DisconnectProject();
            return 0;
        }

        public string GetValue(string prmName)
        {
            if (Params.ContainsKey(prmName))
            {
                return  Params[prmName];
            }
            return "";
        }
        public int GetValue(string prmName, ref string prmValue)
        {
            if (Params.ContainsKey(prmName))
            {
                prmName = Params[prmName];
            }
            return 0;
        }
        public int SetValue(string prmName, string prmValue)
        {
            if (!Params.ContainsKey(prmName))
            {
                Params.Add(prmName, prmValue);
            }
            else
            {
                Params[prmName] = prmValue;
            }
            return 0;
        }

        public int RunQSheel()
        {
            MessageBox.Show("Start QSheel");
            return 0;
        }
        public int WaitTestComplit()
        {
            mStatus = CStatus.init;
            while (CStatus.end_of_test != mStatus)
            {
                MessageBox.Show("Try Polling");
                Thread.Sleep(2000);
            }

            return 0;
        }

        public string getStatus()
        {
            return convertStatusToString(mStatus);
        }

        public string convertStatusToString(CStatus xStatus)
        {
            switch (xStatus)
            {
                case CStatus.busy:
                    return "BUSY";
                    break;
                case CStatus.end_of_test:
                    return "END_OF_TEST";
                    break;
                case CStatus.failed:
                    return "FAILED";
                    break;
                case CStatus.init:
                    return "INIT";
                    break;
                case CStatus.logical_running:
                    return "LOGICAL_RUNNING";
                    break;
                case CStatus.paused:
                    return "PAUSED";
                    break;
                case CStatus.ready:
                    return "READY";
                    break;
                case CStatus.stopped:
                    return "STOPPED";
                    break;
                case CStatus.test_passed:
                    return "TEST_PASSED";
                    break;
                case CStatus.test_failed:
                    return "TEST_FAILED";
                    break;
                case CStatus.retry:
                    return "RETRY";
                    break;
                default:
                    return "";
            }
        }

    }
}
