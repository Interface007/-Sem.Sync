namespace Sem.Sync.LocalSyncManager
{
    using System.Collections.Generic;
    using SyncBase;
    using SyncBase.Binding;

    public class SyncWizardContext
    {
        public Dictionary<string, string> Clients { get; set; }

        public ConnectorInformation Source { get; set; }
        public ConnectorInformation Target { get; set; }

        public SyncWizardContext()
        {
            // todo: replace this with automatic lookup of assemblies
            this.Clients = new Dictionary<string, string>();
            this.Clients.Add("Sem.Sync.XingConnector.ContactClient", "Xing");
            this.Clients.Add("Sem.Sync.OutlookConnector.ContactClient", "Outlook");
            this.Clients.Add("Sem.Sync.FilesystemConnector.ContactClientVCards", "vCards");
            this.Clients.Add("Sem.Sync.FilesystemConnector.GenericClientCsv of StdContact", "xml");

            this.Source = new ConnectorInformation { Name = "Sem.Sync.XingConnector.ContactClient" };
            this.Target = new ConnectorInformation { Name = "Sem.Sync.OutlookConnector.ContactClient" };
        }

        internal void Run()
        {
            var engine = new SyncEngine();
            var commands = SyncCollection.LoadSyncList("Wizard.XSyncList");

            foreach (var command in commands)
            {
                command.SourceConnector = ReplaceToken(command.SourceConnector);
                command.TargetConnector = ReplaceToken(command.TargetConnector);
                command.SourceStorePath = ReplaceToken(command.SourceStorePath);
                command.TargetStorePath = ReplaceToken(command.TargetStorePath);
            }

            engine.Execute(commands);
        }

        private string ReplaceToken(string value)
        {
            return (value ?? "")
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path)
                ;
        }
    }
}
