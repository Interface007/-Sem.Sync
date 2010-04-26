// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiDispatcher.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the UiDispatcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SharedUI.WinForms.Tools
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Windows.Forms;

    using GenericHelpers.Entities;

    using Sem.GenericHelpers;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Interfaces;
    using SyncBase.Merging;

    using UI;

    /// <summary>
    /// The UiDispatcher is a central class to be called from "worker"-classes that need UI interaction
    /// like a logon screen or want to present any kind of UI element. This class then instanciates the
    /// concrete UI element and serves as a kind of "call-router".
    /// </summary>
    public class UiDispatcher : IUiSyncInteraction
    {
        /// <summary>
        /// Displays a modal dialog to let the user decide what attribute values should be saved.
        /// </summary>
        /// <param name="toMerge"> The list of element that should be merged prepared in a specialized list. </param>
        /// <param name="targetList"> The target element list that should later be saved. </param>
        /// <returns> The target element list that should be saved with the updated information. </returns>
        public List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            var ui = new MergeEntities();
            return ui.PerformMerge(toMerge, targetList);
        }

        /// <summary>
        /// Displays a modal dialog to let the user match entities.
        /// </summary>
        /// <param name="sourceList"> The list of "unknown" elements to merge that will provide the profile id.  </param>
        /// <param name="targetList"> The list of "target" elements that should contribute the synchronization id.  </param>
        /// <param name="baselineList"> The baseline list that will be updated to connect a synchronization id to profile ids.  </param>
        /// <param name="identifierToUse"> The identifier to use for matching the entities. </param>
        /// <returns> The baseline list that has been updated to connect a synchronization id to profile ids.  </returns>
        public List<StdElement> PerformEntityMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList, ProfileIdentifierType identifierToUse)
        {
            var ui = new MatchEntities();
            return ui.PerformMatch(sourceList, targetList, baselineList, identifierToUse);
        }

        /// <summary>
        /// requests the logon credential request for an online resource
        /// </summary>
        /// <param name="request">an object containing all information to request the credentiols from the user and pass them back to the callee</param>
        /// <returns>true if the user did click the ok button</returns>
        public bool AskForLogOnCredentials(LogonCredentialRequest request)
        {
            return new LogOn().SetLogonCredentials(request);
        }

        /// <summary>
        /// Asks the user for confirming an action
        /// </summary>
        /// <param name="messageForUser">the message presented to the user</param>
        /// <param name="title">the title of the message presented to the user</param>
        /// <returns>true if the user agrees</returns>
        public bool AskForConfirm(string messageForUser, string title)
        {
            return MessageBox.Show(messageForUser, title, MessageBoxButtons.OKCancel) == DialogResult.OK;
        }

        /// <summary>
        /// Asks the user to resolve a captcha request on a web site. 
        /// TODO: enhance the method to be able to pass back the information
        /// </summary>
        /// <param name="messageForUser"> a message that should be displayed to the user </param>
        /// <param name="title"> the title of the message box </param>
        /// <param name="request"> The information collected while resolving the captcha. </param>
        /// <returns> a <see cref="CaptchaResolveResult"/> instance with information of the web site </returns>
        public CaptchaResolveResult ResolveCaptcha(string messageForUser, string title, CaptchaResolveRequest request)
        {
            if (request.HttpHelper != null)
            {
                return new CaptchaResolve().Resolve(messageForUser, title, request);
            }
            Process.Start(new ProcessStartInfo(request.UrlOfWebSite));
            return new CaptchaResolveResult { UserReportsSuccess = this.AskForConfirm(messageForUser, title) };
        }

        /// <summary>
        /// Asks the user if it's ok to send this information to www.svenerikmatzen.info
        /// </summary>
        /// <param name="content">The content that will be sent</param>
        /// <returns>true if it's ok to send this information</returns>
        public bool AskForConfirmSendingException(string content)
        {
            return new ExceptionOkToSend().AskForOk(content);
        }
    }
}
