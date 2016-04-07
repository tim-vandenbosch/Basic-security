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

        private void encryptHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("encryptFile");
            encryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;


        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("homePage");
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;

        }

        private void decryptHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("decryptFile");
            homePageGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Visible;

        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("homePage");
            homePageGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;


        }
    }
}
