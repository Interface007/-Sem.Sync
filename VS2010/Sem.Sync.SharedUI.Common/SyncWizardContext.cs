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
namespace Sem.Sync.SharedUI.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using GenericHelpers;
    using GenericHelpers.Entities;
    using GenericHelpers.EventArgs;
    using GenericHelpers.Exceptions;
    using GenericHelpers.Interfaces;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.Binding;
    using SyncBase.DetailData;
    using SyncBase.Helpers;
    using SyncBase.Interfaces;

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
        public SyncWizardContext(IUiInteraction uiInteraction)
        {
            this.UiProvider = uiInteraction;
            this.ClientsSource = new Dictionary<string, string>();
            this.ClientsTarget = new Dictionary<string, string>();
            this.Source = new ConnectorInformation();
            this.Target = new ConnectorInformation();

            this.SetupPropertyChanged(true);

            var fileInfo = this.ScanFiles(Directory.GetCurrentDirectory());

            this.ClientsSource = ConnectorListToKeyValuePiars(fileInfo, 1);
            this.ClientsTarget = ConnectorListToKeyValuePiars(fileInfo, 2);

            this.SyncWorkflowsTemplates = new Dictionary<string, string>();
            Tools.EnsurePathExist(WorkingFolderTemplates);
            Directory
                .GetFiles(WorkingFolderTemplates, "*" + SyncListTemplateFileExtension)
                .ForEach(file => this.SyncWorkflowsTemplates.Add(file, Path.GetFileNameWithoutExtension(file)));

            this.ReloadWorkflowDataList();
        }

        private IEnumerable<KeyValuePair<string, string>> ConnectorListToKeyValuePiars(IEnumerable<ConnectorTypeDescription> fileInfo, int capable)
        {
            return (from x in fileInfo
                   where (x.ReadWrite & capable) == capable
                   orderby x.DisplayName
                   select new KeyValuePair<string, string>(x.ClassName, "[" + x.TypeName + "] " + x.DisplayName)).OrderBy(x => x.Value);
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
                if (this.currentSyncWorkflowData == value)
                {
                    return;
                }

                this.currentSyncWorkflowData = value;
                this.LoadFrom(value);
            }
        }

        /// <summary>
        /// Gets or sets the event reporting special events while executing commands.
        /// </summary>
        public Action<object, ProcessingEventArgs> ProcessingEvent { get; set; }

        /// <summary>
        /// Gets or sets the event reporting progress while executing commands.
        /// </summary>
        public EventHandler<ProgressEventArgs> ProgressEvent { get; set; }

        /// <summary>
        /// Gets or sets the event reporting progress when executing commands has been finished or aborted.
        /// </summary>
        public Action<ProgressEventArgs> FinishedEvent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a Cancel operation has been requested.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the execution of a template is locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Gets or sets the UiProvider that will handle the ui interaction.
        /// </summary>
        public IUiInteraction UiProvider { get; set; }

        /// <summary>
        /// Opens the current working folder using the explorer
        /// </summary>
        public void OpenWorkingFolder()
        {
            var engine = new SyncEngine
                             {
                                 WorkingFolder = Config.WorkingFolder
                             };

            engine.OpenWorkingFolder();
        }

        /// <summary>
        /// Resolves path tokens like application folder and working folder.
        /// </summary>
        /// <param name="path"> The path containing token. </param>
        /// <returns> the valid resolved path </returns>
        public static string ResolvePath(string path)
        {
            var engine = new SyncEngine
                             {
                                 WorkingFolder = Config.WorkingFolder
                             };

            return engine.ReplacePathToken(path);
        }

        /// <summary>
        /// Opens the folder for the exceptrion log files using the standard process for folders (Windows Explorer on most systems).
        /// </summary>
        public void OpenExceptionFolder()
        {
            System.Diagnostics.Process.Start(ExceptionHandler.ExceptionWriter[0].Destination);
        }

        /// <summary>
        /// Loads workflow data from a file into this object.
        /// </summary>
        /// <param name="path"> The path to the file containing the workflow </param>
        public void LoadFrom(string path)
        {
            var workFlow = Tools.LoadFromFile<SyncWorkFlow>(path);

            if (workFlow == null)
            {
                return;
            }

            this.SetupPropertyChanged(false);

            this.Source = workFlow.Source;
            this.Target = workFlow.Target;

            if (!this.SyncWorkflowsTemplates.ContainsValue(workFlow.Template))
            {
                var candidates = Directory.GetFiles(WorkingFolderTemplates, Path.GetFileName(workFlow.Template));
                if (candidates.Count() == 1)
                {
                    workFlow.Template = candidates[0];
                }
            }

            this.CurrentSyncWorkflowTemplate = workFlow.Template;
            this.RaisePropertyChanged("CurrentSyncWorkflowData");

            this.SetupPropertyChanged(true);
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

            Tools.SaveToFile(workFlow, fileName);

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
                this.ProcessingEvent(this, new ProcessingEventArgs(string.Format(CultureInfo.CurrentCulture, Properties.Resources.InstalledFileNotFound, templateScriptPath)));
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
                                 UiProvider = this.UiProvider,
                             };

            engine.ProcessingEvent += this.HandleProcessingEvent;
            engine.ProgressEvent += this.ProgressEvent;

            try
            {
                engine.Execute(commands);
            }
            catch (ProcessAbortException)
            {
            }

            engine.ProcessingEvent -= this.HandleProcessingEvent;
            engine.ProgressEvent -= this.ProgressEvent;

            if (this.FinishedEvent != null)
            {
                this.FinishedEvent(new ProgressEventArgs { PercentageDone = 100 });
            }
        }

        /// <summary>
        /// Generates sample data - not yet completed
        /// TODO: Create workflow data files.
        /// </summary>
        public void GenerateSamples()
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
        public void DeleteWorkflowData(string path)
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
        private IEnumerable<ConnectorTypeDescription> ScanFiles(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                if (Path.GetFileName(file).ToUpperInvariant().IsOneOf("CONTACTVIEWER.DLL"))
                {
                    continue;
                }

                var types = new Type[0];

                try
                {
                    // todo: check if the dll is a loadable assembly
                    var assembly = Assembly.LoadFile(file);
                    types = assembly.GetExportedTypes();
                }
                catch (FileNotFoundException ex)
                {
                    this.HandleProcessingEvent(this, new ProcessingEventArgs(ex.Message));
                }
                catch (BadImageFormatException ex)
                {
                    this.HandleProcessingEvent(this, new ProcessingEventArgs(ex.Message));
                }

                foreach (var exportedType in types)
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

                    foreach (var type in new[] { typeof(StdContact), typeof(StdCalendarItem) })
                    {

                        var fullName =
                            exportedType.IsGenericType
                                ? exportedType.FullName.Replace("`1", " of " + type.Name)
                                : exportedType.FullName;

                        var nameToUse = attribute.DisplayName ?? fullName.Replace("`1", "");

                        if (!attribute.Internal && (attribute.CanRead(type) || attribute.CanWrite(type)))
                        {
                            yield return
                                new ConnectorTypeDescription
                                    {
                                        ClassName = fullName,
                                        DisplayName = nameToUse,
                                        ReadWrite = attribute.CanRead(type) ? (attribute.CanWrite(type) ? 3 : 1) : 2,
                                        TypeName = type.Name
                                    };
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Handels the processing events.
        /// </summary>
        /// <param name="s"> The sender of this event. </param>
        /// <param name="e"> The <see cref="ProcessingEventArgs"/> of this event. </param>
        /// <exception cref="ProcessAbortException"> if the property <see cref="Cancel"/> is true </exception>
        private void HandleProcessingEvent(object s, ProcessingEventArgs e)
        {
            if (this.Cancel)
            {
                this.ProcessingEvent(this, new ProcessingEventArgs("Process aborted"));
                this.Cancel = false;
                throw new ProcessAbortException();
            }

            if (this.ProcessingEvent != null)
            {
                this.ProcessingEvent(s, e);
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
                    this.Source.PropertyChanged += this.HandlePropertyChangedEvent;
                }

                if (this.Target != null)
                {
                    this.Target.PropertyChanged += this.HandlePropertyChangedEvent;
                }
            }
            else
            {
                if (this.Source != null)
                {
                    this.Source.PropertyChanged -= this.HandlePropertyChangedEvent;
                }

                if (this.Target != null)
                {
                    this.Target.PropertyChanged -= this.HandlePropertyChangedEvent;
                }
            }
        }

        /// <summary>
        /// Handels property change events vy forwarding them
        /// </summary>
        /// <param name="sender"> The sender object ("Source" or "Target"). </param>
        /// <param name="e"> The event argument. </param>
        private void HandlePropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged((sender == this.Source ? "Source." : "Target.") + e.PropertyName);
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

    internal class ConnectorTypeDescription
    {
        public string ClassName;
        public string DisplayName;
        public int ReadWrite;
        public string TypeName;
    }
}