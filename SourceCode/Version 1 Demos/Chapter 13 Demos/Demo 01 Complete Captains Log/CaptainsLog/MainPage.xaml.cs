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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // When we navigate to the page - put the semi-completed log text in the display

            // Get a reference to the parent application
            App thisApp = App.Current as App;

            // Put the text onto the display
            logTextBox.Text = thisApp.LogEntry;
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
    }
}