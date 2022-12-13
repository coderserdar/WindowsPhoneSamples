using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickReminder
{
    static class ReminderManager
    {
        public static void SetReminder(int minutes, string name)
        {
            Reminder reminder = ScheduledActionService.Find(name) as Reminder;

            if (reminder != null)
            {
                ScheduledActionService.Remove(name);
            }

            reminder = new Reminder(name);

            reminder.BeginTime = DateTime.Now + new TimeSpan(0, minutes, 0);
            reminder.ExpirationTime = reminder.BeginTime;
            reminder.Content = name + " complete";
            reminder.RecurrenceType = RecurrenceInterval.None;
            reminder.NavigationUri = new Uri("/ReminderFiredPage.xaml", UriKind.Relative);

            ScheduledActionService.Add(reminder);
        }

        public static string GetStatus(string name)
        {
            Reminder reminder = ScheduledActionService.Find(name) as Reminder;

            if (reminder == null)
            {
                return "No reminders found";
            }

            if (!reminder.IsScheduled)
            {
                return "No reminders found";
            }

            TimeSpan timeLeft = reminder.BeginTime - DateTime.Now;

            return "Reminder in " + (int)timeLeft.TotalMinutes + ":" + (int)timeLeft.TotalSeconds % 60;
        }
    }
}
