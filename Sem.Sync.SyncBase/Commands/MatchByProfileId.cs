// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchByProfileId.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Match using the profile identifiers - each profile identifier is expected to assigned to a unique entity
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DetailData;

    using Interfaces;

    /// <summary>
    /// Match using the profile identifiers - each profile identifier is expected to assigned to a unique entity
    /// </summary>
    public class MatchByProfileId : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// Match using the profile identifiers - each profile identifier is expected to assigned to a unique entity
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

            if (sourceStorePath == null)
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (targetStorePath == null)
            {
                throw new InvalidOperationException("targetStorePath is null");
            }

            var baseline = baseliClient.GetAll(baselineStorePath);
            
            targetClient.WriteRange(
                MatchThisByProfileId(
                    sourceClient.GetAll(sourceStorePath),
                    baseline),
                targetStorePath);
            return true;
        }

        /// <summary>
        /// Automatically matches without user interaction entities by <see cref="StdContact.PersonalProfileIdentifiers"/>.
        /// </summary>
        /// <param name="target"> the list of <see cref="StdContact"/> that contains the target (here the <see cref="StdElement.Id"/> will be changed if a match is found in the baseline) </param>
        /// <param name="baseline"> the list of <see cref="StdElement"/> that contains the source of the baseline (this will not be changed, but need to contain entries of type <see cref="MatchingEntry"/>) </param>
        /// <returns> the modified list of elements from the <paramref name="target"/> </returns>
        private static List<StdElement> MatchThisByProfileId(List<StdElement> target, IEnumerable<StdElement> baseline)
        {
            foreach (var item in target)
            {
                var localItem = (StdContact)item;
                var corresponding = (from element in baseline
                                     where ((MatchingEntry)element).ProfileId.MatchesAny(localItem.PersonalProfileIdentifiers)
                                     select element).FirstOrDefault();

                // if there is one with a matching profile id, 
                // we overwrite the id
                if (corresponding == null)
                {
                    continue;
                }

                var sourceId = ((MatchingEntry)corresponding).ProfileId;
                var targetId = localItem.PersonalProfileIdentifiers;

                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.ActiveDirectoryId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.FacebookProfileId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.Google);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.LinkedInId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.LotusNotesId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.MeinVZ);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.MicrosoftAccessId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.OracleCrmOnDemandId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.StayFriendsPersonId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.WerKenntWenUrl);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.XingNameProfileId);
                CopyIfDestinationIsNull(sourceId, targetId, ProfileIdentifierType.ExchangeWs);
                
                item.Id = corresponding.Id;
            }

            return target;
        }

        /// <summary>
        /// Sets the profile id if the target property is null.
        /// </summary>
        /// <param name="source">the source profile identifier container</param>
        /// <param name="target">the target profile identifier container</param>
        /// <param name="id">the profile type to copy from source to target</param>
        private static void CopyIfDestinationIsNull(ProfileIdentifiers source, ProfileIdentifiers target, ProfileIdentifierType id)
        {
            if (string.IsNullOrEmpty(target.GetProfileId(id)))
            {
                target.SetProfileId(id, source.GetProfileId(id));
            }
        }
    }
}