namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Collections.Generic;
    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Binding;
using Sem.GenericHelpers;

    public class SyncWizardContext
    {
        public Dictionary<string, string> Clients { get; set; }

        public ConnectorInformation Source { get; set; }
        public ConnectorInformation Target { get; set; }

        public SyncWizardContext()
        {
            this.Clients = new Dictionary<string, string>();
            this.Clients.Add("Sem.Sync.XingConnector.ContactClient", "Xing");
            this.Clients.Add("Sem.Sync.OutlookConnector.ContactClient", "Outlook");
            this.Clients.Add("Sem.Sync.FilesystemConnector.ContactClientVCards", "vCards");
            this.Clients.Add("Sem.Sync.FilesystemConnector.GenericClientCsv of StdContact", "xml");

            this.Source = new ConnectorInformation();
            this.Target = new ConnectorInformation();
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
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path)
                ;
        }
    }

    public class ConnectorInformation
    {
        private string _name;
        private Factory _factory = new Factory("Sem.Sync.SyncBase");
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;

                this.ShowSelectFileDialog = false;
                this.ShowSelectPathDialog = false;

                var sourceTypeAttributes = Type.GetType(_factory.EnrichClassName(this._name)).GetCustomAttributes(typeof(ClientStoragePathDescriptionAttribute), false);
                if (sourceTypeAttributes != null && sourceTypeAttributes.Length > 0)
                {
                    var attribute = (ClientStoragePathDescriptionAttribute)sourceTypeAttributes[0];
                    this.ShowSelectFileDialog = attribute.ReferenceType == ClientPathType.FileSystemFileNameAndPath;
                    this.ShowSelectPathDialog = attribute.ReferenceType == ClientPathType.FileSystemPath;
                    if (string.IsNullOrEmpty(this.Path))
                    {
                        this.Path = attribute.Default;
                    }
                }

            }
        }

        public string Path { get; set; }
        public bool ShowSelectPathDialog { get; set; }
        public bool ShowSelectFileDialog { get; set; }

    }
}
