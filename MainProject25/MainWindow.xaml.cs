using MainProject25.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Data;
using System.Windows.Data;
using System.Data.OleDb;

namespace MainProject25
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
        }
    }
}