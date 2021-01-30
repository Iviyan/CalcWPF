using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calc
{
    /// <summary>
    /// Логика взаимодействия для SimpleCalc.xaml
    /// </summary>
    public partial class SimpleCalc : UserControl
    {
        public SimpleCalc()
        {
            InitializeComponent();
        }

        private void Input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(c => Char.IsDigit(c) || ",+-*/^!()".Contains(c) || Char.ToLower(c) is >= 'a' and <= 'z'))
                e.Handled = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = "";
            Result.Text = "=";
        }

        void DoCalc(bool writeErrorMsg = true)
        {
            try
            {
                string result = Calculator.Calculate(Input.Text).ToString();
                Result.Text = $"= {result}";
            }
            catch (Exception e)
            {
                if (writeErrorMsg) Result.Text = $"{e.Message}";
            }
        }
        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            DoCalc();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (Input.Text.Length == 0 || Input.SelectionStart == 0) return;

            int lastSelectionStart = Input.SelectionStart;
            if (Input.SelectionLength == 0)
            {
                Input.Text = Input.Text.Remove(Input.SelectionStart - 1, 1);
                Input.SelectionStart = lastSelectionStart - 1;
            }
            else
            {
                Input.Text = Input.Text.Remove(Input.SelectionStart, Input.SelectionLength);
                Input.SelectionStart = lastSelectionStart;
            }
        }
        private void SymbolButton_Click(object sender, RoutedEventArgs e)
        {
            void InsertToInput(string s)
            {
                int lastSelectionStart = Input.SelectionStart;
                Input.Text = Input.Text
                    .Remove(Input.SelectionStart, Input.SelectionLength)
                    .Insert(Input.SelectionStart, s);

                Input.SelectionStart = lastSelectionStart + s.Length;
                Input.Focus();
            }

            Button b = (Button)sender;
            InsertToInput((string)b.Content);
        }

        private void AutoCalcButton_Checked(object sender, RoutedEventArgs e)
        {
            DoCalc();
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (AutoCalcButton?.IsChecked == true)
                DoCalc(false);
        }

        private void Result_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Result.Text.TrimStart(new char[] { ' ', '=' }));
        }
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
