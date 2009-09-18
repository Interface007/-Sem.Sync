namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    /// <summary>
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class TechnicalMessage
    {
        [DataMember]
        public string Message { get; set; }
    }
}