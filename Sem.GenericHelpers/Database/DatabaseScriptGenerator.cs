// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseGenerator.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the DatabaseGenerator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class DatabaseScriptGenerator
    {
        public static string CreateScriptFromEntityType(Type entityType, string baseTableName)
        {
            var tables = new List<Table>();
            CreateFromEntityType(entityType, baseTableName, tables);

            var script = new StringBuilder();
            script.Append("-- Database Generation Script for ");
            script.AppendLine(baseTableName);
            script.AppendLine("-- -- Tables");
            script.AppendLine(string.Join("\r\n", from x in tables select x.ToScript()));
            script.AppendLine("-- -- References");
            script.AppendLine(string.Join("\r\n", from x in tables select x.ToScriptReferences()));

            return script.ToString();
        }

        private static Table CreateFromEntityType(Type entity, string entityName, List<Table> tables)
        {
            if (tables.Exists(x => x.Name == entityName))
            {
                return null;
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
                        // because this is an n:n relation, we don't need to add a "local column", but a relationship table
                        CreateLinkedTableFromListType(propertyType, entityName, columnName, thisTable, tables);
                        break;

                    case "System.Enum":
                        // an enum is like a custom type
                        CreateFromEntityType(typeof(EnumHelper), simpleTypeName, tables);
                        AddReferenceColumn(thisTable, simpleTypeName, columnName);
                        break;

                    default:
                        // add a new table fo holding the value of a custom type
                        CreateFromEntityType(propertyType, simpleTypeName, tables);

                        // add the reference column for that table
                        AddReferenceColumn(thisTable, simpleTypeName, columnName);
                        LogUnknownTypes(propertyType);
                        break;
                }
            }

            tables.Add(thisTable);
            return thisTable;
        }

        private static void AddReferenceColumn(Table thisTable, string typeName, string columnName)
        {
            var columName = columnName + "Ref";
            thisTable.Column.Add(columName, typeof(int));
            thisTable.References.Add(columName, typeName);
        }

        private static void CreateLinkedTableFromListType(Type type, string fromTableName, string forColumn, Table tableSource, List<Table> tables)
        {
            var subTypes = type.GetGenericArguments();
            if (subTypes.Length == 1)
            {
                if (subTypes[0] == typeof(string))
                {
                    var tableRef = CreateFromEntityType(typeof(LookupTableString), forColumn, tables);
                    tableRef.References.Add("Source", tableSource.Name);
                    return;
                }

                var entityName = subTypes[0].Name;
                var tableTaget = CreateFromEntityType(subTypes[0], entityName, tables);

                var tableReference = CreateFromEntityType(typeof(ReferenceTable), "From" + fromTableName + "For" + forColumn + "To" + tableTaget.Name, tables);
                tableReference.References.Add("Target", tableTaget.Name);
                tableReference.References.Add("Source", tableSource.Name);
            }
        }

        private static void LogUnknownTypes(Type propertyType)
        {
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
                    break;

                default:
                    Console.WriteLine(propertyType.FullName);
                    break;
            }
        }
    }
}
