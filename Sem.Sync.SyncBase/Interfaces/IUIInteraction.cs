namespace Sem.Sync.SyncBase.Interfaces
{
    public interface IUiInteraction
    {
        bool AskForLogOnCredentials(IClientBase client, string messageForUser, string logOnUserId, string logOnPassword);
        bool AskForConfirm(string messageForUser, string title);
    }
}
