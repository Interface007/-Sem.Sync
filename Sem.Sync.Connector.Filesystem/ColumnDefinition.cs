// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs"  company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ColumnDefinition type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Filesystem
{
    using System.Xml.Serialization;

    /// <summary>
    /// The column definition class describes the column of an export. The properties will
    /// be used to select the property of the exported object that will be stored inside the 
    /// column and the title text of the column
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        public ColumnDefinition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        /// <param name="selector">The path of the property.</param>
        public ColumnDefinition(string selector)
        {
            this.Title = selector;
            this.Selector = selector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDefinition"/> class.
        /// </summary>
        /// <param name="selector">The path of the property.</param>
        /// <param name="formatString">The formatting for the values</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = "This definitely is a string")]
        public ColumnDefinition(string selector, string formatString)
            : this(selector)
        {
            this.FormatString = formatString;
        }

        /// <summary>
        /// Gets or sets the title text of the column.
        /// </summary>
        [XmlAttribute]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the path to the property to be stored.
        /// </summary>
        [XmlAttribute]
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the formatting string for the values of this column.
        /// </summary>
        [XmlAttribute]
        public string FormatString { get; set; }
    }
}