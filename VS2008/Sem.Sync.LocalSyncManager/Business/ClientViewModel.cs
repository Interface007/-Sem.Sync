// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientViewModel.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Implements the functionality of the main program for syncing
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.LocalSyncManager.Business
{
    using System;
    using System.Collections.Generic;

    using GenericHelpers.EventArgs;

    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Binding;

    using Tools;

    /// <summary>
    /// Implements the functionality of the main program for syncing
    /// </summary>
    public class ClientViewModel
    {
        /// <summary>
        /// The main engine to work with synchronization commands
        /// </summary>
        private readonly SyncEngine engine = new SyncEngine
            {
                WorkingFolder = Config.WorkingFolder,
                UiProvider = new UiDispatcher()
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientViewModel"/> class.
        /// </summary>
        internal ClientViewModel()
        {
            this.SyncCommandLists = new List<string>();
            this.SyncCommands = new SyncCollection();
        }

        /// <summary>
        /// Event that fires for each processing status change
        /// </summary>
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;

        /// <summary>
        /// Event that fires when the progress of operations does change
        /// </summary>
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        /// Event for querying log on credentials (connector sources or proxy)
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentials;

        /// <summary>
        /// Gets or sets the list of SyncCommand-Lists read from the file system.
        /// </summary>
        public List<string> SyncCommandLists { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="SyncDescription"/>.
        /// </summary>
        internal SyncCollection SyncCommands { get; set; }

        /// <summary>
        /// Opens the working folder in Windows Ecplorer using a sync command 
        /// </summary>
        public void OpenWorkingFolder()
        {
            this.engine.Execute(
                new SyncDescription
                {
                    Command = SyncCommand.OpenDocument.ToString(),
                    CommandParameter = "{FS:WorkingFolder}"
                });
        }
        
        /// <summary>
        /// Executes the current list of sync commands
        /// </summary>
        internal void Execute()
        {
            this.engine.ProcessingEvent += this.ProcessingEvent;
            this.engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            this.engine.ProgressEvent += this.ProgressEvent;
            var success = this.engine.Execute(this.SyncCommands);
            this.engine.ProgressEvent -= this.ProgressEvent;
            this.engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            this.engine.ProcessingEvent -= this.ProcessingEvent;

            if (!success)
            {
                this.ProcessingEvent(null, new ProcessingEventArgs { Message = "processing canceled" });
            }
        }

        /// <summary>
        /// Executes a single command
        /// </summary>
        /// <param name="item"> The item to be executed. </param>
        internal void Execute(SyncDescription item)
        {
            this.engine.ProcessingEvent += this.ProcessingEvent;
            this.engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            this.engine.ProgressEvent += this.ProgressEvent;
            var success = this.engine.Execute(item);
            this.engine.ProgressEvent -= this.ProgressEvent;
            this.engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            this.engine.ProcessingEvent -= this.ProcessingEvent;

            if (!success)
            {
                this.ProcessingEvent(null, new ProcessingEventArgs { Message = "processing canceled" });
            }
        }
    }
}