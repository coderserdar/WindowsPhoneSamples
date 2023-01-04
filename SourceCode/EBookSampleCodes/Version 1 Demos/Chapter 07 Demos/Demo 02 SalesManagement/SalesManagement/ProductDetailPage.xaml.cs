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
    public partial class ProductDetailPage : PhoneApplicationPage
    {
        public ProductDetailPage()
        {
            InitializeComponent();
        }

        // Viewmodel for the details page
        ProductView view = new ProductView();

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Get the parent application that contains the active custom
            App thisApp = Application.Current as App;

            // Load the active customer into the viewmodel
            view.Load(thisApp.ActiveProduct);

            // Set the data context to the viewmodel
            productDisplayGrid.DataContext = view;
        }



        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the parent application that contains the active custom
            App thisApp = Application.Current as App;

            // Copy the data from the viewmodel into the active customer
            view.Save(thisApp.ActiveProduct);

            // Go back to the previous page
            NavigationService.GoBack();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}