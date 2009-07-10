namespace Sem.Sync.SyncBase.Interfaces
{
    public interface ISyncCommand
    {
        IUiInteraction UiProvider { get; set; }

        bool ExecuteCommand(
            IClientBase sourceClient,
            IClientBase targetClient,
            IClientBase baseliClient,
            string sourceStorePath,
            string targetStorePath,
            string baselineStorePath,
            string commandParameter);
    }
}