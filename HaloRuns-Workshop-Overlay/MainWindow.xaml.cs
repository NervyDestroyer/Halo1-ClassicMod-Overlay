using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                lsParams.mcAutoSearchLocation = srcDefaultSearchLocation;

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

        public static string srcDefaultSearchLocation = "C:\\Program Files (x86)\\Steam\\steamapps";

        private TextBox mcGameLocBox;
        private TextBox mcModLocBox;
        private static string scJsonConfig = "./Config.json";
    }
}