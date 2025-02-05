using System.Diagnostics;
using System.Security.Cryptography;

namespace Erdtree_Launcher
{
    public static class Updater
    {
        private static readonly string public_key = "";
        private static readonly string updateLocation = Path.Combine(Path.GetTempPath(), "Erdtree_Launcher_Update");
        private static readonly string updateExe = Path.Combine(updateLocation, Filenames.LauncherExe);
        private static readonly string updateSignature = Path.Combine(updateLocation, Filenames.LauncherSig);

        public static async Task<bool> IsUpdateAvailable()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            try
            {
                using HttpResponseMessage response = await client.GetAsync(Urls.LauncherUpdate, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                string? redirectedUrl = response.RequestMessage?.RequestUri?.ToString();
                if (string.IsNullOrEmpty(redirectedUrl))
                {
                    MessageBox.Show("Update Check failed", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (redirectedUrl.Contains("/tag/v"))
                {
                    string tag = redirectedUrl.Split("/tag/v")[1];
                    if (Utils.GetVersion().CompareTo(tag) < 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                MessageBox.Show("Update Check failed", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //return Utils.GetVersion().CompareTo(Github.GetRelease(Github.DownloadType.Launcher).TagName) < 0;
        }
        public static async Task DownloadAndInstallUpdate()
        {
            try
            {
                // var release = Github.GetRelease(Github.DownloadType.Launcher);
                // foreach (var asset in release.Assets)
                // {
                //     await ModManager.instance.DownloadFile(asset.BrowserDownloadUrl, updateLocation);
                // }
                await ModManager.instance.DownloadFile(Urls.LauncherDownload, updateExe);
                await ModManager.instance.DownloadFile(Urls.LauncherSignature, updateSignature);
                if (IsSignatureValid())
                {
                    ReplaceSelf(updateExe);
                    MainWindow.instance.CloseLauncher(false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to update: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void ReplaceSelf(string newExePath)
        {
            string currentExePath = Process.GetCurrentProcess().MainModule.FileName;
            string tempScript = Path.GetTempFileName() + ".ps1";

            string scriptContent = $@"
Start-Sleep -Seconds 1
Remove-Item '{currentExePath}' -Force
Move-Item '{newExePath}' '{currentExePath}' -Force
Start-Process '{currentExePath}'
";

            File.WriteAllText(tempScript, scriptContent);

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{tempScript}\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true
            };

            Process.Start(psi);
        }
        private static bool IsSignatureValid()
        {
            if (!File.Exists(updateExe) || !File.Exists(updateSignature))
            {
                return false;
            }
            byte[] file = File.ReadAllBytes(updateExe);
            byte[] signature = File.ReadAllBytes(updateSignature);
            using var rsa = RSA.Create();
            rsa.ImportFromPem(public_key);
            return rsa.VerifyData(file, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}