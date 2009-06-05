// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.FilesystemConnector
{
    #region usings

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml.Serialization;

    using Properties;

    using SyncBase;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    public class ContactClient : StdClient
    {
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
                return "FileSystem Contact Connector - one file for all contacts";
            }
        }

        /// <summary>
        /// This overrides the event of just before accessing the storage location to 
        /// ensure the path for saving/loading data does exist.
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the destination/source of the contact file. </param>
        protected override void BeforeStorageAccess(string clientFolderName)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (File.Exists(clientFolderName))
            {
                var file = new FileStream(clientFolderName, FileMode.Open);
                try
                {
                    if (file.Length > 0)
                    {
                        result = ((List<StdContact>)ContactListFormatter.Deserialize(file)).ToStdElement();
                    }

                    LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, result.Count));
                }
                finally
                {
                    file.Close();
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
            var file = new FileStream(clientFolderName, FileMode.Create);
            try
            {
                var itemsToRemove = new List<StdElement>();
                foreach (var element in elements)
                {
                    SyncTools.ClearNulls(element, typeof(StdContact));
                    if (((StdContact)element).Name == null)
                    {
                        itemsToRemove.Add(element);
                    }
                }

                foreach (var element in itemsToRemove)
                {
                    elements.Remove(element);
                }

                ContactListFormatter.Serialize(file, elements.ToContacts());
            }
            catch (System.Exception ex)
            {
                this.LogProcessingEvent(ex.Message);
            }
            finally
            {
                file.Close();
            }
        }
    }
}
