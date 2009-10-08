// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectorDescriptionAttribute.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Specifies information about the connectors capabilities
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Attributes
{
    using System;

    using DetailData;

    /// <summary>
    /// Specifies information about the connectors capabilities
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConnectorDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorDescriptionAttribute"/> class.
        /// </summary>
        public ConnectorDescriptionAttribute()
        {
            this.CanReadContacts = true;
            this.CanWriteContacts = true;
            this.NeedsCredentials = false;
            this.IsGeneric = false;
            this.MatchingIdentifier = ProfileIdentifierType.Default;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can read data.
        /// </summary>
        public bool CanReadContacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can write data.
        /// </summary>
        public bool CanWriteContacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector needs credentials.
        /// </summary>
        public bool NeedsCredentials { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector should be hidden from the user interface.
        /// </summary>
        public bool Internal { get; set; }

        /// <summary>
        /// Gets or sets the display name for gui implementations.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector does support 
        /// multiple types by a generic type parameter.
        /// </summary>
        public bool IsGeneric { get; set; }

        /// <summary>
        /// Gets or sets the display name for gui implementations.
        /// </summary>
        public ProfileIdentifierType MatchingIdentifier { get; set; }
    }
}
