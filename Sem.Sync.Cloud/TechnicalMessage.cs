namespace Sem.Sync.Cloud
{
    using System.Runtime.Serialization;

    /// <summary>
    /// </summary>
    [DataContract(Namespace = "http://svenerikmatzen.com/Sem/Sync/OnlineStorage")]
    public class TechnicalMessage
    {
        public string Message { get; set; }
        public int MessageId { get; set; }
        public MessageClassification Classification { get; set; }
    }

    public enum MessageClassification
    {
        Default = 0,

        Warning = 1,
        Error = 2,
        Critical = 3,

    }
}