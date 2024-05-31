using System.IO;
using System.Text.Json;

namespace HaloRuns_Workshop_Overlay.src
{
    // Implemented as a thread-safe singleton
    public class AppConfig
    {
        // NOTE: Instance creation is not thread safe, only do once at startup
        public static void CreateInstance(in string arcJsonFile)
        {
            if(srcInstance != null)
            {
                // This is a bug, throw exception
                throw new ApplicationException(
                    "Tried to create Configuration instance, but instance already created");
            }

            srcInstance = new AppConfig(arcJsonFile);
        }

        public static AppConfig GetInstance()
        {
            if (srcInstance == null)
            {
                // This is a bug, throw exception
                throw new ApplicationException(
                    "Tried to get Configuration instance, but instance never created");
            }

            return srcInstance;
        }

        public ConfigParams? ReadFromConfig()
        {
            lock(mrcMutex)
            {
                ConfigParams? lcParams = null;
                try
                {
                    string lcFileText = File.ReadAllText(mrcJsonFile);
                    lcParams = JsonSerializer.Deserialize<ConfigParams>(lcFileText);
                }
                catch (Exception)
                {
                    // do nothing, config is invalid, caller will deal from here
                }

                return lcParams;
            }
        }

        public void WriteToConfig(in ConfigParams arcParams)
        {
            lock (mrcMutex)
            {
                string lcJsonText = JsonSerializer.Serialize(arcParams);
                File.WriteAllText(mrcJsonFile, lcJsonText);
            }
        }

        public class ConfigParams
        {
            public string mrcGameLocation { get; set; } = string.Empty;
            public string mrcModLocation { get; set; } = string.Empty;
            public string mrcAutoSearchLocation { get; set; } = string.Empty;
        }

        private AppConfig(in string arcJsonFile)
        {
            mrcJsonFile = arcJsonFile;
        }

        // Static singleton instance
        private static AppConfig? srcInstance = null;

        // Mutex for locking
        private Mutex mrcMutex = new Mutex();

        private string mrcJsonFile = string.Empty;

    }
}
