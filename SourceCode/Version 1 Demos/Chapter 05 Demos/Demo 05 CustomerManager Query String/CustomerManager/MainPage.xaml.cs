using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CustomerManager.Resources;

namespace CustomerManager
{
    public partial class MainPage : PhoneApplicationPage
    {

        Customers customers = null;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Create the customer data if required

            if (customers == null)
            {
                customers = Customers.MakeTestCustomers();
                customerList.ItemsSource = customers.CustomerList;
            }

            // Clear the selected customer
            customerList.SelectedItem = null;
        }

        private void customerList_SelectionChanged(object sender,
                                  SelectionChangedEventArgs e)
        {
            // Abandon if nothing selected
            if (customerList.SelectedItem == null) return;

            // Get the selected customer from the list
            Customer selectedCustomer = customerList.SelectedItem as Customer;

            // Build a navigation string with the customer information in it
            NavigationService.Navigate(new Uri("/CustomerDetailPage.xaml?" +
                                                "name=" + selectedCustomer.Name + "&" +
                                                "address=" + selectedCustomer.Address,
                                                UriKind.Relative));
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