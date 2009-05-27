namespace Sem.Sync.SyncBase.DetailData
{
    using System;
    using System.Xml.Serialization;

    public class MatchingEntry
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        
        public ProfileIdentifiers ProfileId { get; set; }
    }
}
