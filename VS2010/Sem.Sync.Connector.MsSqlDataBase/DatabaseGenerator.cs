namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System;
    using System.Data.SqlClient;
    using System.Text;

    public class DatabaseGenerator
    {
        public string ConnectionString { get; set; }

        public string CreateFromEntityType(Type entity, string entityName)
        {
            var thisTable = new StringBuilder();
            var subTables = new StringBuilder();
            var foreignKeys = new StringBuilder();

            thisTable.Append("CREATE TABLE ");
            thisTable.Append(this.FixSqlObjectName(entityName));
            thisTable.AppendLine(" (");
            thisTable.AppendLine(string.Format("[{0}Id] int,", entityName));

            var publicProperties = entity.GetProperties();

            foreach (var publicProperty in publicProperties)
            {
                var name = publicProperty.Name;
                var simpleTypeName = publicProperty.PropertyType.Name;
                var baseType = publicProperty.PropertyType.BaseType;
                var baseTypeName =
                    simpleTypeName == "List`1" ? simpleTypeName : 
                    baseType == null ? "none" : 
                    baseType.FullName;

                switch (baseTypeName)
                {
                    case "List`1":
                        subTables.AppendLine(this.CreateLinkedTableFromListType(publicProperty.PropertyType, name));
                        thisTable.AppendLine(string.Format("[{0}Id] int,", name));
                        foreignKeys.AppendLine(string.Format("Add ForeignKey ({0}, {1})", name, name));
                        break;

                    case "System.Enum":
                        subTables.AppendLine(this.CreateLinkedTableFromEnumType(publicProperty.PropertyType, name));
                        thisTable.AppendLine(string.Format("[{0}Id] int,", name));
                        foreignKeys.AppendLine(string.Format("Add ForeignKey ({0}, {1})", name, name));
                        break;

                    default:

                        switch (publicProperty.PropertyType.FullName)
                        {
                            case "System.Int32":
                                thisTable.AppendLine(string.Format("[{0}] int,", name));
                                break;

                            case "System.Int64":
                                thisTable.AppendLine(string.Format("[{0}] long,", name));
                                break;

                            case "System.String":
                                thisTable.AppendLine(string.Format("[{0}] nvarchar(255) NULL,", name));
                                break;

                            case "System.DateTime":
                                thisTable.AppendLine(string.Format("[{0}] DateTime NULL,", name));
                                break;

                            case "System.Boolean":
                                thisTable.AppendLine(string.Format("[{0}] Bool,", name));
                                break;

                            case "System.Guid":
                                thisTable.AppendLine(string.Format("[{0}] UniqueIdentifier NULL,", name));
                                break;

                            case "System.Byte[]":
                                thisTable.AppendLine(string.Format("[{0}] Image NULL,", name));
                                break;

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
                                subTables.AppendLine(this.CreateFromEntityType(publicProperty.PropertyType, name));
                                break;

                            default:
                                Console.WriteLine(publicProperty.PropertyType.FullName);
                                subTables.AppendLine(this.CreateFromEntityType(publicProperty.PropertyType, name));
                                break;
                        }

                        break;
                }
            }

            subTables.AppendLine(thisTable.ToString().Substring(0, thisTable.Length - 3) + ");");
            return subTables.ToString();

            ////using (var connection = new SqlConnection(this.ConnectionString))
            ////{
            ////    connection.Open();


            ////    connection.Close();
            ////}
        }

        private string CreateLinkedTableFromEnumType(Type type, string name)
        {
            return "-- we need to implement a way of creating a lookup table from an enum...";
        }

        private string CreateLinkedTableFromListType(Type type, string name)
        {
            return "-- we need to implement a way of creating a lookup table from an list...";
        }

        private string FixSqlObjectName(string entityName)
        {
            return entityName
                .Replace(" ", "_")
                .Replace(".", "_");
        }
    }
}
