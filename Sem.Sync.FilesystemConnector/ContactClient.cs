﻿namespace Sem.Sync.FilesystemConnector
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
    public class ContactClient : StdClient
    {
        private static readonly XmlSerializer contactListFormatter = new XmlSerializer(typeof(List<StdContact>));

        protected override void BeforeStorageAccess(string clientFolderName)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(clientFolderName));
        }
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (File.Exists(clientFolderName))
            {
                var file = new FileStream(clientFolderName, FileMode.Open);
                try
                {
                    if (file.Length > 0)
                        result = ((List<StdContact>)contactListFormatter.Deserialize(file)).ToStdElement();

                    LogProcessingEvent(string.Format(CultureInfo.CurrentCulture, Resources.uiElementsRead, result.Count));
                }
                finally
                {
                    file.Close();
                }
            }
            return result;
        }
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            var file = new FileStream(clientFolderName, FileMode.Create);
            try
            {
                contactListFormatter.Serialize(file, elements.ToContacts());
            }
            finally
            {
                file.Close();
            }
        }
        
        public override string FriendlyClientName
        {
            get
            {
                return "FileSystem-Contact-Connector";
            }
        }
    }
}
