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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;

namespace CryptoProgramma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region properties encrypteren

        private static string FileForEncrypt = "";
        static string hoofdPad = "C:\\DocumentenCrypto\\";
        string filename = "";
        string[] opgeslagenBestanden = new string[8];
        //bestandnaam (.txt) 0 publickeySender, 1 privatekeySender, 2 publickeyreceiver, 3 privatekeyreceiver
        // 4 pad privatekeysender, 5 pad publickeySender, 6 pad privatekeyreceiver, 7 pad publickeyreceiver,

        Microsoft.Win32.OpenFileDialog browseVenster = new Microsoft.Win32.OpenFileDialog();
        string encryptedFilePath;
        string encryptedFileName;
        static string sKey;
        DES des = new DES();
        AES aes = new AES();
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();

        }

        public void TextBox_Sender_GotFocus(object sender, RoutedEventArgs e)
        {
            senderTxt.Text = senderTxt.Text == "Sender" ? string.Empty : senderTxt.Text;
        }

        public void TextBox_Receiver_GotFocus(object sender, RoutedEventArgs e)
        {
            receiverTxt.Text = receiverTxt.Text == "Receiver" ? string.Empty : receiverTxt.Text;
        }


        #region encription window

        #region home-back-browsebuttons encryption windows

        /// <summary>
        /// Button to open the encrypt window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encryptHomeButton_Click(object sender, RoutedEventArgs e)
        {            
            encryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            padEnFileLbl.Content = "Find your file, Eg. C:\\" ;
            senderTxt.Text = "Sender";
            receiverTxt.Text = "Receiver";
           
        }

        /// <summary>
        /// Buton to return to the home-window from the encryptwindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backButton_EnFileGr_Click(object sender, RoutedEventArgs e)
        {           
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Button to browse to de file for encryption
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseEnBtn_Click(object sender, RoutedEventArgs e)
        {           
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                FileForEncrypt = browseVenster.FileName;
                padEnFileLbl.Content = browseVenster.FileName;
            }
        }

        /// <summary>
        /// Button to return to the home-window after encrypting a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_EnGr_Click(object sender, RoutedEventArgs e)
        {
            

            homePageGrid.Visibility = Visibility.Visible;
            encryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
           

        }

        #endregion

        #region encryptioncode to encrypt de file, generate RSA key & AES, DES semetric keys
        /// <summary>
        /// Button to encryption the file that's given
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            //public en private keys gemaakt en gesaved
            opgeslagenBestanden = RSA.keys(hoofdPad, senderTxt.Text, receiverTxt.Text);

            if(senderTxt.Text != "" && receiverTxt.Text != "" && senderTxt.Text != receiverTxt.Text) {

            if (FileForEncrypt != "" && FileForEncrypt != " ")
            {
                string filetext = File.ReadAllText(FileForEncrypt);
                //hash gemaakt van bestand
                statusLbl.Content = "Hashing file (MD5)";
                encrProgressbar.Value = 0;
                string md5sum = hash(filetext, "MD5");

                //hash signen en opgeslaan
                string signHash = RSA.SignData(md5sum, opgeslagenBestanden[4]);//singen met private key sender
                Directory.CreateDirectory(hoofdPad);
                string hashfilename = System.IO.Path.GetFileNameWithoutExtension(FileForEncrypt);
                //if (!System.IO.File.Exists(hoofdPad + "Hash" + hashfilename + ".txt"))
                //{
                    File.WriteAllText(hoofdPad + "Hash" + hashfilename + ".txt", signHash);
                //}
                nameEnHashLbl.Content = "Hash" + hashfilename + ".txt";
                padEncryptedHash.Content = hoofdPad + "Hash" + hashfilename + ".txt";
                checkBox7.IsChecked = true;

                encryptingGrid.Visibility = Visibility.Visible;
                encryptFileGrid.Visibility = Visibility.Collapsed;
                CryptoProgramWin.Height = 700;
                CryptoProgramWin.Width = 550;

                if (sKeySlider.Value == 2)
                //Hier word de symetric AESkey genereert 
                {
                    statusLbl.Content = "Preparing (AES)";
                    encrProgressbar.Value = 10;

                    sKey = des.GenerateKey();
                    string plainFilePath = padEnFileLbl.Content.ToString();
                    encryptedFileName = SplitNameOfFile(plainFilePath, "AES", ".txt");

                    System.IO.Directory.CreateDirectory(hoofdPad);
                    encryptedFilePath = hoofdPad + encryptedFileName;
                    string destination = encryptedFilePath;

                    //byte[] encryptionKey = GenerateRandom(16);
                    //byte[] encryptionIV = GenerateRandom(16);
                    //byte[] signatureKey = GenerateRandom(64);


                    statusLbl.Content = "Encrypting (AES) ";
                    encrProgressbar.Value = 20;
                    aes.EncryptFile(plainFilePath, destination, sKey);
                    statusLbl.Content = "Finished (AES)";
                    encrProgressbar.Value = 100;

                    // tonen meer info over encrypteren
                    // System.Windows.MessageBox.Show(string.Format(AES.CreateEncryptionInfoXml(signatureKey, encryptionKey, encryptionIV)), "Info about encryption", MessageBoxButton.OK, MessageBoxImage.Information);


                    //opslaan en encrypteren symetrisch AES key
                   Directory.CreateDirectory(hoofdPad);
                    string encryptAESSkey = RSA.Encrypt(sKey, opgeslagenBestanden[7]);
                    filename = System.IO.Path.GetFileNameWithoutExtension(plainFilePath);
                    if (!System.IO.File.Exists(hoofdPad + "SymetricKeyAES" + filename + ".txt"))
                    {

                        File.WriteAllText(hoofdPad + "SymetricKeyAES" + filename + ".txt", encryptAESSkey);
                    }

                    nameEnSymKLbl.Content = "SymetricKeyAES" + filename + ".txt";
                    padEncryptedSkey.Content = hoofdPad + "SymetricKeyAES" + filename + ".txt";
                    checkBox6.IsChecked = true;


                }
                else if (sKeySlider.Value == 1)
                {
                    //Hier word de symetric DESkey genereert 


                    statusLbl.Content = "Preparing (DES)";
                    encrProgressbar.Value = 10;

                    sKey = des.GenerateKey();

                    string source = padEnFileLbl.Content.ToString();
                    encryptedFileName = SplitNameOfFile(source, "DES", ".txt");

                    System.IO.Directory.CreateDirectory(hoofdPad);
                    encryptedFilePath = hoofdPad + encryptedFileName;
                    string destination = encryptedFilePath;

                    statusLbl.Content = "Encrypting (DES)";
                    encrProgressbar.Value = 20;

                    des.EncryptFile(source, destination, sKey);

                    statusLbl.Content = "Finished (DES)";
                    encrProgressbar.Value = 100;
                    // System.Windows.MessageBox.Show("Succesfully Encrypted!", "Info about encryption", MessageBoxButton.OK, MessageBoxImage.Information);

                    //opslaan en encrypteren symetrisch DES key
                    string encryptDESSkey = RSA.Encrypt(sKey, opgeslagenBestanden[7]);
                    Directory.CreateDirectory(hoofdPad);
                    filename = System.IO.Path.GetFileNameWithoutExtension(source);
                    //if (!System.IO.File.Exists(hoofdPad + "SymetricKeyDES" + filename + ".txt"))
                    //{
                        File.WriteAllText(hoofdPad + "SymetricKeyDES" + filename + ".txt", encryptDESSkey);
                    //}
                    nameEnSymKLbl.Content = "SymetricKeyDES" + filename + ".txt";
                    padEncryptedSkey.Content = hoofdPad + "SymetricKeyDES" + filename + ".txt";
                    checkBox6.IsChecked = true;
                }


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

                padEncryptedFile.Content = encryptedFilePath;
                nameEnFileLbl.Content = encryptedFileName;
                checkBox5.IsChecked = true;
            }
            else
            {
                System.Windows.MessageBox.Show("You didn't choose a file");
            }
            }
            else
            {
                System.Windows.MessageBox.Show("You need to fill in all namefields and they need to be different from eachother");
            }
        }

        #endregion


        #endregion

        #region decription window

        #region home-back-browsebuttons decryption windows
        /// <summary>
        /// Button to go to the decryptionpage from the homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decryptHomeButton_Click(object sender, RoutedEventArgs e)
        {

            //mainTabs.SelectedItem = mainTabs.FindName("decryptFile");
            decryptFileGrid.Visibility = Visibility.Visible;
            homePageGrid.Visibility = Visibility.Collapsed;
            fileLbl.Content = "";
            symkeyLbl.Content = "";
            hashLbl.Content = "";
            publicLbl.Content = "";
            privateLbl.Content = "";

        }

        /// <summary>
        /// Button to go to the homepage from de decyptionpage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backButton_DeFileGr_Click(object sender, RoutedEventArgs e)
        {

            homePageGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Button to go to the homepage after decrypting a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_DeGr_Click(object sender, RoutedEventArgs e)
        {

            homePageGrid.Visibility = Visibility.Visible;
            decryptingGrid.Visibility = Visibility.Collapsed;
        }

        #region browse buttons
        /// <summary>
        /// Button to go to browse to the file to decrypt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseFileBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                decryptFile = browseVenster.FileName;
                fileLbl.Content = browseVenster.FileName;
            }

        }

        /// <summary>
        /// Button to go to browse to the encrypted symetric key file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseSemKeyBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                decryptSemKey = browseVenster.FileName;
                symkeyLbl.Content = browseVenster.FileName;
            }

        }

        /// <summary>
        /// Button to go to browse to the sigened hashfile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseHashBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                singedHash = browseVenster.FileName;
                hashLbl.Content = browseVenster.FileName;
            }

        }

        /// <summary>
        /// Button to go to browse to the public key file of the sender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browsePublicBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                pubSender = browseVenster.FileName;
                publicLbl.Content = browseVenster.FileName;
            }

        }

        /// <summary>
        /// Button to go to browse to the private key file of the receiver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browsePrivateBut_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                privReceiv = browseVenster.FileName;
                privateLbl.Content = browseVenster.FileName;
            }

        }
        #endregion

        #endregion


        #region decryptioncode to decrypt the file. And use of the RSA keys & AES, DES semetric keys

        #region properties decrypteren 
        string decryptFile, decryptSemKey, singedHash, pubSender, privReceiv;
        //properties decrypteren

        #endregion

        /// <summary>
        /// Button to go decrypt all given files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {

           
            if (fileLbl.Content.Equals("") || symkeyLbl.Content.Equals("") || hashLbl.Content.Equals("") || publicLbl.Content.Equals("") || privateLbl.Content.Equals(""))
            {
                System.Windows.MessageBox.Show("You miss a file. Check if you have browsed to all your files");
            }
            else
            {
                try
                {

                
                //stap 1 : symetric decrypteren met privesleutel
                string inhouddecryptSemKey = File.ReadAllText(decryptSemKey);
                string ontcijferdeSemKey = "";
                ontcijferdeSemKey = RSA.Decrypt(inhouddecryptSemKey, privReceiv);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles\\" + "DecryptedSkey" +
                    System.IO.Path.GetFileNameWithoutExtension(decryptFile) + ".txt", ontcijferdeSemKey);

                //stap 2:  bestand decrypteren met semetric key
                string destination = hoofdPad + "DecryptedFiles\\" + "DecryptedTxt" +
                                   System.IO.Path.GetFileNameWithoutExtension(decryptFile) + ".txt";

                string method = "";
                if (decryptSemKey.Contains("DES"))
                {
                    method = "DES";
                    des.DecryptFile(decryptFile, destination, ontcijferdeSemKey);
                    Process.Start(destination);
                }
                else if (decryptSemKey.Contains("AES"))
                {
                    method = "AES";
                    aes.DecryptFile(decryptFile, destination, ontcijferdeSemKey);
                    Process.Start(destination);
                }

                //stap 3 : hash berekenen boodschap
                string text = File.ReadAllText(destination);

                switch (method)
                {
                    case "DES":
                        if (!text[text.Length-1].Equals('\n'))
                            text += '\n';
                        break;
                    case "AES":
                        while (text[text.Length - 1].Equals('\0'))
                        {
                            text = text.Substring(0, text.Length - 1);
                        }
                        break;
                    default:
                        Console.WriteLine("Impossible");
                        break;

                }
                string berekendeNieuweHAsh = hash(text, "MD5");

                //Onderstaande code zou moeten werken maar geeft een false trg ipv true 
                //stap 4 & 5: hash verify met publiekesleutel, zelfberekende hash en gesignde hash vergelijken 
                string inhoudSingedHash = File.ReadAllText(singedHash);

                bool verifyHash = RSA.VerifyData(berekendeNieuweHAsh, inhoudSingedHash, pubSender);
                string resultaatHash = "";
                if (verifyHash)
                {
                    resultaatHash = "De gesignde hash, en de zelf berekende hash komen overeen";
                }
                else if (!verifyHash)
                {
                    resultaatHash = "De gesignde hash, en de zelf berekende hash komen NIET overeen";
                }
                //System.Windows.MessageBox.Show(resultaatHash);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles\\" + "ResultaatHash" +
                                 System.IO.Path.GetFileNameWithoutExtension(singedHash) + ".txt", resultaatHash);
                    decryptingGrid.Visibility = Visibility.Visible;
                    decryptFileGrid.Visibility = Visibility.Collapsed;
                }
                catch(Exception ex)
                {
                    System.Windows.MessageBox.Show("Wrong file. Check all uploaded files");
                }
            }
        }

        #endregion


        #endregion

        #region helper functions

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

        /// <summary>
        /// Function to split the name of a file | 
        /// Editor: Nasim
        /// </summary>
        /// <param name="plainFilePath"></param>
        /// <param name="algirithme"></param>
        /// <param name="newSuffix"></param>
        /// <returns></returns>
        private static string SplitNameOfFile(string plainFilePath, string algirithme, string newSuffix)
        {
            return System.IO.Path.GetFileNameWithoutExtension(plainFilePath) + algirithme + newSuffix;
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
        #endregion

        /// <summary>
        /// Button to go to the homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void home_Menu_Selected(object sender, RoutedEventArgs e)
        {
            homePageGrid.Visibility = Visibility.Visible;
            encryptFileGrid.Visibility = Visibility.Collapsed;
            encryptingGrid.Visibility = Visibility.Collapsed;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            decryptingGrid.Visibility = Visibility.Collapsed;
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
        }
        
        #region stenografie window, props and actions

        /// <summary>
        /// Button to get out of the stenografie-window and go to the homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void backSteg_Btn_Click(object sender, RoutedEventArgs e)
        {
            CryptoProgramWin.Height = 500;
            CryptoProgramWin.Width = 500;
            steganografieGrid.Visibility = Visibility.Collapsed;
            homePageGrid.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Button to open the stenografie-options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stega_Menu_Selected(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Selecting the image that will be used for the stenography
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseImageButton_Click(object sender, RoutedEventArgs e)
        {
            browseVenster.Filter = "Image Files (*.jpeg; *.png; *.bmp)| *.jpg; *.png; *.bmp";
            if (browseVenster.ShowDialog() == true)
            {
                string padFoto = browseVenster.FileName;
                //FileNameLabel.Content = selectedFileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(padFoto);
                bitmap.EndInit();
                selectedImage.Source = bitmap;
                labelSelectedImage.Content = padFoto;
            }
        }
        //bron :
        // http://www.c-sharpcorner.com/UploadFile/mahesh/using-xaml-image-in-wpf/

        /// <summary>
        /// Selecting the file that will be used for the stenography
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string filesteg; 
        private void browsFileButton_Click(object sender, RoutedEventArgs e)
        {

            browseVenster.Filter = "Txt Documents|*.txt";
            if (browseVenster.ShowDialog() == true)
            {
                filesteg = browseVenster.FileName;
            }
        }
        
        /// <summary>
        /// Button for hiding the data in the picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            string _text = richTextBox.Text;

            string padFoto = labelSelectedImage.Content.ToString();
            Bitmap bitmap = new Bitmap(padFoto);

            if (_text.Equals(""))
            {
                System.Windows.MessageBox.Show("The text you want to hide can't be empty", "Warning");
                return;
            }

            if (encrypedCheckBox.IsChecked == true)
            {
                if (passwordTextBox.Text.Length < 6)
                {
                    System.Windows.MessageBox.Show("Please enter a password with at least 6 characters", "Warning");
                    return;
                }
                else
                {
                    _text = StenografieCrypto.EncryptStringAES(_text, passwordTextBox.Text);
                }
            }

            bitmap = StenografieHelper.embedText(_text, bitmap);

            System.Windows.MessageBox.Show("Your text was hidden in the image successfully!", "Done");
            maakLeeg();

            Microsoft.Win32.SaveFileDialog save_dialog = new Microsoft.Win32.SaveFileDialog();
            save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            if (save_dialog.ShowDialog() == true)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bitmap.Save(save_dialog.FileName, ImageFormat.Png);
                        }
                        break;
                    case 1:
                        {
                            bitmap.Save(save_dialog.FileName, ImageFormat.Bmp);
                        }
                        break;
                }


            }
        }
        
        private void discoverButton_Click(object sender, RoutedEventArgs e)
        {
            string padFoto = browseVenster.FileName;
            Bitmap bitmap = new Bitmap(padFoto);

            string extractedText = StenografieHelper.extractText(bitmap);

            if (encrypedCheckBox.IsChecked == true)
            {
                try
                {
                    extractedText = StenografieCrypto.DecryptStringAES(extractedText, passwordTextBox.Text);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Wrong password", "Error");

                    return;
                }
            }
            richTextBox.Text = extractedText;
        }

        /// <summary>
        /// Maar de invoervelden leeg
        /// </summary>
        private void maakLeeg()
        {
            richTextBox.Text = "";
            passwordTextBox.Text = "";
            encrypedCheckBox.IsChecked = false;
        }
        #endregion

        #region settings window

        /// <summary>
        /// Button to open the settings-window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settings_Menu_Selected(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Button to edit the path to save the rsakeys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rsaKeys_ChangeBtn_Click(object sender, RoutedEventArgs e)
        { 
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            browseFolder.ShowDialog();
            hoofdPad = browseFolder.SelectedPath + "\\";
            rsaKeys_lbl.Content = hoofdPad;
        }


        /// <summary>
        /// Button to edit the backgroundcolor of the entire program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedEventArgs e)
        {      
            System.Windows.Media.Brush brush = new SolidColorBrush(ClrPcker_Background.SelectedColor.Value);
            SideMenu.Background = brush;
        }

        #endregion

        #region help window

        /// <summary>
        /// Button to open the help-menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void help_Menu_Selected(object sender, RoutedEventArgs e)
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
        #endregion
       
        #region exit program
        /// <summary>
        /// Button to exit the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_Menu_Selected(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        #endregion
    }
}
/* Bronnen:  https://github.com/alicanerdogan/HamburgerMenu */
