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
            string name, address;
            if (NavigationContext.QueryString.TryGetValue("name", out name))
                nameTextBlock.Text = name;
            if (NavigationContext.QueryString.TryGetValue("address", out address))
                addressTextBlock.Text = address;
        }
    }
}