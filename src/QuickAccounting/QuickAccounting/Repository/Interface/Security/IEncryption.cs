namespace QuickAccounting.Repository.Interface.Security
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data using AES encryption.
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Encrypts a plain text string using AES encryption.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <returns>The encrypted text as a Base64 string.</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts an AES encrypted Base64 string.
        /// </summary>
        /// <param name="cipherText">The encrypted text as a Base64 string.</param>
        /// <returns>The decrypted plain text.</returns>
        string Decrypt(string cipherText);
    }
}
