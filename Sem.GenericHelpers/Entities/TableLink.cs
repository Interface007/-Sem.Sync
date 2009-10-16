namespace Sem.GenericHelpers.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TableLink : ColumnDefinition
    {
        public string TableName { get; set; }
        public List<KeyValuePair> JoinBy { get; set; }
        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
