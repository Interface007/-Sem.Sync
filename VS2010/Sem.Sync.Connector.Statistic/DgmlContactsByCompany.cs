// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DgmlContactsByCompany.cs" company="Sven Erik Matzen">
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
    using System.Drawing;
    using System.Linq;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.Statistic.Dgml;
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
    [ClientStoragePathDescription(
        ReferenceType = ClientPathType.FileSystemFileNameAndPath, 
        Default = "diagram.dgml", 
        Mandatory = true,
        WinformsConfigurationClass = typeof(DgmlContactsByCompanyConfiguration))]
    public class DgmlContactsByCompany : StdClient
    {
        /// <summary>
        /// prefix for company name categories. The result will be "Company Name: MyCompany" as a label and ID
        /// </summary>
        private const string NodesCategoryPrefix = "Company Name: ";

        /// <summary>
        /// Name of the link category for private relationship links
        /// </summary>
        private const string LinkCategoryPrivate = "LinkPrivate";

        /// <summary>
        /// Name of the link category for business relationship links
        /// </summary>
        private const string LinkCategoryBusiness = "LinkBusiness";
            
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
        /// Writing will write a DGML file.
        /// </summary>
        /// <param name="elements"> the list of elements that should be written to the target system. </param>
        /// <param name="clientFolderName"> The full path to the destination DGML file. </param>
        /// <param name="skipIfExisting"> not implemented </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.LogProcessingEvent("preparing data...");
            var data = clientFolderName.StartsWith("<") 
                ? Tools.LoadFromString<DgmlContactsByCompanyConfigurationData>(clientFolderName) 
                : new DgmlContactsByCompanyConfigurationData { DestinationPath = clientFolderName };

            elements.ForEach(x => x.NormalizeContent());
            var stdContacts = elements.ToStdContacts().Where(x => x.Name != null).ToList();

            // get all with incoming and outgoing connections
            var connected = (from x in stdContacts where x.Contacts != null from y in x.Contacts select y.Target).Distinct().ToList();
            connected.AddRange(from x in stdContacts where x.Contacts != null && x.Contacts.Count > 0 select x.Id);
            connected = connected.Distinct().ToList();
            stdContacts = (from x in stdContacts where connected.Contains(x.Id) select x).ToList();
            var categories = (from x in stdContacts where !string.IsNullOrWhiteSpace(x.BusinessCompanyName) select NodesCategoryPrefix + x.BusinessCompanyName).Distinct().ToList();
            
            this.LogProcessingEvent("building graph...");

            var graph = new Graph
                {
                    Layout = Layout.ForceDirected,
                    Categories = new List<Category>(from x in categories select new Category(x)),
                    Nodes = new List<Node>(from x in stdContacts select new Node(x, NodesCategoryPrefix + x.BusinessCompanyName)), 
                    Links = new List<Link>(
                        from x in stdContacts 
                        where x.Contacts != null 
                        from y in x.Contacts 
                        where 
                        stdContacts.Exists(z => z.Id == y.Target)
                        && x.Id != y.Target
                        && y.IsBusinessContact
                        select new Link(x.Id, y.Target, LinkCategoryBusiness)),
                };

            graph.Links.AddRange(
                        from x in stdContacts
                        where x.Contacts != null
                        from y in x.Contacts
                        where
                        stdContacts.Exists(z => z.Id == y.Target)
                        && x.Id != y.Target
                        && y.IsPrivateContact
                        select new Link(x.Id, y.Target, LinkCategoryPrivate));

            graph.Categories.Add(new Category(LinkCategoryPrivate, Color.Red, Color.Red));
            graph.Categories.Add(new Category(LinkCategoryBusiness, Color.Blue, Color.Blue));

            if (!string.IsNullOrWhiteSpace(data.GroupingPropertName))
            {
                var distinctGroups = (from x in stdContacts select Tools.GetPropertyValueString(x, data.GroupingPropertName)).Distinct();
                graph.Nodes.AddRange(
                    from y in distinctGroups
                    select new Node("Group@" + y, "Collapsed", y));

                graph.Links.AddRange(
                    from x in stdContacts
                    select new Link("Group@" + Tools.GetPropertyValueString(x, data.GroupingPropertName), "Contains", x.Id.ToString("N")));
            }

            this.LogProcessingEvent("saving statistic file...");
            var fileName = GetSplitElement(data.DestinationPath, 0, clientFolderName);
            Tools.SaveToFile(graph, fileName);

            this.LogProcessingEvent("writing finished");
        }

        /// <summary>
        /// Extracts an indexed element from a "|"-seperated list of elements in a string.
        /// </summary>
        /// <param name="valueToSplit"> The value string to be split. </param>
        /// <param name="idx"> The index of the element to get. </param>
        /// <param name="defaultValue"> The default value (if the element does not exist). </param>
        /// <returns> The element id the number of elements is less than the index </returns>
        private static string GetSplitElement(string valueToSplit, int idx, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(valueToSplit))
            {
                return defaultValue;
            }

            var parts = valueToSplit.Split('|');
            return parts.Length > idx ? parts[idx] : defaultValue;
        }

        #endregion
    }
}
