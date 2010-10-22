// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts from the Google Data API. The implementation does
//   support read and write of contact elements. Currently there's no support for a "clientFolderName" to
//   filter some data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Google
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::Google.Contacts;
    using global::Google.GData.Client;
    using global::Google.GData.Contacts;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts from the Google Data API. The implementation does
    ///   support read and write of contact elements. Currently there's no support for a "clientFolderName" to
    ///   filter some data.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(DisplayName = "Google Mail Contacts Client", CanReadContacts = true, CanWriteContacts = true, 
        MatchingIdentifier = ProfileIdentifierType.Google, NeedsCredentialsDomain = false, NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        #region Constants and Fields

        /// <summary>
        ///   The qualifier for the attribute "home"
        /// </summary>
        private const string GoogleSchemaQualifierHome = "home";

        /// <summary>
        ///   The qualifier for the attribute "home"
        /// </summary>
        private const string GoogleSchemaQualifierMobile = "mobile";

        /// <summary>
        ///   The qualifier for the attribute "work"
        /// </summary>
        private const string GoogleSchemaQualifierWork = "work";

        /// <summary>
        ///   An URI representing the currently logged in user
        /// </summary>
        private Uri contactsUri;

        /// <summary>
        ///   The requester object for the google api
        /// </summary>
        private ContactsRequest requester;

        /// <summary>
        ///   The settings instance
        /// </summary>
        private RequestSettings settings;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the user readable name of the client implementation. This name should
        ///   be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Google-Contact-Client";
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes a list of Contact IDs.
        /// </summary>
        /// <param name="elementsToDelete">
        /// The identifiers. 
        /// </param>
        /// <param name="clientFolderName">
        /// The client folder name. 
        /// </param>
        public override void DeleteElements(List<StdElement> elementsToDelete, string clientFolderName)
        {
            if (elementsToDelete == null)
            {
                return;
            }

            elementsToDelete.ForEach(
                x =>
                this.requester.Get<Contact>(new Uri(x.ExternalIdentifier.GetProfileId(ProfileIdentifierType.Google))).
                    Entries.ForEach(this.requester.Delete));
        }

        #endregion

        #region Methods

        /// <summary>
        /// logs the exception to the console and the <see cref="SyncComponent.LogProcessingEvent(object,Sem.GenericHelpers.EventArgs.ProcessingEventArgs)"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal void LogError(Exception exception)
        {
            var message = exception.Message;
            var gex = exception as GDataRequestException;
            if (gex != null)
            {
                message = gex.ResponseString;
            }

            Tools.DebugWriteLine(exception + " : " + message);
            this.LogException(exception);
        }

        /// <summary>
        /// Overrides the method to read the full list of data. This will read ALL data from the Google Contacts
        ///   account, even if it has already been downloaded in the past.
        /// </summary>
        /// <param name="clientFolderName">The parameter clientFolderName is ignored by this connector implementation.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            try
            {
                this.LogProcessingEvent("downloading contacts ...");
                this.EnsureInitialization();

                var contactsCollection = this.requester.GetContacts();
                foreach (var googleContact in contactsCollection.Entries)
                {
                    try
                    {
                        // get the SemSync Contact ID from the google entity (if there is one)
                        var semSyncIdString =
                            (from x in googleContact.ExtendedProperties where x.Name == "SemSyncId" select x.Value)
                            .FirstOrDefault();

                        // if we didn't set one, create a new Guid, otherwise reconstruct the Guid from the string stored in th Google contact
                        var semSyncId = string.IsNullOrEmpty(semSyncIdString)
                                            ? Guid.NewGuid()
                                            : new Guid(semSyncIdString);

                        // create a new contact entity
                        var stdEntry = new StdContact
                            {
                                Id = semSyncId, 
                                Name = new PersonName(googleContact.Title), 
                                ExternalIdentifier = new ProfileIdentifierDictionary(ProfileIdentifierType.Google, googleContact.Id)
                            };

                        this.LogProcessingEvent("mapping contact {0} ...", stdEntry.Name.ToString());
                        googleContact.SetSyncIdentifier(semSyncId);

                        // the ForEach is an extension method that calls the lambda for each element of an IEnumerable
                        googleContact.PostalAddresses.ForEach(stdEntry.SetAddress);
                        googleContact.Organizations.ForEach(stdEntry.SetBusiness);
                        googleContact.Phonenumbers.ForEach(stdEntry.SetPhone);
                        googleContact.Emails.ForEach(stdEntry.SetEmail);
                        googleContact.IMs.ForEach(stdEntry.SetInstantMessenger);

                        // downloads the image
                        this.LogProcessingEvent("downloading picture {0} ...", stdEntry.Name.ToString());
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
        /// <param name="elements">
        /// The elements to be exported. 
        /// </param>
        /// <param name="clientFolderName">
        /// The parameter clientFolderName is ignored in this connector implementation.
        /// </param>
        /// <param name="skipIfExisting">
        /// The parameter skipIfExisting is ignored in this connector implementation.
        /// </param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            GoogleContactMappingExtensions.GenericUiResponder = this.UiDispatcher;
            this.EnsureInitialization();

            foreach (var stdContact in elements.ToStdContacts())
            {
                try
                {
                    this.LogProcessingEvent(stdContact, "reading contact for update ...");
                    var googleId = stdContact.ExternalIdentifier.GetProfileId(ProfileIdentifierType.Google);
                    var googleContact = new Contact();

                    if (!string.IsNullOrEmpty(googleId))
                    {
                        try
                        {
                            // we need to replace the "base" inside the url with a "full" to get the correct "projection" for extended properties
                            googleContact =
                                this.requester.Retrieve<Contact>(
                                    new Uri(((string)googleId).Replace(@"/base/", @"/full/")));
                        }
                        catch (GDataRequestException ex)
                        {
                            googleId = string.Empty;
                            if (ex.ResponseString != "Contact not found.")
                            {
                                Tools.DebugWriteLine(ex.Message);
                                throw;
                            }
                        }
                    }

                    googleContact.SetSyncIdentifier(stdContact.Id);
                    googleContact.Title = (stdContact.Name ?? new PersonName("(unknown)")).ToString();

                    googleContact.AddEmail(stdContact.PersonalEmailPrimary, GoogleSchemaQualifierHome);
                    googleContact.AddEmail(stdContact.BusinessEmailPrimary, GoogleSchemaQualifierWork);

                    googleContact.AddOrganization(
                        stdContact.BusinessCompanyName, stdContact.BusinessDepartment, stdContact.BusinessPosition);

                    // setting the addresses if available
                    googleContact.AddAddress(stdContact.PersonalAddressPrimary, GoogleSchemaQualifierHome);
                    googleContact.AddAddress(stdContact.BusinessAddressPrimary, GoogleSchemaQualifierWork);
                    googleContact.AddAddress(stdContact.PersonalAddressSecondary, GoogleSchemaQualifierHome);
                    googleContact.AddAddress(stdContact.BusinessAddressSecondary, GoogleSchemaQualifierWork);

                    googleContact.AddPhoneNumber(stdContact.PersonalPhoneMobile, GoogleSchemaQualifierMobile);
                    googleContact.AddPhoneNumber(stdContact.BusinessPhoneMobile, GoogleSchemaQualifierMobile);

                    googleContact.AddImAddress(stdContact.PersonalInstantMessengerAddresses, GoogleSchemaQualifierHome);
                    googleContact.AddImAddress(stdContact.BusinessInstantMessengerAddresses, GoogleSchemaQualifierWork);

                    if (string.IsNullOrEmpty(googleId))
                    {
                        this.LogProcessingEvent(stdContact, "inserting contact ...");

                        // replace the google contact with the new generated version from the server
                        googleContact = this.requester.Insert(this.contactsUri, googleContact);
                        stdContact.ExternalIdentifier.SetProfileId(ProfileIdentifierType.Google, googleContact.Id);
                    }
                    else
                    {
                        this.LogProcessingEvent(stdContact, "updating contact ...");

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
        ///   This performs a setup of the requester to query the Google Data API.
        /// </summary>
        private void EnsureInitialization()
        {
            if (this.settings == null)
            {
                var userName = this.LogOnUserId;
                var passWord = this.LogOnPassword;

                this.settings = new RequestSettings("Sem.Sync.Connector.Google", userName, passWord)
                    {
                       AutoPaging = true 
                    };
            }

            this.requester = this.requester ?? new ContactsRequest(this.settings);

            if (this.contactsUri == null)
            {
                this.contactsUri = new Uri(
                    ContactsQuery.CreateContactsUri(this.LogOnUserId, GroupsQuery.fullProjection));
            }
        }

        #endregion
    }
}