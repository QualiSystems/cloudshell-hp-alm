using System;
using TDAPIOLELib;

namespace TsCloudShellApi
{
    public class TDConnectionServant
    {
        private readonly ITDConnection m_TdConnection;

        public TDConnectionServant(ITDConnection tdConnection) 
        {
            m_TdConnection = tdConnection;
        }

        public string GetTestPathFieldName()
        {
            return GetUserFieldNameByLabel("CloudShellTestPath", "TEST");
        }

        public string GetUserFieldNameByLabel(string labelName, string entity)
        {
            labelName = labelName.ToUpper();
            string columnName = null;
            foreach (TDField field in m_TdConnection.get_Fields(entity))
            {
                if (((FieldProperty)field.Property).UserLabel.ToUpper().Equals(labelName))//"CloudShellTestPath"
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

        public AuthenticationMode GetRunAuthMode()
        {
            const string RunAuthModeKey = "CLOUDSHELL_RUN_AUTH_MODE";
            const string GlobalValue = "GLOBAL";
            const string UserValue = "USER";

            var mode = GetTdParam(RunAuthModeKey);

            mode = mode.ToUpper();

            if (mode == GlobalValue)
                return AuthenticationMode.Global;

            if (mode == UserValue)
                return AuthenticationMode.User;

            throw new Exception(string.Format("{0} has invalid value '{1}'. Please specify one of: {2}, {3}", RunAuthModeKey, mode, GlobalValue, UserValue));
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
