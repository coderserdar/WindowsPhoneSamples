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
using System.Threading;


namespace Intercom
{
    public partial class TalkPage : PhoneApplicationPage
    {
        public TalkPage()
        {
            InitializeComponent();
        }

        // Network connection items
        StreamSocket socket;
        DataReader dataReader;
        DataWriter dataWriter;

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Get the peer
            App thisApp = Application.Current as App;

            try
            {
                socket = await PeerFinder.ConnectAsync(thisApp.ActivePeer);

                // We can preserve battery by not advertising our presence.
                PeerFinder.Stop();

                peerNameTextBox.Text = thisApp.ActivePeer.DisplayName;

                // Start a thread running to process incoming messages

                Thread readThread = new Thread(new ThreadStart(processIncomingMessages));

                readThread.Start();
            }
            catch (Exception ex)
            {
                // Show the message and return to the search page
                MessageBox.Show(ex.Message);
                closeConnections();
                returnToSearch();
            }
        }

        void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (sendTextBox.Text == "")
                return;

            sendMessage(sendTextBox.Text);
        }

        async void sendMessage(string message)
        {
            if (dataWriter == null)
                dataWriter = new DataWriter(socket.OutputStream);

            // Each message is sent in two blocks.
            // The first is the size of the message.
            // The second if the message itself.
            dataWriter.WriteInt32(message.Length);
            await dataWriter.StoreAsync();

            dataWriter.WriteString(message);
            await dataWriter.StoreAsync();

            displaySentText(message);

            sendTextBox.Text = "";

            if (message == "")
            {
                closeConnections();
            }
        }

        async void processIncomingMessages()
        {
            bool finished = false;

            while (!finished)
            {
                try
                {
                    var message = await getMessage();
                    if (message == "")
                        finished = true;
                    // Add to chat
                    displayReceivedText(message);
                }
                catch (Exception)
                {
                    finished = true;
                }
            }
            closeConnections();
            returnToSearch();
        }

        async Task<string> getMessage()
        {
            if (dataReader == null)
                dataReader = new DataReader(socket.InputStream);

            // Each message is sent in two blocks.
            // The first is the size of the message.
            // The second if the message itself.
            //var len = await GetMessageSize();
            await dataReader.LoadAsync(4);
            uint messageLen = (uint)dataReader.ReadInt32();
            await dataReader.LoadAsync(messageLen);
            return dataReader.ReadString(messageLen);
        }

        void displaySentText(string message)
        {
            // Get the peer
            App thisApp = Application.Current as App;

            displayMessage(thisApp.ChatName + " : " + message);
        }

        void displayReceivedText(string message)
        {
            // Get the peer
            App thisApp = Application.Current as App;

            displayMessage(thisApp.ActivePeer.DisplayName + " : " + message);
        }

        void displayMessage(string message)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                chatTextBlock.Text = chatTextBlock.Text + "\r\n" + message;
            });
        }

        void returnToSearch()
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/SearchPage.xaml",
                                    UriKind.Relative));
            });
        }

        void closeConnections()
        {
            if (dataReader != null)
            {
                dataReader.Dispose();
                dataReader = null;
            }

            if (dataWriter != null)
            {
                dataWriter.Dispose();
                dataWriter = null;
            }

            if (socket != null)
            {
                socket.Dispose();
                socket = null;
            }
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Send an empty string to shut down the other end
            sendMessage("");
        }
    }
}