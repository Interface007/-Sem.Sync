// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenDocument.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Performs a shell execute to open the document specified as the command parameter
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System.Diagnostics;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Performs a shell execute to open the document specified as the command parameter
    /// </summary>
    public class OpenDocument : SyncComponent, ISyncCommand
    {
        #region Implemented Interfaces

        #region ISyncCommand

        /// <summary>
        /// Performs a shell execute to open the document specified as the command parameter
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
            if (!string.IsNullOrEmpty(commandParameter))
            {
                this.LogProcessingEvent("starting process: " + commandParameter);
                Process.Start(new ProcessStartInfo(commandParameter));
            }

            return true;
        }

        #endregion

        #endregion
    }
}