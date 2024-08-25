﻿using System;
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

        private readonly int topMostSetRetries = 50;
        private readonly int topMostSetSleepTimeMilliseconds = 100;

        #region user32 dll imports
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOPMOST = 0x00000008;
        #endregion

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

        public bool SetTopMost()
        {
            return SetttingTopMost(true);
        }

        public bool SetNoTopMost()
        {
            return SetttingTopMost(false);
        }

        private bool SetttingTopMost(bool isTopMost)
        {
            if (explorerWindowHandle == IntPtr.Zero ||
                !IsWindow(explorerWindowHandle) ||
                !IsWindowVisible(explorerWindowHandle))
            {
                return false;
            }

            SetForegroundWindow(explorerWindowHandle);

            int retries = topMostSetRetries;
            while (retries-- > 0)
            {
                SetWindowPos(
                explorerWindowHandle,
                isTopMost ? HWND_TOPMOST : HWND_NOTOPMOST,
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_SHOWWINDOW);

                int exStyle = GetWindowLong(explorerWindowHandle, GWL_EXSTYLE);
                bool currentStateIsTopMost = (exStyle & WS_EX_TOPMOST) != 0;
                if (currentStateIsTopMost == isTopMost)
                {
                    return true; 
                }

                Thread.Sleep(topMostSetSleepTimeMilliseconds); 
            }

            return false;
        }

        public bool SetForegroundWindow()
        {
            if (explorerWindowHandle == IntPtr.Zero ||
                !IsWindow(explorerWindowHandle) ||
                !IsWindowVisible(explorerWindowHandle))
            {
                return false;
            }

            return SetForegroundWindow(explorerWindowHandle);
        }

        private void CheckIfWindowClosed(object state)
        {
            IntPtr explorerWindowHandle = (IntPtr)state;

            if (!IsWindowVisible(explorerWindowHandle))
            {
                OnExplorerClosed?.Invoke(explorerWindowHandle);
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
