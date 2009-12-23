// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergeView.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the MergeView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase.Merging;

    /// <summary>
    /// Implements a view entity for a merging poperty list.
    /// </summary>
    public class MergeView
    {
        /// <summary>
        /// Gets or sets the name of the contact to be displayed.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets a PropertyName of a contact.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the value of the property read from the source.
        /// </summary>
        public string SourceValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the property read from the target.
        /// </summary>
        public string TargetValue { get; set; }

        /// <summary>
        /// Gets or sets the information about conflict resolution (source/target entity, what action to perform...).
        /// </summary>
        public MergeConflict Conflict { get; set; }
    }
}