﻿namespace Sem.Sync.LocalSyncManager.Business
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using GenericHelpers;
    using GenericHelpers.EventArgs;

    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Binding;

    using Tools;
    using GenericHelpers.Entities;

    public class SyncWizardContext
    {
        public Dictionary<string, string> ClientsSource { get; set; }
        public Dictionary<string, string> ClientsTarget { get; set; }

        public ConnectorInformation Source { get; set; }
        public ConnectorInformation Target { get; set; }

        public delegate void ProcessEventDelegate(object entity, ProcessingEventArgs eventArgs);

        public SyncWizardContext()
        {
            this.ClientsSource = new Dictionary<string, string>();
            this.ClientsTarget = new Dictionary<string, string>();
            this.Source= new ConnectorInformation();
            this.Target= new ConnectorInformation();

            foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll"))
            {
                // todo: check if the dll is a loadable assembly
                var assembly = Assembly.LoadFile(file);
                foreach (var exportedType in assembly.GetExportedTypes())
                {
                    if (exportedType.GetInterface("IClientBase") == null ||
                        exportedType.FullName == "Sem.Sync.SyncBase.StdClient")
                    {
                        continue;
                    }

                    var sourceTypeAttributes = exportedType.GetCustomAttributes(typeof(ConnectorDescriptionAttribute), false);
                    var attribute = 
                        sourceTypeAttributes.Length == 0 
                        ? new ConnectorDescriptionAttribute()
                        : (ConnectorDescriptionAttribute)sourceTypeAttributes[0];
                        
                    if (attribute.CanRead)
                    {
                        this.ClientsSource.Add(exportedType.FullName, exportedType.Name);
                    }
                        
                    if (attribute.CanWrite)
                    {
                        this.ClientsTarget.Add(exportedType.FullName, exportedType.Name);
                    }
                }
            }
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