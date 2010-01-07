// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeInfo.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ChangeInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Information that describes the changes that have been detected for a contact.
    /// </summary>
    internal class ChangeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeInfo"/> class.
        /// </summary>
        public ChangeInfo()
        {
            this.ChangedProperties = new List<string>();
        }

        /// <summary>
        /// Gets or sets a string to be displayed to identify the changed contact.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the name of the target system where the change has been detected.
        /// </summary>
        public string TargetSystemName { get; set; }

        /// <summary>
        /// Gets or sets a list of properties that have been changed.
        /// </summary>
        public IList<string> ChangedProperties { get; set; }

        /// <summary>
        /// Gets or sets the image of this contact.
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Defines a binding list to perform data binding
        /// </summary>
        internal class BindingList : BindingList<ChangeInfo>
        {
        }
    }
}