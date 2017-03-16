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
            return GetUserFieldNameByLabel("CLOUDSHELL_TEST_PATH", "TEST");
        }

        public string GetUserFieldNameByLabel(string labelName, string entity)
        {
            labelName = labelName.ToUpper();
            string columnName = null;
            foreach (TDField field in m_TdConnection.get_Fields(entity))
            {
                if (((FieldProperty)field.Property).UserLabel.ToUpper().Equals(labelName))//"CLOUDSHELL_TEST_PATH"
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
            const string RunAuthModeKey = "CLOUDSHELL_RUN_AUTH_MODE";
            const string AlmValue = "ALM";
            const string CloudShellValue = "CLOUDSHELL";

            var almMode = GetTdParam(RunAuthModeKey);

            almMode = almMode.ToUpper();

            if (almMode == AlmValue)
                return AuthenticationMode.Alm;

            if (almMode == CloudShellValue)
                return AuthenticationMode.CloudShell;

            throw new Exception(string.Format("{0} has invalid value '{1}'. Please specify one of: {2}, {3}", RunAuthModeKey, almMode, AlmValue, CloudShellValue));
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
