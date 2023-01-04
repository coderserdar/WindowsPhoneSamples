using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CustomerManager
{
    public partial class CustomerDetailPage : PhoneApplicationPage
    {
        public CustomerDetailPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you really want to exit the page?",
                                "Page Exit", MessageBoxButton.OKCancel)
               != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get the parent application that contains the active customer
            App thisApp = Application.Current as App;

            // Set the data context for the display grid to the active
            // customer
            customerDisplayGrid.DataContext = thisApp.ActiveCustomer;
        }
    }
}