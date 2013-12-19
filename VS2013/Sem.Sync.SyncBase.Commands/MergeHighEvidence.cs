// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeHighEvidence.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Merge properties of entities from source to target if there is a very high propability that the
//   source property is "better" than the target property - "better" is accepted if:
//   - the target is NULL
//   - the source datetime is between 1901 and 2200, but the target is not
//   - the target string is empty, but the source is not
//   - the target int is 0, but the source is not
//   - the target Gender is Unspecified
//   - the target CountryCode is unspecified
//   - the target byte[] is of length = 0
//   for complex types each property will be merged individually
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Merge properties of entities from source to target if there is a very high propability that the
    ///   source property is "better" than the target property - "better" is accepted if:
    ///   - the target is NULL
    ///   - the source datetime is between 1901 and 2200, but the target is not
    ///   - the target string is empty, but the source is not
    ///   - the target int is 0, but the source is not
    ///   - the target Gender is Unspecified
    ///   - the target CountryCode is unspecified
    ///   - the target byte[] is of length = 0
    ///   for complex types each property will be merged individually
    /// </summary>
    public class MergeHighEvidence : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Merge properties of entities from source to target if there is a very high propability that the
        ///   source property is "better" than the target property
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

            targetClient.WriteRange(
                targetClient.GetAll(targetStorePath).MergeHighEvidence(sourceClient.GetAll(sourceStorePath)), 
                targetStorePath);
            return true;
        }

        #endregion

        #endregion
    }
}