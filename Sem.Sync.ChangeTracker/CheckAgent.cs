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
    using System.Collections.Generic;
    using System.Linq;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;
    using System.ComponentModel;
    using System.Threading;

    using Test.DataGenerator;

    internal class CheckAgent
    {
        private BackgroundWorker Worker;

        public IEnumerable<ChangeInfo> DetectedChanges { get; private set; }

        public IEnumerable<ConnectorInformation> Connectors { get; private set; }

        public bool Abort { get; set; }

        public CheckAgent()
        {
            this.Connectors = new List<ConnectorInformation>();
            this.DetectedChanges = new List<ChangeInfo>();

            this.Worker = new BackgroundWorker();
            this.Worker.DoWork += this.Worker_DoWork;
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                Thread.Sleep(2000);
                var sourceList = Contacts.GetStandardContactList(false);
                var targetList = Contacts.GetStandardContactList(false);

                var contactsToCompare = from s in sourceList
                                        join t in targetList 
                                        on s.PersonalProfileIdentifiers 
                                        equals t.PersonalProfileIdentifiers
                                        select new
                                            {
                                                source = s, 
                                                target = t
                                            };
                
                foreach (var toCompare in contactsToCompare)
                {
                    this.CompareEntities(toCompare.source, toCompare.target);
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

            if (changeSet.ChangedProperties.Count > 0)
            {
                ((IList<ChangeInfo>)this.DetectedChanges).Add(changeSet);
            }
        }
    }
}
