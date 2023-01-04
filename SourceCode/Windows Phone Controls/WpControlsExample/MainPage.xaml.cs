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
using System.Diagnostics;

namespace WpControlsExample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //Cal.StartingDayOfWeek = DayOfWeek.Monday;
            Cal.EnableGestures = true;
        }

        private void Cal_MonthChanged(object sender, WPControls.MonthChangedEventArgs e)
        {
            Debug.WriteLine("Cal_MonthChanged fired.  New year is " + e.Year.ToString() + " new month is " + e.Month.ToString() );
        }

        private void Cal_MonthChanging(object sender, WPControls.MonthChangedEventArgs e)
        {
            Debug.WriteLine("Cal_MonthChanging fired.  New year is " + e.Year.ToString() + " new month is " + e.Month.ToString());
        }

        private void Cal_SelectionChanged(object sender, WPControls.SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Cal_SelectionChanged fired.  New date is " + e.SelectedDate.ToString());
        }
    }
}