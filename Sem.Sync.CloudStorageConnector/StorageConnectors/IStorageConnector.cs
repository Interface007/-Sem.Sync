namespace Sem.Sync.Cloud.StorageConnectors
{
    using System.IO;
    using System.Xml;

    interface IStorageConnector
    {
        XmlReader GetFullListReader(string clientFolderName);

        StreamWriter CreateFullListWriter(string clientFolderName);
    }
}
