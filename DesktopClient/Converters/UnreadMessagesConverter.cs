#region

using System;
using System.Globalization;
using Avalonia.Data.Converters;

#endregion

namespace DesktopClient.Converters
{
    internal class UnreadMessagesConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return string.Empty;
            }

            var val = (int)value!;

            if(val > 0)
            {
                return $"{val} Нов.";
            }
                
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
