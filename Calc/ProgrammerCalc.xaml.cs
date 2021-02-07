using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
    public partial class ProgrammerCalc : UserControl
    {
        ProgrammerCalcVM VM = new ProgrammerCalcVM();
        public ProgrammerCalc()
        {
            InitializeComponent();
            DataContext = VM;
        }

        enum Status
        {
            EnteringFirstNumber,
            OperatorSelecting,
            EnteringSecondNumber,
            Calculated,
        }
        Status _status = Status.EnteringFirstNumber;
        Status status
        {
            get => _status;
            set
            {
                _status = value;
                bool snv = value == Status.Calculated;
                if (VM.SecondNumberVisibility != snv)
                    VM.SecondNumberVisibility = snv;
            }
        }

        void ClearExpr()
        {
            VM.LeftNumber = 0;
            VM.RightNumber = 0;
            VM.Operator = null;
            status = Status.EnteringFirstNumber;
        }
        void Clear()
        {
            VM.Number = 0;
            ClearExpr();
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (VM.Number == 0 || status == Status.Calculated)
                Clear();
            else
                VM.Number = 0;
        }
        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            if (status == Status.EnteringFirstNumber)
            {
                VM.LeftNumber = VM.Number;
            }
            if (status == Status.OperatorSelecting && VM.RightNumber == 0)
            {
                VM.RightNumber = VM.Number;
                VM.Number = GetOperation(VM.Operator)(VM.LeftNumber, VM.RightNumber);
                status = Status.Calculated;
                return;
            }
            if (status == Status.EnteringSecondNumber)
            {
                VM.RightNumber = VM.Number;
                VM.Number = GetOperation(VM.Operator)(VM.LeftNumber, VM.RightNumber);
                status = Status.Calculated;
                return;
            }
            if (status == Status.Calculated)
            {
                VM.LeftNumber = VM.Number;
                VM.Number = GetOperation(VM.Operator)(VM.LeftNumber, VM.RightNumber);
            }

        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            switch (VM.Radix)
            {
                case 2: VM.Number >>= 1; break;
                case 8: VM.Number >>= 3; break;
                case 10: VM.Number /= 10; break;
                case 16: VM.Number >>= 4; break;
            }
        }
        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            var op = (string)b.Content;

            switch (op)
            {
                case "+/-": VM.Number = -VM.Number; break;
                case "+":
                case "-":
                case "*":
                case "/":
                    if (status == Status.Calculated)
                        VM.RightNumber = 0;
                    if (status is Status.EnteringFirstNumber or Status.Calculated)
                    {
                        VM.LeftNumber = VM.Number;
                        VM.Operator = op;
                        status = Status.OperatorSelecting;
                        break;
                    }

                    if (status == Status.OperatorSelecting)
                        VM.Operator = op;

                    if (status == Status.EnteringSecondNumber)
                    {
                        VM.Operator = op;
                        VM.RightNumber = 0;
                        VM.LeftNumber = VM.Number = GetOperation(VM.Operator)(VM.LeftNumber, VM.Number);
                        status = Status.OperatorSelecting;
                    }

                    break;
            }
        }

        Func<long, long, long> GetOperation(string op) => op switch
        {
            "+" => (long a, long b) => a + b,
            "-" => (long a, long b) => a - b,
            "*" => (long a, long b) => a * b,
            "/" => (long a, long b) => a / b,
            _ => throw new ArgumentException(),
        };

        int CountBit(long n)
        {
            if (n == 0) return 0;
            for (int i = 63; i >= 0; i--)
            {
                if (((1L << i) & n) != 0)
                    return i + 1;
            }
            return 0;
        }
        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            if (status == Status.OperatorSelecting)
            {
                status = Status.EnteringSecondNumber;
                if (VM.RightNumber == 0)
                    VM.Number = 0;
            }
            if (status == Status.Calculated)
                ClearExpr();

            Button b = (Button)sender;
            int digit = "0123456789ABCDEF".IndexOf(((string)b.Content)[0]);
#pragma warning disable CS0675
            int bits = CountBit(VM.Number);
            switch (VM.Radix)
            {
                case 2:
                    if (bits >= 64) return;
                    VM.Number = (VM.Number << 1) | digit;
                    break;
                case 8:
                    if (bits >= 62)
                    {
                        if (bits == 63 && digit is 0 or 1) VM.Number = (VM.Number << 1) | digit;
                        if (bits == 62 && digit is >= 0 and <= 3) VM.Number = (VM.Number << 2) | digit;
                        return;
                    }
                    VM.Number = (VM.Number << 3) | digit; break;
                case 10:
                    try { VM.Number = checked((VM.Number * 10) + digit); }
                    catch (OverflowException) { }
                    break;
                case 16:
                    if (bits >= 61) return;
                    VM.Number = (VM.Number << 4) | digit;
                    break;
            }
#pragma warning restore CS0675
        }

        private void NSPanel_Click(object sender, RoutedEventArgs e)
        {
            var NSPanel = sender as RadioButton;
            int newRadix = (int)NSPanel.Tag;

            if (VM.Radix == newRadix) return;

            VM.Radix = newRadix;
        }
    }
}
