// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBackupStorage.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interface to detect connectors that can backup a complete storage at once
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Interfaces
{
    /// <summary>
    /// Interface to detect connectors that can backup a complete storage at once
    /// </summary>
    public interface IBackupStorage
    {
        #region Public Methods

        /// <summary>
        /// Perform a full backup of the storage - the connector has to choose a meaningfull name for the backup
        /// </summary>
        /// <param name="clientFolderName">
        /// The client Folder Name.
        /// </param>
        void BackupStorage(string clientFolderName);

        /// <summary>
        /// Perform a full restore of the storage - the connector has to choose the correct source for the restore
        /// </summary>
        /// <param name="clientFolderName">
        /// The client Folder Name.
        /// </param>
        void RestoreStorage(string clientFolderName);

        #endregion
    }
}