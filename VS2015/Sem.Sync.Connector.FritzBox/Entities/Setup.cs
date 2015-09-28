// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setup.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Setup type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    /// <summary>
    /// Describes the contact setup
    /// </summary>
    [XmlType(TypeName = "setup")]
    public class Setup
    {
        /// <summary>
        /// Gets or sets the ringtone to be used in case of an incoming call of this contact.
        /// </summary>
        [XmlElement(ElementName = "ringtone")]
        public string Ringtone { get; set; }
    }
}