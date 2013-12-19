// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientPathType.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Specifies the type of path information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Attributes
{
    /// <summary>
    /// Specifies the type of path information
    /// </summary>
    public enum ClientPathType
    {
        /// <summary>
        ///   The path information describes the full qualified path to a file inside the file system
        /// </summary>
        Default = 0, 

        /// <summary>
        ///   The path information describes the full qualified path to a file inside the file system
        /// </summary>
        FileSystemFileNameAndPath = 0, 

        /// <summary>
        ///   The path information describes the full qualified path to a directory inside the file system
        /// </summary>
        FileSystemPath = 1, 

        /// <summary>
        ///   The path information describes something that is relevant for the storage, but we don't have an editor for it
        /// </summary>
        Undefined = 2, 

        /// <summary>
        ///   An interface is defined for displaying the configuration dialog - the details are stored in XML
        /// </summary>
        DialogBased = 3,
    }
}