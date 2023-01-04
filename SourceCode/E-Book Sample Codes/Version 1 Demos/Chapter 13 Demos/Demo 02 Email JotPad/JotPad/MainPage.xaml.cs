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

using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace JotPad
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public void Save()
        {
            saveText("jot.txt", jotTextBox.Text);
        }

        public void Load()
        {
            string text;

            if (loadStateText("jot.txt", out text))
            {
                jotTextBox.Text = text;
                return;
            }

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

        public void SaveState()
        {
            SaveStateText("jot.txt", jotTextBox.Text);
        }

        private void SaveStateText (string filename, string text)
        {
            IDictionary<string, object> stateStore = PhoneApplicationService.Current.State;

            stateStore.Remove(filename);

            stateStore.Add(filename,text);
        }

        private bool loadStateText(string filename, out string result)
        {
            IDictionary<string, object> stateStore = PhoneApplicationService.Current.State;

            result = "";

            try
            {
                result = (string)stateStore[filename];
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void sendMail(string subject, string body)
        {
            Microsoft.Phone.Tasks.
            EmailComposeTask email = new EmailComposeTask();

            email.Body = body;
            email.Subject = subject;
            email.Show();
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
           Load();
        }

        private void mailButton_Click(object sender, RoutedEventArgs e)
        {
            sendMail("From JotPad", jotTextBox.Text);
        }
    }
}