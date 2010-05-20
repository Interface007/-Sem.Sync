// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteElements.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This command deletes files specified by one or more path pattern separated by a line break
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// This command deletes files specified by one or more path pattern separated by a line break
    /// </summary>
    public class DeleteElements : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// This command deletes files specified by one or more path pattern separated by a line break.
        ///   Deletes files from a folder using a search pattern. Use "*" as a place holder for any 
        ///   number of any chars; use "?" as a placeholder for a single char.
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
            if (string.IsNullOrEmpty(targetStorePath))
            {
                throw new InvalidOperationException("targetStorePath is null");
            }

            if (sourceClient != null && string.IsNullOrEmpty(sourceStorePath))
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (sourceClient != null)
            {
                targetClient.DeleteElements(sourceClient.GetAll(sourceStorePath), targetStorePath);
            }
            else
            {
                targetClient.DeleteElements(null, targetStorePath);
            }

            this.LogProcessingEvent("elements deleted from storage path {0}", targetStorePath);
            return true;
        }

        #endregion

        #endregion
    }
}