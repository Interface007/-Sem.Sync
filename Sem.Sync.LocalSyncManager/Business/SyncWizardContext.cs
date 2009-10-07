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
    using System.Linq;
    using System.Reflection;

    using GenericHelpers;
    using GenericHelpers.Entities;
    using GenericHelpers.EventArgs;

    using Properties;

    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Binding;
    using SyncBase.DetailData;
    using SyncBase.Interfaces;

    using Tools;

    using SyncWorkFlow = SyncBase.DetailData.SyncWorkFlow;

    /// <summary>
    /// The context does contain information needed to access the source and the 
    /// target of the sequence to be executed. This includes the types of source 
    /// and target as well as  the paths inside the storage and  the credentials
    /// to authenticate.
    /// </summary>
    public class SyncWizardContext : INotifyPropertyChanged
    {
        /// <summary>
        /// The file extension for data files.
        /// </summary>
        public const string SyncListDataFileExtension = ".DSyncList";

        /// <summary>
        /// The working folder for data files (need to be writable).
        /// </summary>
        public static readonly string WorkingFolderData = Path.Combine(Config.WorkingFolder, "SyncLists");

        /// <summary>
        /// The working folder for template files.
        /// </summary>
        private static readonly string WorkingFolderTemplates = Path.Combine(Directory.GetCurrentDirectory(), "SyncLists");

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

            this.SetupPropertyChanged(true);

            var fileInfo = ScanFiles(Directory.GetCurrentDirectory());

            this.ClientsSource = from x in fileInfo
                                 where (x.Value3 & 1) == 1
                                 orderby x.Value2
                                 select new KeyValuePair<string, string>(x.Value1, x.Value2);
            
            this.ClientsTarget = from x in fileInfo
                                 where (x.Value3 & 2) == 2
                                 orderby x.Value2
                                 select new KeyValuePair<string, string>(x.Value1, x.Value2);

            this.SyncWorkflowsTemplates = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(WorkingFolderTemplates, "*" + SyncListTemplateFileExtension))
            {
                this.SyncWorkflowsTemplates.Add(file, Path.GetFileNameWithoutExtension(file));
            }

            this.ReloadWorkflowDataList();
        }

        /// <summary>
        /// Event that signals a change in the properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the list of available connectors that can read data.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> ClientsSource { get; set; }

        /// <summary>
        /// Gets or sets the list of connectors that can write data.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> ClientsTarget { get; set; }

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
                if (this.currentSyncWorkflowData != value)
                {
                    this.currentSyncWorkflowData = value;
                    this.LoadFrom(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the event reporting special events while executing commands.
        /// </summary>
        public Action<object, ProcessingEventArgs> ProcessingEvent { get; set; }

        /// <summary>
        /// Gets or sets the event reporting progress while executing commands.
        /// </summary>
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
                this.SetupPropertyChanged(false);

                this.Source = workFlow.Source;
                this.Target = workFlow.Target;

                this.CurrentSyncWorkflowTemplate = workFlow.Template;
                this.RaisePropertyChanged("CurrentSyncWorkflowData");

                this.SetupPropertyChanged(true);
            }
        }

        /// <summary>
        /// Saves the current workflow data of this object into a file.
        /// </summary>
        /// <param name="fileNameWithout">The file name to save. </param>
        public void SaveTo(string fileNameWithout)
        {
            if (string.IsNullOrEmpty(fileNameWithout))
            {
                return;
            }

            if (fileNameWithout == "(new)")
            {
                fileNameWithout = Tools.ReplaceInvalidFileCharacters(
                    string.Format(
                            CultureInfo.CurrentCulture,
                            "from {0} to {1} ({2})",
                            this.Source.Name,
                            this.Target.Name,
                            DateTime.Now));
            }

            var workFlow = new SyncWorkFlow
                {
                    Name = Path.GetFileNameWithoutExtension(fileNameWithout),
                    Source = this.Source,
                    Target = this.Target,
                    Template = this.CurrentSyncWorkflowTemplate
                };

            var fileName = Path.Combine(WorkingFolderData, fileNameWithout);
            if (!fileName.EndsWith(SyncListDataFileExtension, StringComparison.OrdinalIgnoreCase))
            {
                fileName = fileName + SyncListDataFileExtension;
            }

            Tools.SaveToFile(
                workFlow,
                fileName,
                typeof(SyncWorkFlow),
                typeof(Credentials),
                typeof(KeyValuePair<string, string>));

            this.ReloadWorkflowDataList();
            this.CurrentSyncWorkflowData = fileName;
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
        /// Generates sample data - not yet completed
        /// TODO: Create workflow data files.
        /// </summary>
        internal void GenerateSamples()
        {
            Tools.EnsurePathExist(WorkingFolderData);
            this.ReloadWorkflowDataList();
        }

        /// <summary>
        /// Deletes a file and reloads the list of available workflows
        /// </summary>
        /// <param name="path">
        /// The path to be deleted.
        /// </param>
        internal void DeleteWorkflowData(string path)
        {
            File.Delete(path);
            this.ReloadWorkflowDataList();
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> with a <see cref="Triple{T1,T2,T3}"/> that includes two
        /// strings and an int describing the classes found in the assemblies of the scanned path. Each class 
        /// does implement <see cref="IClientBase"/> and supports reading and/or writing. The first bit in 
        /// the int is for read-support, the second for write-support.
        /// </summary>
        /// <param name="path"> The path to scan. </param>
        /// <returns> The IEnumerable with the information. </returns>
        private static IEnumerable<Triple<string, string, int>> ScanFiles(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.dll"))
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

                    if (attribute.CanReadContacts || attribute.CanWriteContacts)
                    {
                        yield return
                            new Triple<string, string, int>
                                {
                                    Value1 = fullName,
                                    Value2 = nameToUse,
                                    Value3 = attribute.CanReadContacts ? (attribute.CanWriteContacts ? 3 : 1) : 2
                                };
                    }
                }
            }
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
        /// setup of the property changed event handlers
        /// </summary>
        /// <param name="attach">
        /// a value indicating if the events should be attached (true) or detached (false).
        /// </param>
        private void SetupPropertyChanged(bool attach)
        {
            if (attach)
            {
                if (this.Source != null)
                {
                    this.Source.PropertyChanged += (s, e) => this.RaisePropertyChanged("Source." + e.PropertyName);
                }

                if (this.Target != null)
                {
                    this.Target.PropertyChanged += (s, e) => this.RaisePropertyChanged("Target." + e.PropertyName);
                }
            }
            else
            {
                if (this.Source != null)
                {
                    this.Source.PropertyChanged -= (s, e) => this.RaisePropertyChanged("Source." + e.PropertyName);
                }

                if (this.Target != null)
                {
                    this.Target.PropertyChanged -= (s, e) => this.RaisePropertyChanged("Target." + e.PropertyName);
                }
            }
        }

        /// <summary>
        /// Reloads the list of workflow data
        /// </summary>
        private void ReloadWorkflowDataList()
        {
            Tools.EnsurePathExist(WorkingFolderData);
            this.SyncWorkflowData = new Dictionary<string, string> { { "(new)", "(new)" } };
            foreach (var file in Directory.GetFiles(WorkingFolderData, "*" + SyncListDataFileExtension))
            {
                this.SyncWorkflowData.Add(file, Path.GetFileNameWithoutExtension(file));
            }

            this.RaisePropertyChanged("SyncWorkflowData");
        }

        /// <summary>
        /// replaces some internal token to insert the workflow data into a workflow template.a
        /// </summary>
        /// <param name="value"> The value that may contain tokens. </param>
        /// <returns> The value with replaced tokens. </returns>
        private string ReplaceToken(string value)
        {
            var returnvalue = (value ?? string.Empty)
                .Replace("{source}", this.Source.Name)
                .Replace("{target}", this.Target.Name)
                .Replace("{sourcepath}", this.Source.Path)
                .Replace("{targetpath}", this.Target.Path);

            if (this.Source.ConnectorDescription != null)
            {
                returnvalue = returnvalue.Replace(
                    "{sourcepersonalidentifier}",
                    Enum.GetName(typeof(ProfileIdentifierType), this.Source.ConnectorDescription.MatchingIdentifier));
            }

            if (this.Target.ConnectorDescription != null)
            {
                returnvalue = returnvalue.Replace(
                    "{targetpersonalidentifier}",
                    Enum.GetName(typeof(ProfileIdentifierType), this.Target.ConnectorDescription.MatchingIdentifier));
            }

            return returnvalue;
        }
    }
}