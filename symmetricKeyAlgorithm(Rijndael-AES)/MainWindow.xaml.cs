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

namespace symmetricKeyAlgorithm_Rijndael_AES_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            string plainText = "Hello, World!";    // original plaintext

            string passPhrase = "Pas5pr@se";        // can be any string
            string saltValue = "s@1tValue";        // can be any string
            string hashAlgorithm = "SHA1";             // can be "MD5"
            int passwordIterations = 2;                // can be any number
            string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128

            lblPlainText.Content = "" + plainText;
            

            string cipherText = RijndaelSimple.Encrypt
            (
                plainText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            txtboxEncrypted.Text = cipherText;

            plainText = RijndaelSimple.Decrypt
            (
                cipherText,
                passPhrase,
                saltValue,
                hashAlgorithm,
                passwordIterations,
                initVector,
                keySize
            );

            lblDecrypted.Content = "" + plainText;

        }
    }

}
