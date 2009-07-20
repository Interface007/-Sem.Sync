namespace Sem.Sync.LocalSyncManager
{
    using System.Collections.Generic;

    using SyncBase;
    using SyncBase.Binding;

    public class SyncWizardContext
    {
        public Dictionary<string, string> Clients { get; set; }

        public string Source { get; set; }
        public string SourcePath { get; set; }
        public string Target { get; set; }
        public string TargetPath { get; set; }

        public SyncWizardContext()
        {
            this.Clients = new Dictionary<string, string>();
            this.Clients.Add("Sem.Sync.XingConnector.ContactClient", "Xing");
            this.Clients.Add("Sem.Sync.OutlookConnector.ContactClient", "Outlook");
            this.Clients.Add("Sem.Sync.FilesystemConnector.ContactClientVCards", "vCards");
            this.Clients.Add("Sem.Sync.FilesystemConnector.GenericClientCsv of StdContact", "xml");
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
                command.SourceStorePath = ReplaceToken(command.SourceStorePath);
            }

            engine.Execute(commands);

        }

        private string ReplaceToken(string value)
        {
            return (value ?? "")
                .Replace("{source}", this.Source)
                .Replace("{target}", this.Source)
                .Replace("{sourcepath}", this.SourcePath)
                .Replace("{targetpath}", this.SourcePath)
                ;
        }
    }
}
