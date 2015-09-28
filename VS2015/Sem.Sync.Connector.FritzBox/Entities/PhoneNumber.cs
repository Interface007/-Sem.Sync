// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhoneNumber.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the PhoneNumber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Serialization class for the phone number entities
    /// </summary>
    [XmlType(TypeName = "number")]
    public class PhoneNumber
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class.
        /// </summary>
        public PhoneNumber()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumber"/> class with a predefined type and number.
        /// </summary>
        /// <param name="phoneNumberType"> The phone number type. </param>
        /// <param name="number"> The number. </param>
        public PhoneNumber(PhoneNumberType phoneNumberType, string number)
        {
            this.DestinationType = phoneNumberType;
            this.Number = number;
        }

        /// <summary>
        /// Gets or sets the phone destination type (mobile, home, work).
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public PhoneNumberType DestinationType { get; set; }

        /// <summary>
        /// Gets or sets the quickdial number.
        /// </summary>
        [XmlAttribute(AttributeName = "quickdial")]
        public string QuickdialNumber { get; set; }

        /// <summary>
        /// Gets or sets the priority to use the number.
        /// </summary>
        [XmlAttribute(AttributeName = "prio")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the vanity dialing words.
        /// </summary>
        [XmlAttribute(AttributeName = "vanity")]
        public string Vanity { get; set; }

        /// <summary>
        /// Gets or sets the number to be dialed.
        /// </summary>
        [XmlText]
        public string Number { get; set; }
    }
}
