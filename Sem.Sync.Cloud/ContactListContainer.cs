namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using SyncBase;

    /// <summary>
    /// The container structure to transport information between client and cloud
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class ContactListContainer : ResultBase
    {
        /// <summary>
        /// Gets or sets the list of StdContact elements.
        /// </summary>
        [DataMember]
        public List<StdContact> ContactList { get; set; }

        /// <summary>
        /// Gets or sets the authentiaction credentials.
        /// </summary>
        [DataMember]
        public CloudCredentials Credentials { get; set; }
    }
}