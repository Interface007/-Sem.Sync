// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts persisted to the file system
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(
        Mandatory = true, 
        Default = "{FS:WorkingFolder}\\MSSQLConnector.config", 
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    [ConnectorDescription(DisplayName = "MS SQL-Server Database")]
    public class ContactClient : StdClient
    {
        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "MSSQL-Connector";
            }
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns> The list with the newly added elements </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var connectionString = "Provider=SQLNCLI10.1;Integrated Security=SSPI;Initial Catalog=Addresses;Data Source=WKMATZENSV02;";
            var columns = this.GetColumnDefinition(clientFolderName, typeof(StdContact));

            var context = new DatabaseContext(connectionString);
            return context.OpenTable<StdContact>(string.Empty).ToStdElements();
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// the information to where inside the source the elements should be written - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting">
        /// specifies whether existing elements should be updated or simply left as they are
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var columns = this.GetColumnDefinition(clientFolderName, typeof(StdContact));
            var connectionString = "Provider=SQLNCLI10.1;Integrated Security=SSPI;Initial Catalog=Addresses;Data Source=WKMATZENSV02;";

            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                elements.ToStdContacts().ForEach(x => WriteContact(con, x, columns));
                con.Close();
            }
        }

        /// <summary>
        /// The write contact.
        /// </summary>
        /// <param name="con">
        /// The con.
        /// </param>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="definitions">
        /// The definitions.
        /// </param>
        private static void WriteContact(SqlConnection con, StdElement x, List<ColumnDefinition> definitions)
        {
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT ContactID FROM Contact WHERE ContactID = @contactId'";
            cmd.Parameters.AddWithValue("@contactId", x.Id);
            var ret = cmd.ExecuteScalar();
        }

        private static int WriteEntity<T>(SqlConnection con, T entity)
        {
            var tableName = LookupTableName(typeof(T));
            var idQuery = string.Format("SELECT Id FROM {0} WHERE {1}", tableName, CreateWhereClause(entity));
            var idCommand = new SqlCommand(idQuery, con);

            var id = (int?)idCommand.ExecuteScalar();

            int result = 0;
            if (id == null)
            {
                var idInsert = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, CreateValueNameList(entity), CreateValueList(entity));
                
            }

            return result;
        }

        private static string CreateValueNameList<T>(T entity)
        {
            var list = Tools.GetPropertyList(string.Empty, typeof(T));
            var result = new StringBuilder();
            foreach (var property in list)
            {
                result.Append("[");
                result.Append(property);
                result.Append("],");
            }

            return result.ToString();
        }

        private static string CreateValueList<T>(T entity)
        {
            throw new NotImplementedException();
        }

        private static string LookupTableName(Type type)
        {
            var entityToTable = new SerializableDictionary<string, string>();
            var fullName = type.FullName;
            if (fullName != null)
            {
                if (entityToTable.ContainsKey(fullName))
                {
                    return entityToTable[fullName];
                }
            }

            return type.Name;
        }

        private static string CreateWhereClause<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}