using System.IO.Compression;

namespace Erdtree_Launcher
{
    public partial class ModManager : Form
    {
        private ModType _modType = ModType.Error;
        private HttpClient _httpClient = new();
        private bool isDownloading = false;
        public static readonly string text_yes = "✅";
        public static readonly string text_no = "❌";
        public ModManager()
        {
            InitializeComponent();
        }

        public void ReloadUi()
        {
            MainWindow.instance.ReloadMods();
            lbSeamlessStatus.Text = Validation.IsSeamlessInstalled() ? text_yes : text_no;
            lbSeamlessStatus.ForeColor = Validation.IsSeamlessInstalled() ? Color.Green : Color.Red;
            lbEmlStatus.Text = Validation.IsEldenModLoaderInstalled() ? text_yes : text_no;
            lbEmlStatus.ForeColor = Validation.IsEldenModLoaderInstalled() ? Color.Green : Color.Red;
            lbMe2Status.Text = Validation.IsModEngine2Installed() ? text_yes : text_no;
            lbMe2Status.ForeColor = Validation.IsModEngine2Installed() ? Color.Green : Color.Red;
            btnInstallSeamless.Enabled = !isDownloading;
            btnInstallEml.Enabled = !isDownloading;
            btnInstallMe2.Enabled = !isDownloading;
            listAllMods.Items.Clear();
            foreach (var mod in MainWindow.instance.AvailableMods)
            {
                listAllMods.Items.Add(mod);
            }
        }

        private void ModManager_Load(object sender, EventArgs e)
        {
            rbModTypeEml.BackColor = Mod.GetColor(ModType.EldenModLoader);
            rbModTypeMe2Dll.BackColor = Mod.GetColor(ModType.ModEngine2_Ext_Dll);
            rbModTypeMe2Folder.BackColor = Mod.GetColor(ModType.ModEngine2_Folder);
            ReloadUi();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Hide();
            MainWindow.instance.Show();
            MainWindow.instance.Focus();
        }

        private void BtnInstallSeamless_Click(object sender, EventArgs e)
        {
            InstallModLoader(ModLoaderType.Seamless);
        }

        private void BtnInstallEml_Click(object sender, EventArgs e)
        {
            InstallModLoader(ModLoaderType.EldenModLoader);
        }

        private void BtnInstallMe2_Click(object sender, EventArgs e)
        {
            InstallModLoader(ModLoaderType.ModEngine2);
        }

