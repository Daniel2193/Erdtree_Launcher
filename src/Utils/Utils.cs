using System.Reflection;

namespace Erdtree_Launcher
{
    public static class Utils
    {
        private static readonly string basePath = AppContext.BaseDirectory;
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "[AssemblyVersion not found]";
        }
        public static string GetWindowTitle()
        {
            return "Erdtree Launcher v" + GetVersion();
        }
        public static bool BasegameExeExists()
        {
            return File.Exists(GetFullPath(Filenames.BasegameExe));
        }
        public static void EnsureBasegameExeExistsOrQuit()
        {
            if (!BasegameExeExists())
            {
                MessageBox.Show($"{Filenames.BasegameExe} not found, make sure you copy this launcher into your ELDEN RING/Game folder and run it from there (or via Steam)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                Environment.Exit(1);
            }
        }
        public static string GetFullPath(params string[] paths)
        {
            List<string> arr =
            [
                basePath, .. paths
            ];
            return Path.Combine([.. arr]);
        }
        /*
        public static void CopyDirectory(string source, string target, bool recursive = true)
        {
            Directory.CreateDirectory(target);
            foreach (var file in Directory.GetFiles(source))
            {
                string targetFile = Path.Combine(target, Path.GetFileName(file));
                if (File.Exists(targetFile))
                {
                    if (Filenames.ProtectedFilenames.Contains(Path.GetFileName(file)))
                    {
                        continue;
                    }
                    else
                    {
                        File.Delete(targetFile);
                    }
                }
                File.Copy(file, targetFile);
            }
            if (recursive)
            {
                foreach (var directory in Directory.GetDirectories(source))
                {
                    CopyDirectory(directory, Path.Combine(target, Path.GetFileName(directory)));
                }
            }
        }
        */
        public static void CopyDirectory(string source, string target, bool recursive = true)
        {
            Directory.CreateDirectory(target);

            var files = Directory.GetFiles(source);
            var directories = recursive ? Directory.GetDirectories(source) : Array.Empty<string>();

            Parallel.ForEach(files, file =>
            {
                string targetFile = Path.Combine(target, Path.GetFileName(file));
                if (File.Exists(targetFile) && !Filenames.ProtectedFilenames.Contains(Path.GetFileName(file)))
                {
                    File.Delete(targetFile);
                }
                if (!File.Exists(targetFile))
                {
                    File.Copy(file, targetFile);
                }
            });

            Parallel.ForEach(directories, directory =>
            {
                CopyDirectory(directory, Path.Combine(target, Path.GetFileName(directory)), recursive);
            });
        }
    }
}
