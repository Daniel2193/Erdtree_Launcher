namespace Erdtree_Launcher
{
    public class Mod(string filename, string path, ModType modType, bool enabled, string displayName)
    {
        public string filename = filename;
        public string path = path;
        public string displayName = displayName;
        public ModType modType = modType;
        public bool enabled = enabled;
        public void EnableMod()
        {
            if (modType == ModType.ModEngine2_Folder)
            {
                enabled = true;
            }
            else
            {
                string disabledFilepath = Utils.GetFullPath(path,  filename.Replace(Filenames.SuffixDll, Filenames.SuffixDllDisabled));
                string enabledFilepath = Utils.GetFullPath(path,  filename);
                if (!enabled && File.Exists(enabledFilepath) && modType != ModType.ModEngine2_Seamless)
                {
                    return;
                };
                if (File.Exists(disabledFilepath))
                {
                    if (modType == ModType.ModEngine2_Seamless)
                    {
                        File.Delete(disabledFilepath);
                    }
                    else
                    {
                        File.Move(disabledFilepath, enabledFilepath);
                    }
                    enabled = true;
                }
                else
                {
                    MessageBox.Show("File not found\n\n" + disabledFilepath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void DisableMod()
        {
            if (modType == ModType.ModEngine2_Folder)
            {
                enabled = false;
            }
            else
            {
                string disabledFilepath = Utils.GetFullPath(path, filename.Replace(Filenames.SuffixDll, Filenames.SuffixDllDisabled));
                string enabledFilepath = Utils.GetFullPath(path, filename);
                if (!enabled && File.Exists(disabledFilepath))
                {
                    return;
                };
                if (File.Exists(enabledFilepath))
                {
                    if (modType == ModType.ModEngine2_Seamless)
                    {
                        File.CreateText(disabledFilepath).Close();
                    }
                    else
                    {
                        File.Move(enabledFilepath, disabledFilepath);
                    }
                    enabled = false;
                }
                else
                {
                    MessageBox.Show("File not found\n\n" + enabledFilepath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public override string ToString()
        {
            return displayName.Length > 0 ? displayName : filename.Replace(Foldernames.ModEngine2ModFolderPrefix, "");
        }
        public string ToMe2ConfigEntryString()
        {
            if (modType == ModType.ModEngine2_Folder)
            {
                return "{ enabled = " + enabled.ToString().ToLower() + ", name = \"" + displayName + "\", path = \"" + filename + "\" }";
            }
            else
            {
                return "";
            }
        }
        public Color GetColor()
        {
            return GetColor(modType);
        }
        public static Color GetColor(ModType modType)
        {
            return modType switch
            {
                ModType.ModEngine2_Folder => Color.FromArgb(164, 200, 255),
                ModType.ModEngine2_Ext_Dll => Color.FromArgb(151, 254, 215),
                ModType.ModEngine2_Seamless => Color.FromArgb(151, 254, 215),
                ModType.EldenModLoader => Color.FromArgb(195, 156, 224),
                _ => Color.White,
            };
        }
        public static void ListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            if (sender is not ListBox listBox) return;
            if (listBox.Items[e.Index] is not Mod mod) return;
            e.Graphics.FillRectangle(new SolidBrush(mod.GetColor()), e.Bounds);
            if (e.Font != null)
                e.Graphics.DrawString(mod.ToString(), e.Font, new SolidBrush(Color.Black), e.Bounds, StringFormat.GenericDefault);
        }
    }
    public enum ModLoaderType
    {
        Seamless,
        EldenModLoader,
        ModEngine2
    }
    public enum ModType
    {
        Error,
        EldenModLoader,
        ModEngine2_Folder,
        ModEngine2_Ext_Dll,
        ModEngine2_Seamless,
    }
}
