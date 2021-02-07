using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calc
{
    public class ProgrammerCalcVM : INotifyPropertyChanged
    {
        private long number;
        private long leftNumber;
        private long rightNumber;
        private string @operator;
        private int radix = 10;
        private bool secondNumberVisibility;

        public long Number
        {
            get => number;
            set { number = value; OnPropertyChanged(); }
        }
        public long LeftNumber
        {
            get => leftNumber;
            set { leftNumber = value; OnPropertyChanged(); }
        }
        public long RightNumber
        {
            get => rightNumber;
            set { rightNumber = value; OnPropertyChanged(); }
        }
        public string Operator
        {
            get => @operator;
            set { @operator = value; OnPropertyChanged(); }
        }
        public int Radix
        {
            get => radix;
            set { radix = value; OnPropertyChanged(); }
        }
        public bool SecondNumberVisibility
        {
            get => secondNumberVisibility;
            set { secondNumberVisibility = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
