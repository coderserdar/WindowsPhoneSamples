using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PutMeOnTheMap.Resources;
using Windows.Devices.Geolocation;
using System.Device.Location;

namespace PutMeOnTheMap
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
                locator.MovementThreshold = 20;// distance in meters
                locator.PositionChanged += locator_PositionChanged;
                locator.StatusChanged += locator_StatusChanged;
            }

            base.OnNavigatedTo(e);
        }

        void locator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                updateStatus(args.Status);
            });
        }

        void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                updateDisplay(args.Position);
            });
        }

        private void updateStatus(PositionStatus status)
        {
        }

        private void updateDisplay(Geoposition position)
        {
            GeoCoordinate drawCoordinate = new GeoCoordinate(position.Coordinate.Latitude, position.Coordinate.Longitude);
            myLocationMap.Center = drawCoordinate;
            myLocationMap.ZoomLevel = 13;
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