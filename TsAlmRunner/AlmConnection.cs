using System;
using TDAPIOLELib;
using TsCloudShellApi;

namespace TsAlmRunner
{
    class AlmConnection
    {
        private readonly Logger m_Logger;
        private readonly AlmParameters m_AlmParameters;
        public TDConnectionClass Connection { get; private set; }

        public AlmConnection(Logger logger, AlmParameters almParameters)
        {
            m_Logger = logger;
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
                m_Logger.Error("Open connection to ALM error: " + ex);

                // This is a common error that happens on a clean machine, some of the ALM-Client COM components must be registered first.
                // "Retrieving the COM class factory for component with CLSID {C5CBD7B2-490C-45F5-8C40-B8C3D108E6D7} failed due to the following error: 80040154 Class not registered (Exception from HRESULT: 0x80040154 (REGDB_E_CLASSNOTREG))."
                if (ex.Message.Contains("C5CBD7B2-490C-45F5-8C40-B8C3D108E6D7"))
                {
                    var baseUrl = m_AlmParameters.TdApiHostName;

                    // remove the "/qcbin/wcomsrv.dll" from the end of "http://192.168.42.172:8080/qcbin/wcomsrv.dll"
                    if (!baseUrl.TrimEnd('/').ToLower().EndsWith("qcbin") && baseUrl.Contains("/qcbin"))
                        baseUrl = baseUrl.Substring(0, baseUrl.IndexOf("/qcbin"));

                    var error =
                        "Unable to open ALM connection, some of the client components might not be registered.\n\n" +
                        "Please try to register the components by navigating to:\n" +
                        string.Format("{0}/qcbin/start_a.jsp?Common=true", baseUrl.Trim('/'));

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
