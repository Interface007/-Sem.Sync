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
    public class ContactClient : StdClient
    {
        private static readonly XmlSerializer ContactListFormatter = new XmlSerializer(typeof(List<StdContact>));

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
                        result = ((List<StdContact>)ContactListFormatter.Deserialize(file)).ToStdElement();

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
                var itemsToRemove = new List<StdElement>();
                foreach (var element in elements)
                {
                    SyncTools.ClearNulls(element, typeof(StdContact));
                    if (((StdContact)element).Name == null)
                        itemsToRemove.Add(element);
                }
                
                foreach (var element in itemsToRemove)
                {
                    elements.Remove(element);
                }

                ContactListFormatter.Serialize(file, elements.ToContacts());
            
            }
            catch(System.Exception ex)
            {
                this.LogProcessingEvent(ex.Message);
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
                return "FileSystem Contact Connector - one file for all contacts";
            }
        }
    }
}
