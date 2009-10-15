// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchManually.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MatchManually type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Attributes;
    using DetailData;
    using Interfaces;

    /// <summary>
    /// Opens the matching window and matches using a baseline client
    /// </summary>
    public class MatchManually : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// Opens the matching window and matches using a baseline client
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue" </returns>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
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

            var sourceTypeAttributes = sourceClient.GetType().GetCustomAttributes(typeof(ConnectorDescriptionAttribute), false);
            var identifierToUse = (!string.IsNullOrEmpty(commandParameter))
                                      ? (ProfileIdentifierType)
                                        Enum.Parse(typeof(ProfileIdentifierType), commandParameter, true)
                                      : (sourceTypeAttributes != null && sourceTypeAttributes.Length > 0)
                                            ? ((ConnectorDescriptionAttribute)sourceTypeAttributes[0]).
                                                  MatchingIdentifier
                                            : ProfileIdentifierType.Default;

            var targetMatchList = targetClient.GetAll(targetStorePath);
            var matchResultList =
                ((IUiSyncInteraction)this.UiProvider).PerformEntityMerge(
                    sourceClient.GetAll(sourceStorePath),
                    targetMatchList,
                    baseliClient.GetAll(baselineStorePath),
                    identifierToUse);

            // only write to target if we did get a merge result
            if (targetMatchList != null)
            {
                targetClient.WriteRange(targetMatchList, targetStorePath);
            }

            // only write to target if we did get a merge result
            if (matchResultList != null)
            {
                baseliClient.WriteRange(matchResultList, baselineStorePath);
            }

            return true;
        }
    }
}
