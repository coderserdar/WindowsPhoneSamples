using System;

namespace WPControls
{
    /// <summary>
    /// Interface that needs to be implemented in order to support DatesSource for Calendar control
    /// </summary>
    public interface ISupportCalendarItem
    {
        /// <summary>
        /// Date of the calendar item
        /// </summary>
        DateTime CalendarItemDate { get; }
    }
}
