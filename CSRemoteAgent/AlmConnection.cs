using System;
using QS.ALM.CloudShellApi;
using TDAPIOLELib;

namespace TsAlmRunner
{
    class AlmConnection
    {
        private readonly AlmParameters m_AlmParameters;
        public TDConnectionClass Connection { get; private set; }

        public AlmConnection(AlmParameters almParameters)
        {
            m_AlmParameters = almParameters;
            OpenConnection();
        }

        private void OpenConnection()
        {
            try
            {
                Connection = new TDConnectionClass();
            }
            catch (Exception ex)
            {
                Logger.Error("Open connection to ALM error: " + ex);

                // This is a common error that happens on a clean machine, some of the ALM-Client COM components must be registered first.
                // "Retrieving the COM class factory for component with CLSID {C5CBD7B2-490C-45F5-8C40-B8C3D108E6D7} failed due to the following error: 80040154 Class not registered (Exception from HRESULT: 0x80040154 (REGDB_E_CLASSNOTREG))."
                if (ex.Message.Contains("Class not registered") || ex.Message.Contains("80040154"))
                {
                    var error =
                        "Unable to open ALM connection, some of the client components might not be registered.\n\n" +
                        "Please try to register the components by navigating to:\n" +
                        string.Format("{0}/qcbin/start_a.jsp?Common=true", m_AlmParameters.TdApiHostName);

                    throw new Exception(error);
                }

                throw;
            }

            Connection.InitConnectionEx(m_AlmParameters.TdApiHostName);
            Connection.ConnectProjectEx(m_AlmParameters.DomainName, m_AlmParameters.ProjectName, m_AlmParameters.UserName, m_AlmParameters.Password);
        }

        //private int CloseCon()
        //{
        //    if (conn.Connected)
        //        conn.DisconnectProject();
        //    return 0;
        //}
    }
}
