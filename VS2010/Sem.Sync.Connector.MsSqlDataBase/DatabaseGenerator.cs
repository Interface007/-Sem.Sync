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

    using Sem.Sync.SyncBase.Helpers;

    public class EnumHelper
    {
        public int Value { get; set; }
        public string Description { get; set; }
    }

    public class DatabaseGenerator
    {

        public string ConnectionString { get; set; }


        public Table CreateFromEntityType(Type entity, string entityName, List<Table> tables)
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
                        // add a table for the entity
                        this.CreateLinkedTableFromListType(propertyType, entityName, columnName, thisTable, tables);
                        break;

                    case "System.Enum":
                        this.CreateFromEntityType(typeof(EnumHelper), simpleTypeName, tables);
                        AddReferenceColumn(thisTable, simpleTypeName);
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
                                AddReferenceColumn(thisTable, simpleTypeName);
                                break;

                            default:
                                Console.WriteLine(propertyType.FullName);
                                this.CreateFromEntityType(propertyType, simpleTypeName, tables);
                                AddReferenceColumn(thisTable, simpleTypeName);
                                break;
                        }

                        break;
                }
            }

            tables.Add(thisTable);
            return thisTable;
        }

        private static void AddReferenceColumn(Table thisTable, string typeName)
        {
            var columName = typeName + "Ref";
            for (var i = 1; i < 999; i++)
            {
                if (thisTable.Column.ContainsKey(columName + i))
                {
                    continue;
                }

                columName += i;
                thisTable.Column.Add(columName, typeof(int));
                thisTable.References.Add(columName, typeName);
                break;
            }
        }

        private string CreateLinkedTableFromListType(Type type, string fromTableName, string forColumn, Table tableSource, List<Table> tables)
        {
            var subTypes = type.GetGenericArguments();
            if (subTypes.Length == 1)
            {
                if (subTypes[0] == typeof(string))
                {
                    var tableRef = this.CreateFromEntityType(typeof(ReferenceTableString), forColumn, tables);
                    tableRef.References.Add("Source", tableSource.Name);
                    return forColumn;
                }

                var entityName = subTypes[0].Name;
                var tableTaget = this.CreateFromEntityType(subTypes[0], entityName, tables);

                var tableReference = this.CreateFromEntityType(typeof(ReferenceTable), "From" + fromTableName + "For" + forColumn + "To" + tableTaget.Name, tables);
                tableReference.References.Add("Target", tableTaget.Name);
                tableReference.References.Add("Source", tableSource.Name);

                return entityName;
            }
            return string.Empty;
        }
    }
}
