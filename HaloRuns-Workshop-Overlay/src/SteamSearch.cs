using System.IO;
namespace HaloRuns_Workshop_Overlay.src
{
    public static class SteamSearch
    {
        public static bool FindGameLocation(
            in string acPathToSearch,
            out string arcGameLocation,
            out string arcErrStr)
        {
            arcGameLocation = string.Empty;
            arcErrStr = string.Empty;

            string lcMccLoc = $"{acPathToSearch}/common/{scGameName}".Replace("/", "\\");
            if(!Directory.Exists(lcMccLoc))
            {
                arcErrStr = $"MCC not found";
                return false;
            }

            string lcGameLoc = $"{lcMccLoc}/halo1/maps".Replace("/","\\");
            if (!Directory.Exists(lcGameLoc))
            {
                arcErrStr = $"Halo 1 installation map folder not found. Is Halo 1 installed?";
                return false;
            }

            if(!H1Maps.VerifyMapsExist(lcGameLoc))
            {
                arcErrStr = $"Halo 1 installation map files not found. You may need to verify game files or re-install Halo 1.";
                return false;
            }

            // If here we are good
            arcGameLocation = lcGameLoc;
            return true;
        }

        public static bool FindModLocation(
            in string acPathToSearch,
            out string arcMod,
            out string arcErrStr)
        {
            arcMod = string.Empty;
            arcErrStr = string.Empty;

            string lcModLoc = $"{acPathToSearch}/workshop/content/{scGameId}/{scModId}/maps".Replace("/", "\\");
            if (!Directory.Exists(lcModLoc))
            {
                arcErrStr = $"Workshop Mod folder not found. Make sure the Classic Mod is installed on the Steam Workshop page.";
                return false;
            }

            if (!H1Maps.VerifyMapsExist(lcModLoc))
            {
                arcErrStr = $"Workshop Mod map files not found. You may need to reinstall the Classic Mod from the Steam Workshop.";
                return false;
            }

            // If here we are good
            arcMod = lcModLoc;
            return true;
        }

        private static string scGameName = "Halo The Master Chief Collection";
        private static string scGameId = "976730";
        private static string scModId = "3249878452";
    }
}
