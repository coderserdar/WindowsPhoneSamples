using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AddingMachine.Resources;

using System.Windows.Media;

namespace AddingMachine
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

        private SolidColorBrush errorBrush = new SolidColorBrush(Colors.Red);
        private Brush correctBrush = null;

        private void calculateResult()
        {

            bool errorFound = false;

            if (correctBrush == null)
            {
                correctBrush = firstNumberTextBox.Foreground;
            }

            float v1 = 0;

            if (!float.TryParse(firstNumberTextBox.Text, out v1))
            {
                firstNumberTextBox.Foreground = errorBrush;
                errorFound = true;
            }
            else
            {
                firstNumberTextBox.Foreground = correctBrush;
            }

            float v2 = 0;

            if (!float.TryParse(secondNumberTextBox.Text, out v2))
            {
                secondNumberTextBox.Foreground = errorBrush;
                errorFound = true;
            }
            else
            {
                secondNumberTextBox.Foreground = correctBrush;
            }


            if (errorFound)
            {
                MessageBox.Show("Invalid Input" +
                    System.Environment.NewLine +
                    "Please re-enter");
                resultTextBlock.Text = "0";
            }
            else
            {
                float result = v1 + v2;
                resultTextBlock.Text = result.ToString();
            }
        }

        private void firstNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateResult();
        }

        private void secondNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculateResult();
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