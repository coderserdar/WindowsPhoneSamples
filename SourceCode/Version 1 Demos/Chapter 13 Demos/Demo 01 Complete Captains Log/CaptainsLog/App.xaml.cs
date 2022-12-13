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

using System.IO;
using System.IO.IsolatedStorage;


namespace CaptainsLog
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

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
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        #region Application data management


        #region Program Data

        // This is shared amongst all the pages
        // It is the contents of the log itself, as a large string
        public string LogText;

        // This is also shared, only used by the log entry page
        public string LogEntry = "";

        private void SaveToIsolatedStorage()
        {
            saveTextToIsolatedStorage("Log", LogText);
            saveTextToIsolatedStorage("Entry", LogEntry);
        }

        private void LoadFromIsolatedStorage()
        {
            string temp;
            if (loadText("Log", out temp))
            {
                LogText = temp;
            }

            if (loadText("Entry", out temp))
            {
                LogEntry = temp;
            }
        }


        private void SaveToStateObject()
        {
            SaveTextToStateObject("Log", LogText);
            SaveTextToStateObject("Entry", LogEntry);
        }

        private void LoadFromStateObject()
        {
            string temp;
            if (LoadTextFromStateObject("Log", out temp))
            {
                LogText = temp;
            }

            if (LoadTextFromStateObject("Entry", out temp))
            {
                LogEntry = temp;
            }
        }

        #endregion

        #region Isolated Storage - using the settings store

        private void saveTextToIsolatedStorage(string filename, string text)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            isolatedStore.Remove(filename);
            isolatedStore.Add(filename, text);
            isolatedStore.Save();
        }

        private bool loadText(string filename, out string result)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;

            result = "";
            try
            {
                result = (string)isolatedStore[filename];
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region State Object - stored in memory

        private void SaveTextToStateObject(string filename, string text)
        {
            IDictionary<string, object> stateStore = PhoneApplicationService.Current.State;

            stateStore.Remove(filename);

            stateStore.Add(filename, text);
        }

        private bool LoadTextFromStateObject(string filename, out string result)
        {
            IDictionary<string, object> stateStore = PhoneApplicationService.Current.State;

            result = "";

            if (!stateStore.ContainsKey(filename)) return false;

            result = (string)stateStore[filename];

            return true;
        }

        #endregion

        #endregion

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            // Note: This is not advisable. For larger amounts of data
            // the application should either start up a thread to perform the load
            // or set a flag to trigger the load later on
            // The Launching method should complete as quickly as possible
            LoadFromIsolatedStorage();
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                // We are coming back from a tombstone - 
                // need to restore from the state object
                LoadFromStateObject();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            SaveToIsolatedStorage();
            SaveToStateObject();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // No need to store to the state object - we are not coming back
            SaveToIsolatedStorage();
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
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
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
}