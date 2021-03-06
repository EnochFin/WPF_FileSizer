﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApplication2
{
    public class SizesToGradientConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double currentSize = System.Convert.ToDouble(values[0]);
            double parentSize = System.Convert.ToDouble(values[1]);
            double percentage;
            if (currentSize > 0)
            {
                if (parentSize != 0)
                {
                    percentage = currentSize/parentSize;
                }
                else
                {
                    percentage = 1;
                }
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.GradientStops.Add(new GradientStop(Colors.DeepSkyBlue, 0));
                brush.GradientStops.Add(new GradientStop(Colors.DeepSkyBlue, percentage));
                brush.GradientStops.Add(new GradientStop(Colors.Transparent, percentage));
                return brush;
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}