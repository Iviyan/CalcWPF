using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void input_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(c => Char.IsDigit(c) || ",+-*/".Contains(c)))
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
                string result = Calculator.Calculate(Input.Text.Replace(" ", "")).ToString();
                Result.Text = $"= {result}";
            }
            catch
            {
                if (writeErrorMsg) Result.Text = $"Ошибка";
            }
        }
        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            DoCalc();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (Input.Text.Length > 0)
                Input.Text = Input.Text.Substring(0, Input.Text.Length - 1);
        }
        private void SymbolButton_Click(object sender, RoutedEventArgs e)
        {
            Input.Text += ((Button)sender).Content;
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
    }


}
