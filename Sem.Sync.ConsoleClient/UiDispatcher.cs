// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiDispatcher.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Command line implementation of the UI interaction of the library with the user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ConsoleClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Interfaces;
    using Sem.Sync.SyncBase.Merging;
    using Sem.Sync.SyncBase.Properties;

    /// <summary>
    /// Command line implementation of the UI interaction of the library with the user.
    /// </summary>
    internal class UiDispatcher : IUiSyncInteraction
    {
        #region Properties

        /// <summary>
        ///   Gets or sets UserDomain of the logon information for the remote source.
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        ///   Gets or sets UserName of the logon information for the remote source.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///   Gets or sets UserPassword of the logon information for the remote source.
        /// </summary>
        public string UserPassword { get; set; }

        #endregion

        #region Implemented Interfaces

        #region IUiInteraction

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
        public bool AskForConfirm(string messageForUser, string title)
        {
            if (messageForUser == null)
            {
                throw new ArgumentNullException("messageForUser");
            }

            Console.WriteLine(messageForUser);
            Console.WriteLine(Resources.MessageUiDispatcherAskForConfirm);
            return true;
        }

        /// <summary>
        /// Asks the user if it's ok to send this information to www.svenerikmatzen.info
        /// </summary>
        /// <param name="content">
        /// The content that will be sent
        /// </param>
        /// <returns>
        /// true if it's ok to send this information
        /// </returns>
        public bool AskForConfirmSendingException(string content)
        {
            Console.WriteLine(Resources.UiDispatcherAskForConfirmSendingException01);
            Console.WriteLine(content);
            Console.WriteLine(Resources.UiDispatcherAskForConfirmSendingException02);
            return Console.ReadLine() == "Y";
        }

        /// <summary>
        /// Requests log on credentials from the user and insert them into an object implementing 
        ///   the ICredentialAware interface.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <returns>
        /// a value indicating whether the user did click the cancel button
        /// </returns>
        public bool AskForLogOnCredentials(LogonCredentialRequest request)
        {
            Console.WriteLine(request.MessageForUser);

            request.LogOnCredentials.LogOnDomain = GetInfoWithDefault(
                this.UserDomain, Resources.UiDispatcher_AskForLogOnCredentials_Please_enter_the_user_domain, false);
            request.LogOnCredentials.LogOnUserId = GetInfoWithDefault(
                this.UserName, Resources.UiDispatcher_AskForLogOnCredentials_Please_enter_the_user_name, false);
            request.LogOnCredentials.LogOnPassword = GetInfoWithDefault(
                this.UserPassword, Resources.UiDispatcher_AskForLogOnCredentials_Please_enter_the_user_password, true);

            return request.LogOnCredentials.LogOnPassword.Length > 0;
        }

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
        public CaptchaResolveResult ResolveCaptcha(string messageForUser, string title, CaptchaResolveRequest request)
        {
            Process.Start(new ProcessStartInfo(request.UrlOfWebSite));
            return new CaptchaResolveResult { UserReportsSuccess = this.AskForConfirm(messageForUser, title) };
        }

        #endregion

        #region IUiSyncInteraction

        /// <summary>
        /// Requests a merge action for attributes of conflicting entities
        /// </summary>
        /// <param name="toMerge">
        /// the list of merge conflicts to reslove
        /// </param>
        /// <param name="targetList">
        /// the list of elements that should be changed
        /// </param>
        /// <returns>
        /// the unmodified <paramref name="targetList"/>
        /// </returns>
        public List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            Console.WriteLine(Resources.NotImplementedInteractiveAttributeMerge);
            return targetList;
        }

        /// <summary>
        /// Requests an entity merge of elements
        /// </summary>
        /// <param name="sourceList">
        /// the source entity list
        /// </param>
        /// <param name="targetList">
        /// the list that will be changed
        /// </param>
        /// <param name="baselineList">
        /// a baseline list that helps merging
        /// </param>
        /// <param name="identifierToUse">
        /// The identifier To Use.
        /// </param>
        /// <returns>
        /// The unmodified <paramref name="targetList"/>
        /// </returns>
        public List<StdElement> PerformEntityMerge(
            List<StdElement> sourceList, 
            List<StdElement> targetList, 
            List<StdElement> baselineList, 
            ProfileIdentifierType identifierToUse)
        {
            Console.WriteLine(Resources.NotImplementedInteractiveEntityMerge);
            return targetList;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Queries the user for information, uses the default value, if there is one (this suppresses the user interaction).
        /// </summary>
        /// <param name="defaultValue">
        /// The default value of this information - the user will only be asked, if this value is an empty string.  
        /// </param>
        /// <param name="message">
        /// The messagedisplayed to the user.  
        /// </param>
        /// <param name="hideInput">
        /// If true it hides the user input by setting the ForegroundColor to BackgroundColor. 
        /// </param>
        /// <returns>
        /// The value entered by the user or the default value  
        /// </returns>
        private static string GetInfoWithDefault(string defaultValue, string message, bool hideInput)
        {
            if (string.IsNullOrEmpty(defaultValue))
            {
                Console.WriteLine(message);

                var foregroundColor = Console.ForegroundColor;
                if (hideInput)
                {
                    Console.ForegroundColor = Console.BackgroundColor;
                }

                defaultValue = Console.ReadLine();

                if (hideInput)
                {
                    Console.ForegroundColor = foregroundColor;
                }
            }

            return defaultValue;
        }

        #endregion
    }
}