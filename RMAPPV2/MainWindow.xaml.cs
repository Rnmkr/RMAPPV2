using System;
using System.Windows;

namespace RMAPPV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Source = new Uri("Login.xaml", UriKind.Relative);
        }
    }
}
