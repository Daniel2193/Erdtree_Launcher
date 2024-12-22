using System.Runtime.InteropServices;

#pragma warning disable IDE0052 // Remove unread private members
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace Erdtree_Launcher
{
    internal static class Program
    {
        private static Mutex? mutex;
        private static readonly string AppGuid = "74ba23f8-b346-457f-a495-5bbac317837f";

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            mutex = new(true, AppGuid, out bool isFirstInstance);
            if (!isFirstInstance)
            {
                IntPtr intPtr = FindWindow(null, Utils.GetWindowTitle());
                if (intPtr != IntPtr.Zero)
                {
                    ShowWindow(intPtr, 9);
                    SetForegroundWindow(intPtr);
                }
                return;
            }
            if (!Utils.BasegameExeExists())
            {
                MessageBox.Show($"{Filenames.BasegameExe} not found. Please make sure the launcher is in the correct Location and start it from there or via Steam", "Setup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new MainWindow());
            }
        }
    }
}