using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SimpleSpeaker.Resources;

using Windows.Phone.Speech.Synthesis;

namespace SimpleSpeaker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        bool beenInterrupted = false;

        async private void speaker(string message)
        {
            beenInterrupted = false;
            try
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();
                await synth.SpeakTextAsync(message);
            }
            catch (Exception ex)
            {
                if (((uint)ex.HResult == 0x80045508))
                {
                    // System Call Interrupted thrown by Speech     
                    beenInterrupted = true;
                }
            }
        }

        private void speakButton_Click(object sender, RoutedEventArgs e)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            statusTextBlock.Text = "Speaking";

            speaker(speechTextBox.Text);

            statusTextBlock.Text = "Finished";
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}