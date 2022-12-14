// Copyright (c) Microsoft. All rights reserved.

using SDKTemplate;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FileAccess
{
    /// <summary>
    /// Creating a file.
    /// </summary>
    public sealed partial class Scenario1 : Page
    {
        private MainPage rootPage;

        public Scenario1()
        {
            this.InitializeComponent();
            CreateFileButton.Click += new RoutedEventHandler(CreateFileButton_Click);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rootPage = MainPage.Current;
        }

        private async void CreateFileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            rootPage.sampleFile = await storageFolder.CreateFileAsync(MainPage.filename, CreationCollisionOption.ReplaceExisting);
            rootPage.NotifyUser(String.Format("The file '{0}' was created.", rootPage.sampleFile.Name), NotifyType.StatusMessage);
        }
    }
}
