// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchManually.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Opens the matching window and matches using a baseline client
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Linq;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Opens the matching window and matches using a baseline client
    /// </summary>
    public class MatchManually : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Opens the matching window and matches using a baseline client
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
            CheckParameters(
                targetClient, sourceClient, baseliClient, baselineStorePath, sourceStorePath, targetStorePath);

            var backupConnector = targetClient as IBackupStorage;
            if (backupConnector != null)
            {
                backupConnector.BackupStorage(targetStorePath);
            }

            // todo: split the command parameter in oder to specify whether to add orphaned source entries or skip them.
            var sourceTypeAttributes = sourceClient.GetType().GetCustomAttributes(
                typeof(ConnectorDescriptionAttribute), false);
            var identifierToUse = (!string.IsNullOrEmpty(commandParameter))
                                      ? (ProfileIdentifierType)
                                        Enum.Parse(typeof(ProfileIdentifierType), commandParameter, true)
                                      : (sourceTypeAttributes != null && sourceTypeAttributes.Length > 0)
                                            ? ((ConnectorDescriptionAttribute)sourceTypeAttributes[0]).
                                                  MatchingIdentifier
                                            : ProfileIdentifierType.Default;

            var targetMatchList = targetClient.GetAll(targetStorePath);
            var sourceMatchList = sourceClient.GetAll(sourceStorePath);
            var matchResultList = ((IUiSyncInteraction)this.UiProvider).PerformEntityMerge(
                sourceMatchList, targetMatchList, baseliClient.GetAll(baselineStorePath), identifierToUse);

            // only write to target if we did get a merge result
            if (targetMatchList != null && matchResultList != null)
            {
                targetClient.WriteRange(targetMatchList, targetStorePath);
            }

            // only write to target if we did get a merge result
            if (matchResultList != null)
            {
                var sourceContactList = sourceMatchList.ToStdContacts();
                var matchingEntryList = matchResultList.ToMatchingEntries();

                // Check for new (not matched) contacts and generate new matching entries for such new entries.
                var orphanSource = from x in sourceContactList
                                   join matchEntry in matchingEntryList on x.ExternalIdentifier equals
                                       matchEntry.ProfileId into matchGroup
                                   from y in matchGroup.DefaultIfEmpty()
                                   where
                                       y == null &&
                                       !string.IsNullOrEmpty(x.ExternalIdentifier.GetProfileId(identifierToUse))
                                   select new MatchingEntry { Id = x.Id, ProfileId = x.ExternalIdentifier };

                // add all new contacts matching entries to the base line
                orphanSource.ForEach(matchResultList.Add);

                // write baseline to base line connector
                baseliClient.WriteRange(matchResultList, baselineStorePath);
            }

            return true;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Performs a check for null parameters.
        /// </summary>
        /// <param name="targetClient">
        /// The target client.
        /// </param>
        /// <param name="sourceClient">
        /// The source client.
        /// </param>
        /// <param name="baseliClient">
        /// The baseline client.
        /// </param>
        /// <param name="baselineStorePath">
        /// The baseline storage path.
        /// </param>
        /// <param name="sourceStorePath">
        /// The source storage path.
        /// </param>
        /// <param name="targetStorePath">
        /// The target storage path.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static void CheckParameters(
            IClientBase targetClient, 
            IClientBase sourceClient, 
            IClientBase baseliClient, 
            string baselineStorePath, 
            string sourceStorePath, 
            string targetStorePath)
        {
            if (targetClient == null)
            {
                throw new InvalidOperationException("item.targetClient is null");
            }

            if (sourceClient == null)
            {
                throw new InvalidOperationException("item.sourceClient is null");
            }

            if (baseliClient == null)
            {
                throw new InvalidOperationException("item.baseliClient is null");
            }

            if (baselineStorePath == null)
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (sourceStorePath == null)
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (targetStorePath == null)
            {
                throw new InvalidOperationException("targetStorePath is null");
            }
        }

        #endregion
    }
}