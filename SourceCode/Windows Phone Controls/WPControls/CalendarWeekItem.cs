using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WPControls
{
    /// <summary>
    /// Class representing week number cell
    /// </summary>
    public class CalendarWeekItem : Control
    {
        #region Constructor

        /// <summary>
        /// Create new instance of a calendar week number cell
        /// </summary>
        public CalendarWeekItem()
        {
            DefaultStyleKey = typeof(CalendarWeekItem);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Day number for this calendar cell.
        /// This changes depending on the month shown
        /// </summary>
        public int? WeekNumber
        {
            get { return (int)GetValue(WeekNumberProperty); }
            internal set { SetValue(WeekNumberProperty, value); }
        }

        /// <summary>
        /// Day number for this calendar cell.
        /// This changes depending on the month shown
        /// </summary>
        public static readonly DependencyProperty WeekNumberProperty =
            DependencyProperty.Register("WeekNumber", typeof(int), typeof(CalendarWeekItem), new PropertyMetadata(null));

        #endregion
    }
}
