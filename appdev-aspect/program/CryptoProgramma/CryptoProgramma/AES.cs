using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace CryptoProgramma
{
    /// <summary>
    /// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and 
    /// decrypt data. As long as encryption and decryption routines use the same
    /// parameters to generate the keys, the keys are guaranteed to be the same.
    /// The class uses static functions with duplicate code to make it easier to
    /// demonstrate encryption and decryption logic. In a real-life application, 
    /// this may not be the most efficient way of handling encryption, so - as
    /// soon as you feel comfortable with it - you may want to redesign this class.
    /// </summary>
    public class AES
    {
        /// <summary>
        /// Encrypt a file with AES
        /// </summary>
        /// <param name="plainFilePath">Full path of the file to be encrypted</param>
        /// <param name="encryptedFilePath">Full path of the encrypted file</param>
        /// <param name="key">AES key</param>
        /// <param name="iv">AES IV</param>
        public static void EncryptFile(string plainFilePath, string encryptedFilePath, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 128;
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (FileStream plain = File.Open(plainFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (FileStream encrypted = File.Open(encryptedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (CryptoStream cs = new CryptoStream(encrypted, encryptor, CryptoStreamMode.Write))
                        {
                            plain.CopyTo(cs);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Create encryption information in the form of xml string
        /// </summary>
        /// <param name="signatureKey">Signature Key</param>
        /// <param name="encryptionKey">AES Encryption key</param>
        /// <param name="encryptionIV">AES Encryption key IV</param>
        /// <returns>xml string containing key informations</returns>
        public static string CreateEncryptionInfoXml(byte[] signatureKey, byte[] encryptionKey, byte[] encryptionIV)
        {
            string template = "<EncryptionInfo>" + "<AESKeyValue>" + "<Key/>" + "<IV/>" + "</AESKeyValue>" + "<HMACSHAKeyValue/>" + "</EncryptionInfo>";
            XDocument doc = XDocument.Parse(template);

            doc.Descendants("AESKeyValue").Single().Descendants("Key").Single().Value = Convert.ToBase64String(encryptionKey);
            doc.Descendants("AESKeyValue").Single().Descendants("IV").Single().Value = Convert.ToBase64String(encryptionIV);
            doc.Descendants("HMACSHAKeyValue").Single().Value = Convert.ToBase64String(signatureKey);

            return doc.ToString();
        }
        
        /// <summary>
        /// Encrypt a file with AES
        /// </summary>
        /// <param name="plainFilePath">Full path of the encrypted file</param>
        /// <param name="encryptedFilePath">Full path of the file to be decrypted</param>
        /// <param name="key">AES key</param>
        /// <param name="iv">AES IV</param>
        public static void DecryptFile(string plainFilePath, string encryptedFilePath, byte[] key, byte[] iv)
        {
            using (AesCryptoServiceProvider _aes = new AesCryptoServiceProvider())
            {
                _aes.KeySize = 128;
                _aes.Key = key;
                _aes.IV = iv;
                ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);
                using (FileStream plain = File.Open(plainFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (FileStream encrypted = File.Open(encryptedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (CryptoStream cs = new CryptoStream(plain, decryptor, CryptoStreamMode.Write))
                        {
                            encrypted.CopyTo(cs);
                        }
                    }
                }
            }
        }
    }
}
