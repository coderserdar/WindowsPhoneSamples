using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereAmI.Resources;

using Windows.Devices.Geolocation;
using System.Threading.Tasks;

namespace WhereAmI
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator locator = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Create a new locator if we need one

            if (locator == null)
            {
                locator = new Geolocator();
                locator.DesiredAccuracy = PositionAccuracy.High;
            }

            base.OnNavigatedTo(e);
        }

        async private void findMeButton_Click(object sender, RoutedEventArgs e)
        {
            findMeButton.IsEnabled = false;
            TimeSpan acceptableAge = new TimeSpan(0, 0, 30);
            TimeSpan timeOut = new TimeSpan(0, 1, 0);
            try
            {
                Geoposition position = await locator.GetGeopositionAsync(acceptableAge, timeOut);

                timeTextBlock.Text = position.Coordinate.Timestamp.ToString();
                sourceTextBlock.Text = position.Coordinate.PositionSource.ToString();
                latTextBlock.Text = "Latitude: " + position.Coordinate.Latitude.ToString();
                longTextBlock.Text = "Longitude: " + position.Coordinate.Longitude.ToString();
            }
            catch (System.UnauthorizedAccessException)
            {
                timeTextBlock.Text = "not allowed";
                sourceTextBlock.Text = "";
                latTextBlock.Text = "";
                longTextBlock.Text = "";
            }
            catch (TaskCanceledException)
            {
                timeTextBlock.Text = "cancelled";
                sourceTextBlock.Text = "";
                latTextBlock.Text = "";
                longTextBlock.Text = "";
            }
            finally
            {
                findMeButton.IsEnabled = true;
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