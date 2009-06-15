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
    using System.Windows.Forms;

    using SyncBase;
    using SyncBase.Interfaces;
    using SyncBase.Merging;

    using UI;

    /// <summary>
    /// The UiDispatcher is a central class to be called from "worker"-classes that need UI interaction
    /// like a login screen or want to present any kind of UI element. This class then instanciates the
    /// concrete UI element and serves as a kind of "call-router".
    /// </summary>
    public class UiDispatcher : IUiInteraction
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
        /// <param name="toMerge"> The list of "unknown" elements to merge that will provide the profile id. </param>
        /// <param name="targetList"> The list of "target" elements that should contribute the synchronization id. </param>
        /// <param name="baselineList"> The baseline list that will be updated to connect a synchronization id to profile ids. </param>
        /// <returns> The baseline list that has been updated to connect a synchronization id to profile ids. </returns>
        public List<StdElement> PerformEntityMerge(List<StdElement> toMerge, List<StdElement> targetList, List<StdElement> baselineList)
        {
            var ui = new MatchEntities();
            return ui.PerformMerge(toMerge, targetList, baselineList);
        }

        /// <summary>
        /// requests the logon credential request for an online resource
        /// </summary>
        /// <param name="source">the object that asks for the credentials</param>
        /// <param name="messageForUser">the message the user will see</param>
        /// <param name="logOnUserId">the preset user id if applicable</param>
        /// <param name="logOnPassword">the preset user password if applicable</param>
        /// <returns>true if the user did click the ok button</returns>
        public bool AskForLogOnCredentials(ICredentialAware source, string messageForUser, string logOnUserId, string logOnPassword)
        {
            return new LogOn().SetLoginCredentials(source, messageForUser, logOnUserId, logOnPassword);
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
    }
}
