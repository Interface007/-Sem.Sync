namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class BooleanResultContainer : ResultBase
    {
        [DataMember]
        public bool Result { get; set; }
    }
}
