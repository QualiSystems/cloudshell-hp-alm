using TDAPIOLELib;

namespace CSRAgent
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
            Connection = new TDConnectionClass();
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
