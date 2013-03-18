using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using itsbeta_wp7.ViewModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Input;
using BugSense;
using MSPToolkit.Utilities;
using Microsoft.Phone.Net.NetworkInformation;

namespace itsbeta_wp7
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string ACCESS_TOKEN = "059db4f010c5f40bf4a73a28222dd3e3";
        public const string FlurryKey = "8RFPJWYJKBDJXFJ6D9Q5";

        // Easy access to the root frame
        public PhoneApplicationFrame RootFrame
        {
            get;
            private set;
        }

        // Constructor
        public App()
        {
            BugSenseHandler.Instance.Init(this, "0656161d");
            // Global handler for uncaught exceptions. 
            //UnhandledException += Application_UnhandledException;
            BugSenseHandler.Instance.UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            Telerik.Windows.Controls.InputLocalizationManager.Instance.ResourceManager = AppResources.ResourceManager;
            PrimitivesLocalizationManager.Instance.ResourceManager = AppResources.ResourceManager;
            DataVisualizationLocalizationManager.Instance.ResourceManager = AppResources.ResourceManager;

            /*InputLocalizationManager.Instance.StringLoader = new MyStringLoader();
            InputLocalizationManager.Instance.ResourceManager = myResourceManager;
            LocalizationManager.GlobalStringLoader = new MyStringLoader();
            LocalizationManager.GlobalResourceManager = myResourceManager;*/

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disable user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            RootFrame.Navigating += new NavigatingCancelEventHandler(RootFrame_Navigating);
        }

        private bool firstCheck = true;
        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (firstCheck)
            {
                firstCheck = false;
                e.Cancel = true;
                string fb_id="";
                try
                {
                    Dictionary<string, object> result = IsolatedStorageHelper.LoadSerializableObject<Dictionary<string, object>>("user.xml");
                    fb_id = (string)result["fb_id"];
                }
                catch { };

                RootFrame.Dispatcher.BeginInvoke(delegate
                {
                    if (fb_id != "")
                    {
                        bool hasNetworkConnection =
NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None;
                        if (hasNetworkConnection)
                        {
                            ViewModelLocator.MainStatic.Loading = true;
                            ViewModelLocator.UserStatic.GetItsbetaAchieve();
                        };
                        RootFrame.Navigate(new Uri("/PanoramaPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        RootFrame.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                    };
                });
            }
            else
            {
                firstCheck = false;
            };
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            FlurryWP7SDK.Api.StartSession(FlurryKey);
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            FlurryWP7SDK.Api.StartSession(FlurryKey);
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.

            RadPhoneApplicationFrame frame = new RadPhoneApplicationFrame();
            RootFrame = frame;
            RootFrame.Navigated += CompleteInitializePhoneApplication;
            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;
            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            /*RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;*/
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }

    public class LocalizedStrings
    {
        public LocalizedStrings()
        {
        }

        private static itsbeta_wp7.AppResources localizedResources = new itsbeta_wp7.AppResources();

        public itsbeta_wp7.AppResources LocalizedResources { get { return localizedResources; } }
    }
}
