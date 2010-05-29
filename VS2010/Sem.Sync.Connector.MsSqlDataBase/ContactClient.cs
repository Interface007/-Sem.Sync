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
    using System.Collections.Generic;
    using System.Data.SqlClient;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(Mandatory = true, Default = "{FS:WorkingFolder}\\MSSQLConnector.config", 
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    [ConnectorDescription(DisplayName = "MS SQL-Server Database")]
    public class ContactClient : StdClient
    {
        #region Properties

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

        #endregion

        #region Methods

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
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            return result;
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
            var connectionString =
                "Provider=SQLNCLI10.1;Integrated Security=SSPI;Initial Catalog=SemSync-Private;Data Source=WKMATZENSV02;";

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

        #endregion
    }
}