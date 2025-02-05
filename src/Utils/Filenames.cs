namespace Erdtree_Launcher
{
    public static class Filenames
    {
        public static readonly string BasegameExe = "eldenring.exe";
        public static readonly string LauncherExe = "start_protected_game.exe";
        public static readonly string LauncherSig = "update.sig";
        public static readonly string SeamlessExe = "ersc_launcher.exe";
        public static readonly string SeamlessDll = "ersc.dll";
        public static readonly string SeamlessConfig = "ersc_settings.ini";
        public static readonly string EacExe = "start_protected_game_original.exe";
        public static readonly string EldenModLoaderDll = "dinput8.dll";
        public static readonly string EldenModLoaderDllDisabled = "dinput8";
        public static readonly string EldenModLoaderConfig = "mod_loader_config.ini";
        public static readonly string SuffixDll = ".dll";
        public static readonly string SuffixDllDisabled = ".dll.disabled";
        public static readonly string ModEngine2Exe = "modengine2_launcher.exe";
        public static readonly string ModEngine2Config = "config_et_launcher.toml";
        public static readonly string ModEngine2ConfigSeamless = "config_et_seamless.toml";
        public static readonly string[] ProtectedFilenames =
        [
            "ersc_settings.ini",
            "config_eldenring.toml",
            "config_darksouls3.toml",
            "config_armoredcore6.toml",
        ];
        public static readonly string[] IgnoredFilenames = [
            "license.txt",
            "readme.txt",
        ];
    }
    public static class Foldernames
    {
        //TODO - use Path.Combine
        public static readonly string EldenModLoaderMods = "mods";
        public static readonly string ModEngine2Base = "ModEngine2";
        public static readonly string ModEngine2ExtDlls = "ext_dlls";
        public static readonly string ModEngine2ModFolderPrefix = "mod_";
        public static readonly string SeamlessBase = "SeamlessCoop";
    }
    public static class Urls {
        public static readonly string SeamlessDownload = "https://github.com/LukeYui/EldenRingSeamlessCoopRelease/releases/latest/download/ersc.zip";
        public static readonly string EldenModLoaderDownload = "https://github.com/techiew/EldenRingModLoader/releases/latest/download/EldenModLoader.zip";
        public static readonly string ModEngine2Download = "https://github.com/soulsmods/ModEngine2/releases/download/release-2.1.0/ModEngine-2.1.0.0-win64.zip";
        public static readonly string LauncherUpdate = "https://github.com/Daniel2193/Erdtree_Launcher/releases/latest";
        public static readonly string LauncherDownload = $"{LauncherUpdate}/download/start_protected_game.exe";
        public static readonly string LauncherSignature = $"{LauncherUpdate}/download/update.sig";
        public static string GetDownloadUrl(ModLoaderType type)
        {
            return type switch
            {
                ModLoaderType.EldenModLoader => EldenModLoaderDownload,
                ModLoaderType.ModEngine2 => ModEngine2Download,
                ModLoaderType.Seamless => SeamlessDownload,
                _ => "",
            };
        }
    }
}