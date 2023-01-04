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

using System.IO;
using System.IO.IsolatedStorage;

namespace JotPad
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            saveText("jot.txt", jotTextBox.Text);

        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            string text;

            if (loadText("jot.txt", out text))
            {
                jotTextBox.Text = text;
            }
            else
            {
                jotTextBox.Text = "Type your jottings here....";
            }
        }

        private void saveText(string filename, string text)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            isolatedStore.Remove(filename);
            isolatedStore.Add(filename, text);
            isolatedStore.Save();
        }

        private bool loadText(string filename, out string result)
        {
             IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;

            result = "";
            try
            {
                result = (string)isolatedStore[filename];
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}