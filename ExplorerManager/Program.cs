using System.Runtime.InteropServices;

namespace ExplorerManager
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        static void Main()
        {
            var explorerProcessManager = new ExplorerProcessManager();
            explorerProcessManager.OnExplorerClosed += () =>
            {
                Console.WriteLine("Explorer window has closed. Exiting application.");
                Application.Exit();
            };

            var explorer = explorerProcessManager.StartExplorer();
            if (explorer != IntPtr.Zero)
            {
                Console.WriteLine($"New explorer.exe window found with handle: {explorer}");
                Application.Run();
            }
            else
            {
                Console.WriteLine("No new explorer.exe window was found.");
            }
        }
    }
}