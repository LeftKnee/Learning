using System;
using Windows.UI.Xaml.Data;

namespace IssueTracker.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is DateTimeOffset dateTimeOffset)) return value;

            return string.Format(dateTimeOffset.ToString((string)parameter), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
