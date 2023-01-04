using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RSSReader.Resources;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RSSReader
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
            prog.IsVisible = true;

            WebClient client = new WebClient();

            client.DownloadStringCompleted +=
                                  client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri(siteTextBox.Text));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
                return;
            decodeRSS(e.Result);
        }

        void decodeRSS(string rssText)
        {
            XElement rssElements = XElement.Parse(rssText);
            var postList =
                from item in rssElements.Elements("channel").Elements("item")
                select new RSSPost
                {
                    PostTitle = item.Element("title").Value,
                    DatePosted = item.Element("pubDate").Value,
                    PostLink = item.Element("link").Value
                };

            RSSListBox.ItemsSource = postList;
            prog.IsVisible = false;
        }

        ProgressIndicator prog;

        void makeSamplePosts()
        {
            RSSPost p1 = new RSSPost
            {
                PostTitle = "title1",
                DatePosted = "date1",
                PostLink = "link1"
            };
            RSSPost p2 = new RSSPost
            {
                PostTitle = "title2",
                DatePosted = "date2",
                PostLink = "link2"
            };
            RSSPost p3 = new RSSPost
            {
                PostTitle = "title3",
                DatePosted = "date3",
                PostLink = "link3"
            };

            List<RSSPost> posts = new List<RSSPost>();

            posts.Add(p1);
            posts.Add(p2);
            posts.Add(p3);

            RSSListBox.ItemsSource = posts;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            prog = new ProgressIndicator();
            prog.IsIndeterminate = true;
            prog.IsVisible = false;
            prog.Text = "Fetching rss";
            SystemTray.SetProgressIndicator(this, prog);

            makeSamplePosts();

            base.OnNavigatedTo(e);
        }
    }

    public class RSSPost
    {
        public string PostTitle { get; set; }
        public string DatePosted { get; set; }
        public string PostLink { get; set; }
    }
}