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

namespace SalesManagement
{
    public partial class OrderListPage : PhoneApplicationPage
    {
        public OrderListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Get the parent application that contains the active custom
            App thisApp = Application.Current as App;

            orderList.ItemsSource = thisApp.ActiveCustomer.Orders;
        }


        private void orderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}