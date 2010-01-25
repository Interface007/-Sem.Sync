namespace Sem.Sync.Connector.MsAccess
{
    using System;
    using System.Linq.Expressions;
    using System.Xml.Linq;

    using ExpressionSerialization;
    using System.Xml.Serialization;

    public class Mapping
    {
        readonly ExpressionSerializer _serializer = new ExpressionSerializer();
        
        public string PropertyPath { get; set; }

        public string TableName { get; set; }
        public string FieldName { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool IsAutoValue { get; set; }
        public bool IsLookupValue { get; set; }
        
        public bool NullIfDefault { get; set; }

        public bool IsNumericValue { get; set; }
        
        [XmlIgnore]
        public Expression<Func<Mapping, object, string>> TransformationToDatabase { get; set; }

        [XmlIgnore]
        public Expression<Func<Mapping, object, object>> TransformationFromDatabase { get; set; }

        public XElement TransformationToDatabaseSyntax
        {
            get
            {
                if (this.TransformationToDatabase == null)
                {
                    return null;
                }

                XElement addXml = this._serializer.Serialize(this.TransformationToDatabase);
                return addXml;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                XElement addXml = value;
                this.TransformationToDatabase = this._serializer.Deserialize<Func<Mapping, object, string>>(addXml);
            }
        }

        public XElement TransformationFromDatabaseSyntax
        {
            get
            {
                if (this.TransformationFromDatabase == null)
                {
                    return null;
                }

                XElement addXml = this._serializer.Serialize(this.TransformationFromDatabase);
                return addXml;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                XElement addXml = value;
                this.TransformationFromDatabase = this._serializer.Deserialize<Func<Mapping, object, object>>(addXml);
            }
        }
    }
}
