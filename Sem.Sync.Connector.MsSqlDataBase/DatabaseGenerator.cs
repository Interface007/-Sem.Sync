// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseGenerator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DatabaseGenerator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Sem.Sync.SyncBase.Helpers;

    public class EnumHelper
    {
        public int Value { get; set; }
        public string Description { get; set; }
    }

    public class DatabaseGenerator
    {

        public string ConnectionString { get; set; }


        public void CreateFromEntityType(Type entity, string entityName, List<Table> tables)
        {
            if (tables.Exists(x => x.Name == entityName))
            {
                return;
            }

            var thisTable = new Table { Name = entityName };

            var publicProperties = entity.GetProperties();
            foreach (var publicProperty in publicProperties)
            {
                var columnName = publicProperty.Name;
                var propertyType = publicProperty.PropertyType;

                if (Table.Mapping.ContainsKey(propertyType))
                {
                    thisTable.Column.Add(columnName, propertyType);
                    continue;
                }

                var simpleTypeName = propertyType.Name;
                var baseType = propertyType.BaseType;
                var baseTypeName = simpleTypeName.IsOneOf("List`1", "SerializableDictionary`2")
                                       ? simpleTypeName
                                       : baseType == null
                                             ? "none"
                                             : baseType.Name.IsOneOf("List`1", "SerializableDictionary`2")
                                                   ? baseType.Name
                                                   : baseType.FullName;

                switch (baseTypeName)
                {
                    case "SerializableDictionary`2":
                    case "Dictionary`2":
                    case "List`1":
                        // add a table for the entity
                        this.CreateLinkedTableFromListType(propertyType, entityName, columnName, tables);
                        break;

                    case "System.Enum":
                        this.CreateFromEntityType(typeof(EnumHelper), simpleTypeName, tables);
                        AddReferenceColumn(thisTable, simpleTypeName, tables);
                        break;

                    default:

                        switch (propertyType.FullName)
                        {
                            // this is only to not log the known complex types
                            case "Sem.Sync.SyncBase.DetailData.AddressDetail":
                            case "Sem.Sync.SyncBase.DetailData.PhoneNumber":
                            case "Sem.Sync.SyncBase.DetailData.CountryCode":
                            case "Sem.Sync.SyncBase.DetailData.BusinessHistoryEntry":
                            case "Sem.Sync.SyncBase.DetailData.BusinessCertificate":
                            case "Sem.Sync.SyncBase.DetailData.ContactReference":
                            case "Sem.Sync.SyncBase.DetailData.InstantMessengerAddresses":
                            case "Sem.Sync.SyncBase.DetailData.ImageEntry":
                            case "Sem.Sync.SyncBase.DetailData.LanguageKnowledge":
                            case "Sem.Sync.SyncBase.DetailData.PersonName":
                            case "Sem.Sync.SyncBase.DetailData.SyncData":
                                this.CreateFromEntityType(propertyType, simpleTypeName, tables);
                                thisTable.References.Add(columnName, simpleTypeName);
                                break;

                            default:
                                Console.WriteLine(propertyType.FullName);
                                this.CreateFromEntityType(propertyType, simpleTypeName, tables);
                                thisTable.References.Add(columnName, simpleTypeName);
                                break;
                        }

                        break;
                }
            }

            tables.Add(thisTable);
        }

        private static void AddReferenceColumn(Table thisTable, string columName, List<Table> tables)
        {
            columName += "Ref";
            for (var i = 1; i < 999; i++)
            {
                if (thisTable.Column.ContainsKey(columName + i))
                {
                    continue;
                }

                columName += i;
                thisTable.Column.Add(columName, typeof(int));
                break;
            }
        }

        private void CreateLinkedTableFromListType(Type type, string fromTableName, string forColumn, List<Table> tables)
        {
            var subTypes = type.GetGenericArguments();
            if (subTypes.Length == 1)
            {
                this.CreateFromEntityType(subTypes[0], subTypes[0].Name, tables);
                this.CreateFromEntityType(typeof(ReferenceTable), "From" + fromTableName + "For" + forColumn + "To" + subTypes[0].Name, tables);
            }
            return;
        }
    }

    public class Table
    {
        internal static readonly Dictionary<Type, string> Mapping = new Dictionary<Type, string>();

        static Table()
        {
            Mapping.Add(typeof(int), "INT");
            Mapping.Add(typeof(long), "BIGINT");
            Mapping.Add(typeof(string), "nvarchar(255)");
            Mapping.Add(typeof(System.DateTime), "DateTime NULL");
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
            var thisKeys = new StringBuilder();
            var subTables = new StringBuilder();

            thisTable.Append("CREATE TABLE ");
            var sqlObjectName = this.FixSqlObjectName(this.Name);
            thisTable.Append(sqlObjectName);
            thisTable.AppendLine(" (");

            var pkIdColumnName = string.Format("{0}Id", this.Name);
            thisTable.AppendLine(string.Format("[{0}] int NOT NULL IDENTITY (1, 1),", pkIdColumnName));
            thisKeys.AppendLine(string.Format("ALTER TABLE {0} ADD CONSTRAINT PK_{0} PRIMARY KEY CLUSTERED ({1}) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);", sqlObjectName, pkIdColumnName));

            foreach (var type in Column)
            {
                thisTable.AppendLine(string.Format("[{0}] {1},", type.Key, Mapping[type.Value]));
            }

            subTables.AppendLine(thisTable.ToString().Substring(0, thisTable.Length - 3) + ");");

            foreach (var reference in this.References)
            {
                thisTable.AppendLine(
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

    public class ReferenceTable
    {
        public int Source { get; set; }
        public int Target { get; set; }
    }
}
