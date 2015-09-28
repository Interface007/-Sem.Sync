// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Services.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Services type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    /// <summary>
    /// Describes services availbale for this contact
    /// </summary>
    [XmlType(TypeName = "services")]
    public class Services
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [XmlElement(ElementName = "email")]
        public string Email { get; set; }
    }
}