// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveAction.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The save action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook
{
    /// <summary>
    /// The save action to be done.
    /// </summary>
    internal enum SaveAction
    {
        /// <summary>
        /// Nothing to do.
        /// </summary>
        None, 

        /// <summary>
        /// Update of existing entity needed.
        /// </summary>
        Update, 

        /// <summary>
        /// Creation of new entity needed.
        /// </summary>
        Create
    }
}