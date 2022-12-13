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

using Microsoft.Phone.BackgroundTransfer;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace FileTransfer
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        // Request for file transfer
        BackgroundTransferRequest transferRequest;

        // Destination URI
        Uri downloadUri;

        private void DisplayImage(string downloadFilename)
        {
            // Make sure we only display jpeg images

            string testFilename = downloadFilename.ToLower();

            if ( !testFilename.EndsWith(".jpg")) return;

            // The image will be read from isolated storage into the following byte array

            byte[] data;

            // Read the entire image in one go into a byte array

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {

                // Open the file - error handling omitted for brevity

                using (IsolatedStorageFileStream isfs = isf.OpenFile(downloadFilename, FileMode.Open, FileAccess.Read))
                {
                    data = new byte[isfs.Length];
                    isfs.Read(data, 0, data.Length);
                    isfs.Close();
                }
            }
            // Create memory stream and bitmap

            MemoryStream ms = new MemoryStream(data);
            BitmapImage bi = new BitmapImage();

            // Set bitmap source to memory stream
            bi.SetSource(ms);

            // Create an image UI element – Note: this could be declared in the XAML instead
            Image image = new Image();

            // Set size of image to bitmap size for this demonstration
            image.Height = bi.PixelHeight;
            image.Width = bi.PixelWidth;

            // Assign the bitmap image to the image’s source
            displayImage.Source = bi;

        }

        private void fetchFileButton_Click(object sender, RoutedEventArgs e)
        {
            string transferFileName = filenameTextBlock.Text;

            Uri transferUri = new Uri(Uri.EscapeUriString(transferFileName), UriKind.RelativeOrAbsolute);

            // Create the new transfer request, passing in the URI of the file to 
            // be transferred.
            transferRequest = new BackgroundTransferRequest(transferUri);

            // Set the transfer method. GET and POST are supported.
            transferRequest.Method = "GET";

            // Get the file name from the end of the transfer URI and create a local URI 
            // in the "transfers" directory in isolated storage.
            string downloadFile = transferFileName.Substring(transferFileName.LastIndexOf("/") + 1);

            // Build the URI
            downloadUri = new Uri("shared/transfers/" + downloadFile, UriKind.RelativeOrAbsolute);
            transferRequest.DownloadLocation = downloadUri;

            // Set transfer options
            transferRequest.TransferPreferences = TransferPreferences.AllowCellularAndBattery;

            // Add the transfer request using the BackgroundTransferService. Do this in 
            // a try block in case an exception is thrown.
            try
            {
                BackgroundTransferService.Add(transferRequest);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Unable to add background transfer request. " + ex.Message);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to add background transfer request.");
            }

            // Bind event handlers to the progess and status changed events
            transferRequest.TransferProgressChanged += 
                new EventHandler<BackgroundTransferEventArgs>(
                    request_TransferProgressChanged);

            transferRequest.TransferStatusChanged += 
                new EventHandler<BackgroundTransferEventArgs>(
                    request_TransferStatusChanged);

            // Turn off the button for now
            fetchFileButton.IsEnabled = false;
        }

        void request_TransferStatusChanged(object sender, BackgroundTransferEventArgs e)
        {
            switch (e.Request.TransferStatus)
            {
                case TransferStatus.Completed:
                    // If the status code of a completed transfer is 200 or 206, the
                    // transfer was successful
                    if (transferRequest.StatusCode == 200 || transferRequest.StatusCode == 206)
                    {
                        statusTextBlock.Text = "Completed";

                        // Fetch the file from isolated storage and show it
                        DisplayImage(downloadUri.OriginalString);
                    }
                    else 
                    {
                        statusTextBlock.Text = "Error:" + transferRequest.StatusCode;
                    }

                    // Remove the transfer request in order to make room in the 
                    // queue for more transfers. Transfers are not automatically
                    // removed by the system.

                    try
                    {
                        BackgroundTransferService.Remove(transferRequest);
                    }
                    catch
                    {
                    }

                    // enable the button to make more transfers possible
                    fetchFileButton.IsEnabled = true;
                    break;
            }
        }

        void request_TransferProgressChanged(object sender, BackgroundTransferEventArgs e)
        {
            statusTextBlock.Text = e.Request.BytesReceived + " received.";
        }
    }
}