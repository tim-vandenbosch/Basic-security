using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoProgramma
{
    class RSA
    {
        /* Auteur : Daniela Lupo */
        #region properties
        private static string _private_A;
        private static string _public_A;
        private static string _private_B;
        private static string _public_B;
        public static string public_Receiver, private_Receiver;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();
        #endregion

        public static string[] keys(string hoofdPad, string nameS, string nameR) //string pad, 
        {

            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().

            String public_Pad = hoofdPad + "Keys\\Public_Keys\\"; //PublicKey_" + nameS + ".txt";
            String private_Pad = hoofdPad + "Keys\\Private_Keys\\"; // PrivateKey_" + nameS + ".txt";

            Directory.CreateDirectory(public_Pad);
            Directory.CreateDirectory(private_Pad);

             string[] returnwaarde = new string[8];
            returnwaarde[0] = "PublicKey_" + nameS + ".txt";
            returnwaarde[1] = "PrivateKey_" + nameS + ".txt";

            returnwaarde[2] = "PublicKey_" + nameR + ".txt";
            returnwaarde[3] = "PrivateKey_" + nameR + ".txt";

            RSACryptoServiceProvider rsa1 = new RSACryptoServiceProvider();

            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();

            // Check that the file doesn't already exist. If it doesn't exist, create 
            // the file and write integers 0 - 99 to it. 
            // DANGER: System.IO.File.Create will overwrite the file if it already exists. 
            // This could happen even with random file names, although it is unlikely. 
            returnwaarde[4] = private_Pad + returnwaarde[1];
            //if (!System.IO.File.Exists(returnwaarde[4]))
            //{
                _private_A = rsa1.ToXmlString(true);
                File.WriteAllText(@returnwaarde[4], _private_A);
            //}

            returnwaarde[5] = public_Pad + returnwaarde[0];
            //if (!System.IO.File.Exists(returnwaarde[5]))
            //{
                _public_A = rsa1.ToXmlString(false);
                File.WriteAllText(@returnwaarde[5], _public_A);
            //}

            returnwaarde[6] = private_Pad + returnwaarde[3];
            //if (!System.IO.File.Exists(returnwaarde[6]))
            //{
                _private_B = rsa2.ToXmlString(true);
                File.WriteAllText(@returnwaarde[6], _private_B);
            //}

            returnwaarde[7] = public_Pad + returnwaarde[2];
            //if (!System.IO.File.Exists(returnwaarde[7]))
            //{
                _public_B = rsa2.ToXmlString(false);
                File.WriteAllText(@returnwaarde[7], _public_B);
            //}

            public_Receiver = returnwaarde[7];
            private_Receiver = returnwaarde[6];
            return returnwaarde;
        }
        
        public static string Encrypt(string data, string key)
        {
            var rsa = new RSACryptoServiceProvider();

            string publicB = File.ReadAllText(key);

            rsa.FromXmlString(publicB); //encrypteren met de publickey receiver (publicB)
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

        public static string Decrypt(string data, string k)
        {
            byte [] decryptedByte = new byte[500];
            var rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            byte[] dataByte = new byte[dataArray.Length];

            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

           string key = File.ReadAllText(k);

            rsa.FromXmlString(key); //decryteren met de privatekey receiver (privateB)
            if (System.IO.Path.GetFileNameWithoutExtension(k).StartsWith("PrivateKey_"))
            {
                decryptedByte = rsa.Decrypt(dataByte, false);


            }
            else if (System.IO.Path.GetFileNameWithoutExtension(k).StartsWith("PublicKey_"))
            {
                decryptedByte = rsa.Decrypt(dataByte, true);

            }
            return _encoder.GetString(decryptedByte);

        }
   

        public static string SignData(string message, string privateKey)
        {
            //// The array to store the signed message in bytes
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                //// Write the message to a byte array using UTF8 as the encoding.
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(message);

                try
                {
                    //// Import the private key used for signing the message
                    rsa.FromXmlString(File.ReadAllText(privateKey));

                    //// Sign the data, using SHA512 as the hashing algorithm 
                    signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    //// Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            //// Convert the a base64 string before returning
            return Convert.ToBase64String(signedBytes);
        }


        public static bool VerifyData(string originalMessage, string signedMessage, string publicKey)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] bytesToVerify = encoder.GetBytes(originalMessage);

                byte[] signedBytes = Convert.FromBase64String(signedMessage);
                try
                {
                    rsa.FromXmlString(File.ReadAllText(publicKey));

                    SHA512Managed Hash = new SHA512Managed();

                    byte[] hashedData = Hash.ComputeHash(signedBytes);
                    
                    success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return success;
        }



    }
}
#region Bronnen die gebruikt zijn voor deze klasse
/*  
*   http://stackoverflow.com/questions/18485715/how-to-use-public-and-private-key-encryption-technique-in-c-sharp 
*   https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
*   https://msdn.microsoft.com/en-us/library/ezwyzy7b.aspx
*   https://msdn.microsoft.com/en-us/library/as2f1fez%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396    
// Bron : Sing en verify 
 //http://stackoverflow.com/questions/8437288/signing-and-verifying-signatures-with-rsa-c-sharp
 https://blogs.msdn.microsoft.com/nicold/2007/09/03/how-to-digitally-sign-a-string/

*/
#endregion

/* onderstaande moet ik nog toevoegen aan de MainWindows.xaml.cs
lijn 143 - 154 (ENCRYPTEREN)
 string md5sum = hash(filetext, "MD5");
                // Console.WriteLine(md5sum);
                //lander

                //********Daniela begin ********************

                //public en private keys gemaakt en gesaved !!!! DEZE MOET DAAR WEG
                 
            opgeslagenBestanden = RSA.keys(hoofdPad, senderTxt.Text, receiverTxt.Text);

            //hash geencrypteert en opgeslaan
            //string encryptHash = RSA.Encrypt(md5sum, opgeslagenBestanden[4]); 
            string encryptHash = RSA.SignData(md5sum, opgeslagenBestanden[4]);//singen met private key sender

     by decryptbutton
      private void decryptButton_Click(object sender, RoutedEventArgs e)
        {

            decryptingGrid.Visibility = Visibility.Visible;
            decryptFileGrid.Visibility = Visibility.Collapsed;
            if (fileLbl.Content.Equals("") || symkeyLbl.Content.Equals("") || hashLbl.Content.Equals("") || publicLbl.Content.Equals("") || privateLbl.Content.Equals(""))
            {
                System.Windows.MessageBox.Show("Je mist een bestand, controleer of je alles heb gekozen");
            }
            else
            {
                //stap 1 : symetric decrypteren met privesleutel
                string inhouddecryptSemKey = File.ReadAllText(decryptSemKey);
                string ontcijferdeSemKey = "";
                ontcijferdeSemKey = RSA.Decrypt(inhouddecryptSemKey, privReceiv);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles\\" + "DecryptedSkey" +
                    System.IO.Path.GetFileNameWithoutExtension(decryptFile) + ".txt", ontcijferdeSemKey);

                //stap 2:  bestand decrypteren met semetric key
                //bij het decrypteren komt ook een error idk why

                //Nasim - Nu komt geen error :p 
                string destination = hoofdPad + "DecryptedFiles\\" + "DecryptedTxt" +
                                   System.IO.Path.GetFileNameWithoutExtension(decryptFile) + ".txt";

                if (decryptSemKey.Contains("DES"))
                {
                    des.DecryptFile(decryptFile, destination, ontcijferdeSemKey);
                    Process.Start(destination);
                }
                else if (decryptSemKey.Contains("AES"))
                {

                }
              
                //stap 3 : hash berekenen boodschap
                string berekendeNieuweHAsh = hash(File.ReadAllText(destination), "MD5");

                //Onderstaande code zou moeten werken maar geeft een error 
                //stap 4 & 5: hash verify met publiekesleutel, zelfberekende hash en ontcijferde hash vergelijken 
                string inhouddecryptHash = File.ReadAllText(decryptHash);
                //string ontcijferdeHash = RSA.Decrypt(inhouddecryptHash, pubSender);
                bool ontcijferdeHash = RSA.VerifyData(berekendeNieuweHAsh, inhouddecryptHash, pubSender);
                string resultaatHash="";
                if (ontcijferdeHash)
                {
                    resultaatHash = "De gesignde hash, en de zelf berekende hash komen overeen";
                }else if (!ontcijferdeHash)
                {
                    resultaatHash = "De gesignde hash, en de zelf berekende hash komen NIET overeen";
                }
                System.Windows.MessageBox.Show(resultaatHash);
                Directory.CreateDirectory(hoofdPad + "\\DecryptedFiles");
                File.WriteAllText(hoofdPad + "\\DecryptedFiles\\" + "ResultaatHash" +
              System.IO.Path.GetFileNameWithoutExtension(decryptHash) + ".txt", resultaatHash);
            }
        }


*/
