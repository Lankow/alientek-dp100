using System.Windows;
using Alientek_DP100;

namespace DP100App
{
    public partial class MainWindow : Window
    {
        private readonly AlientekDP100 _alientek;
        public MainWindow()
        {
            _alientek = new AlientekDP100();
            InitializeComponent();
        }
    }
}