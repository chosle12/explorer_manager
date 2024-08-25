using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ExplorerManager
{
    public class ExplorerProcessManager : IDisposable
    {
        private const int MaxWaitTimeInMilliseconds = 2000;
        private const int PollingIntervalInMilliseconds = 50;
        private const int TopMostSetRetries = 50;
        private const int TopMostSetSleepTimeMilliseconds = 100;
        private const int WindowCloseCheckInterval = 100;

        public delegate void ExplorerClosedCallback(IntPtr hWnd);
        public event ExplorerClosedCallback OnExplorerClosed;

        private Timer windowCloseCheckTimer;
        private IntPtr currentExplorerWindowHandle;

        ~ExplorerProcessManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            windowCloseCheckTimer?.Dispose();
        }

        public IntPtr StartExplorer()
        {
            var initialExplorerWindows = GetExplorerWindowHandles();

            Process.Start("explorer.exe");

            currentExplorerWindowHandle = WaitForNewExplorerWindow(initialExplorerWindows);

            if (currentExplorerWindowHandle != IntPtr.Zero)
            {
                StartWindowCloseCheckTimer(currentExplorerWindowHandle);
            }

            return currentExplorerWindowHandle;
        }

        public Process GetProcess()
        {
            if (!IsValidExplorerWindowHandle())
            {
                return null;
            }

            User32.GetWindowThreadProcessId(currentExplorerWindowHandle, out uint processId);

            try
            {
                return Process.GetProcessById((int)processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public bool SetForegroundWindow()
        {
            if (!IsValidExplorerWindowHandle())
            {
                return false;
            }

            return User32.SetForegroundWindow(currentExplorerWindowHandle);
        }

        public bool SetTopMostState(bool isTopMost)
        {
            if (!IsValidExplorerWindowHandle())
            {
                return false;
            }

            User32.SetForegroundWindow(currentExplorerWindowHandle);

            int retries = TopMostSetRetries;
            while (retries-- > 0)
            {
                User32.SetWindowPos(
                    currentExplorerWindowHandle,
                    isTopMost ? User32.HWND_TOPMOST : User32.HWND_NOTOPMOST,
                    0, 0, 0, 0,
                    User32.SWP_NOMOVE | User32.SWP_NOSIZE | User32.SWP_NOACTIVATE | User32.SWP_SHOWWINDOW);

                int exStyle = User32.GetWindowLong(currentExplorerWindowHandle, User32.GWL_EXSTYLE);
                bool currentStateIsTopMost = (exStyle & User32.WS_EX_TOPMOST) != 0;
                if (currentStateIsTopMost == isTopMost)
                {
                    return true;
                }

                Thread.Sleep(TopMostSetSleepTimeMilliseconds);
            }

            return false;
        }

        private IntPtr WaitForNewExplorerWindow(List<IntPtr> initialExplorerWindows)
        {
            int elapsedMilliseconds = 0;
            while (elapsedMilliseconds < MaxWaitTimeInMilliseconds)
            {
                var currentExplorerWindows = GetExplorerWindowHandles();
                var newExplorerWindows = currentExplorerWindows.Except(initialExplorerWindows).ToList();

                if (newExplorerWindows.Any())
                {
                    return newExplorerWindows.First();
                }

                Thread.Sleep(PollingIntervalInMilliseconds);
                elapsedMilliseconds += PollingIntervalInMilliseconds;
            }

            return IntPtr.Zero;
        }

        private void StartWindowCloseCheckTimer(IntPtr hWnd)
        {
            windowCloseCheckTimer = new Timer(OnWindowClosedCheck, hWnd, 0, WindowCloseCheckInterval);
        }

        private bool IsValidExplorerWindowHandle()
        {
            return currentExplorerWindowHandle != IntPtr.Zero &&
                   User32.IsWindow(currentExplorerWindowHandle) &&
                   User32.IsWindowVisible(currentExplorerWindowHandle);
        }

        private void OnWindowClosedCheck(object state)
        {
            IntPtr explorerWindowHandle = (IntPtr)state;

            if (!User32.IsWindowVisible(explorerWindowHandle))
            {
                OnExplorerClosed?.Invoke(explorerWindowHandle);
                windowCloseCheckTimer?.Dispose();
            }
        }

        private List<IntPtr> GetExplorerWindowHandles()
        {
            var explorerWindows = new List<IntPtr>();

            User32.EnumWindows((hWnd, lParam) =>
            {
                User32.GetWindowThreadProcessId(hWnd, out uint processId);

                try
                {
                    var process = Process.GetProcessById((int)processId);
                    if (process.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase) && User32.IsWindowVisible(hWnd))
                    {
                        explorerWindows.Add(hWnd);
                    }
                }
                catch (ArgumentException ex)
                {
                }
                catch (InvalidOperationException ex)
                {
                }

                return true;
            }, IntPtr.Zero);

            return explorerWindows;
        }
    }
}
