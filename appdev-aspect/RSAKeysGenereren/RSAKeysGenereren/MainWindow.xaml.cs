using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace RSAKeysGenereren
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _private_A;
        private static string _public_A;
        private static string _private_B;
        private static string _public_B;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();
        //private static string FileForEncrypt = "";
        private static string enc = "", dec = "";
        private static string FileForEncrypt = "", decryptFile = "";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browseVenster = new OpenFileDialog();
            if (browseVenster.ShowDialog() == true)
            {
                //FileForEncrypt = browseVenster.FileName;
                decryptFile = browseVenster.FileName;
                PadFileLabel.Content = browseVenster.FileName;
               // txtFile.Text = File.ReadAllText(FileForEncrypt);
                txtFile.Text = File.ReadAllText(decryptFile);

            }
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            //if (FileForEncrypt != "" && FileForEncrypt != " ")
            if (decryptFile != "" && decryptFile != " ")
            {
               // RSA();
                dec = Decrypt(decryptFile);
                //  txtEncrypt.Text = enc;
                // txtDecrypt.Text = File.ReadAllText(dec);
                txtDecrypt.Text = dec;


            }
            else
            {
                MessageBox.Show("Je heb nog geen bestand gekozen");
            }
        }

        private static void RSA()
        {
            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();
            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            _private_A = rsa1.ToXmlString(true);
            _public_A = rsa1.ToXmlString(false);
            _private_B = rsa2.ToXmlString(true);
            _public_B = rsa2.ToXmlString(false);

            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Private_A.txt", _private_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Public_A.txt", _public_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Private_B.txt", _private_B);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Public_B.txt", _public_B);

            //string file = System.IO.File.ReadAllText(@"C:\Users\DanielaCarmelina\Desktop\Blub.txt");
            // var text = "Daniela";
            //Console.WriteLine("RSA // Text to encrypt: " + FileForEncrypt);
            enc = Encrypt(FileForEncrypt);
            // Console.WriteLine("RSA // Encrypted Text: " + enc);
            dec = Decrypt(enc);
            dec = Decrypt(@"‪C:\DocumentenCrypto\SymetricKeyDESBlub.txt");

            // Console.WriteLine("RSA // Decrypted Text: " + dec);
        }



        public static string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();

            //string publicB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Public_B.txt");
            string publicB = File.ReadAllText(@"C:\DocumentenCrypto\Keys\Public_Keys\PublicKey_Daniela.txt");
            rsa.FromXmlString(publicB); //encrypteren met de publickey sender
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(", ");
            }

            return sb.ToString();
        }

        public static string Decrypt(string data)
        {
            string datat = File.ReadAllText(data);

            var rsa = new RSACryptoServiceProvider();
            var dataArray = datat.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            //string priveB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Private_B.txt");

          //  string priveB = File.ReadAllText(@"C:\DocumentenCrypto\Keys\Private_Keys\PrivateKey_Mamie.txt");
           string priveB = File.ReadAllText(@"C:\DocumentenCrypto\Keys\Public_Keys\PublicKey_Danie.txt");

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            //var decryptedByte = rsa.Decrypt(dataByte, false);
            var decryptedByte = rsa.Decrypt(dataByte, true);

            return _encoder.GetString(decryptedByte);
        }


    }
}


/* BRONNEN : http://stackoverflow.com/questions/18485715/how-to-use-public-and-private-key-encryption-technique-in-c-sharp 
, https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx , https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx */
