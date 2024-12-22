namespace Erdtree_Launcher
{
    public static class Validation
    {
        public static bool IsEldenModLoaderInstalled()
        {
            return (File.Exists(Utils.GetFullPath(Filenames.EldenModLoaderDll))
                || File.Exists(Utils.GetFullPath(Filenames.EldenModLoaderDllDisabled)))
                && File.Exists(Utils.GetFullPath(Filenames.EldenModLoaderConfig));
        }
        public static bool IsModEngine2Installed()
        {
            return File.Exists(Utils.GetFullPath(Foldernames.ModEngine2Base, Filenames.ModEngine2Exe))
                && Directory.Exists(Utils.GetFullPath(Foldernames.ModEngine2Base, Foldernames.ModEngine2Base.ToLower()));
        }
        public static bool IsSeamlessInstalled()
        {
            return Directory.Exists(Utils.GetFullPath(Foldernames.SeamlessBase))
                && (File.Exists(Utils.GetFullPath(Foldernames.SeamlessBase, Filenames.SeamlessDll)) || File.Exists(Utils.GetFullPath(Foldernames.SeamlessBase, Filenames.SeamlessDll.Replace(Filenames.SuffixDll, Filenames.SuffixDllDisabled))))
                && File.Exists(Utils.GetFullPath(Foldernames.SeamlessBase, Filenames.SeamlessConfig));
        }
        public static bool IsEacInstalled()
        {
            return File.Exists(Utils.GetFullPath(Filenames.EacExe));
        }
        public static bool GetInstalledState(ModLoaderType type)
        {
            return type switch
            {
                ModLoaderType.EldenModLoader => IsEldenModLoaderInstalled(),
                ModLoaderType.ModEngine2 => IsModEngine2Installed(),
                ModLoaderType.Seamless => IsSeamlessInstalled(),
                _ => false,
            };
        }
    }
}