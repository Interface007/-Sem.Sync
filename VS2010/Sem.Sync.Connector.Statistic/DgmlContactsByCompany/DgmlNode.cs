// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlNode.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlNode type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.DgmlContactsByCompany
{
    using System.Xml.Serialization;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// The Node type for DGML. Every entity in DGML is a Node.
    /// </summary>
    public class DgmlNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlNode"/> class.
        /// This ctor is needed for serialization.
        /// </summary>
        public DgmlNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlNode"/> class.
        /// This ctor does create an element node from the ID and the <see cref="StdContact.GetFullName"/> 
        /// or <see cref="StdElement.ToStringSimple"/> methods.
        /// </summary>
        /// <param name="element"> The element to convert. </param>
        public DgmlNode(StdElement element)
        {
            this.Id = element.Id.ToString("N");

            var contact = element as StdContact;
            this.Label =
                contact != null
                ? contact.GetFullName()
                : element.ToStringSimple();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DgmlNode"/> class.
        /// This ctor does create a group with the specified group-type.
        /// </summary>
        /// <param name="id"> The id of this group. </param>
        /// <param name="group"> The group-type (like "Contains"). </param>
        /// <param name="label"> The label to display. </param>
        public DgmlNode(string id, string group, string label)
        {
            this.Id = id;
            this.Label = label;
            this.Group = group;
        }

        [XmlAttribute]
        public string Group { get; set; }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Label { get; set; }

        /// <summary>
        /// for debugging purpose it's really handy to not need to expand the object.
        /// </summary>
        /// <returns> the label and the id </returns>
        public override string ToString()
        {
            return this.Label + " {" + this.Id + "}";
        }
    }
}
