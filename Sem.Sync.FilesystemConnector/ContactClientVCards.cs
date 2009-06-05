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

    using SyncBase;
    using SyncBase.Helpers;

    using Properties;
    using SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    public class ContactClientVCards : StdClient
    {
        private const string VCardFilenameExtension = ".vCard";

        private readonly VCardConverter _converter = new VCardConverter();
        private readonly bool _savePictureExternal;
        
        public ContactClientVCards()
        {
            bool.TryParse(this.GetConfigValue("Save-Pictures-External"), out _savePictureExternal);
        }

        protected override void BeforeStorageAccess(string clientFolderName)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
        }
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (Directory.Exists(clientFolderName))
            {
                foreach (var filePathName in Directory.GetFiles(clientFolderName, "*" + VCardFilenameExtension))
                {
                    result.Add(_converter.VCardToStdContact(File.ReadAllBytes(filePathName), ProfileIdentifierType.Default));
                    LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, Path.GetFileName(filePathName)));
                }
            }
            return result;
        }
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            SyncTools.EnsurePathExist(clientFolderName);
            foreach (var element in elements.ToContacts())
            {
                var fileName = Path.Combine(clientFolderName, SyncTools.NormalizeFileName(element.ToStringSimple()));
                File.WriteAllBytes(fileName + VCardFilenameExtension, VCardConverter.StdContactToVCard(element));
                if (_savePictureExternal && !string.IsNullOrEmpty(element.PictureName))
                {
                    File.WriteAllBytes(fileName + "-" + element.PictureName, element.PictureData);
                }
            }
        }

        public override string FriendlyClientName
        {
            get
            {
                return "FileSystem Contact Connector - individual vCards with external pictures";
            }
        }
    }
}
