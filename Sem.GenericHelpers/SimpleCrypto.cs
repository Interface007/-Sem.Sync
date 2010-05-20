// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleCrypto.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Simple encryption/decryption class.
//   <remarks>
//   This is an extremely simple implementation for which I cannot make any security statements.
//   It's using RSA - but that does NOT make something secure. As long you are not a security
//   professional and know all implications of this class: please consider this class as
//   insecure!
//   </remarks>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using System;
    using System.Collections;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Simple encryption/decryption class.
    ///   <remarks>
    /// This is an extremely simple implementation for which I cannot make any security statements. 
    ///     It's using RSA - but that does NOT make something secure. As long you are not a security 
    ///     professional and know all implications of this class: please consider this class as
    ///     insecure!
    ///   </remarks>
    /// </summary>
    public static class SimpleCrypto
    {
        #region Public Methods

        /// <summary>
        /// Decrypts a base64 encoded string.
        ///   <remarks>
        /// This is an extremely simple implementation for which I cannot make any security statements. 
        ///     It's using RSA - but that does NOT make something secure. As long you are not a security 
        ///     professional and know all implications of this class: please consider this class as
        ///     insecure!
        ///   </remarks>
        /// </summary>
        /// <param name="inputValue">
        /// The input string to be decrypted. 
        /// </param>
        /// <param name="keyAsXml">
        /// The key to decrypt the input serialized as xml including the private portion. 
        /// </param>
        /// <returns>
        /// The decrypted string 
        /// </returns>
        public static string DecryptString(string inputValue, string keyAsXml)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(keyAsXml);

            var keySizeInBit = rsaCryptoServiceProvider.KeySize;
            var base64BlockSize = ((keySizeInBit / 8) % 3 != 0)
                                      ? (((keySizeInBit / 8) / 3) * 4) + 4
                                      : ((keySizeInBit / 8) / 3) * 4;
            var iterations = inputValue.Length / base64BlockSize;
            var arrayList = new ArrayList();

            for (var i = 0; i < iterations; i++)
            {
                var encryptedBytes = Convert.FromBase64String(
                    inputValue.Substring(base64BlockSize * i, base64BlockSize));
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
            }

            return Encoding.UTF32.GetString((byte[])arrayList.ToArray(typeof(byte)));
        }

        /// <summary>
        /// Encrypts a string and encodes it to base64.
        ///   <remarks>
        /// This is an extremely simple implementation for which I cannot make any security statements. 
        ///     It's using RSA - but that does NOT make something secure. As long you are not a security 
        ///     professional and know all implications of this class: please consider this class as
        ///     insecure!
        ///   </remarks>
        /// </summary>
        /// <param name="inputValue">
        /// The input string to be encrypted. 
        /// </param>
        /// <param name="keyAsXml">
        /// The encryption key as xml. 
        /// </param>
        /// <returns>
        /// The encrypted and base64 encoded value 
        /// </returns>
        public static string EncryptString(string inputValue, string keyAsXml)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(keyAsXml);

            var keySizeInBit = rsaCryptoServiceProvider.KeySize;
            var keySize = keySizeInBit / 8;
            var bytes = Encoding.UTF32.GetBytes(inputValue);

            var blockLength = keySize - 42;
            var dataLength = bytes.Length;
            var iterations = dataLength / blockLength;
            var stringBuilder = new StringBuilder();

            for (var i = 0; i <= iterations; i++)
            {
                var tempBytes =
                    new byte[
                        ((dataLength - (blockLength * i)) > blockLength)
                            ? blockLength
                            : (dataLength - (blockLength * i))];

                Buffer.BlockCopy(bytes, blockLength * i, tempBytes, 0, tempBytes.Length);
                var encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);

                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Extracts the public portion of a public private key pair.
        ///   <remarks>
        /// This is an extremely simple implementation for which I cannot make any security statements. 
        ///     It's using RSA - but that does NOT make something secure. As long you are not a security 
        ///     professional and know all implications of this class: please consider this class as
        ///     insecure!
        ///   </remarks>
        /// </summary>
        /// <param name="keyAsXml">
        /// The encryption key as xml. 
        /// </param>
        /// <returns>
        /// the public key portion
        /// </returns>
        public static string ExtractPublic(string keyAsXml)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider();
            rsaCryptoServiceProvider.FromXmlString(keyAsXml);
            return rsaCryptoServiceProvider.ToXmlString(false);
        }

        /// <summary>
        /// Generates a new key pair and exports it.
        ///   <remarks>
        /// This is an extremely simple implementation for which I cannot make any security statements. 
        ///     It's using RSA - but that does NOT make something secure. As long you are not a security 
        ///     professional and know all implications of this class: please consider this class as
        ///     insecure!
        ///   </remarks>
        /// </summary>
        /// <param name="keySize">
        /// The key size to be generated. 
        /// </param>
        /// <returns>
        /// The xml serialized key. 
        /// </returns>
        public static string GenerateNewKey(int keySize)
        {
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);
            return rsaCryptoServiceProvider.ToXmlString(true);
        }

        #endregion
    }
}