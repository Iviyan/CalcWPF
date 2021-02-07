using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Calc
{
    class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Object.Equals(value, parameter);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
    class IsNotEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !Object.Equals(value, parameter);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
    class IsMoreThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double a = AnyNumberToDouble(value);
            double b = AnyNumberToDouble(parameter);
            return a > b;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public double AnyNumberToDouble(object n) => n switch
        {
            byte d => d,
            sbyte d => d,
            short d => d,
            ushort d => d,
            int d => d,
            uint d => d,
            long d => d,
            ulong d => d,
            float d => d,
            double d => d,
            _ => throw new ArgumentException()
        };
    }

    class TernaryConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) => (bool)values[0] ? values[1] : values[2];

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
