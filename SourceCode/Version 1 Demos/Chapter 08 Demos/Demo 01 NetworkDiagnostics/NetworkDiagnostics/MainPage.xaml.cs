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

using Microsoft.Phone.Net.NetworkInformation;


namespace NetworkDiagnostics
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void updateButton()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("Operator: ");
            sb.AppendLine(DeviceNetworkInformation.CellularMobileOperator);

            sb.Append("Network available:  ");
            sb.AppendLine(DeviceNetworkInformation.IsNetworkAvailable.ToString());

            sb.Append("Cellular enabled:  ");
            sb.AppendLine(DeviceNetworkInformation.IsCellularDataEnabled.ToString());

            sb.Append("Roaming enabled:  ");
            sb.AppendLine(DeviceNetworkInformation.IsCellularDataRoamingEnabled.ToString());

            sb.Append("Wi-Fi enabled:  ");
            sb.AppendLine(DeviceNetworkInformation.IsWiFiEnabled.ToString());

            MessageBox.Show(sb.ToString());
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            updateButton();
        }
    }
}