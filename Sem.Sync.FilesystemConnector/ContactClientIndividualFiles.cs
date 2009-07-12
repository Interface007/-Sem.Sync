// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientIndividualFiles.cs" company="Sven Erik Matzen">
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

    using GenericHelpers;

    using Properties;

    using SyncBase;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    public class ContactClientIndividualFiles : StdClient
    {
        /// <summary>
        /// This is the formatter instance for serializing the list of contacts.
        /// </summary>
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(StdContact));

        /// <summary>
        /// Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "FileSystem Contact Connector for individual files";
            }
        }

        /// <summary>
        /// This overrides the event of just before accessing the storage location to 
        /// ensure the path for saving/loading data does exist.
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the destination/source of the contact file. </param>
        protected override void BeforeStorageAccess(string clientFolderName)
        {
            Tools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name of the path that does contain the contact files.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (Directory.Exists(clientFolderName))
            {
                foreach (var filePathName in Directory.GetFiles(clientFolderName, "*.xmlcontact"))
                {
                    using (var file = new FileStream(filePathName, FileMode.Open))
                    {
                        if (file.Length > 0)
                        {
                            result.Add((StdContact)ContactListFormatter.Deserialize(file));
                        }

                        LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, Path.GetFileName(filePathName)));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full path that will get the contact files while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            Tools.EnsurePathExist(clientFolderName);
            foreach (var element in elements)
            {
                using (var file = new FileStream(Path.Combine(clientFolderName, element.ToStringSimple() + ".xmlcontact"), FileMode.Create))
                {
                    ContactListFormatter.Serialize(file, element);
                }
            }
        }
    }
}
