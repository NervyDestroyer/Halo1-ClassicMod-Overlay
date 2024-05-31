using System.IO;

namespace HaloRuns_Workshop_Overlay.src
{
    public static class H1Maps
    {
        private static string[] sacH1Maps =
            {"a10.map", "a30.map", "a50.map", "b30.map", "b40.map", 
            "c10.map", "c20.map", "c40.map", "d20.map", "d40.map"};

        public static bool VerifyMapsExist(in string acDir)
        {
            bool lbFoundAllMaps = true;
            foreach(string lcH1Map in sacH1Maps)
            {
                if(!File.Exists(Path.Combine(acDir, lcH1Map)))
                {
                    lbFoundAllMaps = false;
                    break;
                }
            }
            return lbFoundAllMaps;
        }

        public static void CopyMaps(in string acSrc, in string acDest)
        {
            foreach (string lcH1Map in sacH1Maps)
            {
                File.Copy(
                    Path.Combine(acSrc, lcH1Map), 
                    Path.Combine(acDest, lcH1Map),
                    true);
            }
        }
    }
}
