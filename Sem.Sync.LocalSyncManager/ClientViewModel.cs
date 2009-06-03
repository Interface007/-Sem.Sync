﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientViewModel.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Implements the functionality of the main program for syncing
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.LocalSyncManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    using SharedUI.WinForms.Tools;

    using SyncBase;
    using SyncBase.Binding;
    using SyncBase.EventArgs;
    
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
            this.engine.ProcessingEvent += this.ProcessingEvent;
            this.engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            this.engine.ProgressEvent += this.ProgressEvent;
            this.engine.Execute(this.SyncCommands);
            this.engine.ProgressEvent -= this.ProgressEvent;
            this.engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            this.engine.ProcessingEvent -= this.ProcessingEvent;
            return;
        }

        internal void Execute(SyncDescription item)
        {
            this.engine.ProcessingEvent += this.ProcessingEvent;
            this.engine.QueryForLogOnCredentialsEvent += this.QueryForLogOnCredentials;
            this.engine.ProgressEvent += this.ProgressEvent;
            this.engine.Execute(item);
            this.engine.ProgressEvent -= this.ProgressEvent;
            this.engine.QueryForLogOnCredentialsEvent -= this.QueryForLogOnCredentials;
            this.engine.ProcessingEvent -= this.ProcessingEvent;
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
