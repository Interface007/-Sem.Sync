namespace Sem.Sync.CloudStorageConnector.StorageConnectors
{
    using System.IO;
    using System.Text;
    using System.Xml;

    using Test.DataGenerator;

    /// <summary>
    /// This is a simple stub for "some" cloud storage. It does provide some
    /// test data (different contacts), but does not persist any data.
    /// </summary>
    public class StubStorage : IStorageConnector
    {
        #region IStorageConnector Members

        public XmlReader GetFullListReader(string clientFolderName)
        {
            var result = new StringBuilder(102400);
            result.AppendLine("<?xml version=\"1.0\"?>");
            result.AppendLine("<ArrayOfStdContact xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            Contacts.AddContactWithoutPicture(result);
            Contacts.AddContactWithPicture(result);
            Contacts.AddContactWithNulls(result);

            result.Append("</ArrayOfStdContact>");
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(result.ToString()));
            var text = new StreamReader(textStream);
            var reader = XmlReader.Create(text);
            return reader;
        }

        public StreamWriter CreateFullListWriter(string clientFolderName)
        {
            var destinationStream = new MemoryStream();
            return new StreamWriter(destinationStream);
        }

        #endregion
    }
}