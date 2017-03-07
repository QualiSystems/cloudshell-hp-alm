using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDAPIOLELib;

namespace QS.ALM.CloudShellApi
{
    public class TDConnectionServant
    {
        static private TDConnectionServant m_TDConnectionServant;
        private readonly ITDConnection m_tdConnection;

        public TDConnectionServant(ITDConnection tdConnection) 
        {
            m_tdConnection = tdConnection;
        }

        public string GetQualiTestPathFieldName()
        {
            return GetUserFieldNameByLabel("QUALI_TEST_PATH");
        }

        public string GetUserFieldNameByLabel(string labelName)
        {
            labelName = labelName.ToUpper();
            string columnName = null;
            foreach (TDField field in m_tdConnection.get_Fields("TEST"))
            {
                if (((FieldProperty)field.Property).UserLabel.ToUpper().Equals(labelName))//"QUALI_TEST_PATH"
                {
                    columnName =((FieldProperty)field.Property).DBColumnName;
                }
            }
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new Exception("Please add the '" + labelName + "' user field to Test in Project Entities.");
            }
            return columnName;
        }

        public AuthenticationMode GetAlmMode()
        {
            string almMode = GetTdParam("QS_AUTH_MODE");

            almMode = almMode.ToUpper();
            if (almMode == "ALM")
            {
                return AuthenticationMode.Alm;
            }
            else if (almMode == "CLOUDSHELL")
            {
                return AuthenticationMode.CloudShell;
            }
            else
            {
                throw new Exception("QS_AUTH_MODE unknown : " + almMode);
            }  
        }
        public string GetTdParam(string paramName)
        {
            string tdParam = null;
            paramName = paramName.ToUpper();
            tdParam = m_tdConnection.get_TDParams(paramName);
            if(string.IsNullOrWhiteSpace(tdParam))
            {
                throw new Exception("Missing Parameter '" + paramName + "'. Please add the missing parametes in the Site Configuration tab under the Site Administration.");
            }
            return tdParam;
        }        
    }
}
