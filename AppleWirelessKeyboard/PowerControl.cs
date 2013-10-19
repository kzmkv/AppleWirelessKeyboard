using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace AppleWirelessKeyboard
{
    public class PowerControl
    {
        public static void Hibernate()
        {
            Task.Factory.StartNew(() =>
            {
                if (PowerStatusBox.PowerAction("Hibernate", 10))
                    SetSuspendState(true, true, true);
            });
        }

        public static void Shutdown()
        {
            Task.Factory.StartNew(() =>
            {
                if (PowerStatusBox.PowerAction("Shut Down", 10))
                {
                    ProcessStartInfo si = new ProcessStartInfo("shutdown", "/s /t 0");
                    si.CreateNoWindow = true;
                    si.WindowStyle = ProcessWindowStyle.Hidden;
                    Process.Start(si);
                }
            });
        }

        public static void TurnOffDisplay()
        {
            Task.Factory.StartNew(() =>
                {
                    SendMessage((IntPtr)HWND_BROADCAST, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)MONITOR_OFF);
                });
        }

        [DllImport("powrprof.dll", SetLastError = true)]
        static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        private const UInt32 WM_SYSCOMMAND = 0x0112;
        private const int SC_MONITORPOWER = 0xF170;
        private const int HWND_BROADCAST = 0xFFFF;
        private const int MONITOR_OFF = 2;
    }
}
