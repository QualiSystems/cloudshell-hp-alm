using System.Windows.Forms;
using TsCloudShellApi;

namespace TsTestType.DeveloperTools
{
    public static class HookDeveloperWindow
    {
        private static bool m_IsOpened;
        private static Logger m_Logger;

        static HookDeveloperWindow()
        {
            Application.AddMessageFilter(new KeyMessageFilter());
        }

        public static void HookOnce(Logger logger)
        {
            m_Logger = logger;
            // the static ctor did the rest of the job
        }

        private class KeyMessageFilter : IMessageFilter
        {
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_KEYUP = 0x0101;

            private bool m_CtrlDown;
            private bool m_CapsDown;
            private bool m_ShiftDown;

            public bool PreFilterMessage(ref Message m)
            {
                var key = (Keys)m.WParam;

                if (m.Msg == WM_KEYDOWN)
                {
                    if (key == Keys.ShiftKey)
                        m_ShiftDown = true;
                    else if (key == Keys.ControlKey)
                        m_CtrlDown = true;
                    else if (key == Keys.CapsLock)
                        m_CapsDown = true;
                    else if (key == Keys.T && m_CtrlDown && m_CapsDown && m_ShiftDown)
                        OpenDeveloperWindowIfNeeded();
                }
                else if (m.Msg == WM_KEYUP)
                {
                    if (key == Keys.ShiftKey)
                        m_ShiftDown = false;
                    else if (key == Keys.ControlKey)
                        m_CtrlDown = false;
                    else if (key == Keys.CapsLock)
                        m_CapsDown = false;
                }

                return false;
            }

            private void OpenDeveloperWindowIfNeeded()
            {
                lock (typeof(HookDeveloperWindow))
                {
                    if (m_IsOpened)
                        return;

                    m_IsOpened = true;
                }

                var form = new DeveloperToolsForm(m_Logger);
                form.ShowDialog();

                lock (typeof(HookDeveloperWindow))
                {
                    m_IsOpened = false;
                }
            }
        }
    }
}
