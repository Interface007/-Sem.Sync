// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompanyClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This client is a write only client that aggregates the information to some statistical information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.Statistic.DgmlContactsByCompany;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// This client is a write only client that aggregates the information to some statistical information.
    /// </summary>
    [ConnectorDescription(CanReadContacts = false, CanWriteContacts = true, CanReadCalendarEntries = false, 
        CanWriteCalendarEntries = true, IsGeneric = true, NeedsCredentials = false, DisplayName = "DGML Graph")]
    [ClientStoragePathDescription(ReferenceType = ClientPathType.FileSystemPath)]
    public class DgmlContactsByCompanyClient : StdClient
    {
        #region Public Methods

        /// <summary>
        /// Writes a range of elements to the standard connector.
        /// </summary>
        /// <param name="elements">
        /// The elements. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, true);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writing will write a simple XML with some statistical information.
        /// </summary>
        /// <param name="elements">
        /// the list of elements that should be written to the target system.
        /// </param>
        /// <param name="clientFolderName">
        /// the information to where inside the source the elements should be written - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="skipIfExisting">
        /// specifies whether existing elements should be updated or simply left as they are
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.LogProcessingEvent("preparing data...");
            elements.ForEach(x => x.NormalizeContent());
            var stdContacts = elements.ToStdContacts();

            // get all with incoming and outgoing connections
            var connected = (from x in stdContacts where x.Contacts != null from y in x.Contacts select y.Target).Distinct().ToList();
            connected.AddRange(from x in stdContacts where x.Contacts != null && x.Contacts.Count > 0 select x.Id);
            connected = connected.Distinct().ToList();

            stdContacts = (from contact in stdContacts where connected.Contains(contact.Id) select contact).ToList();

            this.LogProcessingEvent("building graph...");
            var graph = new DgmlDirectedGraph(stdContacts.ToStdElements())
                {
                    Links =
                        new List<DgmlLink>(
                        from x in stdContacts
                        where x.Contacts != null
                        from y in x.Contacts
                        select new DgmlLink(x.Id, y.Target)),
                    Layout = DgmlLayout.ForceDirected,
                };

            graph.Nodes.AddRange(from y in (from x in stdContacts select x.BusinessCompanyName).Distinct() select new DgmlNode("Company@" + y, "Collapsed", y));
            graph.Links.AddRange(from x in stdContacts select new DgmlLink("Company@" + x.BusinessCompanyName, "Contains", x.Id.ToString("N")));

            this.LogProcessingEvent("saving statistic file...");
            Tools.SaveToFile(graph, Path.Combine(clientFolderName, this.FriendlyClientName + ".dgml"));

            this.LogProcessingEvent("writing finished");
        }

        #endregion
    }
}
