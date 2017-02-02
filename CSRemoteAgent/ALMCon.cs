using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDAPIOLELib;

namespace CSRAgent
{
    class ALMCon
    {
        public TDConnection conn;
        private Dictionary<string, string> Params;
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
                conn.InitConnectionEx(this.getValue("TDAPI_host_name"));
                conn.ConnectProjectEx(this.getValue("domain_name"), this.getValue("project_name"), this.getValue("user_name"), this.getValue("password"));
            }
            return 0;
        }
        public int CloseCon()
        {
            if(conn.Connected)
                conn.DisconnectProject();
            return 0;
        }

        public string getValue(string prm_name)
        {
            if (Params.ContainsKey(prm_name))
            {
                return  Params[prm_name];
            }
            return "";
        }
        public int getValue(string prm_name, ref string prm_value)
        {
            if (Params.ContainsKey(prm_name))
            {
                prm_name = Params[prm_name];
            }
            return 0;
        }
        public int setValue(string prm_name, string prm_value)
        {
            if (!Params.ContainsKey(prm_name))
            {
                Params.Add(prm_name, prm_value);
            }
            else
            {
                Params[prm_name] = prm_value;
            }
            return 0;
        }

    }
}
