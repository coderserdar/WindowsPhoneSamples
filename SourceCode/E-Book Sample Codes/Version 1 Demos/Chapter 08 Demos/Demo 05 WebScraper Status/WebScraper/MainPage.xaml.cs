using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WebScraper.Resources;

namespace WebScraper
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();

            client.DownloadStringCompleted += client_DownloadStringCompleted;
            prog.IsVisible = true;
            client.DownloadStringAsync(new Uri(siteTextBox.Text));
        }

        ProgressIndicator prog;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = false;
            prog.Text = "Fetching web page";
            SystemTray.SetProgressIndicator(this, prog);
            base.OnNavigatedTo(e);
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            receivedTextBlock.Text = e.Result;
            prog.IsVisible = false;
        }
    }
}