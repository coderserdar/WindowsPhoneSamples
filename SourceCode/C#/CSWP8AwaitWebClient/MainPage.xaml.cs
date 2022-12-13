/****************************** Module Header ******************************\
* Module Name:    MainPage.xaml.cs
* Project:        CSWP8AwaitWebClient
* Copyright (c) Microsoft Corporation
*
* This demo shows how to make an await WebClient
* (similar to HttpClient in Windows 8).
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/en-us/openness/licenses.aspx#MPL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\*****************************************************************************/

using System;
using System.Windows;
using Microsoft.Phone.Controls;

namespace CSWP8AwaitWebClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            getResult();
        }

        /// <summary>
        /// Get content of URL.
        /// </summary>
        private async void getResult()
        {
            string strRequestUri = "http://www.bing.com";
            string strResult = string.Empty;

            try
            {
                // Create a new instance.
                HttpClient httpClient = new HttpClient();

                try
                {
                    // GetStringAsync
                    strResult = await httpClient.GetStringAsync(strRequestUri);

                    // GetByteArrayAsync
                    byte[] data = await httpClient.GetByteArrayAsync(strRequestUri);
                    // strResult = data.Length.ToString();
                }
                catch (Exception ex)
                {
                    strResult = ex.Message;
                }
            }
            catch (Exception ex)
            {
                strResult = ex.Message;
            }
            finally
            {
                // Show the result.
                this.Dispatcher.BeginInvoke(delegate()
                {
                    tbResult.Text = strResult;
                });               
            }
        }
    }
}