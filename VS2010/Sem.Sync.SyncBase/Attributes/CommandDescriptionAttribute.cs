// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandDescriptionAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Stores information about an implementation of ISyncCommand <see cref="ISyncCommand" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Attributes
{
    using System;

    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Stores information about an implementation of ISyncCommand <see cref="ISyncCommand"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandDescriptionAttribute : Attribute
    {
        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the command is for debugging purpose only.
        ///   Debugging commands perform unusual actions of connectors that do or may do harm to the underlying data.
        ///   An example for a debugging command is UpdateTestData, which will delete all data of the 
        ///   storage behind the client and insert new updated data.
        ///   Debugging commands will display a warning.
        /// </summary>
        public bool DebugOnly { get; set; }

        #endregion
    }
}