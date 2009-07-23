// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AskForContinue.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the AskForContinue type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Commands
{
    using GenericHelpers.Interfaces;

    using Interfaces;

    /// <summary>
    /// Sync command that uses the <see cref="UiProvider"/> to ask the user if the process should continue.
    /// </summary>
    public class AskForContinue : ISyncCommand
    {
        /// <summary>
        /// Gets or sets the UiProvider.
        /// </summary>
        public IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// This command that uses the <see cref="UiProvider"/> to ask the user if the process should continue.
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> True if the response from the <see cref="UiProvider"/> is "continue" </returns>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            return this.UiProvider == null
                || this.UiProvider.AskForConfirm(commandParameter, (targetClient == null) ? "Sem.Sync" : targetClient.FriendlyClientName);
        }
    }
}
