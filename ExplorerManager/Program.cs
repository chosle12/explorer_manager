using System.Runtime.InteropServices;

namespace ExplorerManager
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        static void Main()
        {
            ExplorerProcess.closeCallback += () =>
            {
                Console.WriteLine("Explorer window has closed. Exiting application.");
                Application.Exit();
            };

            var explorerProcess = new ExplorerProcess();
            var newExplorerWindow = explorerProcess.RunProcess();
            if (newExplorerWindow != IntPtr.Zero)
            {
                Console.WriteLine($"New explorer.exe window found with handle: {newExplorerWindow}");
                Application.Run();
            }
            else
            {
                Console.WriteLine("No new explorer.exe window was found.");
            }
        }
    }
}