using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Workmeter.Models;

namespace Workmeter.Views.Converters
{
    public class TaskStateToImageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (TaskState) value == TaskState.Active || (TaskState)value == TaskState.Paused ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}