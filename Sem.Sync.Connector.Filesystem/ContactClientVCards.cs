// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientVCards.cs" company="Sven Erik Matzen">
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

    using GenericHelpers;

    using Properties;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    [ClientStoragePathDescriptionAttribute(
        Mandatory = true, 
        Default = "{FS:WorkingFolder}\\vCards", 
        ReferenceType = ClientPathType.FileSystemPath)]
    [ConnectorDescription(DisplayName = "Filesystem vCards")]
    public class ContactClientVCards : StdClient
    {
        /// <summary>
        /// This is the file name suffix to be used when reading/writing vCards
        /// </summary>
        private const string VCardFilenameExtension = ".vCard";

        /// <summary>
        /// This is the an alternative file name suffix to be used when reading/writing vCards
        /// </summary>
        private const string VCardFilenameExtension2 = ".vcf";

        /// <summary>
        /// This is the objects instance of the vCard-converter class from the base library
        /// </summary>
        private readonly VCardConverter vCardConverter = new VCardConverter();

        /// <summary>
        /// defines whether to save the picture additionally to the file system or only inside the vCard
        /// </summary>
        private readonly bool savePictureExternal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClientVCards"/> class and checks the
        /// config file for configuration entries for this class.
        /// </summary>
        public ContactClientVCards()
        {
            bool.TryParse(this.GetConfigValue("Save-Pictures-External"), out this.savePictureExternal);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClientVCards"/> class and checks the
        /// config file for configuration entries for this class.
        /// </summary>
        /// <param name="savePictureExternal"> determines if the picture should be saved externally, too </param>
        public ContactClientVCards(bool savePictureExternal)
        {
            this.savePictureExternal = savePictureExternal;
        }

        /// <summary>
        /// Gets the user friendly name of the connector
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "FileSystem Contact Connector - individual vCards with external pictures";
            }
        }

        /// <summary>
        /// This overrides the event of just before accessing the storage location to 
        /// ensure the path for saving/loading data does exist.
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the destination/source of the vCards. </param>
        protected override void BeforeStorageAccess(string clientFolderName)
        {
            if (!string.IsNullOrEmpty(clientFolderName))
            {
                Tools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the name of the folder that does contain the vCard files.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added vCard-contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (Directory.Exists(clientFolderName))
            {
                var files = new List<string>(Directory.GetFiles(clientFolderName, "*" + VCardFilenameExtension));
                files.AddRange(Directory.GetFiles(clientFolderName, "*" + VCardFilenameExtension2));

                foreach (var filePathName in files)
                {
                    var newContact = this.vCardConverter.VCardToStdContact(File.ReadAllBytes(filePathName), ProfileIdentifierType.Default);
                    result.Add(newContact);

                    if (this.savePictureExternal)
                    {
                        var picPath = Path.Combine(
                            clientFolderName, Path.GetFileNameWithoutExtension(filePathName) + ".jpg");
                        if (File.Exists(picPath))
                        {
                            newContact.PictureData = File.ReadAllBytes(picPath);
                        }
                    }

                    LogProcessingEvent(newContact, string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, Path.GetFileName(filePathName)));
                }
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName"> the name of the folder that will get the vCard files while exporting data. </param>
        /// <param name="skipIfExisting"> a value indicating whether existing entries should be added overwritten or skipped. </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            Tools.EnsurePathExist(clientFolderName);
            foreach (var element in elements.ToContacts())
            {
                if (element.Name != null)
                {
                    var fileName = Path.Combine(clientFolderName, SyncTools.NormalizeFileName(element.ToStringSimple()));
                    File.WriteAllBytes(fileName + VCardFilenameExtension, VCardConverter.StdContactToVCard(element));
                    if (this.savePictureExternal && !string.IsNullOrEmpty(element.PictureName))
                    {
                        File.WriteAllBytes(fileName + "-" + element.PictureName, element.PictureData);
                    }
                }
            }
        }
    }
}