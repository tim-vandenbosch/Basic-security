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

namespace CryptoProgramma
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

        private void SwitchToNyet1_Click(object sender, RoutedEventArgs e)
        {
            mainTabs.SelectedItem = mainTabs.FindName("nyet1");

        }

        private void switchToNyet2_Click(object sender, RoutedEventArgs e)
        {
            mainTabs.SelectedItem = mainTabs.FindName("nyet2");

        }
    }
}
