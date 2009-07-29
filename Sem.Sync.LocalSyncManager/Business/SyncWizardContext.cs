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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Reflection;

    using Entities;

    using GenericHelpers;
    using GenericHelpers.Entities;
    using GenericHelpers.EventArgs;

    using Properties;
    
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
        /// <summary>
        /// The working folder for data files (need to be writable).
        /// </summary>
        private static readonly string workingFolderData = Path.Combine(Config.WorkingFolder, "SyncLists");

        /// <summary>
        /// The working folder for template files.
        /// </summary>
        private static readonly string workingFolderTemplates = Path.Combine(Directory.GetCurrentDirectory(), "SyncLists");

        /// <summary>
        /// The file extension for data files.
        /// </summary>
        private const string SyncListDataFileExtension = ".DSyncList";

        /// <summary>
        /// The file extension for template files.
        /// </summary>
        private const string SyncListTemplateFileExtension = ".TSyncList";

        /// <summary>
        /// The current synchronization workflow data - this is created and saved by the user in the dialog..
        /// </summary>
        private string currentSyncWorkflowData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncWizardContext"/> class.
        /// </summary>
        public SyncWizardContext()
        {
            this.ClientsSource = new Dictionary<string, string>();
            this.ClientsTarget = new Dictionary<string, string>();
            this.Source = new ConnectorInformation();
            this.Target = new ConnectorInformation();

            // propagate property change event to consuming objects
            this.Source.PropertyChanged += (s, e) => this.RaisePropertyChanged("Source." + e.PropertyName);
            this.Target.PropertyChanged += (s, e) => this.RaisePropertyChanged("Target." + e.PropertyName);

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

                    var fullName = 
                        exportedType.IsGenericType
                        ? exportedType.FullName.Replace("`1", " of StdContact")
                        : exportedType.FullName;

                    var nameToUse = 
                        exportedType.IsGenericType
                        ? (attribute.DisplayName ?? fullName) + " of StdContact"
                        : attribute.DisplayName ?? fullName;
                    
                    if (attribute.CanRead)
                    {
                        this.ClientsSource.Add(fullName, nameToUse);
                    }

                    if (attribute.CanWrite)
                    {
                        this.ClientsTarget.Add(fullName, nameToUse);
                    }
                }
            }

            this.SyncWorkflowsTemplates = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(workingFolderTemplates, "*" + SyncListTemplateFileExtension))
            {
                this.SyncWorkflowsTemplates.Add(file, Path.GetFileNameWithoutExtension(file));
            }

            this.ReloadWorkflowDataList();
        }

        private void ReloadWorkflowDataList()
        {
            Tools.EnsurePathExist(workingFolderData);
            this.SyncWorkflowData = new Dictionary<string, string> { { "(new)", "(new)" } };
            foreach (var file in Directory.GetFiles(workingFolderData, "*" + SyncListDataFileExtension))
            {
                this.SyncWorkflowData.Add(file, Path.GetFileNameWithoutExtension(file));
            }

            this.RaisePropertyChanged(string.Empty);

        }

        /// <summary>
        /// Event that signals a change in the properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the list of available connectors that can read data.
        /// </summary>
        public Dictionary<string, string> ClientsSource { get; set; }

        /// <summary>
        /// Gets or sets the list of connectors that can write data.
        /// </summary>
        public Dictionary<string, string> ClientsTarget { get; set; }

        /// <summary>
        /// Gets or sets the currently selected Source.
        /// </summary>
        public ConnectorInformation Source { get; set; }

        /// <summary>
        /// Gets or sets the currently selected Target.
        /// </summary>
        public ConnectorInformation Target { get; set; }

        /// <summary>
        /// Gets or sets the list of avaliable synchronization workflow templates.
        /// </summary>
        public Dictionary<string, string> SyncWorkflowsTemplates { get; set; }

        /// <summary>
        /// Gets or sets the current synchronization workflow template.
        /// </summary>
        public string CurrentSyncWorkflowTemplate { get; set; }

        /// <summary>
        /// Gets or sets the list of available synchronization workflow data - this is created and saved by the user in the dialog.
        /// </summary>
        public Dictionary<string, string> SyncWorkflowData { get; set; }

        /// <summary>
        /// Gets or sets the current synchronization workflow data - this is created and saved by the user in the dialog..
        /// </summary>
        public string CurrentSyncWorkflowData
        {
            get
            {
                return this.currentSyncWorkflowData;
            }

            set
            {
                this.currentSyncWorkflowData = value;
                this.LoadFrom(value);
            }
        }

        public Action<object, ProcessingEventArgs> ProcessingEvent { get; set; }
        public Action<ProgressEventArgs> ProgressEvent { get; set; }

        /// <summary>
        /// Opens the current working folder using the explorer
        /// </summary>
        public static void OpenWorkingFolder()
        {
            var engine = new SyncEngine
                {
                    WorkingFolder = Config.WorkingFolder
                };

            engine.Execute(
                new SyncDescription
                    {
                        Command = SyncCommand.OpenDocument.ToString(),
                        CommandParameter = "{FS:WorkingFolder}"
                    });
        }

        /// <summary>
        /// Loads workflow data from a file into this object.
        /// </summary>
        /// <param name="path"> The path to the file containing the workflow </param>
        public void LoadFrom(string path)
        {
            var workFlow = Tools.LoadFromFile<SyncWorkFlow>(path);

            if (workFlow != null)
            {
                this.Source = workFlow.Source;
                this.Target = workFlow.Target;
                this.CurrentSyncWorkflowTemplate = workFlow.Template;

                this.Source.PropertyChanged += this.PropertyChanged;
                this.Target.PropertyChanged += this.PropertyChanged;

                this.RaisePropertyChanged(string.Empty);
            }
        }

        /// <summary>
        /// Saves the current workflow data of this object into a file.
        /// </summary>
        /// <param name="fileNameWithoutExtension"> The file name to save to without the file name extension. </param>
        /// <param name="name"> The name to be saved into the file. </param>
        public void SaveTo(string fileNameWithoutExtension, string name)
        {
            var workFlow = new SyncWorkFlow
                {
                    Name = name,
                    Source = this.Source,
                    Target = this.Target,
                    Template = this.CurrentSyncWorkflowTemplate
                };

            Tools.SaveToFile(
                workFlow,
                Path.Combine(workingFolderData, fileNameWithoutExtension) + SyncListDataFileExtension,
                typeof(SyncWorkFlow),
                typeof(Credentials),
                typeof(KeyValuePair<string, string>));
            
            this.ReloadWorkflowDataList();
        }

        /// <summary>
        /// Runs the currently selected template with the currently loaded data.
        /// </summary>
        /// <param name="templateScriptPath">
        /// The path to the template script. 
        /// </param>
        public void Run(string templateScriptPath)
        {
            if (!File.Exists(templateScriptPath))
            {
                this.ProcessingEvent(this, new ProcessingEventArgs(string.Format(CultureInfo.CurrentCulture, Resources.InstalledFileNotFound, templateScriptPath)));
                return;
            }

            var commands = SyncCollection.LoadSyncList(templateScriptPath);

            if (commands == null)
            {
                return;
            }

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
                command.CommandParameter = this.ReplaceToken(command.CommandParameter);
            }

            var engine = new SyncEngine
                {
                    WorkingFolder = Config.WorkingFolder,
                    UiProvider = new UiDispatcher(),
                };

            engine.ProcessingEvent += (s, e) => this.ProcessingEvent(s, e);
            engine.ProgressEvent += (s, e) => this.ProgressEvent(e);

            engine.Execute(commands);

            engine.ProcessingEvent -= (s, e) => this.ProcessingEvent(s, e);
            engine.ProgressEvent -= (s, e) => this.ProgressEvent(e);
        }

        /// <summary>
        /// Calls the event to inform other classes about an internal change of this objects 
        /// state - this will cause the GUI to read the data from this object.
        /// </summary>
        /// <param name="propertyName"> The property name that has been changed. </param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// replecaes some internal token to insert the workflow data into a workflow template.
        /// </summary>
        /// <param name="value"> The value that may contain tokens. </param>
        /// <returns> The value with replaced tokens. </returns>
        private string ReplaceToken(string value)
        {
            return (value ?? string.Empty)
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path)
                .Replace("{sourcepersonalidentifier}", Enum.GetName(typeof(ProfileIdentifierType), this.Source.ConnectorDescription.MatchingIdentifier))
                .Replace("{targetpersonalidentifier}", Enum.GetName(typeof(ProfileIdentifierType), this.Target.ConnectorDescription.MatchingIdentifier));
        }

        internal void GenerateSamples()
        {
            Tools.EnsurePathExist(workingFolderData);
            this.ReloadWorkflowDataList();
        }

        internal void DeleteWorkflowData(string path)
        {
            File.Delete(path);
            this.ReloadWorkflowDataList();
        }
    }
}