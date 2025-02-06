using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Erdtree_Launcher
{
    public partial class MainWindow : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr FindWindowA(IntPtr lpClassName, string lpWindowName);
        internal List<Mod> AvailableMods = [];
        public ModManager modManager = new();
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static MainWindow instance;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private void LoadMe2Mods()
        {
            var modsToRemove = AvailableMods.Where(mod => mod.modType == ModType.ModEngine2_Folder || mod.modType == ModType.ModEngine2_Ext_Dll || mod.modType == ModType.ModEngine2_Seamless).ToList();
            foreach (var mod in modsToRemove)
            {
                AvailableMods.Remove(mod);
            }
            foreach (var mod in Me2.ReadConfigMods())
            {
                AvailableMods.Add(mod);
            }
            if (!Directory.Exists(Utils.GetFullPath(Foldernames.ModEngine2Base)))
            {
                return;
            }
            else
            {
                var dirs = Directory.GetDirectories(Utils.GetFullPath(Foldernames.ModEngine2Base));
                foreach (var dir in dirs)
                {
                    string dirname = dir.Split("\\").Last();
                    if (dirname.StartsWith(Foldernames.ModEngine2ModFolderPrefix))
                    {
                        var existingMod = AvailableMods.Find(x => x.filename == dirname);
                        if (existingMod == null)
                        {
                            existingMod = new(
                                dirname,
                                dir.Split("\\")[^2] + "\\",
                                ModType.ModEngine2_Folder,
                                false,
                                dirname.Replace(Foldernames.ModEngine2ModFolderPrefix, "")
                            );
                            AvailableMods.Add(existingMod);
                        }
                    }
                }
            }
            string dll_path = Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2ExtDlls);
            if (!Directory.Exists(dll_path))
            {
                Directory.CreateDirectory(dll_path);
            }
            else
            {
                var files = Directory.GetFiles(dll_path);
                foreach (var file in files)
                {
                    if (File.Exists(file) && (file.EndsWith(Filenames.SuffixDll) || file.EndsWith(Filenames.SuffixDllDisabled)))
                    {
                        int splitPos = file.LastIndexOf('\\');
                        string filename = file[(splitPos + 1)..].Replace(Filenames.SuffixDllDisabled, Filenames.SuffixDll);
                        Mod tmp = new(filename, file[..splitPos], ModType.ModEngine2_Ext_Dll, file.EndsWith(Filenames.SuffixDll), filename);
                        AvailableMods.Add(tmp);
                    }
                }
            }
        }

        private void LoadEmlMods()
        {
            var modsToRemove = AvailableMods.Where(mod => mod.modType == ModType.EldenModLoader).ToList();
            foreach (var mod in modsToRemove)
            {
                AvailableMods.Remove(mod);
            }
            string dirPath = Utils.GetFullPath(Foldernames.EldenModLoaderMods);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                return;
            }
            else
            {
                var files = Directory.GetFiles(dirPath);
                foreach (var file in files)
                {
                    if (File.Exists(file) && (file.EndsWith(Filenames.SuffixDll) || file.EndsWith(Filenames.SuffixDllDisabled)))
                    {
                        string filename = Path.GetFileName(file).Replace(Filenames.SuffixDllDisabled, Filenames.SuffixDll);
                        string dir = Path.GetDirectoryName(file) ?? string.Empty;
                        Mod tmp = new(filename, dir, ModType.EldenModLoader, file.EndsWith(Filenames.SuffixDll), filename);
                        AvailableMods.Add(tmp);
                    }
                }
            }
        }

        private void LoadModsIntoUi()
        {
            listModsEnabled.Items.Clear();
            listModsDisabled.Items.Clear();
            foreach (Mod mod in AvailableMods)
            {
                if (mod.enabled)
                {
                    listModsEnabled.Items.Add(mod);
                }
                else
                {
                    listModsDisabled.Items.Add(mod);
                }
            }
        }

        public static void DisabledEldenModLoader()
        {
            try
            {
                File.Move(Utils.GetFullPath(Filenames.EldenModLoaderDll), Utils.GetFullPath(Filenames.EldenModLoaderDllDisabled));
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unable to disable Elden Mod Loader\nYou should manually rename \"{Filenames.EldenModLoaderDll}\" to \"{Filenames.EldenModLoaderDllDisabled}\"\n\n{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool PreLaunchChecksEldenModLoader()
        {
            if (!Validation.IsEldenModLoaderInstalled())
            {
                MessageBox.Show($"{Filenames.EldenModLoaderDll} not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (File.Exists(Utils.GetFullPath(Filenames.EldenModLoaderDllDisabled)))
                {
                    try
                    {
                        File.Move(Utils.GetFullPath(Filenames.EldenModLoaderDllDisabled), Utils.GetFullPath(Filenames.EldenModLoaderDll));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Failed to enable {Filenames.EldenModLoaderDll}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
        private static bool PreLaunchChecksModEngine2()
        {
            if (!Validation.IsModEngine2Installed())
            {
                MessageBox.Show("ModEngine2 not found, you can install it in the Mod Manager", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                Me2.WriteConfig();
                return true;
            }
        }
        private void LaunchGame(LaunchType type)
        {
            Utils.EnsureBasegameExeExistsOrQuit();
            if(type == LaunchType.VanillaOnline){
                if(!LaunchVanillaOnline()){
                    EnableUI();
                    return;
                }
            }
            bool useEldenModLoader = type == LaunchType.ModdedForceEldenModLoader || type == LaunchType.ModdedForceBoth || (type == LaunchType.Modded && AvailableMods.Any(mod => mod.enabled && mod.modType == ModType.EldenModLoader));
            bool useModEngine2 = type == LaunchType.ModdedForceModEngine2 || type == LaunchType.ModdedForceBoth || type == LaunchType.Seamless || (type == LaunchType.Modded && AvailableMods.Any(mod => mod.enabled && (mod.modType == ModType.ModEngine2_Folder || mod.modType == ModType.ModEngine2_Ext_Dll || mod.modType == ModType.ModEngine2_Seamless)));
            if (useEldenModLoader)
            {
                if (!PreLaunchChecksEldenModLoader())
                {
                    return;
                }
            }
            if (useModEngine2)
            {
                if (!PreLaunchChecksModEngine2())
                {
                    return;
                }
            }
            DisableUI();
            if (type == LaunchType.Seamless)
            {
                if (!Validation.IsSeamlessInstalled())
                {
                    var res = MessageBox.Show("Seamless Coop is not installed, you can install it in the Mod Manager", "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Validation.IsModEngine2Installed())
                {
                    MessageBox.Show($"The Seamless Coop Launcher ({Filenames.SeamlessExe}) doesn't load the mod if Steam thinks the game is already running. To play Vanilla Seamless Coop, you have to either use Mod Engine 2 or double click {Filenames.SeamlessExe}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string seamlessConfigPath = Utils.GetFullPath(Foldernames.ModEngine2Base, Filenames.ModEngine2ConfigSeamless);
                if (!File.Exists(seamlessConfigPath))
                {
                    File.WriteAllText(seamlessConfigPath, Me2.Me2SeamlessConfigContent());
                }
            }
            if ((type == LaunchType.VanillaOffline || useEldenModLoader) && !useModEngine2)
            {
                if(!LaunchVanillaOffline()){
                    EnableUI();
                    return;
                }
            }
            else if (useModEngine2)
            {
                if(!LaunchModEngine2(type == LaunchType.Seamless ? Filenames.ModEngine2ConfigSeamless : Filenames.ModEngine2Config)){
                    EnableUI();
                    return;
                }
            }
            CloseLauncher(useEldenModLoader);
        }

        private static bool LaunchModEngine2(string configFile)
        {
            ProcessStartInfo pis = new()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"cd '{Utils.GetFullPath(Foldernames.ModEngine2Base)}'; .\\{Filenames.ModEngine2Exe} -t er -c .\\{configFile}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using Process? p = Process.Start(pis);
                p?.WaitForExit();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to launch ModEngine2\n\n" + e.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private static bool LaunchVanillaOffline(bool online = false){
            try
            {
                using Process p = Process.Start(Utils.GetFullPath(Filenames.BasegameExe), online ? "" : "-eac-nop-loaded");
                p.Dispose();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to launch the Game\n\n" + e.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private static bool LaunchVanillaOnline(bool online = false){
            try
            {
                using Process p = Process.Start(Utils.GetFullPath(Filenames.EacExe));
                p.Dispose();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to launch the Game\n\n" + e.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public void CloseLauncher(bool wait)
        {
            if (wait)
            {
                WindowState = FormWindowState.Minimized;
                System.Timers.Timer timer = new(3000);
                int timeout = 10;
                timer.Elapsed += (_, _) =>
                {
                    IntPtr windowHandle = FindWindowA(IntPtr.Zero, Utils.GetFullPath(Filenames.BasegameExe));
                    if (windowHandle != IntPtr.Zero || timeout-- <= 0)
                    {
                        DisabledEldenModLoader();
                        timer.Stop();
                        Application.Exit();
                        Environment.Exit(0);
                    }
                };
                timer.Start();
            }
            else
            {
                Application.Exit();
                Environment.Exit(0);
            }
        }

        public MainWindow()
        {
            instance = this;
            InitializeComponent();
        }

        public void ReloadMods()
        {
            LoadEmlMods();
            LoadMe2Mods();
            Me2.WriteConfig();
            LoadModsIntoUi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = Utils.GetWindowTitle();
            Utils.EnsureBasegameExeExistsOrQuit();
            ReloadMods();
        }

        private void BtnModded_Click(object sender, EventArgs e)
        {
            LaunchGame(LaunchType.Modded);
        }

        private void BtnCoop_Click(object sender, EventArgs e)
        {
            LaunchGame(LaunchType.Seamless);
        }

        private void BtnVanillaOffline_Click(object sender, EventArgs e)
        {
            LaunchGame(LaunchType.VanillaOffline);
        }

        private void BtnVanillaOnline_Click(object sender, EventArgs e)
        {
            LaunchGame(LaunchType.VanillaOnline);
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            modManager.Close();
            Application.Exit();
            Close();
            Environment.Exit(0);
        }

        private ListBox? FindSourceListBox(Mod item)
        {
            // Check which listBox contains the dragged item
            if (listModsEnabled.Items.Contains(item))
                return listModsEnabled;
            else if (listModsDisabled.Items.Contains(item))
                return listModsDisabled;
            return null;
        }

        private void ListMods_DragDrop(object sender, DragEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;
            if (e.Data != null && e.Data.GetDataPresent(typeof(Mod)))
            {
                var mod = e.Data.GetData(typeof(Mod)) as Mod;
                if (mod == null) return;
                var sourceBox = FindSourceListBox(mod);
                if (sourceBox == null) return;
                if (sourceBox.Name == listBox.Name || sourceBox == listBox)
                {
                    return;
                }
                if (sourceBox == listModsEnabled)
                {
                    mod.DisableMod();
                    sourceBox.Items.Remove(mod);
                    listBox.Items.Add(mod);
                }
                else if (sourceBox == listModsDisabled)
                {
                    mod.EnableMod();
                    sourceBox.Items.Remove(mod);
                    listBox.Items.Add(mod);
                }
                if (mod.modType == ModType.ModEngine2_Folder)
                {
                    Me2.WriteConfig();
                }
            }
            return;
        }

        private void ListMods_DragOver(object _, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void ListMods_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is not ListBox listBox) return;

            // Get the item under the mouse cursor
            int index = listBox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                // Start dragging the selected item
                listBox.DoDragDrop(listBox.Items[index], DragDropEffects.Move);
            }
        }

        private void BtnModManager_Click(object sender, EventArgs e)
        {
            modManager.Hide();
            modManager.Show();
            modManager.ReloadUi();
        }

        private void ListMods_DrawItem(object sender, DrawItemEventArgs e)
        {
            Mod.ListBox_DrawItem(sender, e);
        }

        private void ListModsEnabled_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(await Updater.IsUpdateAvailable()){
                var res = MessageBox.Show("An update is available, do you want to install it?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(res == DialogResult.Yes){
                    DisableUI(true);
                    await Updater.DownloadAndInstallUpdate();
                    EnableUI();
                }
            }
        }

        private void DisableUI(bool update = false){
            btnVanillaOffline.Enabled = false;
            btnVanillaOnline.Enabled = false;
            btnModded.Enabled = false;
            btnCoop.Enabled = false;
            btnUpdate.Enabled = false;
            listModsEnabled.Enabled = false;
            listModsDisabled.Enabled = false;
            if(update){
                btnQuit.Enabled = false;
            }
        }

        private void EnableUI(){
            btnVanillaOffline.Enabled = true;
            btnVanillaOnline.Enabled = true;
            btnModded.Enabled = true;
            btnCoop.Enabled = true;
            btnUpdate.Enabled = true;
            listModsEnabled.Enabled = true;
            listModsDisabled.Enabled = true;
            btnQuit.Enabled = true;
        }
    }

    public enum LaunchType
    {
        VanillaOffline,
        VanillaOnline,
        Seamless,
        Modded,
        ModdedForceEldenModLoader,
        ModdedForceModEngine2,
        ModdedForceBoth
    }
}
