using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Security.Cryptography;
using System.IO;

namespace CryptoProgramma
{
    public class DES
    {
        public static void EncryptFile(string source, string destination, string sKey)
        {
            // declaratie
            FileStream fsInput = new FileStream(source, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(destination, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 1];

            // verwijzingen
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            
            // acties
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);

            // afsluiting
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }
        
        //function to generate a 64 bit key
        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);

        }
        
        public static void DecryptFile(string source, string destination, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            DES.Mode = CipherMode.CFB;
            DES.Padding = PaddingMode.ISO10126;

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(source, FileMode.Open, FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            //Print the contents of the decrypted file.

           string text = (new StreamReader(cryptostreamDecr).ReadToEnd());
            File.WriteAllText(destination + "DecryptedTxt" +
                         System.IO.Path.GetFileNameWithoutExtension(source) + ".txt",text );


            //FileStream fsDecrypted = new FileStream(destination, FileMode.Create, FileAccess.Write);
            //cryptostreamDecr.CopyTo(fsDecrypted);
            //fsDecrypted.Flush();
            //fsDecrypted.Close();

            //StreamWriter fsDecrypted = new StreamWriter(destination);
            //fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            //fsDecrypted.Flush();
            //fsDecrypted.Close();

        }
    }
}
