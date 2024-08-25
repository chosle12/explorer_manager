using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ExplorerManager
{
    public class ExplorerProcessManager
    {
        private const int MaxWaitTimeInMilliseconds = 2000;
        private const int PollingIntervalInMilliseconds = 50;

        private const int TopMostSetRetries = 50;
        private const int TopMostSetSleepTimeMilliseconds = 100;

        public delegate void ExplorerClosedCallback(IntPtr hWnd);
        public event ExplorerClosedCallback OnExplorerClosed;

        private Timer windowCloseCheckTimer;
        private IntPtr explorerWindowHandle;

        public IntPtr StartExplorer()
        {
            var initialExplorerWindows = GetExplorerWindowHandles();

            Process.Start("explorer.exe");

            explorerWindowHandle = IntPtr.Zero;
            int elapsedMilliseconds = 0;
            while (elapsedMilliseconds < MaxWaitTimeInMilliseconds)
            {
                var currentExplorerWindows = GetExplorerWindowHandles();
                var newExplorerWindows = currentExplorerWindows.Except(initialExplorerWindows).ToList();

                if (newExplorerWindows.Any())
                {
                    explorerWindowHandle = newExplorerWindows.First();
                    break;
                }

                Thread.Sleep(PollingIntervalInMilliseconds);
                elapsedMilliseconds += PollingIntervalInMilliseconds;
            }

            if (explorerWindowHandle != IntPtr.Zero)
            {
                windowCloseCheckTimer = new Timer(OnWindowClosedCheck, explorerWindowHandle, 0, 100);
            }

            return explorerWindowHandle;
        }

        public Process GetProcess()
        {
            if (!IsValidExplorerWindowHandle())
            {
                return null;
            }

            User32.GetWindowThreadProcessId(explorerWindowHandle, out uint processId);

            try
            {
                return Process.GetProcessById((int)processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public bool SetTopMost()
        {
            return SetTopMostState(true);
        }

        public bool SetNoTopMost()
        {
            return SetTopMostState(false);
        }

        public bool SetForegroundWindow()
        {
            if (!IsValidExplorerWindowHandle())
            {
                return false;
            }

            return User32.SetForegroundWindow(explorerWindowHandle);
        }

        private bool SetTopMostState(bool isTopMost)
        {
            if (!IsValidExplorerWindowHandle())
            {
                return false;
            }

            User32.SetForegroundWindow(explorerWindowHandle);

            int retries = TopMostSetRetries;
            while (retries-- > 0)
            {
                User32.SetWindowPos(
                    explorerWindowHandle,
                    isTopMost ? User32.HWND_TOPMOST : User32.HWND_NOTOPMOST,
                    0, 0, 0, 0,
                    User32.SWP_NOMOVE | User32.SWP_NOSIZE | User32.SWP_NOACTIVATE | User32.SWP_SHOWWINDOW);

                int exStyle = User32.GetWindowLong(explorerWindowHandle, User32.GWL_EXSTYLE);
                bool currentStateIsTopMost = (exStyle & User32.WS_EX_TOPMOST) != 0;
                if (currentStateIsTopMost == isTopMost)
                {
                    return true;
                }

                Thread.Sleep(TopMostSetSleepTimeMilliseconds);
            }

            return false;
        }

        private bool IsValidExplorerWindowHandle()
        {
            return explorerWindowHandle != IntPtr.Zero &&
                   User32.IsWindow(explorerWindowHandle) &&
                   User32.IsWindowVisible(explorerWindowHandle);
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
                catch (ArgumentException)
                {
                    // 로깅 또는 다른 처리를 할 수 있습니다.
                }
                catch (InvalidOperationException)
                {
                    // 로깅 또는 다른 처리를 할 수 있습니다.
                }

                return true;
            }, IntPtr.Zero);

            return explorerWindows;
        }
    }
}
