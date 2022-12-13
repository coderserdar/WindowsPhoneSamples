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


        private SolidColorBrush errorBrush = new SolidColorBrush(Colors.Red);
        private Brush correctBrush = null;
      
        private void calculateResult()
        {

            bool errorFound = false;

            if (correctBrush == null)
            {
                correctBrush = firstNumberTextBox.Foreground;
            }

            float v1 = 0;

            if (!float.TryParse(firstNumberTextBox.Text, out v1))
            {
                firstNumberTextBox.Foreground = errorBrush;
                errorFound = true;
            }
            else
            {
                firstNumberTextBox.Foreground = correctBrush;
            }

            float v2 = 0;

            if (!float.TryParse(secondNumberTextBox.Text, out v2))
            {
                secondNumberTextBox.Foreground = errorBrush;
                errorFound = true;
            }
            else
            {
                secondNumberTextBox.Foreground = correctBrush;
            }


            if (errorFound)
            {
                resultTextBlock.Text = "Invalid inputs";
            }
            else
            {
                float result = v1 + v2;
                resultTextBlock.Text = result.ToString();
            }
        }

        private void equalsButton_Click(object sender, RoutedEventArgs e)
        {
            calculateResult();
        }
    }
}
