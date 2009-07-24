// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SyncWizardContext.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The context does contain information needed to access the source and the
//   target of the sequence to be executed. This includes the types of source
//   and target as well as  the paths inside the storage and  the credentials
//   to authenticate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.LocalSyncManager.Business
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;

    using GenericHelpers;
    using GenericHelpers.Entities;
    
    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Binding;
    using SyncBase.DetailData;

    using Tools;
    
    /// <summary>
    /// The context does contain information needed to access the source and the 
    /// target of the sequence to be executed. This includes the types of source 
    /// and target as well as  the paths inside the storage and  the credentials
    /// to authenticate.
    /// </summary>
    public class SyncWizardContext : INotifyPropertyChanged
    {
        public Dictionary<string, string> ClientsSource { get; set; }
        public Dictionary<string, string> ClientsTarget { get; set; }

        public ConnectorInformation Source { get; set; }
        public ConnectorInformation Target { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        
        public SyncWizardContext()
        {
            this.ClientsSource = new Dictionary<string, string>();
            this.ClientsTarget = new Dictionary<string, string>();
            this.Source = new ConnectorInformation();
            this.Target = new ConnectorInformation();

            this.Source.PropertyChanged += (s, e) => this.RaisePropertyChanged(string.Empty);
            this.Target.PropertyChanged += (s, e) => this.RaisePropertyChanged(string.Empty);

            foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll"))
            {
                // todo: check if the dll is a loadable assembly
                var assembly = Assembly.LoadFile(file);
                foreach (var exportedType in assembly.GetExportedTypes())
                {
                    if (exportedType.GetInterface("IClientBase") == null ||
                        exportedType.FullName == "Sem.Sync.SyncBase.StdClient" ||
                        exportedType.IsGenericType)
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
                        this.ClientsSource.Add(exportedType.FullName, attribute.DisplayName ?? exportedType.FullName);
                    }
                        
                    if (attribute.CanWrite)
                    {
                        this.ClientsTarget.Add(exportedType.FullName, attribute.DisplayName ?? exportedType.FullName);
                    }
                }
            }
        }

        public void LoadFrom(string path)
        {
            var connectors = Tools.LoadFromFile<List<ConnectorInformation>>(path);
            this.Source = connectors[0];
            this.Target = connectors[1];

            this.Source.PropertyChanged += this.PropertyChanged;
            this.Target.PropertyChanged += this.PropertyChanged;

            this.RaisePropertyChanged(string.Empty);
        }

        public void SaveTo(string path)
        {
            var connectors = new List<ConnectorInformation> { this.Source, this.Target };
            Tools.SaveToFile(connectors, path, typeof(Credentials));
        }

        public void Run(string templateScript, ProcessEvent processingEvent)
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
                command.SourceCredentials = (command.SourceConnector != null && command.SourceConnector == "{source}") ? this.Source.LogonCredentials : command.SourceCredentials;
                command.SourceCredentials = (command.SourceConnector != null && command.SourceConnector == "{target}") ? this.Target.LogonCredentials : command.SourceCredentials;
                command.TargetCredentials = (command.TargetConnector != null && command.TargetConnector == "{source}") ? this.Source.LogonCredentials : command.TargetCredentials;
                command.TargetCredentials = (command.TargetConnector != null && command.TargetConnector == "{target}") ? this.Target.LogonCredentials : command.TargetCredentials;
                
                command.SourceConnector = this.ReplaceToken(command.SourceConnector);
                command.TargetConnector = this.ReplaceToken(command.TargetConnector);
                command.SourceStorePath = this.ReplaceToken(command.SourceStorePath);
                command.TargetStorePath = this.ReplaceToken(command.TargetStorePath);
            }

            engine.Execute(commands);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
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