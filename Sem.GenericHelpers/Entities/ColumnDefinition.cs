// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDefinition.cs"  company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ColumnDefinition type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using ExpressionSerialization;

    /// <summary>
    /// The column definition class describes the column of an export. The properties will
    /// be used to select the property of the exported object that will be stored inside the 
    /// column and the title text of the column
    /// </summary>
    [XmlInclude(typeof(TableLink))]
    public class ColumnDefinition
    {
        /// <summary>
        /// provides serialization capability for Expressions (e.g. Lambda)
        /// </summary>
        private readonly ExpressionSerializer serializer = new ExpressionSerializer();

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
        /// Gets or sets the title text of the column or field name in a database.
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
        /// When using both directions (read and write) <see cref="TransformationFromDatabase"/> 
        /// and <see cref="TransformationToDatabase"/> might be a better choice, because that way
        /// you can define both ways - but its a lot more complex to write in XML.
        /// </summary>
        [XmlAttribute]
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is the primary key.
        /// </summary>
        [XmlAttribute, DefaultValue(false)]
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is an automatically generated
        /// value. If this property is "true", there will no INSERT or UPDATE statement be created 
        /// in case of database access.
        /// </summary>
        [XmlAttribute, DefaultValue(false)]
        public bool IsAutoValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column should be used as a lookup column.
        /// If "true" the values in this column will be used to search for the entity inside the
        /// table. You should only set this value to "true" is a match in this column really means
        /// that both rows (source and target) are the same entity.
        /// </summary>
        [XmlAttribute, DefaultValue(false)]
        public bool IsLookupValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NULL should be inserted into the column for
        /// database operations if the value is equal to the default value of the datatype.
        /// </summary>
        [XmlAttribute, DefaultValue(false)]
        public bool NullIfDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is a numeric one.
        /// </summary>
        [XmlAttribute, DefaultValue(false)]
        public bool IsNumericValue { get; set; }

        /// <summary>
        /// Gets or sets the transformation expression to transform from entity to destination.
        /// </summary>
        [XmlIgnore]
        public Expression<Func<ColumnDefinition, object, string>> TransformationToDatabase { get; set; }

        /// <summary>
        /// Gets or sets the transformation expression to transform from source to entity.
        /// </summary>
        [XmlIgnore]
        public Expression<Func<ColumnDefinition, object, object>> TransformationFromDatabase { get; set; }

        /// <summary>
        /// Gets or sets the serialized transformation expression to transform from entity to destination.
        /// This property is for serialization purpose only.
        /// </summary>
        public XElement TransformationToDatabaseSyntax
        {
            get
            {
                return 
                    this.TransformationToDatabase == null 
                    ? null : 
                    this.serializer.Serialize(this.TransformationToDatabase);
            }

            set
            {
                if (value != null)
                {
                    this.TransformationToDatabase = this.serializer.Deserialize<Func<ColumnDefinition, object, string>>(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the serialized transformation expression to transform from source to entity.
        /// This property is for serialization purpose only.
        /// </summary>
        public XElement TransformationFromDatabaseSyntax
        {
            get
            {
                return 
                    this.TransformationFromDatabase == null 
                    ? null : 
                    this.serializer.Serialize(this.TransformationFromDatabase);
            }

            set
            {
                if (value != null)
                {
                    this.TransformationFromDatabase = this.serializer.Deserialize<Func<ColumnDefinition, object, object>>(value);
                }
            }
        }
    }
}