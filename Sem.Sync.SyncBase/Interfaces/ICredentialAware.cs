namespace Sem.Sync.SyncBase.Interfaces
{
    public interface ICredentialAware
    {
        string LogOnDomain { get; set; }
        string LogOnUserId { get; set; }
        string LogOnPassword { get; set; }
    }
}
