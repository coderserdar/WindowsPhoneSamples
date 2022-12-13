﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DatagramListen.Resources;

using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.System.Threading;
using Windows.Storage.Streams;
using Windows.Networking.Connectivity;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DatagramListen
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Network Details Display

        string WiFiAddress = "";

        /* Network Interface types 
         * 
         *         1 Some other type of network interface.
         *         6 An Ethernet network interface. (emulator)
         *         9 A token ring network interface.
         *         23 A PPP network interface.
         *         24 A software loopback network interface.
         *         37 An ATM network interface.
         *         71 An IEEE 802.11 wireless network interface.
         *         131 A tunnel type encapsulation network interface.
         *         144 An IEEE 1394 (Firewire) high performance serial bus network interface.
         *         244 3G cellular
         *         
         *         http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib
         *         
        */


        Dictionary<uint, string> networkDecode = null;

        string decodeNetworkNumber(uint number)
        {
            if (networkDecode == null)
            {
                networkDecode = new Dictionary<uint, string>();
                networkDecode.Add(1, "Network");
                networkDecode.Add(6, "A Ethernet(emulator)");
                networkDecode.Add(9, "Token ring");
                networkDecode.Add(23, "PPP");
                networkDecode.Add(24, "Lopback");
                networkDecode.Add(37, "ATM");
                networkDecode.Add(71, "Wifi");
                networkDecode.Add(131, "Tunnel");
                networkDecode.Add(144, "Firewire");
                networkDecode.Add(244, "3G cellular");
            }
            if (networkDecode.ContainsKey(number))
            {
                return networkDecode[number];
            }
            else
            {
                return "unknown";
            }
        }

        #endregion

        ObservableCollection<string> IPAddressList = new ObservableCollection<string>();

        void showNetworks()
        {
            IPAddressList.Clear();
            IPAddresesListbox.ItemsSource = IPAddressList;

            var ipAddress = NetworkInformation.GetHostNames();

            foreach (var name in ipAddress)
            {
                uint itype = name.IPInformation.NetworkAdapter.IanaInterfaceType;
                string text = name.RawName + " " + itype + " " +
                    decodeNetworkNumber(itype);
                IPAddressList.Add(text);
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            showNetworks();
        }

        DatagramSocket receiveSocket = new DatagramSocket();

        async private void ListenButton_Click(object sender, RoutedEventArgs e)
        {
            receiveSocket.MessageReceived += receiveSocket_MessageReceived;
            await receiveSocket.BindEndpointAsync(new HostName(HostTextBox.Text), PortTextBox.Text);

        }

        async void receiveSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            var dataReader = new DataReader(args.GetDataStream());

            uint numBytesRead;

            while (true)
            {
                numBytesRead = await dataReader.LoadAsync(sizeof(uint));

                if (numBytesRead != sizeof(uint))
                    return;

                uint numCharactersInMessage = dataReader.ReadUInt32();

                numBytesRead = await dataReader.LoadAsync(numCharactersInMessage);

                if (numBytesRead == 0)
                    return;

                string result = dataReader.ReadString(numBytesRead);

                DisplayReceivedMessage(result);

            }
        }

        private void DisplayReceivedMessage(string p)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                ResponseTextBox.Text = p;
            });
        }
    }
}