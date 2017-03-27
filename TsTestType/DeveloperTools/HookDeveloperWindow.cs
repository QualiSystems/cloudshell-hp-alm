using System.Diagnostics;
using System.Windows.Forms;

namespace TsTestType.DeveloperTools
{
    static class HookDeveloperWindow
    {
        private static bool m_IsOpened;

        static HookDeveloperWindow()
        {
            Application.AddMessageFilter(new KeyMessageFilter());
        }

        public static void HookOnce()
        {
            // Nothing to do. the static ctor will do the job
        }

        private class KeyMessageFilter : IMessageFilter
        {
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_KEYUP = 0x0101;

            private bool m_CtrlDown;
            private bool m_TabDown;
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
                    else if (key == Keys.Tab)
                        m_TabDown = true;
                    else if (key == Keys.D && m_CtrlDown && m_TabDown && m_ShiftDown)
                        OpenDeveloperWindowIfNeeded();

                    Trace.WriteLine("Key down: " + key);
                }
                else if (m.Msg == WM_KEYUP)
                {
                    if (key == Keys.ShiftKey)
                        m_ShiftDown = false;
                    else if (key == Keys.ControlKey)
                        m_CtrlDown = false;
                    else if (key == Keys.Tab)
                        m_TabDown = false;

                    Trace.WriteLine("Key up: " + key);
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

                var form = new DeveloperToolsForm();
                form.ShowDialog();

                lock (typeof(HookDeveloperWindow))
                {
                    m_IsOpened = false;
                }
            }
        }
    }
}
