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
using WPControls;

namespace WpControlsExample
{
    public class ColorConverter : IDateToBrushConverter
    {

        public Brush Convert(DateTime dateTime, bool isSelected, Brush defaultValue, BrushType brushType)
        {
            if (brushType == BrushType.Background)
            {
                if (dateTime == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 5))
                {
                    return new SolidColorBrush(Colors.Yellow);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                if (dateTime == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 6))
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return defaultValue;
                }
            }

        }
    }
}
