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

using Microsoft.Phone.Scheduler;

namespace EggTimer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        int eggTime = 5;

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timeSlider != null)
            {
                eggTime = (int) (timeSlider.Value + 0.5f);
                timeTextBlock.Text = eggTime.ToString();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            timeSlider.Minimum = 1;
            timeSlider.Maximum = 10;
            timeSlider.Value = eggTime;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Reminder eggReminder = ScheduledActionService.Find("Egg Timer") as Reminder;

            if ( eggReminder != null ) 
            {
                ScheduledActionService.Remove("Egg Timer");
            }

            eggReminder = new Reminder("Egg Timer");

            eggReminder.BeginTime = DateTime.Now + new TimeSpan(0, eggTime, 0);
            eggReminder.Content = "Egg Ready";
            eggReminder.RecurrenceType = RecurrenceInterval.None;
            eggReminder.NavigationUri = new Uri("/EggReadyPage.xaml", UriKind.Relative);

            ScheduledActionService.Add(eggReminder);
        }
    }
}