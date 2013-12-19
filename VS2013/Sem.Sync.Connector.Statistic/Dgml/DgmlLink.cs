// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlLink.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DgmlLink type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic.Dgml
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a link between entities.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// Needed for serialization.
        /// </summary>
        public Link()
        {
            this.Category = "Contains";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="source"> The source entity id. </param>
        /// <param name="target"> The target entity id. </param>
        public Link(Guid source, Guid target)
        {
            this.Source = source.ToString("N"); 
            this.Target = target.ToString("N"); 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Link"/> class.
        /// </summary>
        /// <param name="source"> The source entity id.  </param>
        /// <param name="category"> The link category. </param>
        /// <param name="target"> The target entity id.  </param>
        public Link(string source, string category, string target)
        {
            this.Source = source;
            this.Category = category;
            this.Target = target; 
        }

        public Link(Guid source, Guid target, string category)
            : this(source, target)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets or sets the source entities ID of the link.
        /// </summary>
        [XmlAttribute]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the target entities ID of the link.
        /// </summary>
        [XmlAttribute]
        public string Target { get; set; }

        /// <summary>
        /// Gets or sets the category of the link.
        /// </summary>
        [XmlAttribute]
        public string Category { get; set; }
    }
}
