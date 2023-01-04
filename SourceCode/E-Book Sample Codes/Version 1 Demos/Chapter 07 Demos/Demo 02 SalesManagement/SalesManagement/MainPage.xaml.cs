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

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the customer list page
            NavigationService.Navigate(new Uri("/CustomerListPage.xaml",
                               UriKind.RelativeOrAbsolute));
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the detail page
            NavigationService.Navigate(new Uri("/ProductListPage.xaml",
                               UriKind.RelativeOrAbsolute));
        }
    }
}