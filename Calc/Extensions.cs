using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calc
{
    public class CustomInput
    {
        public string Text { get; set; }
        public int SelectionOffset { get; set; } = 0;
        public int SelectionLength { get; set; } = 0;
    }
    public class Extensions
    {
        public static readonly DependencyProperty CustomInputProperty =
            DependencyProperty.RegisterAttached("CustomInput", typeof(CustomInput), typeof(Extensions), new PropertyMetadata(default(CustomInput)));

        public static void SetCustomInput(UIElement element, CustomInput value)
        {
            element.SetValue(CustomInputProperty, value);
        }

        public static CustomInput GetCustomInput(UIElement element)
        {
            return element.GetValue(CustomInputProperty) as CustomInput;
        }
    }
}
