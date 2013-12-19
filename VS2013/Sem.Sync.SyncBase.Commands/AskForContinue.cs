// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AskForContinue.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Sync command that uses the <see cref="SyncComponent.UiProvider" /> to ask the user if the process should continue.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.GenericHelpers.Interfaces;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Sync command that uses the <see cref="SyncComponent.UiProvider"/> to ask the user if the process should continue.
    /// </summary>
    public class AskForContinue : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// This command that uses the <see cref="SyncComponent.UiProvider"/> to ask the user if the process should continue.
        /// </summary>
        /// <param name="sourceClient">The source client can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="targetClient">The target client can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="baseliClient">The baseline client can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="sourceStorePath">The source storage path can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="targetStorePath">The target storage path can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="baselineStorePath">The baseline storage path can be NULL - there is no interaction with clients in this command.</param>
        /// <param name="commandParameter">The message to be displayed. Must be a non-zero-length string.</param>
        /// <returns>True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue".</returns>
        public bool ExecuteCommand(
            IClientBase sourceClient, 
            IClientBase targetClient, 
            IClientBase baseliClient, 
            string sourceStorePath, 
            string targetStorePath, 
            string baselineStorePath, 
            string commandParameter)
        {
            ////Bouncer.ForCheckData(() => this.UiProvider).Assert(new IsNotNullRule<IUiInteraction>());
            Bouncer.ForCheckData(() => commandParameter).Assert(new StringMinLengthRule(), 1);

            return this.UiProvider == null ||
                   this.UiProvider.AskForConfirm(
                       commandParameter, (targetClient == null) ? "Sem.Sync" : targetClient.FriendlyClientName);
        }

        #endregion

        #endregion
    }
}