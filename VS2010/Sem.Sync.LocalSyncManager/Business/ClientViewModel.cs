// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientViewModel.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Implements the functionality of the main program for syncing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager.Business
{
    using System;
    using System.Collections.Generic;

    using Sem.GenericHelpers.EventArgs;
    using Sem.Sync.LocalSyncManager.Tools;
    using Sem.Sync.SharedUI.WinForms.Tools;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Binding;

    /// <summary>
    /// Implements the functionality of the main program for syncing
    /// </summary>
    public class ClientViewModel
    {
        #region Constants and Fields

        /// <summary>
        ///   The main engine to work with synchronization commands
        /// </summary>
        private readonly SyncEngine engine = new SyncEngine
            {
               WorkingFolder = Config.WorkingFolder, UiProvider = new UiDispatcher() 
            };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ClientViewModel" /> class.
        /// </summary>
        internal ClientViewModel()
        {
            this.SyncCommandLists = new List<string>();
            this.SyncCommands = new SyncCollection();
        }

        #endregion

        #region Events

        /// <summary>
        ///   Event that fires for each processing status change
        /// </summary>
        public event EventHandler<ProcessingEventArgs> ProcessingEvent;

        /// <summary>
        ///   Event that fires when the progress of operations does change
        /// </summary>
        public event EventHandler<ProgressEventArgs> ProgressEvent;

        /// <summary>
        ///   Event for querying log on credentials (connector sources or proxy)
        /// </summary>
        public event EventHandler<QueryForLogOnCredentialsEventArgs> QueryForLogOnCredentials;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the list of SyncCommand-Lists read from the file system.
        /// </summary>
        public List<string> SyncCommandLists { get; set; }

        /// <summary>
        ///   Gets or sets a collection of <see cref = "SyncDescription" />.
        /// </summary>
        internal SyncCollection SyncCommands { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens the working folder in Windows Ecplorer using a sync command
        /// </summary>
        public void OpenWorkingFolder()
        {
            this.engine.OpenWorkingFolder();
        }

        #endregion

        #region Methods

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
        /// <param name="item">
        /// The item to be executed. 
        /// </param>
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

        #endregion
    }
}