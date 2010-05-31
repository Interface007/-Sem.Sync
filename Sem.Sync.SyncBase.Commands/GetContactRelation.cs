// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetContactRelation.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Command class that takes source, target and base clients as well as an <see cref="IExtendedReader" />
//   in the command parameter to read contact relations from an external source. See the <see cref="ExecuteCommand" />
//   method for more information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Command class that takes source, target and base clients as well as an <see cref="IExtendedReader"/>
    ///   in the command parameter to read contact relations from an external source. See the <see cref="ExecuteCommand"/>
    ///   method for more information.
    /// </summary>
    public class GetContactRelation : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// This command does lookup contact ralations for a List of <see cref="StdContact"/>. The connector that does read the 
        ///   data need to be specified inside the <paramref name="commandParameter"/> and must implement the interface <see cref="IExtendedReader"/>.
        ///   Only contacts that can be mapped to internal Ids using the <paramref name="baseliClient"/> will be collected - no additional
        ///   data is collected (there's no profile lookup for the unknown contacts).
        /// </summary>
        /// <param name="sourceClient"> The source client provides a set of <see cref="StdContact"/> entries for that the contact relations should be read. </param>
        /// <param name="targetClient"> The target client will write the processed list of <see cref="StdContact"/> that now does contain the contact relations. </param>
        /// <param name="baseliClient"> The baseline client provides lookup data for determine valid translations for the relation ids read from the connector specified in the <paramref name="commandParameter"/>. </param>
        /// <param name="sourceStorePath"> The source storage path. </param>
        /// <param name="targetStorePath"> The target storage path. </param>
        /// <param name="baselineStorePath"> The baseline storage path. </param>
        /// <param name="commandParameter"> The connector to the contact relations (must support <see cref="IExtendedReader"/>). </param>
        /// <returns> True if the response from the <see cref="SyncComponent.UiProvider"/> is "continue"  </returns>
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

            if (baseliClient == null)
            {
                throw new InvalidOperationException("item.baseliClient is null");
            }

            if (string.IsNullOrEmpty(commandParameter))
            {
                throw new InvalidOperationException("commandParameter is null or empty");
            }

            var extendedClient = new Factory().GetNewObject(commandParameter) as IExtendedReader;
            if (extendedClient == null)
            {
                throw new InvalidOperationException("extendedClient is null or not an IExtendedReader");
            }

            ((SyncComponent)extendedClient).ProcessingEvent += this.LogProcessingEvent;

            // get the baseline 
            var baseline = baseliClient.GetAll(baselineStorePath);

            // get all source elements
            var elements = sourceClient.GetAll(sourceStorePath);

            // add the matching profile ids from the baseline as StdContact - 
            // .ToContacts().ToStdElement() will convert each MatchingEntry 
            // of the list into a StdContact
            elements.MergeHighEvidence(baseline.ToStdContacts().ToStdElements());

            ((StdClient)extendedClient).UiDispatcher = this.UiProvider;

            // fill the extended contact information
            var matchEntities = baseline.ToMatchingEntries();
            elements.ForEach(e => extendedClient.FillContacts(e, matchEntities));

            // copy to the target connector
            targetClient.AddRange(elements, targetStorePath);
            baseliClient.WriteRange(baseline, baselineStorePath);

            ((SyncComponent)extendedClient).ProcessingEvent -= this.LogProcessingEvent;

            return true;
        }

        #endregion

        #endregion
    }
}