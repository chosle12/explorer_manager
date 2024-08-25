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
        private readonly int maxWaitTimeInMilliseconds = 2000;
        private readonly int pollingIntervalInMilliseconds = 50;


        #region user32 dll imports
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);
        #endregion

        public delegate void ExplorerClosedCallback();
        public event ExplorerClosedCallback OnExplorerClosed;

        private Timer windowCloseCheckTimer;
        private IntPtr explorerWindowHandle;

        public IntPtr StartExplorer()
        {
            var initialExplorerWindows = GetExplorerWindowHandles();

            Process.Start("explorer.exe");

            explorerWindowHandle = IntPtr.Zero;
            int elapsedMilliseconds = 0;

            while (elapsedMilliseconds < maxWaitTimeInMilliseconds)
            {
                var currentExplorerWindows = GetExplorerWindowHandles();
                var newExplorerWindows = currentExplorerWindows.Except(initialExplorerWindows).ToList();

                if (newExplorerWindows.Any())
                {
                    explorerWindowHandle = newExplorerWindows.First();
                    break;
                }

                Thread.Sleep(pollingIntervalInMilliseconds);
                elapsedMilliseconds += pollingIntervalInMilliseconds;
            }

            if (explorerWindowHandle != IntPtr.Zero)
            {
                windowCloseCheckTimer = new Timer(CheckIfWindowClosed, explorerWindowHandle, 0, 100);
            }

            return explorerWindowHandle;
        }

        public Process GetProcess()
        {
            if (explorerWindowHandle == IntPtr.Zero ||
                !IsWindow(explorerWindowHandle) ||
                !IsWindowVisible(explorerWindowHandle))
            {
                return null;
            }

            GetWindowThreadProcessId(explorerWindowHandle, out uint processId);

            try
            {
                return Process.GetProcessById((int)processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
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

            try
            {
                EnumWindows((hWnd, lParam) =>
                {
                    try
                    {
                        GetWindowThreadProcessId(hWnd, out uint processId);

                        var process = Process.GetProcessById((int)processId);

                        if (process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) && IsWindowVisible(hWnd))
                        {
                            explorerWindows.Add(hWnd);
                        }
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (InvalidOperationException)
                    {
                    }

                    return true;
                }, IntPtr.Zero);
            }
            catch (Exception)
            {
            }

            return explorerWindows;
        }
    }
}
