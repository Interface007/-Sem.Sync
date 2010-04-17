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
        /// Internal representation of the need to provide a domain for the credentials.
        /// </summary>
        private bool needsCredentialsDomain;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectorDescriptionAttribute"/> class.
        /// </summary>
        public ConnectorDescriptionAttribute()
        {
            this.CanReadContacts = true;
            this.CanWriteContacts = true;
            this.NeedsCredentials = false;
            this.NeedsCredentialsDomain = true;
            this.IsGeneric = false;
            this.MatchingIdentifier = ProfileIdentifierType.Default;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can read contact data.
        /// </summary>
        public bool CanReadContacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can write contact data.
        /// </summary>
        public bool CanWriteContacts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can read calendar data.
        /// </summary>
        public bool CanReadCalendarEntries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector can write calendar data.
        /// </summary>
        public bool CanWriteCalendarEntries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector needs credentials.
        /// </summary>
        public bool NeedsCredentials { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the connector needs credentials.
        /// <para>Returns only true if the property <see cref="NeedsCredentials"/> is true AND this property is set to true.</para>
        /// <para>Default is "true".</para>
        /// </summary>
        public bool NeedsCredentialsDomain
        {
            get
            {
                return this.NeedsCredentials 
                    && this.needsCredentialsDomain;
            }

            set
            {
                this.needsCredentialsDomain = value;
            }
        }

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

        public bool CanRead(Type entityType)
        {
            switch (entityType.Name)
            {
                case "StdContact":
                    return this.CanReadContacts;

                case "StdCalendarItem":
                    return this.CanReadCalendarEntries;

                default:
                    return false;
            }
        }

        public bool CanWrite(Type entityType)
        {
            switch (entityType.Name)
            {
                case "StdContact":
                    return this.CanWriteContacts;

                case "StdCalendarItem":
                    return this.CanWriteCalendarEntries;

                default:
                    return false;
            }
        }
    }
}
