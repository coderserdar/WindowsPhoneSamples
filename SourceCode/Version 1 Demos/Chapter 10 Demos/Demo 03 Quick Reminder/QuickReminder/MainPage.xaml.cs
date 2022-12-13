using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using QuickReminder.Resources;
using Windows.Phone.Speech.VoiceCommands;

namespace QuickReminder
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

        async void setupVoiceCommands()
        {
            await VoiceCommandService.InstallCommandSetsFromFileAsync(
                new Uri("ms-appx:///VoiceCommandDefinition.xml", UriKind.RelativeOrAbsolute));
        }

        #region Number decoding

        struct numberDecode
        {
            public string Text;
            public int Value;
            public numberDecode(string inText, int inValue)
            {
                Text = inText;
                Value = inValue;
            }
        }


        numberDecode[] numbers = new[] { 
          new numberDecode("one",1),
          new numberDecode("five",5),
          new numberDecode("ten",10),
          new numberDecode("twenty",20)
        };

        int lookUpNumber(string number)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i].Text == number)
                    return numbers[i].Value;
            }
            throw new Exception("Voice number not found");
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            setupVoiceCommands();

            // Is this a new activation or a resurrection from tombstone?
            if (e.NavigationMode == System.Windows.Navigation.NavigationMode.New)
            {
                // Was the application launched using a Voice Command?
                if (NavigationContext.QueryString.ContainsKey("voiceCommandName"))
                {
                    // If so, get the name of the Voice Command.
                    string voiceCommandName = NavigationContext.QueryString["voiceCommandName"];

                    switch (voiceCommandName)
                    {
                        case "SetReminder":
                            string delayNumberString = NavigationContext.QueryString["number"];
                            int delay = lookUpNumber(delayNumberString);
                            ReminderManager.SetReminder(delay, "Quick Reminder");
                            break;
                    }
                }
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