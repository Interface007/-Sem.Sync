// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Contact type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Implements the contact serialization class of a fritz box contact entry
    /// </summary>
    [XmlType(TypeName = "contact")]
    public class Contact
    {
        /// <summary>
        /// Gets or sets the category for this person (normal or vip).
        /// </summary>
        [XmlElement(ElementName = "category")]
        public PersonCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        [XmlElement(ElementName = "person")]
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the phone entries of this contact.
        /// </summary>
        [XmlArray(ElementName = "telephony")]
        [XmlArrayItem(ElementName = "number")]
        public List<PhoneNumber> Telephony { get; set; }

        /// <summary>
        /// Gets or sets the service entry (unknow what this will control).
        /// </summary>
        [XmlElement(ElementName = "services")]
        public Services Services { get; set; }

        /// <summary>
        /// Gets or sets the setup entry (unknow what this will control).
        /// </summary>
        [XmlElement(ElementName = "setup")]
        public Setup Setup { get; set; }
    }
}
