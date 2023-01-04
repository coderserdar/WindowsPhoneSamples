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

namespace CustomerManager
{
    public partial class CustomerDetailPage : PhoneApplicationPage
    {
        public CustomerDetailPage()
        {
            InitializeComponent();
        }

        // Viewmodel for the details page
        CustomerView view = new CustomerView();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Get the parent application that contains the active custom
            App thisApp = Application.Current as App;

            // Load the active customer into the viewmodel
            view.Load(thisApp.ActiveCustomer);

            // Set the data context to the viewmodel
            customerDisplayGrid.DataContext = view;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the parent application that contains the active custom
            App thisApp = Application.Current as App;

            // Copy the data from the viewmodel into the active customer
            view.Save(thisApp.ActiveCustomer);

            // Go back to the previous page
            NavigationService.GoBack();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}