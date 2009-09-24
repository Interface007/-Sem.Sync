﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.ActiveDirectory
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.ActiveDirectory;
    using System.IO;
    using System.Text;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    /// <summary>
    /// connector to Active Directory via LDAP
    /// </summary>
    [ClientStoragePathDescription(ReferenceType = ClientPathType.Undefined, Mandatory = true)]
    [ConnectorDescription(CanRead = true, CanWrite = false, NeedsCredentials = true,
        DisplayName = "Active Directory", MatchingIdentifier = ProfileIdentifierType.ActiveDirectoryId)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// registry path to store credentials
        /// </summary>
        private const string RegBasePath = "software\\Sem.Sync\\ActiveDirectoryConnector";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
        {
            this.DumpPath = this.GetConfigValue("DumpPath");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class and initializes the
        /// path to save the data to.
        /// </summary>
        /// <param name="dumpDataToFolder"> The folder that should get the data. </param>
        public ContactClient(string dumpDataToFolder)
        {
            this.DumpPath = dumpDataToFolder;
        }

        /// <summary>
        /// Gets the user friendly name of this connector
        /// </summary>
        public override string FriendlyClientName
        {
            get { return "Active-Directory-Connector"; }
        }

        /// <summary>
        /// Gets or sets path to be used to save the data (might be null or empty).
        /// </summary>
        private string DumpPath { get; set; }

        /// <summary>
        /// writes a list of contacts to the active directory
        /// </summary>
        /// <param name="elements">the list of stdcontact elements</param>
        /// <param name="clientFolderName">not used in this implementation</param>
        /// <param name="skipIfExisting">determines whether existing entries should be overwritten or only new entries will be cretated</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a full list depending on the clientFolderName parameter.
        /// </summary>
        /// <param name="clientFolderName">a filter expression for the active directory query</param>
        /// <param name="result">a list to add the resulting contacts</param>
        /// <returns>the list with added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            try
            {
                // we should have a look for "good" credentials
                this.LogProcessingEvent("preparing log on");
                this.PrepareCredentials();

                // if domainController does not contain a "." we assume this to be a domain name 
                // rather than a domain controller, so we lookup the first controller we can get
                this.LogProcessingEvent("detecting domain controller");
                var domainController = this.LogOnDomain;
                if (!string.IsNullOrEmpty(domainController) && !domainController.Contains("."))
                {
                    domainController = GetDCs(domainController)[0];
                }

                // open the directory using explicit or implicit credentials
                this.LogProcessingEvent("opening ldap connection");
                var entry =
                    string.IsNullOrEmpty(this.LogOnPassword)
                    ? new DirectoryEntry("LDAP://" + domainController)
                    : new DirectoryEntry("LDAP://" + domainController, this.LogOnUserId, this.LogOnPassword);

                var search = new DirectorySearcher(entry)
                {
                    Filter = clientFolderName
                };

                this.LogProcessingEvent("receiving data ...");
                var resultList = search.FindAll();

                foreach (SearchResult searchItem in resultList)
                {
                    var newContact = ConvertToContact(searchItem);

                    if (string.IsNullOrEmpty(newContact.ToStringSimple()))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(this.DumpPath))
                    {
                        DumpUserInformation(
                            searchItem,
                            Path.Combine(this.DumpPath, SyncTools.NormalizeFileName(newContact.ToStringSimple()) + ".txt"));
                    }

                    this.LogProcessingEvent(newContact, "adding new element");
                    result.Add(newContact);
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(ex.Message);
            }

            return result;
        }
        
        /// <summary>
        /// lookup the list of domain controllers for the specified domain
        /// </summary>
        /// <param name="domainName">
        /// The domain Name to get the DC for.
        /// </param>
        /// <returns>
        /// The list of DCs in this domain.
        /// </returns>
        private static List<string> GetDCs(string domainName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domainName);
            var domain = Domain.GetDomain(context);
            var domainControllerList = new List<string>();

            foreach (DomainController server in domain.DomainControllers)
            {
                domainControllerList.Add(server.Name);
            }

            return domainControllerList;
        }
        
        /// <summary>
        /// Extract contact information from an Active Directory entry
        /// </summary>
        /// <param name="searchItem">the Active Directory entry to process</param>
        /// <returns>a standard contact entity</returns>
        private static StdContact ConvertToContact(SearchResult searchItem)
        {
            return new StdContact
            {
                Id = Guid.NewGuid(),
                InternalSyncData = new SyncData
                {
                    DateOfCreation = GetPropDate(searchItem.Properties, "whencreated"),
                    DateOfLastChange = GetPropDate(searchItem.Properties, "whenchanged"),
                },
                BusinessAddressPrimary = new AddressDetail
                {
                    CountryName = GetPropString(searchItem.Properties, "co"),
                    StateName = GetPropString(searchItem.Properties, "st"),
                    PostalCode = GetPropString(searchItem.Properties, "postalcode"),
                    CityName = GetPropString(searchItem.Properties, "l"),
                    StreetName = GetPropString(searchItem.Properties, "streetaddress"),
                    Phone = new PhoneNumber(GetPropString(searchItem.Properties, "telephonenumber")),
                    Room = GetPropString(searchItem.Properties, "physicaldeliveryofficename", "roomnumber"),
                },
                BusinessPhoneMobile = new PhoneNumber(GetPropString(searchItem.Properties, "mobile")),
                BusinessPosition = GetPropString(searchItem.Properties, "title"),
                BusinessCompanyName = GetPropString(searchItem.Properties, "company"),
                BusinessDepartment = GetPropString(searchItem.Properties, "department"),
                BusinessEmailPrimary = GetPropString(searchItem.Properties, "mail"),

                PersonalAddressPrimary = new AddressDetail
                {
                    Phone = new PhoneNumber(GetPropString(searchItem.Properties, "homephone")),
                },
                Name = new PersonName
                {
                    FirstName = GetPropString(searchItem.Properties, "givenname"),
                    LastName = GetPropString(searchItem.Properties, "sn"),
                },
                PersonGender =
                    SyncTools.GenderByText(GetPropString(searchItem.Properties, "personaltitle")),

                AdditionalTextData = GetPropString(searchItem.Properties, "info"),

                PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.ActiveDirectoryId, GetPropString(searchItem.Properties, "CN"))
            };
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection">the result property collection to search</param>
        /// <param name="propName">the name of the property to extract</param>
        /// <returns>the string that has been extracted</returns>
        private static string GetPropString(ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0 &&
                thePropertyCollection[propName].Count > 0)
            {
                return thePropertyCollection[propName][0].ToString();
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection">the result property collection to search</param>
        /// <param name="propNamesByPriotity">the name of the property to extract</param>
        /// <returns>the string that has been extracted</returns>
        private static string GetPropString(ResultPropertyCollection thePropertyCollection, params string[] propNamesByPriotity)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0)
            {
                foreach (var name in propNamesByPriotity)
                {
                    if (thePropertyCollection[name].Count > 0)
                    {
                        return thePropertyCollection[name][0].ToString();
                    }
                }                
            }

            return null;
        }

        /// <summary>
        /// extracts the first element of a property collection as string
        /// </summary>
        /// <param name="thePropertyCollection">the result property collection to search</param>
        /// <param name="propName">the name of the property to extract</param>
        /// <returns>the date that has been extracted</returns>
        private static DateTime GetPropDate(ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0 &&
                thePropertyCollection[propName].Count > 0)
            {
                return (DateTime)thePropertyCollection[propName][0];
            }

            return new DateTime();
        }

        /// <summary>
        /// dump active directory information to the file system.
        /// </summary>
        /// <param name="searchItem">the active directory entry to process</param>
        /// <param name="path">the file system path to write to</param>
        private static void DumpUserInformation(SearchResult searchItem, string path)
        {
            var content = new StringBuilder();

            Tools.EnsurePathExist(Path.GetDirectoryName(path));

            foreach (var name in searchItem.Properties.PropertyNames)
            {
                foreach (var propItem in searchItem.Properties[name.ToString()])
                {
                    content.AppendLine(name + " ... " + propItem);
                }
            }

            File.WriteAllText(path, content.ToString());
        }

        /// <summary>
        /// retrieves credentials from the registry if there is something configured,
        /// otherwise we will ask the user or use the currently loged in user account
        /// </summary>
        private void PrepareCredentials()
        {
            if (string.IsNullOrEmpty(this.LogOnPassword))
            {
                this.LogOnDomain = Tools.GetRegValue(RegBasePath, "domainName", string.Empty);
                this.LogOnUserId = Tools.GetRegValue(RegBasePath, "username", "{default}");
                this.LogOnPassword = Tools.GetRegValue(RegBasePath, "password", "{ask}");
            }

            // check if the user an empty string, in this case ask the user for credentials
            if (string.IsNullOrEmpty(this.LogOnUserId) || this.LogOnPassword == "{ask}")
            {
                this.LogOnPassword = string.Empty;
                this.QueryForLogOnCredentials("Please provide logon credentials for LDAP query.\nPress cancle to use current user.");
            }

            // if we have a user name and it does contain a backslash, we need to split the domain from the user name
            if (!string.IsNullOrEmpty(this.LogOnUserId) && this.LogOnUserId.Contains("\\"))
            {
                this.LogOnDomain = this.LogOnUserId.Split('\\')[0];
                this.LogOnUserId = this.LogOnUserId.Split('\\')[1];
            }

            // if the user id is {default} we need to get the domain from the currently logged in user
            if (!string.IsNullOrEmpty(this.LogOnUserId) && this.LogOnUserId != "{default}")
            {
                return;
            }

            var currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (currentIdentity != null)
            {
                this.LogOnDomain = currentIdentity.Name;
            }

            if (!string.IsNullOrEmpty(this.LogOnDomain) && this.LogOnDomain.Contains("\\"))
            {
                this.LogOnDomain = this.LogOnDomain.Split('\\')[0];
            }
        }
    }
}