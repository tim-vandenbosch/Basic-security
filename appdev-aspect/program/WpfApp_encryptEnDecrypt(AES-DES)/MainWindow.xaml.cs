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
using System.Security.Cryptography;
using System.IO;

namespace WpfApp_encryptEnDecrypt_DES_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // variables for AES
        string passPhrase = "Pas5pr@se";        // can be any string
        string saltValue = "s@1tValue";        // can be any string
        string hashAlgorithm = "SHA1";             // can be "MD5"
        int passwordIterations = 2;                // can be any number
        string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        int keySize = 256;                // can be 192 or 128
        string cipherText, plainText;


        public MainWindow()
        {

            InitializeComponent();

            btn_decrypt.Visibility = Visibility.Hidden;
            txtBlock_resultTxt.Visibility = Visibility.Hidden;

        }

        private void btn_encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (txtBox_txt.Text != "")
            {

                if (comboBox.SelectedIndex == 0)
                {
                    plainText = txtBox_txt.Text;    // original plaintext
                    cipherText = AES.Encrypt
                       (
                           plainText,
                           passPhrase,
                           saltValue,
                           hashAlgorithm,
                           passwordIterations,
                           initVector,
                           keySize
                       );

                    txtBlock_crypt.Text = cipherText;

                    txtBox_txt.Visibility = Visibility.Hidden;
                    btn_encrypt.Visibility = Visibility.Hidden;
                    txtBlock_urTxt.Visibility = Visibility.Hidden;

                    btn_decrypt.Visibility = Visibility.Visible;
                    txtBlock_resultTxt.Visibility = Visibility.Visible;
                    MessageBox.Show("AES Encryption!", "AES", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                   else if (comboBox.SelectedIndex == 1)
                {
                    string cryptedString = DES.Encrypt(txtBox_txt.Text);
                    txtBlock_crypt.Text = cryptedString;

                    txtBox_txt.Visibility = Visibility.Hidden;
                    btn_encrypt.Visibility = Visibility.Hidden;
                    txtBlock_urTxt.Visibility = Visibility.Hidden;

                    btn_decrypt.Visibility = Visibility.Visible;
                    txtBlock_resultTxt.Visibility = Visibility.Visible;
                    MessageBox.Show("DES Encryption!", "DES", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("The string which needs to be encrypted/decrypted can not be null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btn_decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                plainText = AES.Decrypt
            (
                cipherText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

                txtBlock_crypt.Text = plainText;
                txtBlock_resultTxt.Text = "Decrypt Result: ";

                txtBox_txt.Visibility = Visibility.Visible;
                btn_encrypt.Visibility = Visibility.Visible;
                txtBlock_urTxt.Visibility = Visibility.Visible;

                btn_decrypt.Visibility = Visibility.Hidden;

                MessageBox.Show("AES Decryption!", "AES", MessageBoxButton.OK, MessageBoxImage.Information);

            }

            else if (comboBox.SelectedIndex == 1)
            {
                string decryptedString = DES.Decrypt(txtBlock_crypt.Text);
                txtBlock_crypt.Text = decryptedString;
                txtBlock_resultTxt.Text = "Decrypt Result: ";

                txtBox_txt.Visibility = Visibility.Visible;
                btn_encrypt.Visibility = Visibility.Visible;
                txtBlock_urTxt.Visibility = Visibility.Visible;

                btn_decrypt.Visibility = Visibility.Hidden;

                MessageBox.Show("DES Decryption!", "DES", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }


    }
}
