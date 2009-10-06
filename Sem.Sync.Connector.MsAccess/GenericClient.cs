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
    using System.Linq;

    using SyncBase;
    using SyncBase.Attributes;
    using System.Data.OleDb;
    using Sem.GenericHelpers;    

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
            var description = new SourceDescription();

            var mappings = description.Mappings;
            var con = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", description.DatabasePath));
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM " + description.MainTable;
            

            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                var newContact = new StdContact();
                foreach (var mappingItem in mappings)
                {
                    if (mappingItem.TableName == description.MainTable)
                    {
                        Tools.SetPropertyValue(newContact, mappingItem.PropertyPath, reader[mappingItem.FieldName].ToString());
                    }
                }

                result.Add(newContact);
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
            var description = new SourceDescription();

            var mappings = description.Mappings;
            var con = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False", description.DatabasePath));
            con.Open();

            var pkName = (from x in mappings where x.IsPrimaryKey == true select x.FieldName).FirstOrDefault();
            var insertColumns = (from x in mappings where x.IsAutoValue == false select x.FieldName);

            var cmd = con.CreateCommand();
            foreach (StdContact item in elements)
            {
                var id = item.PersonalProfileIdentifiers.MicrosoftAccessId;

                var txt = string.Format(
                    "SELECT Count(*) AS X FROM {0} WHERE ((({0}.[{2}])={1}));",
                    description.MainTable,
                    id,
                    pkName);

                cmd.CommandText = txt;
                if (int.Parse(cmd.ExecuteScalar().ToString()) == 0)
                {
                    var values = (from x in mappings where x.IsAutoValue == false select Tools.GetPropertyValue(item, x.PropertyPath).ToString());

                    cmd.CommandText = string.Format(
                    "INSERT INTO {0} ({1}) VALUES ({2})",
                    description.MainTable,
                    "[" + insertColumns.ConcatElementsToString("],[") + "]",
                    "'" + values.ConcatElementsToString("','") + "'");
                }

                mappings.Where(x => !x.IsAutoValue).ForEach( 
                    mappingItem =>
                    {

                        cmd.CommandText = string.Format(
                            "UPDATE {2} SET [{0}] = '{1}' WHERE {4} = {3}",
                            mappingItem.FieldName,
                            Tools.GetPropertyValue(item, mappingItem.PropertyPath),
                            description.MainTable,
                            id,
                            pkName);

                        cmd.ExecuteNonQuery();
                    });
            }
            cmd.Dispose();
        }
    }
}
