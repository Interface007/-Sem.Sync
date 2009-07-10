namespace Sem.Sync.SyncBase.Commands
{
    using System.Diagnostics;

    using Interfaces;

    public class OpenDocument : ISyncCommand
    {
        public IUiInteraction UiProvider { get; set; }

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (!string.IsNullOrEmpty(commandParameter))
            {
                Process.Start(new ProcessStartInfo(commandParameter));
            }
            return true;
        }
    }
}
