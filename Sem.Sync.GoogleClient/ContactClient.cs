// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.GoogleClient
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GenericHelpers;

    using Google.Contacts;
    using Google.GData.Client;
    using Google.GData.Contacts;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(
        DisplayName = "Google Mail Contacts Client",
        CanRead = true,
        CanWrite = true,
        MatchingIdentifier = ProfileIdentifierType.Google,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        #region const
        /// <summary>
        /// The qualifier for the attribute "work"
        /// </summary>
        private const string GoogleSchemaQualifierWork = "work";

        /// <summary>
        /// The qualifier for the attribute "home"
        /// </summary>
        private const string GoogleSchemaQualifierHome = "home";
        
        /// <summary>
        /// The qualifier for the attribute "home"
        /// </summary>
        private const string GoogleSchemaQualifierMobile = "mobile";
        #endregion const

        #region members
        /// <summary>
        /// The settings instance
        /// </summary>
        private RequestSettings settings;
        
        /// <summary>
        /// The requester object for the google api
        /// </summary>
        private ContactsRequest requester;

        /// <summary>
        /// An URI representing the currently logged in user
        /// </summary>
        private Uri contactsUri;
        #endregion members

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Google-Contact-Client";
            }
        }

        /// <summary>
        /// logs the exception to the console and the <see cref="SyncComponent.LogProcessingEvent(object,Sem.GenericHelpers.EventArgs.ProcessingEventArgs)"/>.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        internal void LogError(Exception exception)
        {
            Console.WriteLine(exception + " : " + exception.Message);
            this.LogProcessingEvent("Error while executing client: {0}", exception.Message);
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            try
            {
                this.EnsureInitialization();

                var f = this.requester.GetContacts();
                foreach (var googleContact in f.Entries)
                {
                    try
                    {
                        // todo: the extended properties are not filled - find out why and how to solve that issue
                        var semSyncIdString = (from x in googleContact.ExtendedProperties where x.Name == "SemSyncId" select x.Value).FirstOrDefault();
                        var semSyncId = string.IsNullOrEmpty(semSyncIdString) ? Guid.NewGuid() : new Guid(semSyncIdString);

                        var stdEntry = new StdContact
                        {
                            Id = semSyncId,
                            Name = new PersonName(googleContact.Title),
                            PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.Google, googleContact.Id)
                        };

                        googleContact.SetSyncIdentifier(semSyncId);

                        // the Invoke is an extension method that calls the lambda for each element of an IEnumerable
                        googleContact.PostalAddresses.Invoke(x => stdEntry.SetAddress(x));
                        googleContact.Organizations.Invoke(x => stdEntry.SetBusiness(x));
                        googleContact.Phonenumbers.Invoke(x => stdEntry.SetPhone(x));
                        googleContact.Emails.Invoke(x => stdEntry.SetEmail(x));
                        
                        // downloads the image
                        stdEntry.SetPicture(googleContact, this.requester);

                        result.Add(stdEntry);
                    }
                    catch (GDataRequestException ex)
                    {
                        this.LogProcessingEvent("Error while executing client: {0}", ex.Message);
                    }
                }
            }
            catch (GDataRequestException ex)
            {
                this.LogProcessingEvent("Error while executing client: {0}", ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full name including path of the file that will get the contacts while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            this.EnsureInitialization();

            foreach (var stdContact in elements.ToContacts())
            {
                try
                {
                    var googleId = stdContact.PersonalProfileIdentifiers.GoogleId;
                    var googleContact = new Contact();

                    if (!string.IsNullOrEmpty(googleId))
                    {
                        try
                        {
                            googleContact = this.requester.Retrieve<Contact>(new Uri(googleId));
                        }
                        catch (GDataRequestException ex)
                        {
                            googleId = string.Empty;
                            if (ex.ResponseString != "Contact not found.")
                            {
                                Console.WriteLine(ex.Message);
                                throw;
                            }
                        }
                    }

                    googleContact.SetSyncIdentifier(stdContact.Id);
                    googleContact.Title = (stdContact.Name ?? new PersonName("(unknown)")).ToString();

                    googleContact.AddEmail(stdContact.PersonalEmailPrimary, GoogleSchemaQualifierHome);
                    googleContact.AddEmail(stdContact.BusinessEmailPrimary, GoogleSchemaQualifierWork);

                    googleContact.AddOrganization(stdContact.BusinessCompanyName, stdContact.BusinessDepartment, stdContact.BusinessPosition);

                    // setting the addresses if available
                    googleContact.AddAddress(stdContact.PersonalAddressPrimary, GoogleSchemaQualifierHome);
                    googleContact.AddAddress(stdContact.BusinessAddressPrimary, GoogleSchemaQualifierWork);
                    googleContact.AddAddress(stdContact.PersonalAddressSecondary, GoogleSchemaQualifierHome);
                    googleContact.AddAddress(stdContact.BusinessAddressSecondary, GoogleSchemaQualifierWork);

                    googleContact.AddPhoneNumber(stdContact.PersonalPhoneMobile, GoogleSchemaQualifierMobile);
                    googleContact.AddPhoneNumber(stdContact.BusinessPhoneMobile, GoogleSchemaQualifierMobile);

                    if (string.IsNullOrEmpty(googleId))
                    {
                        // replace the google contact with the new generated version from the server
                        googleContact = this.requester.Insert(this.contactsUri, googleContact);
                        stdContact.PersonalProfileIdentifiers.GoogleId = googleContact.Id;
                    }
                    else
                    {
                        // replace the google contact with the updated version
                        googleContact = this.requester.Update(googleContact);
                    }

                    // now we have a contact at the server, so we can upload the photo
                    googleContact.UpdatePhoto(stdContact, this);
                }
                catch (Exception ex)
                {
                    this.LogError(ex);
                }
            }
        }

        /// <summary>
        /// Initializes the object after calling the <see cref="StdClient.ReadFullList"/> or <see cref="StdClient.WriteFullList"/> method.
        /// This performs a setup of the requester to query the Google Data API.
        /// </summary>
        private void EnsureInitialization()
        {
            if (this.settings == null)
            {
                var userName = this.LogOnUserId;
                var passWord = this.LogOnPassword;

                this.settings = new RequestSettings("Sem.Sync.GoogleClient", userName, passWord)
                    {
                        AutoPaging = true
                    };
            }

            this.requester = this.requester ?? new ContactsRequest(this.settings);

            if (this.contactsUri == null)
            {
                this.contactsUri = new Uri(ContactsQuery.CreateContactsUri(this.LogOnUserId, GroupsQuery.fullProjection));
            }
        }
    }
}
