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
        private static string FileForEncrypt = "";


        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browseVenster = new OpenFileDialog();
            if (browseVenster.ShowDialog() == true)
            {
                FileForEncrypt = browseVenster.FileName;
                PadFileLabel.Content = browseVenster.FileName;
                txtFile.Text = File.ReadAllText(FileForEncrypt);
            }
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileForEncrypt != "" && FileForEncrypt != " ")
            {
                RSA();
                txtEncrypt.Text = enc;
                txtDecrypt.Text = File.ReadAllText(dec);
            }
            else
            {
                MessageBox.Show("Je heb nog geen bestand gekozen");
            }
        }

        private static void RSA()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            _private_A = rsa.ToXmlString(true);
            _public_A = rsa.ToXmlString(false);
            _private_B = rsa.ToXmlString(true);
            _public_B = rsa.ToXmlString(false);

            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Private_A.txt", _private_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_A\Public_A.txt", _public_A);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Private_B.txt", _private_B);
            File.WriteAllText(@"C:\TestCrypto\Keys_B\Public_B.txt", _private_B);

            //string file = System.IO.File.ReadAllText(@"C:\Users\DanielaCarmelina\Desktop\Blub.txt");
            // var text = "Daniela";
            //Console.WriteLine("RSA // Text to encrypt: " + FileForEncrypt);
            enc = Encrypt(FileForEncrypt);
            // Console.WriteLine("RSA // Encrypted Text: " + enc);
            dec = Decrypt(enc);
            // Console.WriteLine("RSA // Decrypted Text: " + dec);
        }



        public static string Encrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();

            string publicB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Public_B.txt");

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
                    sb.Append(",");
            }

            return sb.ToString();
        }

        public static string Decrypt(string data)
        {
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];
            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            string priveB = File.ReadAllText(@"C:\TestCrypto\Keys_B\Private_B.txt");

            rsa.FromXmlString(priveB); //decryteren met de privatekey sender
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }


    }
}


/* BRONNEN : http://stackoverflow.com/questions/18485715/how-to-use-public-and-private-key-encryption-technique-in-c-sharp 
, https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx , https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx */
