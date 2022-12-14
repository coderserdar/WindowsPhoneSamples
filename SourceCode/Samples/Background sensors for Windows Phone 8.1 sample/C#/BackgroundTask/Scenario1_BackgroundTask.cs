// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.Devices.Background;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Storage;

namespace BackgroundTasks
{
    public sealed class Scenario1_BackgroundTask : IBackgroundTask, IDisposable
    {
        private Accelerometer _accelerometer;
        private BackgroundTaskDeferral _deferral;
        private ulong _sampleCount;

        /// <summary> 
        /// Background task entry point.
        /// </summary> 
        /// <param name="taskInstance"></param>
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            _accelerometer = Accelerometer.GetDefault();

            if (null != _accelerometer)
            {
                _sampleCount = 0;

                // Select a report interval that is both suitable for the purposes of the app and supported by the sensor.
                uint minReportIntervalMsecs = _accelerometer.MinimumReportInterval;
                _accelerometer.ReportInterval = minReportIntervalMsecs > 16 ? minReportIntervalMsecs : 16;

                // Subscribe to accelerometer ReadingChanged events.
                _accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);

                // Take a deferral that is released when the task is completed.
                _deferral = taskInstance.GetDeferral();

                // Get notified when the task is canceled.
                taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);

                // Store a setting so that the app knows that the task is running.
                ApplicationData.Current.LocalSettings.Values["IsBackgroundTaskActive"] = true;
            }
        }

        /// <summary> 
        /// Called when the background task is canceled by the app or by the system.
        /// </summary> 
        /// <param name="sender"></param>
        /// <param name="reason"></param>
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            ApplicationData.Current.LocalSettings.Values["TaskCancelationReason"] = reason.ToString();
            ApplicationData.Current.LocalSettings.Values["SampleCount"] = _sampleCount;
            ApplicationData.Current.LocalSettings.Values["IsBackgroundTaskActive"] = false;

            // Complete the background task (this raises the OnCompleted event on the corresponding BackgroundTaskRegistration).
            _deferral.Complete();
        }

        /// <summary>
        /// Frees resources held by this background task.
        /// </summary>
        public void Dispose()
        {
            if (null != _accelerometer)
            {
                _accelerometer.ReadingChanged -= new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
                _accelerometer.ReportInterval = 0;
            }
        }

        /// <summary>
        /// This is the event handler for acceleroemter ReadingChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
        {
            _sampleCount++;

            // Save the sample count if the foreground app is visible.
            bool appVisible = (bool)ApplicationData.Current.LocalSettings.Values["IsAppVisible"];
            if (appVisible)
            {
                ApplicationData.Current.LocalSettings.Values["SampleCount"] = _sampleCount;
            }
        }
    }
}
