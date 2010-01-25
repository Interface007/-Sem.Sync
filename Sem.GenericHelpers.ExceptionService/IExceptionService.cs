// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExceptionService.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This service accepts data to be logged centrally for the development team.
//   The data is logged in a way that development can access the data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.ExceptionService
{
    using System.ServiceModel;

    /// <summary>
    /// This service accepts data to be logged centrally for the development team.
    /// The data is logged in a way that development can access the data.
    /// </summary>
    [ServiceContract(Namespace = "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService")]
    public interface IExceptionService
    {
        /// <summary>
        /// Logs the submitted exception data.
        /// </summary>
        /// <param name="exceptionData"> The exception data. </param>
        /// <returns> true is the data has been logged successfully, false otherwise. </returns>
        [OperationContract]
        bool WriteExceptionData(string exceptionData);

        /// <summary>
        /// Gets a public key to encrypt the exception data. Using message level encryption with
        /// public key, we ensure privacy even in environments with trusted proxies breaking ssl.
        /// </summary>
        /// <returns>
        /// The public key portion of the (currently) RSA key.
        /// </returns>
        [OperationContract]
        string GetEncryptionKey();
    }
}
