using System;
using System.Globalization;
using System.Windows.Data;
using Workmeter.Models;

namespace Workmeter.Views.Converters
{
    public class TaskStateToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = string.Empty;
            switch ((TaskState)value)
            {
                case TaskState.Active:
                    path = "../Images/play.png";
                    break;
                case TaskState.Paused:
                    path = "../Images/pause.png";
                    break;
                case TaskState.Stopped:
                case TaskState.Hidden:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
            return new Uri(path, UriKind.Relative);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}