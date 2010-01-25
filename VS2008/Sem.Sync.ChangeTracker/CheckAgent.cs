// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckAgent.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the CheckAgent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;

    using GenericHelpers;
    using GenericHelpers.Entities;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// <para>The agent encapsulates a thread that does query the source and scans it for 
    /// changes since the last processing. It also provides a list for the changes that have been 
    /// found (<see cref="DetectedChanges"/>) that does provide binding capabilities and
    /// does raise an event <see cref="DataChanged"/> in case of found changes.</para>
    /// The list is automatically shrinked to a maximum of <see cref="MaxEntries"/> entries.
    /// </summary>
    internal class CheckAgent
    {
        /// <summary>
        /// This backgroundworker does provide the threading for scanning the 
        /// source in the background.
        /// </summary>
        private readonly BackgroundWorker worker;

        /// <summary>
        /// the last time the connectors have been checked.
        /// </summary>
        private DateTime lastRun = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckAgent"/> class.
        /// </summary>
        public CheckAgent()
        {
            this.Connectors = new List<ConnectorInformation>();
            this.DetectedChanges = new ChangeInfo.BindingList();
            this.MaxEntries = 10;

            this.worker = new BackgroundWorker();
            this.worker.DoWork += this.Worker_DoWork;
            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// The event of detecting changes - this will be fired once for a complete scan of a connector.
        /// </summary>
        public event EventHandler DataChanged;

        /// <summary>
        /// Gets or sets the maximum count of elements in the list - the oldest entries will be deleted first.
        /// </summary>
        public int MaxEntries { get; set; }

        /// <summary>
        /// Gets the list of changes that have been detected - this is a <see cref="BindingList{T}"/> for databinding.
        /// </summary>
        public ChangeInfo.BindingList DetectedChanges { get; private set; }

        /// <summary>
        /// Gets the list of connectors that will be queried.
        /// </summary>
        public IEnumerable<ConnectorInformation> Connectors { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to abort the workerthread.
        /// </summary>
        public bool Abort { get; set; }

        /// <summary>
        /// Loop for the worker thread to scan the sources for changes.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments for this thread. </param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                var engine = new SyncEngine();
                var listOfSources = Tools.LoadFromFile<List<SyncDescription>>("sources.xml");

                if (listOfSources == null)
                {
                    listOfSources = new List<SyncDescription>
                        {
                            new SyncDescription
                                {
                                    SourceConnector = "Sem.Sync.Test.DataGenerator.Contacts",
                                    SourceStorePath = "somepath",
                                    SourceCredentials = new Credentials
                                        {
                                            LogOnDomain = "the domain",
                                            LogOnUserId = "user name",
                                            LogOnPassword = "some pass"
                                        },
                                    BaselineConnector = "Sem.Sync.Test.DataGenerator.Contacts",
                                    BaselineStorePath = "somepath",
                                    BaselineCredentials = new Credentials
                                        {
                                            LogOnDomain = "the domain",
                                            LogOnUserId = "user name",
                                            LogOnPassword = "some pass"
                                        },
                                }
                        };

                    Tools.SaveToFile(listOfSources, "sources.xml");
                }

                // pause the thread - we don't need to query the connectors too often.
                Thread.Sleep(2000);

                if (this.lastRun.AddMinutes(10) >= DateTime.Now)
                {
                    continue;
                }

                foreach (var syncDescription in listOfSources)
                {
                    var sourceList = engine.SetupConnector(
                        syncDescription.SourceConnector,
                        syncDescription.SourceCredentials).GetAll(syncDescription.SourceStorePath).ToContacts();

                    var baselineConnector = engine.SetupConnector(
                        syncDescription.BaselineConnector, 
                        syncDescription.BaselineCredentials);

                    var baselineList = baselineConnector.GetAll(syncDescription.BaselineStorePath).ToContacts();

                    var contactsToCompare = from s in sourceList
                                            join t in baselineList
                                                on s.PersonalProfileIdentifiers
                                                equals t.PersonalProfileIdentifiers
                                            select new
                                                {
                                                    source = s,
                                                    Baseline = t
                                                };
                    foreach (var toCompare in contactsToCompare)
                    {
                        this.CompareEntities(toCompare.source, toCompare.Baseline);
                    }

                    baselineConnector.WriteRange(
                        sourceList.ToStdElement(), 
                        syncDescription.BaselineStorePath);

                    if (this.DataChanged != null)
                    {
                        this.DataChanged(this, new EventArgs());
                    }
                }
            }
            while (!this.Abort);
        }

        /// <summary>
        /// Compares two contact instances and reports the difference.
        /// </summary>
        /// <param name="baselineContact"> The contact from the base line (how it has been read last time). </param>
        /// <param name="currentContact"> The contact currently read from the source. </param>
        private void CompareEntities(StdContact baselineContact, StdContact currentContact)
        {
            var changes =
                SyncTools.DetectConflicts(
                    SyncTools.BuildConflictTestContainerList(
                        new List<StdElement>(new[] { currentContact }),
                        new List<StdElement>(new[] { baselineContact }),
                        null,
                        typeof(StdContact)),
                    true);

            var changeSet = new ChangeInfo();

            foreach (var change in changes)
            {
                changeSet.ChangedProperties.Add(
                    change.PathToProperty + " " +
                    change.PropertyConflict + ": " +
                    change.SourceElement);
            }

            if (changeSet.ChangedProperties.Count <= 0)
            {
                return;
            }

            changeSet.DisplayName = string.Format("{0} has {1} properties changed.", baselineContact.Name, changeSet.ChangedProperties.Count);
            this.DetectedChanges.Add(changeSet);

            while (this.DetectedChanges.Count > this.MaxEntries)
            {
                this.DetectedChanges.RemoveAt(0);
            }
        }
    }
}
