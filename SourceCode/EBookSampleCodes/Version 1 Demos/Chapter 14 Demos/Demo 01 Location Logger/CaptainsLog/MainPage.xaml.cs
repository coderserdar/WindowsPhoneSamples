#define DEBUG_AGENT


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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using Microsoft.Phone.Scheduler;

using LocationTaskAgent;

namespace CaptainsLog
{

    /// <summary>
    ///  Simple log file application to show how to persist data and
    ///  manage tombstoning
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        #region Background location tracking 

        PeriodicTask periodicTask = null;
        string periodicTaskName = "CaptainTracker";

        private void RemoveAgent(string name)
        {
            try
            {
                ScheduledActionService.Remove(name);
            }
            catch (Exception)
            {
            }
        }

        private void StopTracking()
        {
            RemoveAgent(periodicTaskName);
        }

private void StartTracking()
{
    periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

    // If the task already exists and the IsEnabled property is false, background
    // agents have been disabled by the user
    if (periodicTask != null && !periodicTask.IsEnabled)
    {
        MessageBox.Show("Background agents for this application have been disabled by the user.");
        return;
    }

    // If the task already exists and background agents are enabled for the
    // application, you must remove the task and then add it again to update 
    // the schedule
    if (periodicTask != null && periodicTask.IsEnabled)
    {
        RemoveAgent(periodicTaskName);
    }

    periodicTask = new PeriodicTask(periodicTaskName);

    // The description is required for periodic agents. This is the string that the user
    // will see in the background services Settings page on the device.
    periodicTask.Description = "Log Tracker";
    ScheduledActionService.Add(periodicTask);

    // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
    ScheduledActionService.LaunchForTest(periodicTaskName, TimeSpan.FromSeconds(60));
#endif
}

        #endregion

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // When we navigate to the page - put the semi-completed log text in the display

            // Get a reference to the parent application
            App thisApp = App.Current as App;

            // Put the text onto the display
            logTextBox.Text = thisApp.LogEntry;

            // Now set up the text in the background text button

            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            if (periodicTask == null)
            {
                trackingControlButton.Content = "Start Tracking";
            }
            else
            {
                trackingControlButton.Content = "Stop Tracking";
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // When we leave the page - store the semi-completed log text in the application

            // Get a reference to the parent application
            App thisApp = App.Current as App;

            // Store the text in the application
            thisApp.LogEntry = logTextBox.Text;
        }

        private void storeButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime timeStamp = DateTime.Now;
            string timeStampString = timeStamp.ToShortDateString() + " " + timeStamp.ToShortTimeString() + System.Environment.NewLine;

            // Get a reference to the parent application - which holds the log string
            App thisApp = App.Current as App; 
            
            // Add the new text onto the end
            thisApp.LogText = thisApp.LogText + timeStampString + logTextBox.Text + System.Environment.NewLine;

            // Clear the log for next time
            logTextBox.Text = "";
        }

        private void viewButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to the viewing page
            NavigationService.Navigate(new Uri("/LogPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void trackingControlButton_Click(object sender, RoutedEventArgs e)
        {
            periodicTask = ScheduledActionService.Find(periodicTaskName) as PeriodicTask;

            if (periodicTask == null)
            {
                StartTracking();
                trackingControlButton.Content = "Stop Tracking";
            }
            else
            {
                StopTracking();
                trackingControlButton.Content = "Start Tracking";
            }
        }
    }
}