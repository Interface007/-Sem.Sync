// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientStoragePathDescriptionAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Specifies the type of path information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Attributes
{
    using System;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Stores information about the ClientStoragePath property of the <see cref="StdClient"/> 
    ///   methods <see cref="StdClient.WriteFullList"/> and <see cref="StdClient.ReadFullList"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ClientStoragePathDescriptionAttribute : Attribute
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a default value for the parameter - this can include token information that is 
        ///   processed by the calling method.
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the property is irrelevant for the functionality of the client.
        /// </summary>
        public bool Irrelevant { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the property is mandatory for the functionality of the client.
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating the type of information being stored in the path
        /// </summary>
        public ClientPathType ReferenceType { get; set; }

        /// <summary>
        ///   Gets or sets a class implementing the <see cref="IConfigurable"/>
        /// </summary>
        public Type WinformsConfigurationClass { get; set; }

        #endregion
    }
}