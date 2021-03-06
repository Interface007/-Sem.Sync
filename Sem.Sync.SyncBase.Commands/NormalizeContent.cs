﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NormalizeContent.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Normalizes content by using lookup tables and removes unneeded whitespace
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Normalizes content by using lookup tables and removes unneeded whitespace
    /// </summary>
    public class NormalizeContent : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Normalizes content by using lookup tables and removes unneeded whitespace
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

            var elements = sourceClient.GetAll(sourceStorePath);

            foreach (var element in elements)
            {
                this.LogProcessingEvent(element, "normalizing ... ");
                element.NormalizeContent();
            }

            targetClient.WriteRange(elements, targetStorePath);
            return true;
        }

        #endregion

        #endregion
    }
}