namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CloudCredentials
    {
        [DataMember]
        public string AccountId { get; set; }

        [DataMember]
        public string AccountDomain { get; set; }

        [DataMember]
        public string AccountPassword { get; set; }
    }
}