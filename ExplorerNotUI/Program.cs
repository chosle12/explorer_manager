using System.Runtime.InteropServices;

namespace ExplorerManager
{
    internal static class Program
    {
        static void Main()
        {
            var explorerProcessManager = new ExplorerProcessManager();
            explorerProcessManager.OnExplorerClosed += (hWnd) =>
            {
                Application.Exit();
            };

            var explorer = explorerProcessManager.StartExplorer();
            if (explorer != IntPtr.Zero)
            {
                Application.Run();
            }
        }
    }
}