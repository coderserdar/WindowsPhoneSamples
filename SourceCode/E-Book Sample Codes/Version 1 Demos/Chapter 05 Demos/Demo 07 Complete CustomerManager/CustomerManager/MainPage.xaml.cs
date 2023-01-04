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

using System.Collections.ObjectModel;

namespace CustomerManager
{
    public partial class MainPage : PhoneApplicationPage
    {
        ObservableCollection<Customer> observableCustomers;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the parent application that contains the customers list
            App thisApp = Application.Current as App;

            // Make an observable collection of these
            observableCustomers =
                new ObservableCollection<Customer>(thisApp.ActiveCustomerList.CustomerList);

            // Set the list to display the observable collection
            customerList.ItemsSource = observableCustomers;
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

            if (thisApp.ActiveCustomer != null)
            {
                // Find the customer in the list
                int pos = observableCustomers.IndexOf(
                                                thisApp.ActiveCustomer);
                // Remove it
                observableCustomers.RemoveAt(pos);
                // Put it back again - to force a change
                observableCustomers.Insert(pos, thisApp.ActiveCustomer);
            }

            // Clear the selected item 
            customerList.SelectedItem = null;
        }
    }
}