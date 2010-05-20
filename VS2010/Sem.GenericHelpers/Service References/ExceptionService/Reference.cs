// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reference.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The i exception service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.ExceptionService
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    /// <summary>
    /// The i exception service.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(
        Namespace = "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService", 
        ConfigurationName = "ExceptionService.IExceptionService")]
    public interface IExceptionService
    {
        #region Public Methods

        /// <summary>
        /// The get encryption key.
        /// </summary>
        /// <returns>
        /// The get encryption key.
        /// </returns>
        [System.ServiceModel.OperationContractAttribute(
            Action =
                "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService/IExceptionServ" +
                "ice/GetEncryptionKey", 
            ReplyAction =
                "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService/IExceptionServ" +
                "ice/GetEncryptionKeyResponse")]
        string GetEncryptionKey();

        /// <summary>
        /// The write exception data.
        /// </summary>
        /// <param name="exceptionData">
        /// The exception data.
        /// </param>
        /// <returns>
        /// The write exception data.
        /// </returns>
        [System.ServiceModel.OperationContractAttribute(
            Action =
                "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService/IExceptionServ" +
                "ice/WriteExceptionData", 
            ReplyAction =
                "http://www.svenerikmatzen.info/Sem.GenericHelpers.ExceptionService/IExceptionServ" +
                "ice/WriteExceptionDataResponse")]
        bool WriteExceptionData(string exceptionData);

        #endregion
    }

    /// <summary>
    /// The i exception service channel.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExceptionServiceChannel : Sem.GenericHelpers.ExceptionService.IExceptionService, 
                                                System.ServiceModel.IClientChannel
    {
    }

    /// <summary>
    /// The exception service client.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public class ExceptionServiceClient :
        System.ServiceModel.ClientBase<IExceptionService>, 
        Sem.GenericHelpers.ExceptionService.IExceptionService
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionServiceClient"/> class.
        /// </summary>
        public ExceptionServiceClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        public ExceptionServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ExceptionServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionServiceClient"/> class.
        /// </summary>
        /// <param name="endpointConfigurationName">
        /// The endpoint configuration name.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ExceptionServiceClient(
            string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionServiceClient"/> class.
        /// </summary>
        /// <param name="binding">
        /// The binding.
        /// </param>
        /// <param name="remoteAddress">
        /// The remote address.
        /// </param>
        public ExceptionServiceClient(
            Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        #endregion

        #region Implemented Interfaces

        #region IExceptionService

        /// <summary>
        /// The get encryption key.
        /// </summary>
        /// <returns>
        /// The get encryption key.
        /// </returns>
        public string GetEncryptionKey()
        {
            return base.Channel.GetEncryptionKey();
        }

        /// <summary>
        /// The write exception data.
        /// </summary>
        /// <param name="exceptionData">
        /// The exception data.
        /// </param>
        /// <returns>
        /// The write exception data.
        /// </returns>
        public bool WriteExceptionData(string exceptionData)
        {
            return base.Channel.WriteExceptionData(exceptionData);
        }

        #endregion

        #endregion
    }
}