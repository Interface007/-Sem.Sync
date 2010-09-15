// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyAll.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This command copies all data from the source connector to the target connector
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// This command copies all data from the source connector to the target connector
    /// </summary>
    public class CopyAll : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// Copy all entries from the source client to the destination client;
        ///   Overwrite existing entries
        /// </summary>
        /// <param name="sourceClient"> The source client instance that is the source of data for the copy operation. </param>
        /// <param name="targetClient"> The source client instance that is the target for the data of the copy operation. </param>
        /// <param name="baseliClient"> The baseline client is not utilized in this command. </param>
        /// <param name="sourceStorePath"> The storage path for the source connector. </param>
        /// <param name="targetStorePath"> The storage path for the target connector </param>
        /// <param name="baselineStorePath"> The baseline client (and so the storage path) is not utilized in this command. </param>
        /// <param name="commandParameter"> In this command there is no need for a parameter. </param>
        /// <returns> Always true. </returns>
        public bool ExecuteCommand(
            IClientBase sourceClient, 
            IClientBase targetClient, 
            IClientBase baseliClient, 
            string sourceStorePath, 
            string targetStorePath, 
            string baselineStorePath, 
            string commandParameter)
        {
            Bouncer.ForCheckData(() => sourceClient).Assert(new IsNotNullRule<IClientBase>());
            Bouncer.ForCheckData(() => targetClient).Assert(new IsNotNullRule<IClientBase>());

            targetClient.AddRange(sourceClient.GetAll(sourceStorePath), targetStorePath);

            return true;
        }
    }
}