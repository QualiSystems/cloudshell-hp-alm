using System;
using TDAPIOLELib;

namespace QS.ALM.CloudShellApi
{
    public class TDConnectionServant
    {
        private readonly ITDConnection m_TdConnection;

        public TDConnectionServant(ITDConnection tdConnection) 
        {
            m_TdConnection = tdConnection;
        }

        public string GetQualiTestPathFieldName()
        {
            return GetUserFieldNameByLabel("QUALI_TEST_PATH", "TEST");
        }

        public string GetUserFieldNameByLabel(string labelName, string entity)
        {
            labelName = labelName.ToUpper();
            string columnName = null;
            foreach (TDField field in m_TdConnection.get_Fields(entity))
            {
                if (((FieldProperty)field.Property).UserLabel.ToUpper().Equals(labelName))//"QUALI_TEST_PATH"
                {
                    columnName =((FieldProperty)field.Property).DBColumnName;
                }
            }
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new Exception(string.Format("Please add a user field with label '{0}' to the '{1}' entity under Project Entities.", labelName, entity));
            }
            return columnName;
        }

        public AuthenticationMode GetAlmMode()
        {
            var almMode = GetTdParam("QS_AUTH_MODE");

            almMode = almMode.ToUpper();

            if (almMode == "ALM")
                return AuthenticationMode.Alm;
            
            if (almMode == "CLOUDSHELL")
                return AuthenticationMode.CloudShell;
            
            throw new Exception("QS_AUTH_MODE unknown : " + almMode);
        }

        public string GetTdParam(string paramName, string defaultValue = null)
        {
            paramName = paramName.ToUpper();
            var tdParam = m_TdConnection.get_TDParams(paramName);

            if (string.IsNullOrWhiteSpace(tdParam))
            {
                if (!string.IsNullOrEmpty(defaultValue))
                    return defaultValue;

                throw new Exception("Missing Parameter '" + paramName + "'. Please add the missing parametes in the Site Configuration tab under the Site Administration.");
            }

            return tdParam;
        }
    }
}
