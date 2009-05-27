namespace Sem.Sync.SyncBase.DetailData
{
    public enum ProfileIdentifierType
    {
        Default = 0,
        XingProfileId,
        FacebookProfileId,
        ActiveDirectoryId,
    }
        
    public class ProfileIdentifiers
    {
        public string XingProfileId { get; set; }
        public string FacebookProfileId { get; set; }
        public string ActiveDirectoryId { get; set; }
    }
}