// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling elements
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.Connector.MsAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using GenericHelpers;
    using GenericHelpers.Entities;

    using SyncBase;
    using SyncBase.Attributes;

    /// <summary>
    /// Class that provides a connector to Microsoft Access Database files. The connector
    /// accepts a configuration file in the "clientFolderName" of the <see cref="ReadFullList"/>
    /// and the <see cref="WriteFullList"/> methods. This configuration holds the 
    /// path of the database file and the field mapping. Microsoft Access workgroup files are
    /// currently not directly supported.
    /// In the current state the mapper does only support a single table and maps the properties
    /// to the type <see cref="StdContact"/> using property selectors. See the documentation 
    /// for a sample how to configure this.
    /// </summary>
    [ConnectorDescription(DisplayName = "Microsoft Acess Database",
        CanReadContacts = true,
        CanWriteContacts = true)]
    [ClientStoragePathDescriptionAttribute(
        Mandatory = true,
        Default = "{FS:WorkingFolder}\\sample.config",
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    public class GenericClient : StdClient
    {
        /// <summary>
        /// The update statement for database uperations
        /// </summary>
        private const string SqlStatementUpdate = "UPDATE {2} SET [{0}] = {1} WHERE {4} = {3}";

        /// <summary>
        /// The select statement for the database selecting all columns and all rows
        /// </summary>
        private const string SqlStatementSelectAll = "SELECT * FROM [{0}]";

        /// <summary>
        /// The select statement for selecting the primary key value of a specific row
        /// </summary>
        private const string SqlStatementSelectPk = "SELECT TOP 1 [{0}] AS X FROM [{1}] WHERE ((([{1}].[{2}])={3}));";

        /// <summary>
        /// The value string for a NULL value in the database sql dialect
        /// </summary>
        private const string SqlDatabaseNullString = "NULL";

        /// <summary>
        /// The insert statement for inserting a new row into the database
        /// </summary>
        private const string SqlStatementInsertRow = "INSERT INTO {0} ({1}) VALUES ({2})";

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Microsoft Access Generic Connector";
            }
        }

        /// <summary>
        /// Reads all data from the configured table and maps it to the <see cref="StdContact"/> entity.
        /// This method does use an unconditional SELECT * FROM ..., so it might affect performance
        /// in case of large result sets.
        /// </summary>
        /// <param name="clientFolderName">the configuration file for the data source</param>
        /// <param name="result">The list of elements that should get the elements. The elements 
        /// will be added to the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var description = GetDescription(clientFolderName);

            var mappings = description.ColumnDefinitions;
            using (var con = new OleDbConnection(GetConnectionString(description)))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = string.Format(SqlStatementSelectAll, description.MainTable);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var newContact = new StdContact();
                        foreach (var mappingItem in mappings)
                        {
                            try
                            {
                                var value = reader[mappingItem.Title].ToString();

                                if (mappingItem.TransformationFromDatabase != null)
                                {
                                    value = mappingItem.TransformationFromDatabase.Compile()(mappingItem, value).ToString();
                                }

                                Tools.SetPropertyValue(
                                    newContact,
                                    mappingItem.Selector,
                                    value);
                            }
                            catch (Exception ex)
                            {
                                this.LogProcessingEvent(ex.Message);
                            }
                        }

                        result.Add(newContact);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Writes the information from the <see cref="StdContact"/> entities back to the database table.
        /// This method has not been optimized yet and does produce a high amount of update statements!
        /// TODO: Optimize the method to only use one UPDATE statement per row.
        /// </summary>
        /// <param name="elements">the list of elements that will be written to the target system.</param>
        /// <param name="clientFolderName">configiguration file for the target database and mapping</param>
        /// <param name="skipIfExisting">this implementation does ignore the skipIfExisting parameter</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var description = GetDescription(clientFolderName);

            var columns = description.ColumnDefinitions;
            var primaryKeyName = description.GetPrimaryKeyName();
            var lookupColumns = from x in columns where x.IsLookupValue || x.IsPrimaryKey select x;

            using (var con = new OleDbConnection(GetConnectionString(description)))
            {
                con.Open();

                foreach (StdContact item in elements)
                {
                    try
                    {
                        var currentItem = item;
                        var id = (from l in lookupColumns 
                                  select GetPrimaryKeyForEntity(con, description, l, currentItem))
                                  .Where(x => !string.IsNullOrEmpty(x))
                                  .FirstOrDefault();

                        if (string.IsNullOrEmpty(id))
                        {
                            this.InsertNewItemToDatabase(con, description, currentItem);
                            continue;
                        }

                        columns.Where(x => !x.IsAutoValue).ForEach(
                            mappingItem =>
                            {
                                using (var cmd = con.CreateCommand())
                                {
                                    cmd.CommandText = string.Format(
                                        SqlStatementUpdate,
                                        mappingItem.Title,
                                        FormatForDatabase(Tools.GetPropertyValue(currentItem, mappingItem.Selector), mappingItem),
                                        description.MainTable,
                                        id,
                                        primaryKeyName);

                                    cmd.ExecuteNonQuery();
                                }
                            });
                    }
                    catch (Exception ex)
                    {
                        this.LogProcessingEvent(item, ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Performs a lookup for the PK value of a given entity by querying the table using a specific field mapping
        /// </summary>
        /// <param name="connection"> The database connection to the microsoft access database file. </param>
        /// <param name="description"> The source description including the table name to be used. </param>
        /// <param name="fieldMapping"> The field mapping. </param>
        /// <param name="contact"> The contact for find. </param>
        /// <returns> the primary key value of the entity or null if not in database </returns>
        private static string GetPrimaryKeyForEntity(OleDbConnection connection, SourceDescription description, ColumnDefinition fieldMapping, StdContact contact)
        {
            var value = FormatForDatabase(Tools.GetPropertyValue(contact, fieldMapping.Selector), fieldMapping);
            if (value == SqlDatabaseNullString)
            {
                return null;
            }

            using (var cmd = connection.CreateCommand())
            {
                var text = string.Format(
                   SqlStatementSelectPk,
                   description.GetPrimaryKeyName(),
                   description.MainTable,
                   fieldMapping.Title,
                   FormatForDatabase(Tools.GetPropertyValue(contact, fieldMapping.Selector), fieldMapping));

                cmd.CommandText = text;
                var result = cmd.ExecuteScalar();
                return (result ?? string.Empty).ToString();
            }
        }

        /// <summary>
        /// Read mapping description from file - create a sample file if it does not exist
        /// </summary>
        /// <param name="clientFolderName"> The file that does contain the database mapping description </param>
        /// <returns> a deserialized mapping description </returns>
        private static SourceDescription GetDescription(string clientFolderName)
        {
            if (!File.Exists(clientFolderName))
            {
                Tools.SaveToFile(SourceDescription.GetDefaultSourceDescription(), clientFolderName);
            }

            return Tools.LoadFromFile<SourceDescription>(clientFolderName);
        }

        /// <summary>
        /// Formats an object for database operations to SQL synthax
        /// </summary>
        /// <param name="toBeFormatted"> The object to be formatted. </param>
        /// <param name="mappingItem"> The mapping item for this object. </param>
        /// <returns> A SQL formatted object. </returns>
        private static string FormatForDatabase(object toBeFormatted, ColumnDefinition mappingItem)
        {
            if (toBeFormatted == null)
            {
                return SqlDatabaseNullString;
            }

            if (mappingItem.NullIfDefault && (toBeFormatted == toBeFormatted.GetType().GetConstructor(new Type[] { })))
            {
                return SqlDatabaseNullString;
            }

            var returnValue = mappingItem.IsNumericValue ? string.Format(CultureInfo.InvariantCulture, "{0}", toBeFormatted) : "'" + toBeFormatted.ToString().Replace("'", "''") + "'";

            if (mappingItem.TransformationToDatabase != null)
            {
                returnValue = mappingItem.TransformationToDatabase.Compile()(mappingItem, toBeFormatted);
            }

            return returnValue;
        }

        /// <summary>
        /// parses the database path and checks if it's already a complete connection string or just a file name.
        /// If the database path does not contain a "=" character it's identified as an OLEDB connection string.
        /// In case of being just a database file name, it's interpreted as a path to a Microsoft Access database file.
        /// </summary>
        /// <param name="description"> The source description containing the database path. </param>
        /// <returns> a connection string to open the database </returns>
        private static string GetConnectionString(SourceDescription description)
        {
            if (!description.DatabasePath.Contains("="))
            {
                return string.Format(
                    "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False",
                    description.DatabasePath);
            }

            return description.DatabasePath;
        }

        /// <summary>
        /// Inserts a new item into the database
        /// </summary>
        /// <param name="con"> The database connection. </param>
        /// <param name="description"> The source description. </param>
        /// <param name="currentItem"> The item to be inserted. </param>
        private void InsertNewItemToDatabase(OleDbConnection con, SourceDescription description, StdContact currentItem)
        {
            var insertColumns = from x in description.ColumnDefinitions where !x.IsAutoValue select x.Title;

            var values = from x in description.ColumnDefinitions
                         where x.IsAutoValue == false
                         select FormatForDatabase(Tools.GetPropertyValue(currentItem, x.Selector), x);

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format(
                    SqlStatementInsertRow,
                    description.MainTable,
                    "[" + insertColumns.ConcatElementsToString("],[") + "]",
                    values.ConcatElementsToString(","));
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (OleDbException ex)
                {
                    this.LogProcessingEvent(currentItem, "error writing element: " + ex.Message);
                }
            }
        }
    }
}
