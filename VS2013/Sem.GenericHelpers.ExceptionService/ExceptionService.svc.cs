// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionService.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the <see cref="IExceptionService" /> interface to log the exception information into the file system.
//   The path is read from the config file and the file name is composed from server side only information, so there's no way
//   to place a file in a faulty directory, not to place a file with an artificial extension.
//   <para>
//   Even if you implement a key with 2048 bit or more for the encryption: don't consider this approach as
//   "secure" - you have the service, the service provides the key, but you don't have something that proves that I
//   did issue the key. Security is more than encryption - so because even the encryption is open source in this
//   project, you need to double check the configuration and be sure that there is no man in the middle who knows how
//   to fake my exception server ;-)
//   </para>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.ExceptionService
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel.Activation;

    /// <summary>
    /// Implements the <see cref="IExceptionService"/> interface to log the exception information into the file system.
    ///   The path is read from the config file and the file name is composed from server side only information, so there's no way
    ///   to place a file in a faulty directory, not to place a file with an artificial extension.
    ///   <para>
    /// Even if you implement a key with 2048 bit or more for the encryption: don't consider this approach as 
    ///     "secure" - you have the service, the service provides the key, but you don't have something that proves that I
    ///     did issue the key. Security is more than encryption - so because even the encryption is open source in this 
    ///     project, you need to double check the configuration and be sure that there is no man in the middle who knows how 
    ///     to fake my exception server ;-)
    /// </para>
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExceptionService : IExceptionService
    {
        #region Constants and Fields

        /// <summary>
        ///   Reads the path of the folder once.
        /// </summary>
        private static readonly string DestinationFolder =
            ConfigurationManager.AppSettings["ExceptionDestinationFolder"];

        #endregion

        #region Implemented Interfaces

        #region IExceptionService

        /// <summary>
        /// Gets a public key to encrypt the exception data. Using message level encryption with
        ///   public key, we ensure privacy even in environments with trusted proxies breaking ssl.
        /// </summary>
        /// <returns>
        /// The public key portion of the (currently) RSA key.
        /// </returns>
        public string GetEncryptionKey()
        {
            var key = File.ReadAllText(Path.Combine(DestinationFolder, "PublicKey.xml"));
            return key;
        }

        /// <summary>
        /// Log the submitted data into the file system.
        /// </summary>
        /// <param name="exceptionData">
        /// The exception data. 
        /// </param>
        /// <returns>
        /// true if the data has been logged successfully 
        /// </returns>
        public bool WriteExceptionData(string exceptionData)
        {
            // don't accept more than 40kbytes per message
            if (exceptionData.Length > 40980)
            {
                return false;
            }

            // don't accept more than 1 message per minute = 56 MByte per day maximum
            var fileNamePattern = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd-HH-mm}*.*", DateTime.Now);
            if (Directory.GetFiles(DestinationFolder, fileNamePattern, SearchOption.AllDirectories).Length > 0)
            {
                return false;
            }

            // don't accept more than 100 files (who should handle it?)
            if (Directory.GetFiles(DestinationFolder).Length > 100)
            {
                return false;
            }

            // the file name does contain a fresh GUID, so it is unique.
            var fileName = string.Format(
                CultureInfo.InvariantCulture, 
                "{0:yyyy-MM-dd-HH-mm-ss}-{1}.xml", 
                DateTime.Now, 
                Guid.NewGuid().ToString("N"));
            File.WriteAllText(Path.Combine(DestinationFolder, fileName), exceptionData);

            return true;
        }

        #endregion

        #endregion
    }
}