using QuickAccounting.Repository.Interface.Security;
using System.Security.Cryptography;
using System.Text;

namespace QuickAccounting.Repository.Repository.Security
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data using AES encryption.
    /// </summary>
    public class EncryptionService : IEncryption
    {
        #region Constants
        private const int KeySize = 32; // 256 bits
        private const int IvSize = 16; // 128 bits

        #endregion

        #region Fields
        /// <summary>
        /// Encryption key (32 bytes = 256 bits).
        /// </summary>
        private readonly byte[] _key = Encoding.UTF8.GetBytes("H1iJ#2kLM3nOpQr4!V5zW6yX8@9o0Z7g");

        /// <summary>
        /// Initialization vector (16 bytes = 128 bits).
        /// </summary>
        private readonly byte[] _iv = Encoding.UTF8.GetBytes("9xX0zZ1#Yw$2Ab3C");

        #endregion

        #region Public Methods
        /// <summary>
        /// Encrypts the given plaintext using AES encryption.
        /// </summary>
        /// <param name="plainText">The plaintext to encrypt.</param>
        /// <returns>The encrypted text as a Base64-encoded string.</returns>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                throw new ArgumentException("Input text cannot be null or empty.", nameof(plainText));

            // Validate key size
            if (_key.Length != KeySize)
                throw new InvalidOperationException($"The key must be {KeySize} bytes (256 bits) in length.");

            // Validate IV size
            if (_iv.Length != IvSize)
                throw new InvalidOperationException($"The IV must be {IvSize} bytes (128 bits) in length.");

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _key;
                    aesAlg.IV = _iv;

                    using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Encryption operation failed.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during encryption.", ex);
            }
        }

        /// <summary>
        /// Decrypts the given ciphertext using AES decryption.
        /// </summary>
        /// <param name="cipherText">The Base64-encoded encrypted text.</param>
        /// <returns>The decrypted plaintext.</returns>
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentException("Input cipher text cannot be null or empty.", nameof(cipherText));

            // Validate key size
            if (_key.Length != KeySize)
                throw new InvalidOperationException($"The key must be {KeySize} bytes (256 bits) in length.");

            // Validate IV size
            if (_iv.Length != IvSize)
                throw new InvalidOperationException($"The IV must be {IvSize} bytes (128 bits) in length.");

            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = _key;
                    aesAlg.IV = _iv;

                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("The cipher text format is invalid. Ensure it is Base64-encoded.", nameof(cipherText), ex);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidOperationException("Decryption operation failed. The cipher text may be corrupted or the key/IV is incorrect.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred during decryption.", ex);
            }
        }

        #endregion
    }
}
