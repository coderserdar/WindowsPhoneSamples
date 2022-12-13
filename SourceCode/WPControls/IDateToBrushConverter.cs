using System.Windows.Media;
using System;

namespace WPControls
{
    /// <summary>
    /// This converter can be used to control day number color and background color
    /// for each day
    /// </summary>
    public interface IDateToBrushConverter
    {
        /// <summary>
        /// Perform conversion of a date to color
        /// This can be used to color a cell based on a date passed it
        /// </summary>
        /// <param name="dateTime">Date for the calendar cell</param>
        /// <param name="isSelected">Indicates if a date is selected by the user</param>
        /// <param name="defaultValue">Brush that will be used by default
        /// if the converter did not exists
        /// </param>
        /// <param name="brushType">Type of conversion to perform - foreground or background</param>
        /// <returns>New Brush to color day number or calendar cell background</returns>
        Brush Convert(DateTime dateTime, bool isSelected, Brush defaultValue, BrushType brushType);
    }
}
