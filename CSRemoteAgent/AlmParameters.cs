using System;
using System.Collections.Generic;

namespace CSRAgent
{
    class AlmParameters
    {
        private readonly Dictionary<string, string> m_Dictionary = new Dictionary<string, string>();

        public void SetValue(string prmName, string prmValue)
        {
            m_Dictionary[prmName] = prmValue;
        }

        public string GetValue(string paramName)
        {
            if (!m_Dictionary.ContainsKey(paramName))
                throw new Exception("Parameter not found: " + paramName);
            
            return m_Dictionary[paramName];
        }

        public bool ContainsParameter(string prmName)
        {
            return m_Dictionary.ContainsKey(prmName);
        }

        public string TdApiHostName { get { return GetValue("TDAPI_host_name"); } }
        public string HostName { get { return GetValue("host_name"); } }
        public string DomainName { get { return GetValue("domain_name"); } }
        public string ProjectName { get { return GetValue("project_name"); } }
        public string UserName { get { return GetValue("user_name"); } }
        public string Password { get { return GetValue("password"); } }
        public string TestSetId { get { return GetValue("test_set_id"); } }
        public string TestCycleIdInteger { get { return GetValue("testcycle_id_integer"); } }
        public string TestSet { get { return GetValue("test_set"); } }
        public string TestName { get { return GetValue("test_name"); } }
    }
}
