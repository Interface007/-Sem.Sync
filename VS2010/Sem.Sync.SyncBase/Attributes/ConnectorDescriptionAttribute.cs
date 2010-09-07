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
    using System.Reflection;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Specifies information about the connectors capabilities
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConnectorDescriptionAttribute : Attribute
    {
        #region Constants and Fields

        /// <summary>
        ///   Internal representation of the need to provide a domain for the credentials.
        /// </summary>
        private bool needsCredentialsDomain;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ConnectorDescriptionAttribute" /> class.
        /// </summary>
        public ConnectorDescriptionAttribute()
        {
            this.CanReadContacts = true;
            this.CanWriteContacts = true;
            this.NeedsCredentials = false;
            this.NeedsCredentialsDomain = true;
            this.IsGeneric = false;
            this.ContentIsPrivate = true;
            this.MatchingIdentifier = ProfileIdentifierType.Default;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the connector can read calendar data.
        /// </summary>
        public bool CanReadCalendarEntries { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector can read contact data.
        /// </summary>
        public bool CanReadContacts { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector can write calendar data.
        /// </summary>
        public bool CanWriteCalendarEntries { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector can write contact data.
        /// </summary>
        public bool CanWriteContacts { get; set; }

        /// <summary>
        ///   Gets or sets the display name for gui implementations.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector should be hidden from the user interface.
        /// </summary>
        public bool Internal { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector does support 
        ///   multiple types by a generic type parameter.
        /// </summary>
        public bool IsGeneric { get; set; }

        /// <summary>
        ///   Gets or sets the display name for gui implementations.
        /// </summary>
        public ProfileIdentifierType MatchingIdentifier { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector needs credentials.
        /// </summary>
        public bool NeedsCredentials { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the connector needs credentials.
        ///   <para>Returns only true if the property <see cref = "NeedsCredentials" /> is true AND this property is set to true.</para>
        ///   <para>Default is "true".</para>
        /// </summary>
        public bool NeedsCredentialsDomain
        {
            get
            {
                return this.NeedsCredentials && this.needsCredentialsDomain;
            }

            set
            {
                this.needsCredentialsDomain = value;
            }
        }

        public bool ContentIsPrivate { get; set; }

        public bool ContentIsBusiness { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// returns true if the specified type can be read by this connector
        /// </summary>
        /// <param name="entityType">
        /// the type to be read
        /// </param>
        /// <returns>
        /// true if the connector can read
        /// </returns>
        public bool CanRead(MemberInfo entityType)
        {
            if (entityType == null)
            {
                return false;
            }

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

        /// <summary>
        /// returns true if the specified type can be written by this connector
        /// </summary>
        /// <param name="entityType">
        /// the type to be written
        /// </param>
        /// <returns>
        /// true if the connector can write
        /// </returns>
        public bool CanWrite(MemberInfo entityType)
        {
            if (entityType == null)
            {
                return false;
            } 
            
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

        #endregion
    }
}