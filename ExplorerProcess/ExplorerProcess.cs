using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Timer = System.Threading.Timer;

namespace ExplorerManager
{
    public class ExplorerProcess
    {
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        public delegate void CloseCallback();
        public static CloseCallback closeCallback;

        private static Timer _timer;

        public IntPtr RunProcess()
        {
            var initialWindows = GetExplorerWindows();

            Process.Start("explorer.exe");

            IntPtr newWindowHandle = IntPtr.Zero;
            int maxWaitTime = 2000;
            int elapsed = 0;
            int sleepInterval = 50;

            while (elapsed < maxWaitTime)
            {
                var currentWindows = GetExplorerWindows();
                var newWindows = currentWindows.Except(initialWindows).ToList();
                if (newWindows.Any())
                {
                    newWindowHandle = newWindows.First();
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(sleepInterval);
                    elapsed += sleepInterval;
                }
            }

            if (newWindowHandle != IntPtr.Zero)
            {
                _timer = new Timer(CheckWindowClosed, newWindowHandle, 0, 100);
            }

            return newWindowHandle;
        }

        private static void CheckWindowClosed(object state)
        {
            IntPtr windowHandle = (IntPtr)state;

            if (!IsWindowVisible(windowHandle))
            {
                closeCallback?.Invoke();
                _timer?.Dispose();
            }
        }

        private List<IntPtr> GetExplorerWindows()
        {
            var windows = new List<IntPtr>();

            EnumWindows((hWnd, lParam) =>
            {
                uint processId;
                GetWindowThreadProcessId(hWnd, out processId);

                var process = Process.GetProcessById((int)processId);

                if (process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) && IsWindowVisible(hWnd))
                {
                    windows.Add(hWnd);
                }

                return true;
            }, IntPtr.Zero);

            return windows;
        }
    }
}
