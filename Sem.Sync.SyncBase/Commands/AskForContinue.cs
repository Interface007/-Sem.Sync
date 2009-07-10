namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Interfaces;

    public class AskForContinue : ISyncCommand
    {
        public IUiInteraction UiProvider { get; set; }

        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (this.UiProvider != null)
            {
                return this.UiProvider.AskForConfirm(commandParameter, targetClient.FriendlyClientName);
            }
            return true;
        }
    }
}
