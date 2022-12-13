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

using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace PictureDisplay
{
    public partial class MainPage : PhoneApplicationPage
    {

    PhotoChooserTask photoChooser;

    // Constructor
    public MainPage()
    {
        InitializeComponent();

        photoChooser = new PhotoChooserTask();

        photoChooser.Completed += new EventHandler<PhotoResult>(photoChooser_Completed);
    }

        void photoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                selectedImage.Source = new BitmapImage(new Uri(e.OriginalFileName));
            }
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            photoChooser.Show();
        }
    }
}