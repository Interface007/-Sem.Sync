namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Table
    {
        internal static readonly Dictionary<Type, string> Mapping = new Dictionary<Type, string>();

        static Table()
        {
            Mapping.Add(typeof(int), "INT");
            Mapping.Add(typeof(long), "BIGINT");
            Mapping.Add(typeof(string), "nvarchar(255)");
            Mapping.Add(typeof(DateTime), "DateTime NULL");
            Mapping.Add(typeof(bool), "BIT");
            Mapping.Add(typeof(Guid), "UNIQUEIDENTIFIER NULL");
            Mapping.Add(typeof(byte[]), "Image NULL");
        }

        public string Name { get; set; }

        public Dictionary<string, Type> Column { get; set; }

        public Dictionary<string, string> References { get; set; }

        public Table()
        {
            this.Column = new Dictionary<string, Type>();
            this.References = new Dictionary<string, string>();
        }

        public string ToScript()
        {
            var thisTable = new StringBuilder();
            var subTables = new StringBuilder();

            thisTable.Append("CREATE TABLE ");
            var sqlObjectName = this.FixSqlObjectName(this.Name);
            thisTable.Append(sqlObjectName);
            thisTable.AppendLine(" (");

            var pkIdColumnName = string.Format("{0}Id", this.Name);
            thisTable.AppendLine(string.Format("[{0}] int NOT NULL IDENTITY (1, 1),", pkIdColumnName));

            foreach (var type in Column)
            {
                thisTable.AppendLine(string.Format("[{0}] {1},", type.Key, Mapping[type.Value]));
            }

            subTables.AppendLine(thisTable.ToString().Substring(0, thisTable.Length - 3) + ");");

            subTables.AppendLine(string.Format("ALTER TABLE {0} ADD CONSTRAINT PK_{0} PRIMARY KEY CLUSTERED ({1}) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);", sqlObjectName, pkIdColumnName));
            foreach (var reference in this.References)
            {
                subTables.AppendLine(
                    string.Format("ALTER TABLE {0} ADD CONSTRAINT FK_{0}_{1} FOREIGN KEY ( {1} ) REFERENCES {2} ( {2}Id )", 
                        this.Name,
                        reference.Key, 
                        reference.Value));
            }

            return subTables.ToString();
        }

        private string FixSqlObjectName(string entityName)
        {
            return entityName
                .Replace(" ", "_")
                .Replace(".", "_");
        }

        public override string ToString()
        {
            return this.Name + " (" + this.Column.Count + " columns)";
        }
    }
}