using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApplication1
{
    public class SizesToPercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double currentSize = System.Convert.ToDouble(values[0]);
            double parentSize = System.Convert.ToDouble(values[1]);
            double percentage;
            if (parentSize != 0)
            {
                percentage = currentSize / parentSize;
            }
            else
            {
                percentage = 1;
            }
            return percentage.ToString("P");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}