        private void BtnImportMod_Click(object sender, EventArgs e)
        {
            if (_modType == ModType.Error)
            {
                MessageBox.Show("Mod Type not selected", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_modType == ModType.ModEngine2_Folder)
            {
                if (tbImportModDisplayName.Text.Trim() == "")
                {
                    MessageBox.Show("Mod Display Name should be empty", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                FolderBrowserDialog fbd = new()
                {
                    Description = "Select the mod Folder",
                    ShowNewFolderButton = false,
                };
                var result = fbd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var selectedFolder = fbd.SelectedPath;
                    var modName = tbImportModDisplayName.Text;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        modName = modName.Replace(c.ToString(), "");
                    }
                    var targetModPath = Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2ModFolderPrefix + modName.Replace(" ", ""));
                    if (Directory.Exists(targetModPath))
                    {
                        MessageBox.Show("Mod folder name already exists", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Utils.CopyDirectory(selectedFolder, targetModPath);
                    MessageBox.Show("Mod imported successfully", "Import successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (_modType == ModType.ModEngine2_Ext_Dll || _modType == ModType.EldenModLoader)
            {
                ImportDllZipMod(_modType);
            }

            _modType = ModType.Error;
            ReloadUi();
        }
        private static void ImportDllZipMod(ModType type)
        {
            if (type != ModType.ModEngine2_Ext_Dll && type != ModType.EldenModLoader)
            {
                throw new Exception("Invalid ModType for DLL import");
            }
            OpenFileDialog ofd = new()
            {
                Title = "Select the mod DLL",
                Filter = "DLL Files|*.dll|Zip Files|*.zip",
                Multiselect = true,
                CheckFileExists = true,
                CheckPathExists = true,
            };
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                var files = ofd.FileNames;
                foreach (var file in files)
                {
                    string targetPath = "";
                    string filename = Path.GetFileName(file);
                    if (filename.EndsWith(Filenames.SuffixDll))
                    {
                        if (type == ModType.ModEngine2_Ext_Dll)
                        {
                            targetPath = Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2ExtDlls, filename);
                        }
                        else if (type == ModType.EldenModLoader)
                        {
                            targetPath = Utils.GetFullPath(Foldernames.EldenModLoaderMods, filename);
                        }
                        if (File.Exists(targetPath))
                        {
                            var res = MessageBox.Show($"{filename} already exists\nReplace existing file?", "Import Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (res != DialogResult.Yes)
                            {
                                return;
                            }
                            else
                            {
                                File.Delete(targetPath);
                            }
                        }
                        File.Copy(file, targetPath);
                    }
                    else if (filename.EndsWith(".zip"))
                    {
                        var tmpPath = Path.Combine(Path.GetTempPath(), "Erdtree_Launcher");
                        if (Directory.Exists(tmpPath))
                        {
                            Directory.Delete(tmpPath, true);
                        }
                        Directory.CreateDirectory(tmpPath);
                        try
                        {
                            ZipFile.ExtractToDirectory(file, tmpPath, true);
                            var extractedFiles = Directory.GetFiles(tmpPath);
                            foreach (var extractedFile in extractedFiles)
                            {
                                string extractedFilename = Path.GetFileName(extractedFile);
                                if(Filenames.IgnoredFilenames.Contains(extractedFilename)){
                                    continue;
                                }
                                if (type == ModType.ModEngine2_Ext_Dll)
                                {
                                    targetPath = Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2ExtDlls,  extractedFilename);
                                }
                                else if (type == ModType.EldenModLoader)
                                {
                                    targetPath = Utils.GetFullPath(Foldernames.EldenModLoaderMods, extractedFilename);
                                }
                                if (File.Exists(targetPath))
                                {
                                    if(Filenames.ProtectedFilenames.Contains(extractedFilename))
                                    {
                                        continue;
                                    }
                                    var res = MessageBox.Show($"{extractedFilename} already exists\nReplace existing file?", "Import Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                    if (res != DialogResult.Yes)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        File.Delete(targetPath);
                                    }
                                }
                                File.Copy(extractedFile, targetPath);
                            }
                            var extractedFolders = Directory.GetDirectories(tmpPath);
                            string targetFolder = "";
                            if (type == ModType.ModEngine2_Ext_Dll)
                            {
                                targetFolder = Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2ExtDlls);
                            }
                            else if (type == ModType.EldenModLoader)
                            {
                                targetFolder = Utils.GetFullPath(Foldernames.EldenModLoaderMods);
                            }
                            foreach (var extractedFolder in extractedFolders)
                            {
                                Utils.CopyDirectory(extractedFolder, targetFolder);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error while extracting {filename}\n\n{ex.Message}", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
        }

        private async Task DownloadFile(string url, string outputPath)
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var contentLength = response.Content.Headers.ContentLength ?? -1L;
            var totalRead = 0L;
            var buffer = new byte[8192];
            bool isMoreToRead;
            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            do
            {
                var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                if (read == 0)
                {
                    isMoreToRead = false;
                    continue;
                }

                await fileStream.WriteAsync(buffer, 0, read);
                totalRead += read;

                if (contentLength != -1)
                {
                    int progress = (int)(totalRead * 100L / contentLength);
                    pbInstallLoader.Value = progress; // Update progress bar
                    lbDownloadProgress.Text = $"Progress: {progress}%";
                }
            } while (isMoreToRead = totalRead < contentLength);
        }
        private void BtnReloadUi_Click(object sender, EventArgs e)
        {
            ReloadUi();
        }

        private void RbModTypeEml_CheckedChanged(object sender, EventArgs e)
        {
            _modType = ModType.EldenModLoader;
            tbImportModDisplayName.Enabled = false;
            tbImportModDisplayName.Text = "";
        }
        private void RbModTypeMe2Dll_CheckedChanged(object sender, EventArgs e)
        {
            _modType = ModType.ModEngine2_Ext_Dll;
            tbImportModDisplayName.Enabled = false;
            tbImportModDisplayName.Text = "";
        }
        private void RbModTypeMe2Folder_CheckedChanged(object sender, EventArgs e)
        {
            _modType = ModType.ModEngine2_Folder;
            tbImportModDisplayName.Enabled = true;
        }
        private async void InstallModLoader(ModLoaderType type)
        {
            if (isDownloading)
            {
                return;
            }
            string name = GetModLoaderName(type);
            if (Validation.GetInstalledState(type))
            {
                var res = MessageBox.Show($"{name} is already installed, Reinstall?\n", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes)
                {
                    return;
                }
            }
            isDownloading = true;
            ReloadUi();
            var tmpPath = Path.Combine(Path.GetTempPath(), "Erdtree_Launcher");
            if (Directory.Exists(tmpPath))
            {
                Directory.Delete(tmpPath, true);
            }
            Directory.CreateDirectory(tmpPath);
            try
            {
                var file = Path.Combine(tmpPath, "downloaded_package.zip");
                await DownloadFile(Urls.GetDownloadUrl(type), file);
                ZipFile.ExtractToDirectory(file, (type == ModLoaderType.ModEngine2 || type == ModLoaderType.Seamless) ? tmpPath : Utils.GetFullPath(""), type != ModLoaderType.Seamless);
                if (type == ModLoaderType.EldenModLoader)
                {
                    MainWindow.DisabledEldenModLoader();
                }
                else if (type == ModLoaderType.ModEngine2)
                {
                    if (Directory.GetDirectories(tmpPath).Length != 1)
                    {
                        MessageBox.Show($"Error while extracting {name}\n", "Extraction Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ResetDownloadState();
                        return;
                    }
                    var extractedFolder = Directory.GetDirectories(tmpPath)[0];
                    Utils.CopyDirectory(extractedFolder, Utils.GetFullPath(Foldernames.ModEngine2Base));
                }
                else if (type == ModLoaderType.Seamless)
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while installing {name}\n\n{ex.Message}", "Install Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetDownloadState();
                return;
            }
            ResetDownloadState();
            ReloadUi();
            MessageBox.Show($"Successfully installed {name}", "Install successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static string GetModLoaderName(ModLoaderType type)
        {
            return type switch
            {
                ModLoaderType.EldenModLoader => "Elden Mod Loader",
                ModLoaderType.ModEngine2 => "Mod Engine 2",
                ModLoaderType.Seamless => "Seamless Coop",
                _ => "[Unknown Mod Loader]",
            };
        }
        private void ResetDownloadState()
        {
            isDownloading = false;
            pbInstallLoader.Value = 0;
            lbDownloadProgress.Text = "Ready";
        }

        private void ListAllMods_DrawItem(object sender, DrawItemEventArgs e)
        {
            Mod.ListBox_DrawItem(sender, e);
        }

        private void ModManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
