using System.Text.RegularExpressions;
using Tomlyn;
using Tomlyn.Model;

namespace Erdtree_Launcher
{
    public static class Me2
    {
        public static List<Mod> ReadConfigMods()
        {
            List<Mod> result = [];
            string configPath = Utils.GetFullPath(Foldernames.ModEngine2Base, Filenames.ModEngine2Config);
            if (!File.Exists(configPath) || !Validation.IsModEngine2Installed())
            {
                WriteConfig();
            }
            var document = Toml.Parse(File.ReadAllText(configPath));
            if (document.HasErrors)
            {
                return result;
            }
            var root = document.ToModel();
            if (root is null ||
                !root.TryGetValue("extension", out var extensionNode) || extensionNode is not TomlTable extensionTable ||
                !extensionTable.TryGetValue("mod_loader", out var modloaderNode) || modloaderNode is not TomlTable modloaderTable ||
                !modloaderTable.TryGetValue("mods", out var modsNode) || modsNode is not TomlArray mods)
            {
                return result;
            }
            else
            {
                foreach (var modEntry in mods)
                {
                    if (modEntry is TomlTable modTable)
                    {
                        var name = modTable.TryGetValue("name", out var nameNode) ? nameNode as string : null;
                        var path = modTable.TryGetValue("path", out var pathNode) ? pathNode as string : null;
                        var enabled = modTable.TryGetValue("enabled", out var enabledNode) && enabledNode is bool b && b;
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(path) && !result.Any(x => x.displayName == name || x.path == path))
                        {
                            result.Add(new Mod(path, Foldernames.ModEngine2Base, ModType.ModEngine2_Folder, enabled, name));
                        }
                    }
                }
            }
            if (Validation.IsSeamlessInstalled())
            {
                string path = Path.Combine(AppContext.BaseDirectory, Foldernames.SeamlessBase);
                result.Add(new Mod(Filenames.SeamlessDll, path, ModType.ModEngine2_Seamless, !File.Exists(Path.Combine(path, Filenames.SeamlessDll.Replace(Filenames.SuffixDll, Filenames.SuffixDllDisabled))), "Seamless Coop"));
            }
            return result;
        }
        public static void WriteConfig()
        {
            var root = new TomlTable();
            var modengineTable = new TomlTable();
            var ext_dllsArray = new TomlArray();
            foreach (var mod in MainWindow.instance.AvailableMods.Where(x => x.enabled && (x.modType == ModType.ModEngine2_Ext_Dll || x.modType == ModType.ModEngine2_Seamless)))
            {
                if (mod.modType == ModType.ModEngine2_Seamless)
                {
                    ext_dllsArray.Add(Path.Combine(mod.path, Filenames.SeamlessDll));
                    continue;
                }
                else
                {
                    ext_dllsArray.Add(Path.Combine(Foldernames.ModEngine2ExtDlls, mod.filename));
                }
            }
            modengineTable.Add("external_dlls", ext_dllsArray);
            modengineTable.Add("debug", false);
            var extensionTable = new TomlTable();
            var modLoaderTable = new TomlTable();
            var modsArray = new TomlArray();
            var scylla_hideTable = new TomlTable{
                { "enabled", false }
            };
            foreach (var mod in MainWindow.instance.AvailableMods.Where(x => x.modType == ModType.ModEngine2_Folder))
            {
                modsArray.Add(mod.ToMe2ConfigEntryString());
            }
            modLoaderTable.Add("enabled", true);
            modLoaderTable.Add("loose_params", false);
            modLoaderTable.Add("mods", modsArray);
            extensionTable.Add("scylla_hide", scylla_hideTable);
            extensionTable.Add("mod_loader", modLoaderTable);
            root.Add("modengine", modengineTable);
            root.Add("extension", extensionTable);
            string libOutput = Toml.FromModel(root, new TomlModelOptions
            {
                IncludeFields = true,
                IgnoreMissingProperties = false,
            });
            string dirPath = Utils.GetFullPath(Foldernames.ModEngine2Base);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllText(Utils.GetFullPath(Foldernames.ModEngine2Base, Filenames.ModEngine2Config), PostFix(libOutput));
        }
        public static string PostFix(string input)
        {
            string step2 = Regex.Replace(input, @"""({.*?})""", "$1");
            return step2.Replace("\\\"", "\"");
        }
        public static string Me2SeamlessConfigContent()
        {
            return $"[modengine]\ndebug = false\nexternal_dlls = [\"{Path.Combine(AppContext.BaseDirectory, Foldernames.SeamlessBase, Filenames.SeamlessDll).Replace("\\", "\\\\")}\"]\n[extension.scylla_hide]\nenabled = false\n[extension.mod_loader]\nenabled = true\nloose_params = false\nmods = []";
        }
    }
}