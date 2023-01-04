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
            using (IsolatedStorageFile isf =
                          IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream =
                                 isf.CreateFile(filename))
                {
                    StreamWriter writer = new StreamWriter(rawStream);
                    writer.Write(text);
                    writer.Close();
                }
            }
        }

        private bool loadText(string filename, out string result)
        {
            result = "";
            using (IsolatedStorageFile isf =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream =
                            isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(rawStream);
                            result = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}