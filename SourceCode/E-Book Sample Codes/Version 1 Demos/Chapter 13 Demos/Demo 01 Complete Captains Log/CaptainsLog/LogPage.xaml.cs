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

namespace CaptainsLog
{
    public partial class LogPage : PhoneApplicationPage
    {
        public LogPage()
        {
            InitializeComponent();
        }

        protected override void  OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
 	        base.OnNavigatedTo(e);
            
            // When we navigate to the page - put the log text into the display

            // Get a reference to the parent application
            App thisApp = App.Current as App;

            // Put the text onto the display
            completeLogTextBlock.Text = thisApp.LogText;
        }
    }
}