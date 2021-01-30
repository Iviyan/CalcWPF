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
            Main.Content = new SimpleCalc();
        }

        private void SimpleCalcItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new SimpleCalc();
        }

        private void ScientificCalcItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new ScientificCalc();
        }
    }
}
