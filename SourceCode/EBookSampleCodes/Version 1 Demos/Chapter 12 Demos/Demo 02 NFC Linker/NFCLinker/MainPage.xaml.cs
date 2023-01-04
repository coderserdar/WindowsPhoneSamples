using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NFCLinker.Resources;


using Windows.Networking.Proximity;
using Windows.Storage.Streams;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;

namespace NFCLinker
{
    public partial class MainPage : PhoneApplicationPage
    {
ProximityDevice device;

// Constructor
public MainPage()
{
    InitializeComponent();
    device = ProximityDevice.GetDefault();
}

        private void writeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (urlTextBox.Text.Length == 0)
                return;

            statusTextBlock.Text = "Tap the tag";

            IBuffer linkBuffer = Encoding.Unicode.GetBytes(urlTextBox.Text).AsBuffer();
            device.PublishBinaryMessage("WindowsURI:WriteTag", linkBuffer, LinkSavedCallback);
        }

        private void LinkSavedCallback(ProximityDevice sender, long messageId)
        {
            sender.StopPublishingMessage(messageId);

            Dispatcher.BeginInvoke(() =>
            {
                statusTextBlock.Text = "Tag written";
            });
        }
    }
}