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

namespace AddingMachine
{

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void equalsButton_Click(object sender, RoutedEventArgs e)
        {
            float v1 = 0;
            float v2 = 0;
            v1 = float.Parse(firstNumberTextBox.Text);
            v2 = float.Parse(secondNumberTextBox.Text);

            float result = v1 + v2;

            resultTextBlock.Text = result.ToString();
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            // Could put special orientation changing code here
        }

    }
}