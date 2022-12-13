using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;

namespace Intercom
{
    public partial class FindPage : PhoneApplicationPage
    {
        public FindPage()
        {
            InitializeComponent();
        }

        // Error code constants
        const uint ERR_BLUETOOTH_OFF = 0x8007048F;      // The Bluetooth radio is off
        const uint ERR_MISSING_CAPS = 0x80070005;       // A capability is missing from your WMAppManifest.xml
        const uint ERR_NOT_ADVERTISING = 0x8000000E;    // You are currently not advertising your presence using PeerFinder.Start()

        ObservableCollection<PeerWrapper> peerInfo;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App thisApp = Application.Current as App;

            // Set up the PeerFinder
            PeerFinder.DisplayName = thisApp.ChatName;
            PeerFinder.ConnectionRequested += PeerFinder_ConnectionRequested;
            PeerFinder.Start();

            // Create a collection of peers that we have searched for
            peerInfo = new ObservableCollection<PeerWrapper>();

            // Bind the collection to a display element
            peersListBox.ItemsSource = peerInfo;
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

            PeerFinder.ConnectionRequested -= PeerFinder_ConnectionRequested;
            base.OnNavigatingFrom(e);
        }



        async void showPeers()
        {
            try
            {
                var peers = await PeerFinder.FindAllPeersAsync();

                // Clear the list of names
                peerInfo.Clear();

                if (peers.Count > 0)
                {
                    // Add peers to list
                    foreach (var peer in peers)
                    {
                        peerInfo.Add(new PeerWrapper(peer));
                    }
                }
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == ERR_BLUETOOTH_OFF)
                {
                    MessageBox.Show("Bluetooth turned off");
                    ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
                    connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Bluetooth;
                    connectionSettingsTask.Show();
                }
                else if ((uint)ex.HResult == ERR_MISSING_CAPS)
                {
                    MessageBox.Show("No Bluetooth capability");
                }
                else if ((uint)ex.HResult == ERR_NOT_ADVERTISING)
                {
                    MessageBox.Show("Not advertising host");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        void PeerFinder_ConnectionRequested(object sender, ConnectionRequestedEventArgs args)
        {
            try
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    // Ask the user if they want to accept the incoming request.
                    var result = MessageBox.Show(args.PeerInformation.DisplayName, "Do you want to chat?", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        startChat(args.PeerInformation);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            showPeers();
        }

        void startChat(PeerInformation peer)
        {
            // Store the peer details in the app
            App thisApp = Application.Current as App;

            thisApp.ActivePeer = peer;

            // Navigate to the chat page

            NavigationService.Navigate(new Uri("/TalkPage.xaml",
                                        UriKind.Relative));
        }

        private void peersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (peersListBox.SelectedItem != null)
            {
                // Get the selected peer
                PeerWrapper selectedPeerInfo = peersListBox.SelectedItem as PeerWrapper;

                startChat(selectedPeerInfo.PeerInfo);
            }
        }
    }

    /// <summary>
    ///  Class to wrap around the PeerInformation so I can use the displayname on the screen
    /// </summary>
    public class PeerWrapper
    {
        public PeerWrapper(PeerInformation peerInformation)
        {
            this.PeerInfo = peerInformation;
        }

        public PeerInformation PeerInfo { get; set; }

        public override string ToString()
        {
            return PeerInfo.DisplayName;
        }
    }

}