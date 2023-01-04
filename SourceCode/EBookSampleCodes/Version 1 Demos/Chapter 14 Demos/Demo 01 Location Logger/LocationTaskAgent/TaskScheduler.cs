#define DEBUG_AGENT

using Microsoft.Phone.Scheduler;

using Microsoft.Phone.Shell;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Device.Location;

using System.Threading;

namespace LocationTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {

        #region Isolated Storage - using the settings store

        private void saveTextToIsolatedStorage(string filename, string text)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;
            isolatedStore.Remove(filename);
            isolatedStore.Add(filename, text);
            isolatedStore.Save();
        }

        private bool loadTextFromIsolatedStorage(string filename, out string result)
        {
            IsolatedStorageSettings isolatedStore = IsolatedStorageSettings.ApplicationSettings;

            result = "";
            try
            {
                result = (string)isolatedStore[filename];
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        static ManualResetEvent GPSDoneFlag = new ManualResetEvent(false);

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            string message = "";

            string logString = "";

            if (loadTextFromIsolatedStorage("Log", out logString))
            {
                message = "Loaded ";
            }
            else
            {
                message = "Initialised ";
            }

            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            GeoPosition<GeoCoordinate> position = null;

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(delegate(object sender, GeoPositionStatusChangedEventArgs e)
                {
                    if (e.Status == GeoPositionStatus.Ready)
                    {
                        position = watcher.Position;
                        GPSDoneFlag.Set();
                    }

                });

            GPSDoneFlag.Reset();

            watcher.Start();

            // Wait for 10 seconds to give time to get position
            GPSDoneFlag.WaitOne(10000);

            // When we get here we got the position or we timed out

            string positionString;

            if (position != null)
                positionString = position.Location.ToString();
            else
                positionString = "Not Known";

            DateTime timeStamp = DateTime.Now;
            string timeStampString = timeStamp.ToShortDateString() + " " +
                timeStamp.ToShortTimeString() + System.Environment.NewLine;

            logString = logString + timeStampString + positionString
                + System.Environment.NewLine;

            saveTextToIsolatedStorage("Log", logString);

            ShellToast toast = new ShellToast();
            toast.Title = "Captain's Log";
            toast.Content = message + positionString;
            toast.Show();

#if DEBUG_AGENT
            ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(10));
#endif

            NotifyComplete();
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            throw new NotImplementedException();
        }

    }
}
