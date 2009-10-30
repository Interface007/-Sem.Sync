namespace Sem.GenericHelpers.Entities
{
    using System.Collections.Generic;

    public class TableLink : ColumnDefinition
    {
        public string TableName { get; set; }
        public List<KeyValuePair> JoinBy { get; set; }
        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
