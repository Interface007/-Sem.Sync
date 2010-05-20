// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetectConflicts.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Detects merge conflicts and resolves them using user interaction
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Detects merge conflicts and resolves them using user interaction
    /// </summary>
    public class DetectConflicts : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Detects merge conflicts and resolves them using user interaction
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

            if (sourceStorePath == null)
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (targetStorePath == null)
            {
                throw new InvalidOperationException("targetStorePath is null");
            }

            var targetList = targetClient.GetAll(targetStorePath);
            var sourceList = sourceClient.GetAll(sourceStorePath);
            var type = targetList.Count > 0
                           ? targetList[0].GetType()
                           : sourceList.Count > 0 ? sourceList[0].GetType() : typeof(StdElement);

            var mergeResultList =
                ((IUiSyncInteraction)this.UiProvider).PerformAttributeMerge(
                    SyncTools.DetectConflicts(
                        SyncTools.BuildConflictTestContainerList(
                            sourceList, 
                            targetList, 
                            (baseliClient == null) ? null : baseliClient.GetAll(baselineStorePath), 
                            type), 
                        true), 
                    targetList);

            // only write to target if we did get a merge result
            if (mergeResultList != null)
            {
                targetClient.WriteRange(mergeResultList, targetStorePath);
            }

            return true;
        }

        #endregion

        #endregion
    }
}