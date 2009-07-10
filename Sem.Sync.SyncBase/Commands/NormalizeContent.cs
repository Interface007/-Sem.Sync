namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using Interfaces;

    public class NormalizeContent : ISyncCommand
    {
        public IUiInteraction UiProvider { get; set; }

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (targetClient == null) throw new InvalidOperationException("item.targetClient is null");
            if (sourceClient == null) throw new InvalidOperationException("item.sourceClient is null");
            targetClient.WriteRange(
                targetClient.Normalize(targetClient.GetAll(targetStorePath)),
                sourceStorePath);
            return true;
        }
    }
}
