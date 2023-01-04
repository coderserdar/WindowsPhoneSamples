using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AskAboutCheese.Resources;
using Windows.Phone.Speech.Recognition;

namespace AskAboutCheese
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

        SpeechRecognizerUI recoWithUI;

        private async void askButton_Click(object sender, RoutedEventArgs e)
        {
            recoWithUI = new SpeechRecognizerUI();

            // Build a string array, create a grammar from it, and add it to the speech recognizer's grammar set.
            string[] strengthNames = { "weak", "mild", "medium", "strong", "english" };

            recoWithUI.Recognizer.Grammars.AddGrammarFromList("cheeseStrength", strengthNames);

            recoWithUI.Settings.ListenText = "How strong do you like your cheese?";

            recoWithUI.Recognizer.Grammars["cheeseStrength"].Enabled = true;

            try
            {
                SpeechRecognitionUIResult recoResult = await recoWithUI.RecognizeWithUIAsync();
                if (recoResult.RecognitionResult.TextConfidence == SpeechRecognitionConfidence.High)
                {
                    MessageBox.Show("Cheese: " + recoResult.RecognitionResult.Text);
                }
                else
                {
                    MessageBox.Show("Sorry, didn't get that");
                }
            }
            catch
            {
                return;
            }

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