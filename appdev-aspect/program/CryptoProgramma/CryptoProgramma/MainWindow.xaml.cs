using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static string FileForEncrypt = "";
        static string  hoofdPad = "C:\\";
        string[] opgeslagenBestanden = new string[8];



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

        private void backButton_EnFileGr_Click(object sender, RoutedEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("homePage");

            if (FileForEncrypt != "" && FileForEncrypt != " ")
            {
                //Hier word de symetric AESkey genereert 
                string filetext = File.ReadAllText(FileForEncrypt);

               if(sKeySlider.Value == 2)
                {
                    string cipherText = RijndaelSimple.Encrypt (
                            filetext,   // original plaintext
                         "Pas5pr@se",    // can be any string
                        "s@1tValue",      // can be any string
                        "SHA1",          // can be "MD5"
                          2,            // can be any number
                        "@1B2c3D4e5F6g7H8",// must be 16 bytes
                          256       // can be 192 or 128
                           );

                }else if(sKeySlider.Value == 1)
                {

                }

                //public en private keys gemaakt en gesaved
                opgeslagenBestanden =  RSA.keys(hoofdPad, senderTxt.Text, receiverTxt.Text);
            }
            else
            {
                MessageBox.Show("Je heb nog geen bestand gekozen");
            }


            encryptingGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 700;
            CryptoProgramWin.Width = 550;

            namePrKeySenderLbl.Content = Convert.ToString(opgeslagenBestanden[1]);
            padPrivateSenderLbl.Content = Convert.ToString(opgeslagenBestanden[4]);
            checkBox1.IsChecked = true;


            namePuKeySenderLbl.Content = Convert.ToString(opgeslagenBestanden[0]);
            padPublicSenderLbl.Content = Convert.ToString(opgeslagenBestanden[5]);
            checkBox2.IsChecked = true;

            namePrKeyReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[3]);
            padPrivateReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[6]);
            checkBox3.IsChecked = true;

            namePuKeyReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[2]);
            padPublicReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[7]);
            checkBox4.IsChecked = true;

        }

        private void backBtn_EnGr_Click(object sender, RoutedEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            encryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;

        }

        private void decryptHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("decryptFile");
            decryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;

        }

        private void backButton_DeFileGr_Click(object sender, RoutedEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;
        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            //mainTabs.SelectedItem = mainTabs.FindName("homePage");

            //plainText = RijndaelSimple.Decrypt
            //(
            //    cipherText,
            //    passPhrase,
            //    saltValue,
            //    hashAlgorithm,
            //    passwordIterations,
            //    initVector,
            //    keySize
            //);

            //lblDecrypted.Content = "" + plainText;

            decryptingGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;
        }

        private void backBtn_DeGr_Click(object sender, RoutedEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            decryptingGrid.Visibility = Visibility.Collapsed;

        }



        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }


        private void home_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
        }

        private void stega_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            steganografieGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            settingsGrid.Visibility = Visibility.Collapsed;

        }

        private void settings_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            settingsGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            steganografieGrid.Visibility = Visibility.Collapsed;
            rsaKeys_lbl.Content = hoofdPad;

        }

        private void help_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            helpGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            steganografieGrid.Visibility = Visibility.Collapsed;
            settingsGrid.Visibility = Visibility.Collapsed;

        }

        private void exit_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void backSteg_Btn_Click(object sender, RoutedEventArgs e)
        {
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            steganografieGrid.Visibility = Visibility.Collapsed;
            homePageGrid.Visibility = Visibility.Visible;


        }

        private void browseEnBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browseVenster = new OpenFileDialog();
            if (browseVenster.ShowDialog() == true)
            {
                FileForEncrypt = browseVenster.FileName;
                padEnFileLbl.Content = browseVenster.FileName;
            }

        }

        private void rsaKeys_ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browseVenster = new OpenFileDialog();
            if (browseVenster.ShowDialog() == true)
            {
                string pad;
                int nr;
                pad = browseVenster.FileName;
                nr  = pad.LastIndexOf('\\');
                hoofdPad = pad.Substring(0, nr);
                rsaKeys_lbl.Content = hoofdPad ;
            }
        }
    }
}
/* Bronnen:  https://github.com/alicanerdogan/HamburgerMenu */
