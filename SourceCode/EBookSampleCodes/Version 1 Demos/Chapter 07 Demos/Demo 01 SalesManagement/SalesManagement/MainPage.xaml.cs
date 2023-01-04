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

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SalesManagement
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the parent application that contains the customers list
            App thisApp = Application.Current as App;

            // Set the list to display the customers

            var customers = from Customer customer 
                                in thisApp.ActiveDB.CustomerTable
                            // where customer.Name.StartsWith("S")
                            select customer;
            customerList.ItemsSource = customers;
        }

        private void customerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Set the customer being edited to the selected customer and then open it

            // If no customer is selected, just return
            if (customerList.SelectedItem == null) return;

            // Get the parent application that contains the customer being edited
            App thisApp = Application.Current as App;

            // Set this to the selected customer
            thisApp.ActiveCustomer = customerList.SelectedItem as Customer;

            // Navigate to the detail page
            NavigationService.Navigate(new Uri("/CustomerDetailPage.xaml",
                               UriKind.RelativeOrAbsolute));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Get the parent application that contains the customer list
            App thisApp = Application.Current as App;

            // Clear the selected item 
            customerList.SelectedItem = null;
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            App thisApp = Application.Current as App;

            thisApp.ActiveDB.SubmitChanges();
        }
    }
}