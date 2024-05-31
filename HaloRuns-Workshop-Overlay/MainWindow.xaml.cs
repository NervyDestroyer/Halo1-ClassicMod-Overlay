using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using HaloRuns_Workshop_Overlay.src;

namespace HaloRuns_Workshop_Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Create config instance and set up defaults if no config file
            InitializeConfig();

            // Show the main window
            InitializeComponent();

            // Initialize text boxes
            InitializeTextBoxes();
        }
            
        private void InitializeConfig()
        {
            // Create config instance and load config if possible
            AppConfig.CreateInstance(scJsonConfig);

            // Set up defaults if we couldn't load config file
            AppConfig.ConfigParams? lsParams = AppConfig.GetInstance().ReadFromConfig();
            if (lsParams == null)
            {
                lsParams = new AppConfig.ConfigParams();
                lsParams.mcAutoSearchLocation = scDefaultSearchLocation;

                // See if we can find the game and/or mod. We will ignore any errors since this is just startup
                string lcGameLoc;
                string lcModLoc;
                string lcErrStr;
                SteamSearch.FindGameLocation(lsParams.mcAutoSearchLocation, out lcGameLoc, out lcErrStr);
                SteamSearch.FindModLocation(lsParams.mcAutoSearchLocation, out lcModLoc, out lcErrStr);

                // If error, the out strings were set to empty which suffices here
                lsParams.mcGameLocation = lcGameLoc;
                lsParams.mcModLocation = lcModLoc;

                // Write out config
                AppConfig.GetInstance().WriteToConfig(lsParams);
            }
        }

        private void InitializeTextBoxes()
        {
            TextBox? lcGameLoc = this.FindName("TextBox_GameLocation") as TextBox;
            if(lcGameLoc == null)
            {
                throw new ApplicationException("INTERNAL ERROR: Could not find GameLocation TextBox");
            }

            TextBox? lcModLoc = this.FindName("TextBox_WorkshopLocation") as TextBox;
            if (lcModLoc == null)
            {
                throw new ApplicationException("INTERNAL ERROR: Could not find ModLocation TextBox");
            }

            AppConfig.ConfigParams? lsParams = AppConfig.GetInstance().ReadFromConfig();
            if (lsParams == null)
            {
                throw new ApplicationException("INTERNAL ERROR: Could not open configuration despite it being successfully created");
            }

            mcGameLocBox = lcGameLoc;
            mcModLocBox = lcModLoc;

            mcGameLocBox.Text = lsParams.mcGameLocation;
            mcModLocBox.Text = lsParams.mcModLocation;
        }

        private void OnClick_ChangeGameLocation(object sender, EventArgs e)
        {
            // Open folder dialogue
            string? lcNewFolder = FileDialogue.OpenFolder(
                mcGameLocBox.Text.Replace("/", "\\"), 
                "Select Halo 1 maps folder");

            if(lcNewFolder != null)
            {
                mcGameLocBox.Text = lcNewFolder;
            }
        }

        private void OnClick_ChangeModLocation(object sender, EventArgs e)
        {
            // Open folder dialogue
            string? lcNewFolder = FileDialogue.OpenFolder(
                mcModLocBox.Text.Replace("/", "\\"),
                "Select Classic Mod maps folder");

            if (lcNewFolder != null)
            {
                mcModLocBox.Text = lcNewFolder;
            }
        }

        private void OnClick_OverlayClassic(object sender, EventArgs e)
        {
            Overlay.OverlayClassicMod(
                mcGameLocBox.Text.Replace("/", "\\"),
                mcModLocBox.Text.Replace("/", "\\"));
        }

        private void OnClick_MccRestore(object sender, EventArgs e)
        {
            Overlay.RestoreMcc(
                mcGameLocBox.Text.Replace("/", "\\"));
        }

        private void OnClick_AutoFind(object sender, EventArgs e)
        {
            // Get params
            AppConfig.ConfigParams? lsParams = AppConfig.GetInstance().ReadFromConfig();
            if(lsParams == null)
            {
                lsParams = new AppConfig.ConfigParams();
                lsParams.mcAutoSearchLocation = scDefaultSearchLocation;
            }

            string? lcSearchLoc = FileDialogue.OpenFolder(lsParams.mcAutoSearchLocation, "Open steamapps folder");
            if(lcSearchLoc == null)
            {
                return;
            }

            string lcGameLoc;
            string lcModLoc;
            string lcErrStr;

            bool lbStatus = SteamSearch.FindGameLocation(lcSearchLoc, out lcGameLoc, out lcErrStr);
            if(!lbStatus)
            {
                MessageBox.Show($"Error finding H1 Maps:\n{lcErrStr}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            mcGameLocBox.Text = lcGameLoc;

            lbStatus = SteamSearch.FindModLocation(lcSearchLoc, out lcModLoc, out lcErrStr);
            if (!lbStatus)
            {
                MessageBox.Show($"Error finding Classic Workshop installation:\n{lcErrStr}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            mcModLocBox.Text = lcModLoc;

            // If here write out new values
            lsParams.mcAutoSearchLocation = lcSearchLoc;
            lsParams.mcGameLocation = lcGameLoc;
            lsParams.mcModLocation = lcModLoc;

            AppConfig.GetInstance().WriteToConfig(lsParams);

            MessageBox.Show($"Successfully found MCC and Mod Installation!", "Success!", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void OnClick_OpenWorkshop(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(scWorkshopLink) { UseShellExecute = true });
        }

        public static string scDefaultSearchLocation = "C:\\Program Files (x86)\\Steam\\steamapps";
        public static string scWorkshopLink = "https://steamcommunity.com/sharedfiles/filedetails/?id=3249878452";

        private TextBox mcGameLocBox;
        private TextBox mcModLocBox;
        private static string scJsonConfig = "./Config.json";
    }
}