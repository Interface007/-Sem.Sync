//-----------------------------------------------------------------------
// <copyright file="SyncData.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    /// <summary>
    /// This class describes entity independent synchronization properties.
    /// </summary>
    [Serializable]
    public class SyncData
    {
        /// <summary>
        /// Gets or sets thet date time property of the last change of a property if implemented by the source.
        /// </summary>
        public DateTime DateOfLastChange { get; set; }

        /// <summary>
        /// Gets or sets thet date time property of the creation of this entity if implemented by the source.
        /// </summary>
        public DateTime DateOfCreation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is tagged for deletion. If this property is set to true,
        /// the item should be deleted in native stores.
        /// </summary>
        public bool Deleted { get; set; }
    }
}