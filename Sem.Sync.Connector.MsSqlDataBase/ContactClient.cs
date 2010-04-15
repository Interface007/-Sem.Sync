// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.SqlClient;
using System.Linq;
using Sem.GenericHelpers;
using Sem.GenericHelpers.Entities;
using Sem.Sync.SyncBase.Helpers;

namespace Sem.Sync.Connector.MsSqlDatabase
{
    using System.Collections.Generic;
    using System.Globalization;

    using SyncBase;
    using SyncBase.Attributes;

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
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
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
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
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
            var columns = this.GetColumnDefinition<StdContact>(clientFolderName);
            var connectionString = "Provider=SQLNCLI10.1;Integrated Security=SSPI;Initial Catalog=SemSync-Private;Data Source=WKMATZENSV02;";

            using (var con = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                con.Open();
                elements.ToContacts().ForEach(x => this.WriteContact(con, x, columns));
                con.Close();
            }
        }

        private void WriteContact(SqlConnection con, StdContact x, List<ColumnDefinition> definitions)
        {
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT ContactID FROM Contact WHERE ContactID = @contactId'";
            cmd.Parameters.AddWithValue("@contactId", x.Id);
            var ret = cmd.ExecuteScalar();
        }
    }
}
