using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Calc
{
    class RadixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long num = (long)value;
            int toNS = (int)parameter;
            return NumberFormatter.FormatNumber(System.Convert.ToString(num, toNS), toNS);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class RadixMConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            long num = (long)values[0];
            int toNS = (int)values[1];
            return NumberFormatter.FormatNumber(System.Convert.ToString(num, toNS), toNS);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class NumberFormatter
    {
        public static string FormatNumber(string num, int radix)
        {
            int digits = radix switch
            {
                >= 3 and <= 10 => 3,
                2 or > 10 => 4,
                _ => throw new ArgumentException(),
            };

            var res = new StringBuilder(num.Length + num.Length / digits + 1);
            foreach (var s in SplitEnd(num, digits))
                res.Append(s).Append(' ');
            res.Remove(res.Length - 1, 1);

            return res.ToString();
        }
        static IEnumerable<string> SplitEnd(string str, int l)
        {
            if (String.IsNullOrEmpty(str)) yield break;
            int i = str.Length % l;
            if (i > 0) yield return str.Substring(0, i);

            for (; i < str.Length; i += l)
                yield return str.Substring(i, l);
        }
    }
}
