//-----------------------------------------------------------------------
// <copyright file="ISyncCommand.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Interfaces
{
    using GenericHelpers.Interfaces;

    /// <summary>
    /// Base interface for commands of the sync library.
    /// </summary>
    public interface ISyncCommand
    {
        /// <summary>
        /// Gets or sets a class implementing the <see cref="IUiInteraction"/> interface to provide
        /// user driven input.
        /// </summary>
        IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// Execution invocation of the command.
        /// </summary>
        /// <param name="sourceClient">the source connector - in most cases this will be the connector that does provide data</param>
        /// <param name="targetClient">the target connector - in most cases this will be the connector that does receive data</param>
        /// <param name="baselineClient">the baseline connector - in most cases this will be the connector that does provide reference data</param>
        /// <param name="sourceStorePath">the logical path inside the source store</param>
        /// <param name="targetStorePath">the logical path inside the target store</param>
        /// <param name="baselineStorePath">the logical path inside the baseline store</param>
        /// <param name="commandParameter">additional commands for the execution of the command</param>
        /// <returns>true if the execution should continue, false if the execution should stop</returns>
        bool ExecuteCommand(
            IClientBase sourceClient,
            IClientBase targetClient,
            IClientBase baselineClient,
            string sourceStorePath,
            string targetStorePath,
            string baselineStorePath,
            string commandParameter);
    }
}