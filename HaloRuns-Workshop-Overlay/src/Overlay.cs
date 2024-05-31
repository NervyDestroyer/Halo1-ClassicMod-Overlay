using System.IO;
using System.Windows;

namespace HaloRuns_Workshop_Overlay.src
{
    public static class Overlay
    {
        public static void RestoreMcc(in string acMccDir)
        {
            lock(scMutex)
            {
                // Check if MCC directory is valid
                if (!Directory.Exists(acMccDir) || !H1Maps.VerifyMapsExist(acMccDir))
                {
                    MessageBox.Show(
                        $"Specified Game folder either does not exist or does not contain Halo 1 map files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // This requires a verify game files
                string lcBackupDir = Path.Combine(acMccDir, scBackupFolder);
                if (!Directory.Exists(lcBackupDir) || !H1Maps.VerifyMapsExist(lcBackupDir))
                {
                    MessageBox.Show(
                        $"You either never attempted to Overlay Classic Mod files yet or the MCC backup folder is corrupted.\n" +
                        "If backup is corrupted, verify game files via Steam.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                H1Maps.CopyMaps(lcBackupDir, acMccDir);

                MessageBox.Show($"MCC restored successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.None);

            } // End lock
        }

        public static void OverlayClassicMod(in string acMccDir, in string acModDir)
        {
            lock (scMutex)
            {
                // Check if MCC directory is valid
                if (!Directory.Exists(acMccDir) || !H1Maps.VerifyMapsExist(acMccDir))
                {
                    MessageBox.Show(
                        $"Specified Game folder either does not exist or does not contain Halo 1 map files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Check if mod directory is valid
                if (!Directory.Exists(acModDir) || !H1Maps.VerifyMapsExist(acModDir))
                {
                    MessageBox.Show(
                        $"Specified Mod folder either does not exist or does not contain Classic map files", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Back up maps if not done yet
                BackupMccMapsIfNotDone(acMccDir);

                // Overlay classic mod
                H1Maps.CopyMaps(acModDir, acMccDir);

                MessageBox.Show($"Mod overlayed successfully!", "Success!", MessageBoxButton.OK, MessageBoxImage.None);

            } // end lock
        }

        private static void BackupMccMapsIfNotDone(in string acMccDir)
        {
            // If a backup does not exist, create one
            string lcBackupDir = Path.Combine(acMccDir, scBackupFolder);
            if(!Directory.Exists(lcBackupDir) || !H1Maps.VerifyMapsExist(lcBackupDir))
            {
                Directory.CreateDirectory(lcBackupDir);
                H1Maps.CopyMaps(acMccDir, lcBackupDir);
            }
        }

        private static Mutex scMutex = new Mutex();
        private static string scBackupFolder = "OverlayMccBackup";
    }
}
