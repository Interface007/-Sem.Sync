namespace Sem.Sync.LocalSyncManager.Business
{
    using System.Collections.Generic;

    using GenericHelpers;
    using GenericHelpers.EventArgs;

    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Binding;

    using Tools;
    using GenericHelpers.Entities;

    public class SyncWizardContext
    {
        public Dictionary<string, string> Clients { get; set; }

        public ConnectorInformation Source { get; set; }
        public ConnectorInformation Target { get; set; }

        public delegate void ProcessEventDelegate(object entity, ProcessingEventArgs eventArgs);

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

        public void LoadFrom(string path)
        {
            var connectors = Tools.LoadFromFile<List<ConnectorInformation>>(path);
            this.Source = connectors[0];
            this.Target = connectors[1];
        }

        public void SaveTo(string path)
        {
            var connectors = new List<ConnectorInformation>();
            connectors.Add(this.Source);
            connectors.Add(this.Target);
            Tools.SaveToFile(connectors, path, typeof(Credentials));
        }

        public void Run(string templateScript, ProcessEventDelegate processingEvent)
        {
            var engine = new SyncEngine
            {
                WorkingFolder = Config.WorkingFolder,
                UiProvider = new UiDispatcher()
            };

            engine.ProcessingEvent += (s, e) => processingEvent(s, e);

            var commands = SyncCollection.LoadSyncList(templateScript);

            foreach (var command in commands)
            {
                command.SourceConnector = this.ReplaceToken(command.SourceConnector);
                command.TargetConnector = this.ReplaceToken(command.TargetConnector);
                command.SourceStorePath = this.ReplaceToken(command.SourceStorePath);
                command.TargetStorePath = this.ReplaceToken(command.TargetStorePath);
            }

            engine.Execute(commands);
        }

        private string ReplaceToken(string value)
        {
            return (value ?? string.Empty)
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path);
        }
    }
}