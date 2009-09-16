﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUiInteraction.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the IUiInteraction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Interfaces
{
    using Entities;

    /// <summary>
    /// Interface for objects that are able to route UI access to the correct objects
    /// </summary>
    public interface IUiInteraction
    {
        /// <summary>
        /// Requests log on credentials from the user and insert them into an object implementing 
        /// the ICredentialAware interface.
        /// </summary>
        /// <param name="client">the object that should get the credentials</param>
        /// <param name="messageForUser">a message that should be displayed to the user</param>
        /// <param name="logOnUserId">a pre-selection for the user part of the credentails</param>
        /// <param name="logOnPassword">a pre-selection for the password part of the credentails</param>
        /// <returns>a value indicating whether the user did click the cancel button</returns>
        bool AskForLogOnCredentials(ICredentialAware client, string messageForUser, string logOnUserId, string logOnPassword);

        /// <summary>
        /// Requests a confirmation from the user
        /// </summary>
        /// <param name="messageForUser">a message that should be displayed to the user</param>
        /// <param name="title">the title of the message box</param>
        /// <returns>a value indicating whether the user did click the "ok" button</returns>
        bool AskForConfirm(string messageForUser, string title);

        /// <summary>
        /// Asks the user to resolve a captcha request on a web site. 
        /// TODO: enhance the method to be able to pass back the information
        /// </summary>
        /// <param name="messageForUser"> a message that should be displayed to the user </param>
        /// <param name="title"> the title of the message box </param>
        /// <param name="request"> The information collected while resolving the captcha. </param>
        /// <returns> a <see cref="CaptchaResolveResult"/> instance with information of the web site </returns>
        CaptchaResolveResult ResolveCaptcha(string messageForUser, string title, CaptchaResolveRequest request);
    }
}