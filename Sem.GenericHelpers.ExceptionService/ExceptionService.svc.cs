// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionService.svc.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the <see cref="IExceptionService" /> interface to log the exception information into the file system.
//   The path is read from the config file and the file name is composed from server side only information, so there's no way
//   to place a file in a faulty directory, not to place a file with an artificial extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.ExceptionService
{
    using System;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Implements the <see cref="IExceptionService"/> interface to log the exception information into the file system.
    /// The path is read from the config file and the file name is composed from server side only information, so there's no way
    /// to place a file in a faulty directory, not to place a file with an artificial extension.
    /// </summary>
    public class ExceptionService : IExceptionService
    {
        /// <summary>
        /// Reads the path of the folder once.
        /// </summary>
        private static readonly string DestinationFolder = ConfigurationManager.AppSettings["ExceptionDestinationFolder"];

        /// <summary>
        /// Log the submitted data into the file system.
        /// </summary>
        /// <param name="exceptionData"> The exception data. </param>
        /// <returns> true if the data has been logged successfully </returns>
        public bool WriteExceptionData(string exceptionData)
        {
            // don't accept more than 40kbytes per message
            if (exceptionData.Length > 40980)
            {
                return false;
            }

            // don't accept more than 1 message per minute = 56 MByte per day maximum
            var fileNamePattern = string.Format("{0:yyyy-MM-dd-HH-mm}*.*", DateTime.Now);
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
            var fileName = string.Format("{0:yyyy-MM-dd-HH-mm-ss}-{1}.xml", DateTime.Now, Guid.NewGuid().ToString("N"));
            File.WriteAllText(Path.Combine(DestinationFolder, fileName), exceptionData);

            return true;
        }

        /// <summary>
        /// Gets a public key to encrypt the exception data. Using message level encryption with
        /// public key, we ensure privacy even in environments with trusted proxies breaking ssl.
        /// </summary>
        /// <returns>
        /// The public key portion of the (currently) RSA key.
        /// </returns>
        public string GetEncryptionKey()
        {
            var key = File.ReadAllText(Path.Combine(DestinationFolder, "PublicKey.xml"));
            return key;
        }
    }
}