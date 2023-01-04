using System;

namespace WPControls
{
    /// <summary>
    /// Event arguments for SelectionChanged event of the calendar
    /// </summary>
    public class SelectionChangedEventArgs : EventArgs
    {
        // ReSharper disable UnusedMember.Local
        private SelectionChangedEventArgs() { }
        // ReSharper restore UnusedMember.Local

        internal SelectionChangedEventArgs(DateTime dateTime)
        {
            SelectedDate = dateTime;
        }

        /// <summary>
        /// Date that is currently selected on the calendar
        /// </summary>
        public DateTime SelectedDate { get; private set; }
    }
}
