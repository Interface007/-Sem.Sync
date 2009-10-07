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
    using System.IO;
    using System.Linq;

    using GenericHelpers;
    using SyncBase;
    using SyncBase.Attributes;
    using System.Globalization;

    /// <summary>
    /// Class that provides a memory only connector to speed up operations.
    /// By adding a <see cref="ConnectorDescriptionAttribute"/> with CanRead = false and CanWrite = false
    /// it's invisible to the client GUI. This attribute is not respected by the engine - only by the GUI.
    /// </summary>
    [ConnectorDescription(DisplayName = "MicrosoftAcess-Client",
        CanReadContacts = true,
        CanWriteContacts = true)]
    public class GenericClient : StdClient
    {
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
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var description = GetDescription(clientFolderName);

            var mappings = description.Mappings;
            using (var con = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", description.DatabasePath)))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM " + description.MainTable;

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var newContact = new StdContact();
                        foreach (var mappingItem in mappings)
                        {
                            if (string.IsNullOrEmpty(mappingItem.TableName) || mappingItem.TableName == description.MainTable)
                            {
                                Tools.SetPropertyValue(
                                    newContact, mappingItem.PropertyPath, reader[mappingItem.FieldName].ToString());
                            }
                        }

                        result.Add(newContact);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var description = GetDescription(clientFolderName);

            var mappings = description.Mappings;
            var primaryKeyName = description.GetPrimaryKeyName();
            var lookupColumns = from x in mappings where x.IsLookupValue || x.IsPrimaryKey select x;
            var insertColumns = from x in mappings where !x.IsAutoValue select x.FieldName;

            using (var con = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", description.DatabasePath)))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    foreach (StdContact item in elements)
                    {
                        var currentItem = item;
                        var id = (from l in lookupColumns select this.GetPrimaryKeyForEntity(con, description, l, currentItem)).Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();

                        if (string.IsNullOrEmpty(id))
                        {
                            var values = from x in mappings
                                         where x.IsAutoValue == false
                                         select FormatForDatabase(Tools.GetPropertyValue(currentItem, x.PropertyPath), x);

                            cmd.CommandText = string.Format(
                                "INSERT INTO {0} ({1}) VALUES ({2})",
                                description.MainTable,
                                "[" + insertColumns.ConcatElementsToString("],[") + "]",
                                values.ConcatElementsToString(","));
                            try
                            {
                                cmd.ExecuteNonQuery();
                                continue;
                            }
                            catch (OleDbException ex)
                            {
                                this.LogProcessingEvent(currentItem, "error writing element: " + ex.Message);
                            }
                        }

                        mappings.Where(x => !x.IsAutoValue).ForEach(
                            mappingItem =>
                            {
                                cmd.CommandText = string.Format(
                                    "UPDATE {2} SET [{0}] = {1} WHERE {4} = {3}",
                                    mappingItem.FieldName,
                                    FormatForDatabase(Tools.GetPropertyValue(currentItem, mappingItem.PropertyPath), mappingItem),
                                    description.MainTable,
                                    id,
                                    primaryKeyName);

                                cmd.ExecuteNonQuery();
                            });
                    }
                }
            }
        }

        private string GetPrimaryKeyForEntity(OleDbConnection connection, SourceDescription description, Mapping idMapping, StdContact contact)
        {
            var value = FormatForDatabase(Tools.GetPropertyValue(contact, idMapping.PropertyPath), idMapping);
            if (value == "NULL")
            {
                return null;
            }

            using (var cmd = connection.CreateCommand())
            {
                var text = string.Format(
                   "SELECT [{0}] AS X FROM [{1}] WHERE ((([{1}].[{2}])={3}));",
                   description.GetPrimaryKeyName(),
                   description.MainTable,
                   idMapping.FieldName,
                   FormatForDatabase(Tools.GetPropertyValue(contact, idMapping.PropertyPath), idMapping));

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
        private static string FormatForDatabase(object toBeFormatted, Mapping mappingItem)
        {
            if (toBeFormatted == null)
            {
                return "NULL";
            }

            if (mappingItem.NullIfDefault && (toBeFormatted == toBeFormatted.GetType().GetConstructor(new Type[] { })))
            {
                return "NULL";
            }

            var returnValue = mappingItem.IsNumericValue ? string.Format(CultureInfo.InvariantCulture, "{0}", toBeFormatted) : "'" + toBeFormatted.ToString().Replace("'", "''") + "'";

            return returnValue;
        }
    }
}
