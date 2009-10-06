namespace Sem.Sync.Connector.MsAccess
{
    public class Mapping
    {
        public string PropertyPath { get; set; }

        public string TableName { get; set; }
        public string FieldName { get; set; }

        public bool IsPrimaryKey { get; set; }
        public bool IsAutoValue { get; set; }

        public bool NullIfDefault { get; set; }
    }
}
