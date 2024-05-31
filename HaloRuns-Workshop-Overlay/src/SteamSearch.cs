using System.IO;
namespace HaloRuns_Workshop_Overlay.src
{
    public static class SteamSearch
    {
        public static bool FindGameLocation(
            in string arcPathToSearch,
            out string arcGameLocation,
            out string arcErrStr)
        {
            arcGameLocation = string.Empty;
            arcErrStr = string.Empty;

            string lcMccLoc = $"{arcPathToSearch}/common/{srcGameName}";
            if(!Directory.Exists(lcMccLoc))
            {
                arcErrStr = $"MCC not found at {lcMccLoc}";
                return false;
            }

            string lcGameLoc = $"{lcMccLoc}/halo1/maps";
            if (!Directory.Exists(lcGameLoc))
            {
                arcErrStr = $"Halo 1 installation map files not found at {lcGameLoc}. Is Halo 1 installed?";
                return false;
            }

            // If here we are good
            arcGameLocation = lcGameLoc.Replace("\\","/");
            return true;
        }

        public static bool FindModLocation(
            in string arcPathToSearch,
            out string arcMod,
            out string arcErrStr)
        {
            arcMod = string.Empty;
            arcErrStr = string.Empty;

            string lcModLoc = $"{arcPathToSearch}/workshop/content/{srcGameId}/{srcModId}/maps";
            if (!Directory.Exists(lcModLoc))
            {
                arcErrStr = $"Workshop mod not found at {lcModLoc}. Make sure the Classic Mod is installed on the Steam Workshop page.";
                return false;
            }

            // If here we are good
            arcMod = lcModLoc.Replace("\\", "/");
            return true;
        }

        private static string srcGameName = "Halo The Master Chief Collection";
        private static string srcGameId = "976730";
        private static string srcModId = "3249878452";
    }
}
