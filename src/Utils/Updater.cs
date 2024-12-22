using System.Security.Cryptography;

namespace Erdtree_Launcher{
    public static class Updater{
        private static readonly string public_key = "";
        private static readonly string updateLocation = Path.Combine(Path.GetTempPath(), "Erdtree_Launcher_Update");
        private static readonly string updateExe = Path.Combine(updateLocation, "start_protected_game.exe");
        private static readonly string updateSignature = Path.Combine(updateLocation, "update.sig");
        public static bool IsUpdateAvailable(){
            //TODO - Implement this
            return false;
        }
        public static void DownloadAndInstallUpdate(){
            //TODO - Implement this
            
        }
        private static bool IsSignatureValid(){
            if(!File.Exists(updateExe) || !File.Exists(updateSignature)){
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