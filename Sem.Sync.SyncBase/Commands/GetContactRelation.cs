// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CopyAll.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Helpers;

    using Interfaces;
    using Sem.GenericHelpers;

    /// <summary>
    /// </summary>
    public class GetContactRelation : SyncComponent, ISyncCommand
    {
        /// <summary>
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

            var extendedClient = new Factory().GetNewObject(commandParameter) as IExtendedReader;
            
            if (extendedClient == null)
            {
                throw new InvalidOperationException("extendedClient is null or not an IExtendedReader");
            }

            // get the baseline 
            var baseline = baseliClient.GetAll(baselineStorePath);

            // get all source elements
            var elements = sourceClient.GetAll(sourceStorePath);

            // add the matching profile ids from the baseline as StdContact - 
            // .ToContacts().ToStdElement() will convert each MatchingEntry 
            // of the list into a StdContact
            elements.MergeHighEvidence(baseline.ToContacts().ToStdElement());

            ((StdClient)extendedClient).UiDispatcher = this.UiProvider;
            
            // fill the extended contact information
            elements.ForEach(e => extendedClient.FillContacts(e, baseline.ToMatchingEntries()));

            // copy to the target connector
            targetClient.AddRange(elements, targetStorePath);
            baseliClient.WriteRange(baseline, baselineStorePath);

            return true;
        }
    }
}
