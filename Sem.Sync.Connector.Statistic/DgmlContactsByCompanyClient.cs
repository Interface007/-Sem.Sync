// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompanyClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This client is a write only client that aggregates the information to some statistical information.
//   the following image shows the resulting graph of such an analysis:
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Statistic
{
    using System.Collections.Generic;
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
    [ConnectorDescription(
        CanReadContacts = false, CanWriteContacts = true, 
        CanReadCalendarEntries = false, CanWriteCalendarEntries = false, 
        IsGeneric = false, NeedsCredentials = false, 
        DisplayName = "DGML Graph")]
    [ClientStoragePathDescription(ReferenceType = ClientPathType.FileSystemFileNameAndPath, Default = "diagram.dgml", Mandatory = true)]
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
            var stdContacts = elements.ToStdContacts().Where(x => x.Name != null).ToList();

            // get all with incoming and outgoing connections
            var connected = (from x in stdContacts where x.Contacts != null from y in x.Contacts select y.Target).Distinct().ToList();
            connected.AddRange(from x in stdContacts where x.Contacts != null && x.Contacts.Count > 0 select x.Id);
            connected = connected.Distinct().ToList();
            stdContacts = (from x in stdContacts where connected.Contains(x.Id) select x).ToList();
            var categories = (from x in stdContacts where !string.IsNullOrWhiteSpace(x.BusinessCompanyName) select x.BusinessCompanyName).Distinct();
            
            this.LogProcessingEvent("building graph...");
            
            var graph = new DgmlDirectedGraph
                {
                    Layout = DgmlLayout.ForceDirected,
                    Categories = new List<DgmlCategory>(from x in categories select new DgmlCategory(x)),
                    Nodes = new List<DgmlNode>(from x in stdContacts select new DgmlNode(x, x.BusinessCompanyName)), 
                    Links = new List<DgmlLink>(
                        from x in stdContacts 
                        where x.Contacts != null 
                        from y in x.Contacts 
                        where 
                        stdContacts.Exists(z => z.Id == y.Target)
                        && x.Id != y.Target
                        select new DgmlLink(x.Id, y.Target)),
                };

            var selector = clientFolderName.Contains("|") ? clientFolderName.Split('|')[1] : string.Empty;
            if (!string.IsNullOrWhiteSpace(selector))
            {
                var distinctGroups = (from x in stdContacts select Tools.GetPropertyValueString(x, selector)).Distinct();
                graph.Nodes.AddRange(
                    from y in distinctGroups
                    select new DgmlNode("Group@" + y, "Collapsed", y));

                graph.Links.AddRange(
                    from x in stdContacts
                    select new DgmlLink("Group@" + Tools.GetPropertyValueString(x, selector), "Contains", x.Id.ToString("N")));
            }

            this.LogProcessingEvent("saving statistic file...");
            var fileName = clientFolderName.Contains("|") ? clientFolderName.Split('|')[0] : clientFolderName;
            Tools.SaveToFile(graph, fileName);

            this.LogProcessingEvent("writing finished");
        }

        #endregion
    }
}
