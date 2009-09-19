namespace Sem.Sync.Cloud
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using SyncBase;

    /// <summary>
    /// The container structure to transport information between client and cloud
    /// </summary>
    [DataContract]
    public class ContactListContainer
    {
        /// <summary>
        /// Gets or sets the list of StdContact elements.
        /// </summary>
        [DataMember]
        public List<StdContact> ContactList { get; set; }

        /// <summary>
        /// Gets or sets a list of messages to be exchanged in conjunction with the contact item list.
        /// </summary>
        [DataMember]
        public List<TechnicalMessage> Messages { get; set; }

        /// <summary>
        /// Gets or sets the authentiaction credentials.
        /// </summary>
        [DataMember]
        public CloudCredentials Credentials { get; set; }
    }
}