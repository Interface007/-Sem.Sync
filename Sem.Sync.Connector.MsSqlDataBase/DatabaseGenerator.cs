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
            var createScript = new StringBuilder();
            createScript.Append("CREATE TABLE ");
            createScript.Append(FixSqlObjectName(entityName));
            createScript.AppendLine(" (");
            var publicProperties = entity.GetProperties();

            foreach (var publicProperty in publicProperties)
            {
                var baseType = publicProperty.PropertyType.BaseType.FullName;
                switch (baseType)
                {
                    case "System.Enum":

                        break;
                    default:

                        switch (publicProperty.PropertyType.FullName)
                        {
                            case "System.Int32":
                                createScript.AppendLine(string.Format("[{0}] int,", publicProperty.Name));
                                break;

                            case "System.String":
                                createScript.AppendLine(string.Format("[{0}] nvarchar(255) NULL,", publicProperty.Name));
                                break;

                            case "Sem.Sync.SyncBase.DetailData.AddressDetail":
                            case "Sem.Sync.SyncBase.DetailData.PhoneNumber":
                            case "Sem.Sync.SyncBase.DetailData.CountryCode":
                                this.CreateFromEntityType(publicProperty.PropertyType, publicProperty.Name);
                                break;

                            default:
                                Console.WriteLine(publicProperty.PropertyType.FullName);
                                if (!publicProperty.PropertyType.IsPrimitive)
                                {
                                    this.CreateFromEntityType(publicProperty.PropertyType, publicProperty.Name);
                                }
                                break;
                        }

                        break;
                }
            }

            createScript.Remove(createScript.Length - 2, 1);
            createScript.AppendLine(");");
            return createScript.ToString();

            //using (var connection = new SqlConnection(this.ConnectionString))
            //{
            //    connection.Open();


            //    connection.Close();
            //}
        }

        private string FixSqlObjectName(string entityName)
        {
            return entityName
                .Replace(" ", "_")
                .Replace(".", "_");
        }
    }
}
