using System;
using System.Windows.Data;
using System.Windows;

namespace WPControls
{
    /// <summary>
    /// Converter that converts Boolean to visibility flag
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If value is true, returns Visibility.Visible, else Visibility.Collapsed
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture">Current culture</param>
        /// <returns>Visibility value</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Optional parameter</param>
        /// <param name="culture">Current culture</param>
        /// <returns>Not implemented</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
