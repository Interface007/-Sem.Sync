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

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.Test.DataGenerator;

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
        private BackgroundWorker Worker;

        public int MaxEntries { get; set; }

        public ChangeInfo.BindingList DetectedChanges { get; private set; }

        public IEnumerable<ConnectorInformation> Connectors { get; private set; }

        public event EventHandler DataChanged;

        public bool Abort { get; set; }

        public CheckAgent()
        {
            this.Connectors = new List<ConnectorInformation>();
            this.DetectedChanges = new ChangeInfo.BindingList();
            this.MaxEntries = 10;

            this.Worker = new BackgroundWorker();
            this.Worker.DoWork += this.Worker_DoWork;
            this.Worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                var engine = new SyncEngine();
                var listOfSources = Tools.LoadFromFile<List<SyncDescription>>("sources.xml");

                if (listOfSources == null)
                {
                    listOfSources = new List<SyncDescription>();
                    listOfSources.Add(
                        new SyncDescription
                        {
                            SourceConnector = "Sem.Sync.Test.DataGenerator",
                            BaselineConnector = "Sem.Sync.Test.DataGenerator",
                        });
                    Tools.SaveToFile(listOfSources, "sources.xml");
                }

                Thread.Sleep(2000);

                foreach (var syncDescription in listOfSources)
                {
                    var sourceList = engine.SetupConnector(
                        syncDescription.SourceConnector,
                        syncDescription.SourceCredentials).GetAll(syncDescription.SourceStorePath).ToContacts();

                    var targetList = engine.SetupConnector(
                        syncDescription.TargetConnector,
                        syncDescription.TargetCredentials).GetAll(syncDescription.TargetStorePath).ToContacts();

                    var contactsToCompare = from s in sourceList
                                            join t in targetList
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

                    if (this.DataChanged != null)
                    {
                        this.DataChanged(this, new EventArgs());
                    }
                }
            }
            while (!this.Abort);
        }

        private void CompareEntities(StdContact oldContact, StdContact newContact)
        {
            var changes =
                SyncTools.DetectConflicts(
                    SyncTools.BuildConflictTestContainerList(
                        new List<StdElement>(new[] { newContact }),
                        new List<StdElement>(new[] { oldContact }),
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

            changeSet.DisplayName = string.Format("{0} has {1} properties changed.", oldContact.Name, changeSet.ChangedProperties.Count);
            this.DetectedChanges.Add(changeSet);

            while (this.DetectedChanges.Count > this.MaxEntries)
            {
                this.DetectedChanges.RemoveAt(0);
            }
        }
    }
}
