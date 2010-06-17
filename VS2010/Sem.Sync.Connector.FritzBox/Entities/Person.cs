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

    /// <summary>
    /// Implements a serialization class for a person
    /// </summary>
    [XmlType(TypeName = "person")]
    public class Person
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        [XmlElement(ElementName = "realName")]
        public string RealName { get; set; }

        /// <summary>
        /// Gets or sets a URL to an image.
        /// </summary>
        [XmlElement(ElementName = "ImageURL")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Returns the name of the person.
        /// </summary>
        /// <returns>The property <see cref="RealName"/> </returns>
        public override string ToString()
        {
            return this.RealName;
        }
    }
}
