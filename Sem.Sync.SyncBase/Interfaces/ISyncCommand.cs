namespace Sem.Sync.SyncBase.Interfaces
{
    using GenericHelpers.Interfaces;

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