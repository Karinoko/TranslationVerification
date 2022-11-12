using Microsoft.Win32;
using System.IO;

namespace TranslationVerification
{
    internal class RegistryOperator
    {
        /// <summary>
        /// Ustawia ścieżki bazowe przy uruchomieniu programu jeśli te są NULL w rejestrze
        /// </summary>
        static public void SetRegistryKeyOnStart()
        {
            RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey("Weryfikator");
            if (registryKey == null)
            {
                registryKey = Registry.CurrentUser.CreateSubKey("Weryfikator");
                registryKey.SetValue("sourcePath", GlobalData.sourcePath);
                registryKey.SetValue("translPath", GlobalData.translPath);
            }
            registryKey.Close();
        }

        static public void CheckPath()
        {
            CheckPath(GlobalData.sourcePath, "sourcePath", out GlobalData.sourcePath);
            CheckPath(GlobalData.translPath, "translPath", out GlobalData.translPath);
        }

        static private void CheckPath(string pathIn, string source, out string pathOut)
        {
            pathOut = string.Empty;
            if (!Directory.Exists(pathIn))
            {
                RegistryKey? registryKey = Registry.CurrentUser.OpenSubKey("Weryfikator", RegistryKeyPermissionCheck.ReadWriteSubTree);
                pathOut = registryKey.GetValue(source).ToString();
                if (!Directory.Exists(pathOut))
                {
                    DirectoryOperator.FindSourceDirectory();
                    registryKey.SetValue(source, GlobalData.sourcePath);
                }
            }
            else
                pathOut = pathIn;
        }

    }
}
