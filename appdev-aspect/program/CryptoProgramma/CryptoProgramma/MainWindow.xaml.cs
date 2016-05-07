using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Forms;
using Xceed.Wpf.Toolkit;

namespace CryptoProgramma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties
        private static string FileForEncrypt = "";
        // static string PadRSAKeys = "C\\"; // --> Deze waarde is nooit gebruikt
        //********Daniela begin ********************
        static string  hoofdPad = "C:\\DocumentenCrypto\\";
        string filename = "";   
        string[] opgeslagenBestanden = new string[8];
        Microsoft.Win32.OpenFileDialog browseVenster = new Microsoft.Win32.OpenFileDialog();
        //********Daniela end ********************

        // edit nasim
        string encryptedFilePath;
        string encryptedFileName;
        static string sKey;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void encryptHomeButton_Click(object sender, RoutedEventArgs e)
        {            //Daniela
            //mainTabs.SelectedItem = mainTabs.FindName("encryptFile");
            encryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            padEnFileLbl.Content = "";
            senderTxt.Text = "";
            receiverTxt.Text = "";
            //Daniela
        }

        private void backButton_EnFileGr_Click(object sender, RoutedEventArgs e)
        {            //Daniela
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
        }

        private void browseEnBtn_Click(object sender, RoutedEventArgs e)
        {            //Daniela
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                FileForEncrypt = browseVenster.FileName;
                padEnFileLbl.Content = browseVenster.FileName;
            }
        }

        // edit Nasim
        private static string SplitNameOfFile(string plainFilePath, string algirithme, string newSuffix)
        {
            return System.IO.Path.GetFileNameWithoutExtension(plainFilePath) + algirithme + newSuffix;
        }
        
        /// <summary>
        /// Generate random byte array
        /// </summary>
        /// <param name="length">array length</param>
        /// <returns>Random byte array</returns>
        private static byte[] GenerateRandom(int length)
        {
            byte[] bytes = new byte[length];
            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(bytes);
            }
            return bytes;
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            //public en private keys gemaakt en gesaved
            //Daniela
            opgeslagenBestanden = RSA.keys(hoofdPad, senderTxt.Text, receiverTxt.Text);
            //de publieke en private sleutel worden gegenereerd en opgeslagen


            if (FileForEncrypt != "" && FileForEncrypt != " ")
            {
                //Hier word de symetric AESkey genereert 
                string filetext = File.ReadAllText(FileForEncrypt);
                //lander
                //hash gemaakt van bestand
                statusLbl.Content = "Hashing file (MD5)";
                encrProgressbar.Value = 0;
                string md5sum = hash(FileForEncrypt);
                // Console.WriteLine(md5sum);
                //lander

                //********Daniela begin ********************
                //hash geencrypteert en opgeslaan
                string encryptHash = RSA.Encrypt(md5sum);
                Directory.CreateDirectory(hoofdPad);
               string hashfilename = System.IO.Path.GetFileNameWithoutExtension(FileForEncrypt);
                if (!System.IO.File.Exists(hoofdPad + "Hash" + hashfilename + ".txt"))
                {
                    File.WriteAllText(hoofdPad + "Hash" + hashfilename + ".txt", encryptHash);
                }
                nameEnHashLbl.Content = "Hash" + hashfilename + ".txt";
                padEncryptedHash.Content = hoofdPad + "Hash" + hashfilename + ".txt";
                checkBox7.IsChecked = true;


                encryptingGrid.Visibility = Visibility.Visible;
                encryptFileGrid.Visibility = Visibility.Collapsed;
                CryptoProgramWin.Height = 700;
                CryptoProgramWin.Width = 550;
                //********Daniela end ********************

                if (sKeySlider.Value == 2)
                {
                    statusLbl.Content = "Preparing (AES)";
                    encrProgressbar.Value = 10;
                    //****************door Nasim toegevoed*******************
                    string plainFilePath = padEnFileLbl.Content.ToString();
                    encryptedFileName = SplitNameOfFile(plainFilePath, "AES", ".txt");
                    //Krijg anders error als ik Keys map verwijder? Maakt deze gewoon aan indien deze nog niet bestaat
                    System.IO.Directory.CreateDirectory(hoofdPad);
                    encryptedFilePath = hoofdPad + encryptedFileName;

                    byte[] encryptionKey = GenerateRandom(16);
                    byte[] encryptionIV = GenerateRandom(16);
                    byte[] signatureKey = GenerateRandom(64);

                    statusLbl.Content = "Encrypting (AES) ";
                    encrProgressbar.Value = 20;
                    AES.EncryptFile(plainFilePath, encryptedFilePath, encryptionKey, encryptionIV);
                    statusLbl.Content = "Finished (AES)";
                    encrProgressbar.Value = 100;

                    // tonen meer info over encrypteren
                    // System.Windows.MessageBox.Show(string.Format(AES.CreateEncryptionInfoXml(signatureKey, encryptionKey, encryptionIV)), "Info about encryption", MessageBoxButton.OK, MessageBoxImage.Information);
                    //*************************END**************************
                    //********Daniela begin ********************
                    //opslaan en encrypteren symetrisch DES key
                    //Directory.CreateDirectory(hoofdPad);
                    //string encryptAESSkey = RSA.Encrypt(encryptionKey);
                    //filename = System.IO.Path.GetFileNameWithoutExtension(plainFilePath);
                    //if (!System.IO.File.Exists(hoofdPad + "SymetricKeyAES" + filename + ".txt"))
                    //{
                    //    File.WriteAllText(hoofdPad + "SymetricKeyAES" + filename + ".txt", encryptAESSkey);
                    //}
                    //********Daniela end ********************


                }
                else if (sKeySlider.Value == 1)
                {
                    //****************door Nasim toegevoed*******************

                    statusLbl.Content = "Preparing (DES)";
                    encrProgressbar.Value = 10;

                    sKey = DES.GenerateKey();

                    string source = padEnFileLbl.Content.ToString();
                    encryptedFileName = SplitNameOfFile(source, "DES", ".txt");
                    //Krijg anders error als ik Keys map verwijder? Maakt deze gewoon aan indien deze nog niet bestaat
                    System.IO.Directory.CreateDirectory(hoofdPad);
                    encryptedFilePath = hoofdPad + encryptedFileName;
                    string destination = encryptedFilePath;

                    statusLbl.Content = "Encrypting (DES)";
                    encrProgressbar.Value = 20;

                    DES.EncryptFile(source, destination, sKey);

                    statusLbl.Content = "Finished (DES)";
                    encrProgressbar.Value = 100;
                    // System.Windows.MessageBox.Show("Succesfully Encrypted!", "Info about encryption", MessageBoxButton.OK, MessageBoxImage.Information);
                    //*************************END**************************


                    //********Daniela begin ********************
                    //opslaan en encrypteren symetrisch DES key
                   string encryptDESSkey =  RSA.Encrypt(sKey);
                    Directory.CreateDirectory(hoofdPad);
                    filename = System.IO.Path.GetFileNameWithoutExtension(source);
                    if (!System.IO.File.Exists(hoofdPad + "SymetricKeyDES" + filename + ".txt"))
                    {
                        File.WriteAllText(hoofdPad + "SymetricKeyDES" + filename + ".txt", encryptDESSkey);
                    }
                    nameEnSymKLbl.Content = "SymetricKeyDES" + filename + ".txt";
                    padEncryptedSkey.Content = hoofdPad + "SymetricKeyDES" + filename + ".txt";
                    checkBox6.IsChecked = true;
                    //********Daniela end ********************
                }

                //********Daniela begin ********************
                

                namePrKeySenderLbl.Content = Convert.ToString(opgeslagenBestanden[1]);
                padPrivateSenderLbl.Content = Convert.ToString(opgeslagenBestanden[4]);
                checkBox1.IsChecked = true; 
                //toont de naam en pad van de private key zender

                namePuKeySenderLbl.Content = Convert.ToString(opgeslagenBestanden[0]);
                padPublicSenderLbl.Content = Convert.ToString(opgeslagenBestanden[5]);
                checkBox2.IsChecked = true;
                //toont de naam en pad van de publieke key zender

                namePrKeyReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[3]);
                padPrivateReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[6]);
                checkBox3.IsChecked = true;
                //toont de naam en pad van de private key ontvanger

                namePuKeyReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[2]);
                padPublicReceiverLbl.Content = Convert.ToString(opgeslagenBestanden[7]);
                checkBox4.IsChecked = true;
                //toont de naam en pad van de private key ontvanger
                //******Daniela einde*****

                padEncryptedFile.Content = encryptedFilePath;
                nameEnFileLbl.Content = encryptedFileName;
                checkBox5.IsChecked = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Je heb nog geen bestand gekozen");
            }
        }

        private void backBtn_EnGr_Click(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            homePageGrid.Visibility = Visibility.Visible;
            encryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            //********Daniela end ********************

        }

        private void decryptHomeButton_Click(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            //mainTabs.SelectedItem = mainTabs.FindName("decryptFile");
            decryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            //********Daniela end ********************

        }

        private void backButton_DeFileGr_Click(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            homePageGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            //********Daniela end ********************

        }


        private void backBtn_DeGr_Click(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            homePageGrid.Visibility = Visibility.Visible;
            decryptingGrid.Visibility = Visibility.Collapsed;
            //********Daniela end ********************

        }
        
        private void exit_Menu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //********Daniela begin ********************

            System.Windows.Application.Current.Shutdown();
            //********Daniela end ********************

        }

        private void backSteg_Btn_Click(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            steganografieGrid.Visibility = Visibility.Collapsed;
            homePageGrid.Visibility = Visibility.Visible;
            //********Daniela end ********************

        }

        private void rsaKeys_ChangeBtn_Click(object sender, RoutedEventArgs e)
        { //kevin
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            browseFolder.ShowDialog() ;
            hoofdPad = browseFolder.SelectedPath + "\\";
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedEventArgs e)
        {       //kevin
            Brush brush = new SolidColorBrush(ClrPcker_Background.SelectedColor.Value);
            SideMenu.Background= brush;            
        }

        /**
        * <summary>
        * Calculates the specified hash of a message
        * </summary>
        * <param name="message">The message</param>
        * <param name="type">The type of hash (for example: "MD5" or "SHA1")</param>
        * <returns>A hash of message</returns>
        */
        private String hash(String message, String type)
        {
            byte[] enc = new UTF8Encoding().GetBytes(message);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName(type)).ComputeHash(enc);
            return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
        }

        /**
         * <summary>
         * Calculates the MD5 hash of a specified file
         * </summary>
         * <param name="path">The path to the file</param>
         * <returns>MD5 hash of specified file</returns>
         */
        private String hash(String path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty).ToLower();
                }
            }
        }

        private void home_Menu_Selected(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            //********Daniela end ********************

        }

        private void stega_Menu_Selected(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            steganografieGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            settingsGrid.Visibility = Visibility.Collapsed;
            //********Daniela end ********************

        }

        private void settings_Menu_Selected(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************

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
            //********Daniela end ********************

        }

        private void help_Menu_Selected(object sender, RoutedEventArgs e)
        {
            //********Daniela begin ********************
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
            //********Daniela end ********************

        }
        //properties decrypteren
        string decryptFile, decryptSemKey, decryptHash, pubSender, privReceiv;

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {

            decryptingGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            if(fileLbl.Content.Equals("") || symkeyLbl.Content.Equals("") || hashLbl.Content.Equals("") || publicLbl.Content.Equals("") || privateLbl.Content.Equals(""))
            {
                System.Windows.MessageBox.Show("Je mist een bestand, controleer of je alles heb gekozen");
            }
            else
            {
                //symetric decrypteren met privesleutel
                string inhouddecryptSemKey = File.ReadAllText(decryptSemKey);
                string ontcijferdeSemKey = RSA.Decrypt(inhouddecryptSemKey, privReceiv);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles" + "DecryptedSkey" +
                    System.IO.Path.GetFileNameWithoutExtension(decryptFile) +".txt", ontcijferdeSemKey);

                // bestand decrypteren met semetric key

                //hash berekenen boodschap

                //hash decryperen met publiekesleutel
                string inhouddecryptHash = File.ReadAllText(decryptHash);
                string ontcijferdeHash = RSA.Decrypt(inhouddecryptHash, pubSender);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles" + "DecryptedHash" +
                    System.IO.Path.GetFileNameWithoutExtension(decryptHash) + ".txt", ontcijferdeHash);

                //zelfberekende hash en ontcijferde hash vergelijken 

            }
        }

        private void browseFileBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                decryptFile = browseVenster.FileName;
                fileLbl.Content = browseVenster.FileName;
            }
           
        }

        private void browseSemKeyBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                decryptSemKey = browseVenster.FileName;
                symkeyLbl.Content = browseVenster.FileName;
            }
           
        }

        private void browseHashBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                decryptHash = browseVenster.FileName;
                hashLbl.Content = browseVenster.FileName;
            }
           
        }

        private void browsePublicBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                pubSender = browseVenster.FileName;
                publicLbl.Content = browseVenster.FileName;
            }
          
        }

        private void browsePrivateBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                privReceiv = browseVenster.FileName;
                privateLbl.Content = browseVenster.FileName;
            }
           
        }
    }
}
/* Bronnen:  https://github.com/alicanerdogan/HamburgerMenu */
