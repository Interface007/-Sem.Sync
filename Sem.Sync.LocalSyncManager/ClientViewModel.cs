using Sem.Sync.SharedUI.WinForms.UI;

namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    
    using SyncBase;
    using SyncBase.Binding;
    using SyncBase.Interfaces;
    using SyncBase.EventArgs;

    using Tools;
    
    /// <summary>
    /// Implements the functionality of the main program for syncing
    /// </summary>
    public class ClientViewModel : IMergeConflictResolver
    {
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;
        public event EventHandler<ProgressEventArgs> ProgressEvent;
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLoginCredentials;

        public List<String> SyncCommandLists { get; set; }
        internal SyncCollection SyncCommands { get; set; }
        internal string Status { get; set; }

        public readonly SyncEngine Engine = new SyncEngine
                                             {
                                                 WorkingFolder = Config.WorkingFolder,
                                                 ConflictSolver = new MergeWindow()
                                             };

        internal ClientViewModel()
        {
            this.SyncCommandLists = new List<string>();
            this.SyncCommands = new SyncCollection();
        }

        internal bool Execute()
        {
            Engine.ProcessingEvent += this.ProcessingEvent;
            Engine.QueryForLogOnCredentialsEvent += this.QueryForLoginCredentials;
            Engine.ProgressEvent += ProgressEvent;
            var result = this.Engine.Execute(this.SyncCommands);
            Engine.ProgressEvent -= ProgressEvent;
            Engine.QueryForLogOnCredentialsEvent -= this.QueryForLoginCredentials;
            Engine.ProcessingEvent -= this.ProcessingEvent;
            return result;
        }

        internal bool Execute(SyncDescription item)
        {
            Engine.ProcessingEvent += this.ProcessingEvent;
            Engine.QueryForLogOnCredentialsEvent += this.QueryForLoginCredentials;
            Engine.ProgressEvent += ProgressEvent;
            var result = this.Engine.Execute(item);
            Engine.ProgressEvent -= ProgressEvent;
            Engine.QueryForLogOnCredentialsEvent -= this.QueryForLoginCredentials;
            Engine.ProcessingEvent -= this.ProcessingEvent;
            return result;
        }

        internal void LoadSyncList(string p)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));
            var file = new FileStream(p, FileMode.Open);
            this.SyncCommands = (SyncCollection)formatter.Deserialize(file);
            file.Close();
        }

        public List<StdElement> PerformMerge(List<SyncBase.Merging.MergeConflict> toMerge, List<StdElement> targetList)
        {
            var ui = new MergeWindow();
            ui.PerformMerge(toMerge, targetList);

            return null;
        }
    }
}
