using TsCloudShellApi;

namespace TsTestType
{
    internal class TestTypeLogger
    {
        private static Logger m_Logger;

        public static Logger Instance
        {
            get
            {
                if (m_Logger == null)
                    m_Logger = new Logger("CustomTest");

                return m_Logger;
            }
        }

    }
}
