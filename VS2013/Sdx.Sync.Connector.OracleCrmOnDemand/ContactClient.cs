// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sdx.Sync.Connector.OracleCrmOnDemand.ContactSR;
    using Sdx.Sync.Connector.OracleCrmOnDemand.Helpers;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Implements a read/write connector to "Oracle CRM on Demand"
    /// </summary>
    [ClientStoragePathDescription(
        ReferenceType = ClientPathType.DialogBased,
        WinformsConfigurationClass = typeof(ContactClientConfiguration))]
    [ConnectorDescription(
        DisplayName = "Oracle CRM On Demand",
        CanReadContacts = true,
        NeedsCredentials = true,
        MatchingIdentifier = ProfileIdentifierType.OracleCrmOnDemandId)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// There is an undocumented limit of 1000 updates per session, so we
        /// need to open a new session each 1000 updates
        /// </summary>
        public const int UndocumentedMaximumNumberOfContactUpdatesPerSession = 1000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
        {
            Utils.IgnoreCertificateError(); 
        }

        /// <summary>
        /// This method does NOT get all elements before adding the new element. It will only match the element by the Id
        /// </summary>
        /// <param name="elements">list of all the elements to add</param>
        /// <param name="clientFolderName">the outlook folder to use</param>
        public override void AddRange(List<StdElement> elements, string clientFolderName)
        {
            this.WriteFullList(elements, clientFolderName, false);
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">The clientFolderName is currently ignored by the connector.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            this.LogProcessingEvent("starting read operation");
            var configurationData = this.GetConfigurationData(clientFolderName);
            var contactClient = new ContactAccess(this.LogOnDomain, configurationData);
            contactClient.ProcessingEvent += this.LogProcessingEvent;

            var contacts = null as IEnumerable<ContactData>;
            ////contacts = Tools.LoadFromFile<List<ContactData>>("exportedContacts.xml");
            this.LogProcessingEvent("login using credentials for {0}...", this.LogOnUserId);
            if (contactClient.LogOn(this.LogOnUserId, this.LogOnPassword))
            {
                this.LogProcessingEvent("login succeeded");
                this.LogProcessingEvent("reading data ...");

                // read from oracle and save to cache file
                contacts = contactClient.QueryContactsByFilter(configurationData.FilterList);
                ////Tools.SaveToFile(contacts, "exportedContacts.xml");

                this.LogProcessingEvent("log off ...");
                contactClient.LogOff();
            }

            this.LogProcessingEvent("converting data ...");
            if (contacts != null)
            {
                contacts.ForEach(x => result.Add(x.ToStdContact(configurationData.GetAllAttributes)));
            }

            this.LogProcessingEvent("cleaning entities ...");
            CleanUpEntities(result);

            Tools.DebugWriteLine("{0} Finished Read process ({1} entries)...", DateTime.Now, result.Count);
            contactClient.ProcessingEvent -= this.LogProcessingEvent;
            return result;
        }

        /// <summary>
        /// Virtual write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written -
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.LogProcessingEvent("starting update operation");
            var configurationData = this.GetConfigurationData(clientFolderName);
            var contactClient = new ContactAccess(this.LogOnDomain, configurationData);
            contactClient.ProcessingEvent += this.LogProcessingEvent;

            this.LogProcessingEvent("start writing {0} contacts");

            // map stdelement -> List<contact>, we will need to do multiple linq queries against the local version,
            // so we manifest the list into a concrete form instead of just defining the query.
            this.LogProcessingEvent("converting data ...");
            var localVersionsOfContacts = (from element in elements 
                                           select element.ToOracleContact(configurationData.GetAllAttributes))
                                           .ToList();
            this.LogProcessingEvent("finished converting data ...");

            IEnumerable<ContactData> contactsToSendForUpdate;
            var contactsAlreadySent = 0;
            var totalNumberOfContacts = localVersionsOfContacts.Count();

            // the oracle web-service throws a "Bad Request 400" exception if you try to
            // update more than 1000 contacts per session, so you need to divide the full
            // list into smaller pieces
            while (
                (contactsToSendForUpdate = localVersionsOfContacts
                                            .Skip(contactsAlreadySent)
                                            .Take(UndocumentedMaximumNumberOfContactUpdatesPerSession))
                .Count() > 0)
            {
                this.LogProcessingEvent("login using credentials for {0}...", this.LogOnUserId);
                if (!contactClient.LogOn(this.LogOnUserId, this.LogOnPassword))
                {
                    this.LogProcessingEvent("login failed - aborting process");
                    break;
                }

                this.LogProcessingEvent("login succeeded");
                this.LogProcessingEvent("writing next {0} contacts. {1} of {2} contacts written by now.", contactsToSendForUpdate.Count(), contactsAlreadySent, totalNumberOfContacts);

                contactClient.UpdateContacts(contactsToSendForUpdate);
                contactsAlreadySent += contactsToSendForUpdate.Count();

                this.LogProcessingEvent("done writing {0} contacts. {1} of {2} contacts written by now", contactsToSendForUpdate.Count(), contactsAlreadySent, totalNumberOfContacts);
                this.LogProcessingEvent("log off ...");
                contactClient.LogOff();
            }

            contactClient.ProcessingEvent -= this.LogProcessingEvent;
        }

        /// <summary>
        /// Gets the extended configuration or default values (in case of <paramref name="clientFolderName"/> not being a 
        /// serialized set of extended configuration properties)
        /// </summary>
        /// <param name="clientFolderName"> The client folder name specification of the extended configuration as xml. </param>
        /// <returns> the extended configuration object </returns>
        private ContactClientConfigurationData GetConfigurationData(string clientFolderName)
        {
            ContactClientConfigurationData contactClientConfigurationData;
            if (clientFolderName.StartsWith("<", StringComparison.Ordinal))
            {
                contactClientConfigurationData = Tools.LoadFromString<ContactClientConfigurationData>(clientFolderName);
            }
            else
            {
                contactClientConfigurationData = new ContactClientConfigurationData(clientFolderName)
                {
                    GetAllAttributes = this.GetConfigValueBoolean("GetAllAttributes"),
                    PageSize = this.GetConfigValueInt("PageSize", 100)
                };
            }

            return contactClientConfigurationData;
        }
    }
}