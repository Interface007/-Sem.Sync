namespace Sem.Sync.OnlineStorage2
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using Connector.Filesystem;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;

    using SyncBase.Helpers;

    /// <summary>
    /// Service implementation for the <see cref="IContactService"/> interface.
    /// </summary>
    public class ContactService : IContactService
    {
        /// <summary>
        /// The file system path to store the information.
        /// </summary>
        private readonly string storagePath = ConfigurationManager.AppSettings["storageFolder"];

        /// <summary>
        /// Reads the contacts from a contact store specified in the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="clientFolderName"> The client folder name. </param>
        /// <returns> A contact list container with the contacts from the folder. </returns>
        public Stream GetAll(string clientFolderName)
        {
            var contactList = new ContactClientIndividualFiles().GetAll(this.storagePath).ToStdContacts();

            var memStream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(memStream, contactList);
            memStream.Position = 0;

            return memStream;
        }

        /// <summary>
        /// Writes contacts to a contact store specified in the parameter <paramref name="clientFolderName"/>.
        /// </summary>
        /// <param name="elements"> The elements to be written. </param>
        /// <returns> A value indicating whether the operation was successfull. </returns>
        public bool WriteFullList(Stream elements)
        {
            var formatter = new BinaryFormatter();
            using(var stream = new MemoryStream())
            {
                elements.CopyTo(stream);
                stream.Position = 0;
                var stdContacts = formatter.Deserialize(stream) as List<StdElement>;

                new ContactClientIndividualFiles().WriteRange(stdContacts, this.storagePath);

            }
            return true;
        }
    }

}
