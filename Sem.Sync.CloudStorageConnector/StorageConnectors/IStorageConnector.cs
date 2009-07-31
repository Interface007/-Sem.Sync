namespace Sem.Sync.CloudStorageConnector.StorageConnectors
{
    using System.IO;
    using System.Xml;

    interface IStorageConnector
    {
        XmlReader GetFullListReader(string clientFolderName);

        StreamWriter CreateFullListWriter(string clientFolderName);
    }
}