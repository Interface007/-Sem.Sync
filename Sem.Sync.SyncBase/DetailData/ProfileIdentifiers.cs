namespace Sem.Sync.SyncBase.DetailData
{
    public enum ProfileIdentifierType
    {
        Default = 0,
        XingProfileId,
        FacebookProfileId,
    }
        
    public class ProfileIdentifiers
    {
        public string XingProfileId { get; set; }
        public string FacebookProfileId { get; set; }
    }
}