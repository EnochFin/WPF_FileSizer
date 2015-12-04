using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApplication1
{
    public class BytesToUnitsConverter : IValueConverter
    {
        private static readonly string[] Units = {"b", "kb", "mb", "gb", "tb"};

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = System.Convert.ToDouble(value);
            int unit = 0;
            while (size > 1024)
            {
                size /= 1024;
                unit++;
            }
            return size.ToString("F") + " " + Units[unit];

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}