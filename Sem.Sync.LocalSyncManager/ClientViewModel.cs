namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    
    using SyncBase;
    using SyncBase.Binding;
    using SyncBase.EventArgs;
    
    using SharedUI.WinForms.Tools;

    using Tools;
    
    /// <summary>
    /// Implements the functionality of the main program for syncing
    /// </summary>
    public class ClientViewModel
    {
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;
        public event EventHandler<ProgressEventArgs> ProgressEvent;
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentials;

        public List<String> SyncCommandLists { get; set; }
        internal SyncCollection SyncCommands { get; set; }

        private readonly SyncEngine engine = new SyncEngine
                                             {
                                                 WorkingFolder = Config.WorkingFolder,
                                                 ConflictSolver = new UiDispatcher()
                                             };

        internal ClientViewModel()
        {
            this.SyncCommandLists = new List<string>();
            this.SyncCommands = new SyncCollection();
        }

        internal void Execute()
        {
            engine.ProcessingEvent += this.ProcessingEvent;
            engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            engine.ProgressEvent += ProgressEvent;
            this.engine.Execute(this.SyncCommands);
            engine.ProgressEvent -= ProgressEvent;
            engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            engine.ProcessingEvent -= this.ProcessingEvent;
            return;
        }

        internal void Execute(SyncDescription item)
        {
            engine.ProcessingEvent += this.ProcessingEvent;
            engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            engine.ProgressEvent += ProgressEvent;
            this.engine.Execute(item);
            engine.ProgressEvent -= ProgressEvent;
            engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            engine.ProcessingEvent -= this.ProcessingEvent;
            return;
        }

        internal void LoadSyncList(string p)
        {
            var formatter = new XmlSerializer(typeof(SyncCollection));
            var file = new FileStream(p, FileMode.Open);
            this.SyncCommands = (SyncCollection)formatter.Deserialize(file);
            file.Close();
        }
    }
}
