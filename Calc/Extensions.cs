using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

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

    public sealed class Int32Extension : MarkupExtension
    {
        public Int32Extension(int value) => Value = value;
        public int Value { get; set; }
        public override Object ProvideValue(IServiceProvider sp) => Value;
    };
    public sealed class Int64Extension : MarkupExtension
    {
        public Int64Extension(long value) => Value = value;
        public long Value { get; set; }
        public override Object ProvideValue(IServiceProvider sp) => Value;
    };
}
