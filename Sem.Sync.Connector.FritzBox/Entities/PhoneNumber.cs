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
    using System.Xml.Serialization;

    [XmlType(TypeName = "number")]
    public class PhoneNumber
    {
        [XmlAttribute(AttributeName = "type")]
        public PhoneNumberType DestinationType { get; set; }

        [XmlAttribute(AttributeName = "quickdial")]
        public int QuickdialNumber { get; set; }

        [XmlAttribute(AttributeName = "prio")]
        public int Priority { get; set; }

        [XmlAttribute(AttributeName = "vanity")]
        public string Vanity { get; set; }

        [XmlText()]
        public string Number { get; set; }
    }
}
