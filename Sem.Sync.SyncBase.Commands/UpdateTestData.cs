﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdateTestData.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This command copies all data from the source connector to the target connector
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// This command copies all data from the source connector to the target connector
    /// </summary>
    [CommandDescription(DebugOnly = true)]
    public class UpdateTestData : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Updates test data inside a storage.
        ///   Overwrite existing entries
        /// </summary>
        /// <param name="sourceClient">
        /// The source client.
        /// </param>
        /// <param name="targetClient">
        /// The target client.
        /// </param>
        /// <param name="baseliClient">
        /// The baseline client.
        /// </param>
        /// <param name="sourceStorePath">
        /// The source storage path.
        /// </param>
        /// <param name="targetStorePath">
        /// The target storage path.
        /// </param>
        /// <param name="baselineStorePath">
        /// The baseline storage path.
        /// </param>
        /// <param name="commandParameter">
        /// The command parameter.
        /// </param>
        /// <returns>
        /// True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue" 
        /// </returns>
        public bool ExecuteCommand(
            IClientBase sourceClient, 
            IClientBase targetClient, 
            IClientBase baseliClient, 
            string sourceStorePath, 
            string targetStorePath, 
            string baselineStorePath, 
            string commandParameter)
        {
            if (targetClient == null)
            {
                throw new InvalidOperationException("item.targetClient is null");
            }

            if (sourceClient == null)
            {
                throw new InvalidOperationException("item.sourceClient is null");
            }

            targetClient.AddRange(sourceClient.GetAll(sourceStorePath), targetStorePath);

            return true;
        }

        #endregion

        #endregion
    }
}