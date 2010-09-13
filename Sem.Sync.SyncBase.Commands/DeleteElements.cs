// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteElements.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This command deletes files specified by one or more path pattern separated by a line break
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// This command deletes files specified by one or more path pattern separated by a line break
    /// </summary>
    public class DeleteElements : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// This command deletes files specified by one or more path pattern separated by a line break.
        ///   Deletes files from a folder using a search pattern. Use "*" as a place holder for any 
        ///   number of any chars; use "?" as a placeholder for a single char.
        /// </summary>
        /// <param name="sourceClient">The source client - will delete "all elements" if this parameter is NULL.</param>
        /// <param name="targetClient">The target client - must not be null.</param>
        /// <param name="baseliClient">The baseline client - can be null, because there is no interaction with the baseline in this command.</param>
        /// <param name="sourceStorePath">The source storage path - the elements found in this path will be deleted.</param>
        /// <param name="targetStorePath">The target storage path - the deletion will take place in this path.</param>
        /// <param name="baselineStorePath">The baseline storage path - can be null, because there is no interaction with the baseline in this command.</param>
        /// <param name="commandParameter">The command parameter - can be null, because there is no parameter for this command.</param>
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
            Bouncer.ForCheckData(() => targetClient).Assert(Rules.ObjectNotNullRule<IClientBase>());

            targetClient.DeleteElements(
                sourceClient != null 
                ? sourceClient.GetAll(sourceStorePath) 
                : null, 
                targetStorePath);

            this.LogProcessingEvent("elements deleted from storage path {0}", targetStorePath);
            return true;
        }
    }
}