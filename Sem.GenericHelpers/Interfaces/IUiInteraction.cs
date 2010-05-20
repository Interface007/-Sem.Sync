// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUiInteraction.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interface for objects that are able to route UI access to the correct objects
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Interfaces
{
    using Sem.GenericHelpers.Entities;

    /// <summary>
    /// Interface for objects that are able to route UI access to the correct objects
    /// </summary>
    public interface IUiInteraction
    {
        #region Public Methods

        /// <summary>
        /// Requests a confirmation from the user
        /// </summary>
        /// <param name="messageForUser">
        /// a message that should be displayed to the user
        /// </param>
        /// <param name="title">
        /// the title of the message box
        /// </param>
        /// <returns>
        /// a value indicating whether the user did click the "ok" button
        /// </returns>
        bool AskForConfirm(string messageForUser, string title);

        /// <summary>
        /// Asks the user if it's ok to send this information to www.svenerikmatzen.info
        /// </summary>
        /// <param name="content">
        /// The content that will be sent
        /// </param>
        /// <returns>
        /// true if it's ok to send this information
        /// </returns>
        bool AskForConfirmSendingException(string content);

        /// <summary>
        /// Requests log on credentials from the user and insert them into an object implementing 
        ///   the ICredentialAware interface.
        /// </summary>
        /// <param name="request">
        /// an object containing all information to request the credentiols from the user and pass them back to the callee
        /// </param>
        /// <returns>
        /// a value indicating whether the user did click the cancel button
        /// </returns>
        bool AskForLogOnCredentials(LogonCredentialRequest request);

        /// <summary>
        /// Asks the user to resolve a captcha request on a web site. 
        ///   TODO: enhance the method to be able to pass back the information
        /// </summary>
        /// <param name="messageForUser">
        /// a message that should be displayed to the user 
        /// </param>
        /// <param name="title">
        /// the title of the message box 
        /// </param>
        /// <param name="request">
        /// The information collected while resolving the captcha. 
        /// </param>
        /// <returns>
        /// a <see cref="CaptchaResolveResult"/> instance with information of the web site 
        /// </returns>
        CaptchaResolveResult ResolveCaptcha(string messageForUser, string title, CaptchaResolveRequest request);

        #endregion
    }
}