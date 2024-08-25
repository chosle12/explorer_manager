using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace ExplorerManager
{
    public class ExplorerProcessManager
    {
        #region user32 dll imports
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        #endregion

        public delegate void ExplorerClosedCallback();
        public event ExplorerClosedCallback OnExplorerClosed;

        private Timer windowCloseCheckTimer;

        public IntPtr StartExplorer()
        {
            var initialExplorerWindows = GetExplorerWindowHandles();

            Process.Start("explorer.exe");

            IntPtr newExplorerWindowHandle = IntPtr.Zero;
            const int maxWaitTimeInMilliseconds = 2000;
            const int pollingIntervalInMilliseconds = 50;
            int elapsedMilliseconds = 0;

            while (elapsedMilliseconds < maxWaitTimeInMilliseconds)
            {
                var currentExplorerWindows = GetExplorerWindowHandles();
                var newExplorerWindows = currentExplorerWindows.Except(initialExplorerWindows).ToList();

                if (newExplorerWindows.Any())
                {
                    newExplorerWindowHandle = newExplorerWindows.First();
                    break;
                }

                Thread.Sleep(pollingIntervalInMilliseconds);
                elapsedMilliseconds += pollingIntervalInMilliseconds;
            }

            if (newExplorerWindowHandle != IntPtr.Zero)
            {
                windowCloseCheckTimer = new Timer(CheckIfWindowClosed, newExplorerWindowHandle, 0, 100);
            }

            return newExplorerWindowHandle;
        }

        private void CheckIfWindowClosed(object state)
        {
            IntPtr explorerWindowHandle = (IntPtr)state;

            if (!IsWindowVisible(explorerWindowHandle))
            {
                OnExplorerClosed?.Invoke();
                windowCloseCheckTimer?.Dispose();
            }
        }

        private List<IntPtr> GetExplorerWindowHandles()
        {
            var explorerWindows = new List<IntPtr>();

            EnumWindows((hWnd, lParam) =>
            {
                GetWindowThreadProcessId(hWnd, out uint processId);

                var process = Process.GetProcessById((int)processId);

                if (process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) && IsWindowVisible(hWnd))
                {
                    explorerWindows.Add(hWnd);
                }

                return true;
            }, IntPtr.Zero);

            return explorerWindows;
        }
    }
}
