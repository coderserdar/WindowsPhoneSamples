//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SDKTemplate;
using System;
using System.Text;
using Windows.Globalization;
using Windows.Globalization.DateTimeFormatting;

namespace CalendarSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CalendarEnumerationAndMath : Page
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;

        public CalendarEnumerationAndMath()
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
        /// This is the click handler for the 'Default' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            // This scenario uses the Windows.Globalization.Calendar class to enumerate through a calendar and
            // perform calendar math
            StringBuilder results = new StringBuilder();

            results.AppendLine("The number of years in each era of the Japanese era calendar is not regular. It is determined by the length of the given imperial era:\n");

            // Create Japanese calendar.
            Calendar calendar = new Calendar(new[] { "en-US" }, CalendarIdentifiers.Japanese, ClockIdentifiers.TwentyFourHour);

            // Enumerate all supported years in all supported Japanese eras.
            for (calendar.Era = calendar.FirstEra; true; calendar.AddYears(1))
            {
                // Process current era.
                results.AppendLine("Era " + calendar.EraAsString() + " contains " + calendar.NumberOfYearsInThisEra + " year(s)");

                // Enumerate all years in this era.
                for (calendar.Year = calendar.FirstYearInThisEra; true; calendar.AddYears(1))
                {
                    // Begin sample processing of current year.

                    // Move to first day of year. Change of month can affect day so order of assignments is important.
                    calendar.Month = calendar.FirstMonthInThisYear;
                    calendar.Day = calendar.FirstDayInThisMonth;

                    // Set time to midnight (local).
                    calendar.Period = calendar.FirstPeriodInThisDay;    // All days have 1 or 2 periods depending on clock type
                    calendar.Hour = calendar.FirstHourInThisPeriod;     // Hours start from 12 or 0 depending on clock type
                    calendar.Minute = 0;
                    calendar.Second = 0;
                    calendar.Nanosecond = 0;

                    if (calendar.Year % 1000 == 0)
                    {
                        results.AppendLine();
                    }
                    else if (calendar.Year % 10 == 0)
                    {
                        results.Append(".");
                    }

                    // End sample processing of current year.

                    // Break after processing last year.
                    if (calendar.Year == calendar.LastYearInThisEra)
                    {
                        break;
                    }
                }
                results.AppendLine();

                // Break after processing last era.
                if (calendar.Era == calendar.LastEra)
                {
                    break;
                }
            }

            // This section shows enumeration through the hours in a day to demonstrate that the number of time units in a given period (hours in a day, minutes in an hour, etc.)
            // should not be regarded as fixed. With Daylight Saving Time and other local calendar adjustments, a given day may have not have 24 hours, and
            // a given hour may not have 60 minutes, etc.
            results.AppendLine("\nThe number of hours in a day is not invariable. The US calendar transitions from DST to standard time on 4 November 2012:\n");

            // Create a DateTimeFormatter to display dates
            DateTimeFormatter displayDate = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longdate");

            // Create a gregorian calendar for the US with 12-hour clock format
            Calendar currentCal = new Windows.Globalization.Calendar(new string[] { "en-US" }, CalendarIdentifiers.Gregorian, ClockIdentifiers.TwentyFourHour, "america/los_angeles");

            // Set the calendar to a the date of the Daylight Saving Time-to-Standard Time transition for the US in 2012.
            // DST ends in the US at 02:00 on 4 November 2012
            DateTime dstDate = new DateTime(2012, 11, 4);  
            currentCal.SetDateTime(dstDate);

            // Set the current calendar to one day before DST change. Create a second calendar for comparision and set it to one day after DST change.
            Calendar endDate = currentCal.Clone();
            currentCal.AddDays(-1);
            endDate.AddDays(1);

            // Enumerate the day before, the day of, and the day after the 2012 DST-to-Standard time transition
            while (currentCal.Day <= endDate.Day)
            {
                // Process current day.
                DateTimeOffset date = currentCal.GetDateTime();
                results.AppendFormat("{0} contains {1} hour(s)\n", displayDate.Format(date), currentCal.NumberOfHoursInThisPeriod);

                // Enumerate all hours in this day.
                // Create a calendar to represent the following day.
                Calendar nextDay = currentCal.Clone();
                nextDay.AddDays(1);
                for (currentCal.Hour = currentCal.FirstHourInThisPeriod; true; currentCal.AddHours(1)) 
                {
                    // Display the hour for each hour in the day.             
                    results.AppendFormat("{0} ", currentCal.HourAsPaddedString(2));

                    // Break upon reaching the next period (i.e. the first period in the following day).
                    if (currentCal.Day == nextDay.Day && currentCal.Period == nextDay.Period) 
                    {
                        break;
                    }
                }

                results.AppendLine();
            }

            // Display results
            OutputTextBlock.Text = results.ToString();
        }
    }
}
