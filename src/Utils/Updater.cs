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
        }
        public static async Task DownloadAndInstallUpdate()
        {
            try
            {
                if(!Directory.Exists(updateLocation)){
                    Directory.CreateDirectory(updateLocation);
                }
                await ModManager.instance.DownloadFile(Urls.LauncherDownload, updateExe);
                //await ModManager.instance.DownloadFile(Urls.LauncherSignature, updateSignature);
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
            var mainModule = Process.GetCurrentProcess().MainModule;
            string currentExePath;
            if (mainModule == null)
            {
                currentExePath = Utils.GetFullPath(Filenames.LauncherExe);
            }
            else
            {
                currentExePath = mainModule.FileName;
            }
            //TODO - Remove
            currentExePath = Utils.GetFullPath(Filenames.LauncherExe);
            MessageBox.Show(currentExePath);
            //
            string tempScript = Path.GetTempFileName() + ".ps1";

            string scriptContent = $@"
Start-Sleep -Seconds 3
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
                CreateNoWindow = false
            };

            Process.Start(psi);
        }
        private static bool IsSignatureValid()
        {
            return true;
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