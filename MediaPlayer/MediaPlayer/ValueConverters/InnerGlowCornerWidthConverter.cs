﻿using System;
using System.Linq;
using System.Windows.Data;
using System.Windows;

namespace MediaPlayer.ValueConverters
{
    [ValueConversion(typeof(Thickness), typeof(double))]
    public class InnerGlowCornerWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Thickness thickness = (Thickness)value;
            String side = (String)parameter;

            switch (side)
            {
                case "TopLeft":
                    return thickness.Left * 2;
                case "TopRight":
                    return thickness.Right * 2;
                case "BottomLeft":
                    return thickness.Left * 2;
                case "BottomRight":
                    return thickness.Right * 2;
                default:
                    throw new ArgumentException("Unknown parameter value specified: " + side);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
