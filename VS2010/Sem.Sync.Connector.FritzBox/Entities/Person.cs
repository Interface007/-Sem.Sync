// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Person type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.FritzBox.Entities
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "person")]
    public class Person
    {
        [XmlAttribute(AttributeName = "realName")]
        public string RealName { get; set; }

        [XmlAttribute(AttributeName = "ImageURL")]
        public string ImageUrl { get; set; }
    }
}
