using System.IO;
using System.Text.Json;

namespace HaloRuns_Workshop_Overlay.src
{
    // Implemented as a thread-safe singleton
    public class AppConfig
    {
        // NOTE: Instance creation is not thread safe, only do once at startup
        public static void CreateInstance(in string acJsonFile)
        {
            if(scInstance != null)
            {
                // This is a bug, throw exception
                throw new ApplicationException(
                    "Tried to create Configuration instance, but instance already created");
            }

            scInstance = new AppConfig(acJsonFile);
        }

        public static AppConfig GetInstance()
        {
            if (scInstance == null)
            {
                // This is a bug, throw exception
                throw new ApplicationException(
                    "Tried to get Configuration instance, but instance never created");
            }

            return scInstance;
        }

        public ConfigParams? ReadFromConfig()
        {
            lock(mcMutex)
            {
                ConfigParams? lcParams = null;
                try
                {
                    string lcFileText = File.ReadAllText(mcJsonFile);
                    lcParams = JsonSerializer.Deserialize<ConfigParams>(lcFileText);
                }
                catch (Exception)
                {
                    // do nothing, config is invalid, caller will deal from here
                }

                return lcParams;
            }
        }

        public void WriteToConfig(in ConfigParams acParams)
        {
            lock (mcMutex)
            {
                string lcJsonText = JsonSerializer.Serialize(acParams);
                File.WriteAllText(mcJsonFile, lcJsonText);
            }
        }

        public class ConfigParams
        {
            public string mcGameLocation { get; set; } = string.Empty;
            public string mcModLocation { get; set; } = string.Empty;
            public string mcAutoSearchLocation { get; set; } = string.Empty;
        }

        private AppConfig(in string acJsonFile)
        {
            mcJsonFile = acJsonFile;
        }

        // Static singleton instance
        private static AppConfig? scInstance = null;

        // Mutex for locking
        private Mutex mcMutex = new Mutex();

        private string mcJsonFile = string.Empty;

    }
}
