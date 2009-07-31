// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.CloudStorageConnector
{
    #region usings

    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Cloud.StorageConnectors;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescriptionAttribute(
        Mandatory = true,
        Default = "{FS:WorkingFolder}\\Public",
        ReferenceType = ClientPathType.FileSystemFileNameAndPath)]
    [ConnectorDescription(DisplayName = "Cloud Contact Connector to be used inside the cloud")]
    public class ContactClient : StdClient
    {
        private readonly IStorageConnector storage = new StubStorage();

        /// <summary>
        /// This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(List<StdContact>));

        /// <summary>
        /// Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Cloud Contact Connector";
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            using (var reader = storage.GetFullListReader(clientFolderName))
            {
                if (reader != null)
                {
                    result = ((List<StdContact>)ContactListFormatter.Deserialize(reader)).ToStdElement();
                    CleanUpEntities(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full name including path of the file that will get the contacts while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            using (var writer = storage.CreateFullListWriter(clientFolderName))
            {
                try
                {
                    CleanUpEntities(elements);
                    ContactListFormatter.Serialize(writer, elements.ToContacts());
                }
                catch (System.Exception ex)
                {
                    this.LogProcessingEvent(ex.Message);
                }
            }
        }
    }
}
