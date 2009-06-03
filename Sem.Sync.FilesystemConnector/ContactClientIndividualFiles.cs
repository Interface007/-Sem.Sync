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

    using SyncBase;
    using SyncBase.Helpers;

    using Properties;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts
    /// </summary>
    public class ContactClientIndividualFiles : StdClient
    {
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(List<StdContact>));

        protected override void BeforeStorageAccess(string clientFolderName)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
        }
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (Directory.Exists(clientFolderName))
            {
                foreach (var filePathName in Directory.GetFiles(clientFolderName, "*.xmlcontact"))
                {
                    var file = new FileStream(filePathName, FileMode.Open);
                    try
                    {
                        if (file.Length > 0)
                            result.Add((StdElement)ContactListFormatter.Deserialize(file));

                        LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, Path.GetFileName(filePathName)));
                    }
                    finally
                    {
                        file.Close();
                    }
                }
            }
            return result;
        }
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            SyncTools.EnsurePathExist(clientFolderName);
            foreach (var element in elements)
            {
                var file = new FileStream(Path.Combine(clientFolderName, element.ToStringSimple() + ".xmlcontact"), FileMode.Create);
                try
                {
                    ContactListFormatter.Serialize(file, elements.ToContacts());
                }
                finally
                {
                    file.Close();
                }
            }
        }

        public override string FriendlyClientName
        {
            get
            {
                return "FileSystem Contact Connector for individual files";
            }
        }
    }
}
