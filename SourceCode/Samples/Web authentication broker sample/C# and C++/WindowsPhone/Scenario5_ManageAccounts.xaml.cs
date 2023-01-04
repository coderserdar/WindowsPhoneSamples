﻿//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
//
//*********************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SDKTemplate;
using System;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Security.Credentials;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Data.Json;
using Windows.Storage;


namespace WebAuthentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario5 : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()

        MainPage rootPage = MainPage.Current;

        public Scenario5()
        {
            this.InitializeComponent();

        }



        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when the navigation is about to change to a different page. You can use this function for cleanup.
        /// </summary>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
           
        }


        private void DebugPrint(String Trace)
        {
            
        }
    }
}

