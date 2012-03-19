// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts persisted to the file system
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.LotusNotes
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Domino;
using Sem.Sync.Connector.LotusNotes.Properties;
using System.Globalization;
  
    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription( ReferenceType = ClientPathType.Undefined, Mandatory = true)]
    [ConnectorDescription(DisplayName = "Lotus Notes",
        CanWriteContacts = false, CanReadContacts = true, NeedsCredentials = true, CanReadCalendarEntries = false,
        MatchingIdentifier = ProfileIdentifierType.LotusNotesId)]
    public class ContactClient : StdClient
    {
        private NotesSession _lotesNotesSession = null;
        //private NotesSession _lotusNotesServerSession = null;
        private NotesDatabase _localDatabase = null;
        private NotesView _contactsView = null;
        //private NotesDatabase _serverDatabase = null;
        //private NotesView _peopleView = null;
        //private string _lotusCientPassword = null;
        //private string _lotusnotesserverName = null;
        //private bool _IsfetchServerData = false;

        #region Methods

        /// <summary>
        /// Overrides virtual read method for full list of elements
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result">
        /// The list of elements that should get the elements. The elements should be added to
        ///   the list instead of replacing it.
        /// </param>
        /// <returns>
        /// The list with the newly added elements
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            
            string currentElementName = string.Empty;
            try
            {
                this.LogProcessingEvent(Resources.uiLogginIn);
                //Lotus Notes Object Creation
                _lotesNotesSession = new Domino.NotesSessionClass();
                //Initializing Lotus Notes Session
                _lotesNotesSession.Initialize(this.LogOnPassword); //Passwort
                _localDatabase = _lotesNotesSession.GetDatabase("", "names.nsf", false);  //Database for Contacts default names.nsf

                this.LogProcessingEvent(Resources.uiPreparingList);

                string viewname = "$People";
                _contactsView = _localDatabase.GetView(viewname);
                // TODO: implement reading from the Lotus Notes server and map the entities to StdContact instances
                if (_contactsView == null)
                {
                    this.LogProcessingEvent(Resources.uiNoViewFound, viewname);
                }
                else
                {
                    NotesViewEntryCollection notesViewCollection = _contactsView.AllEntries;
                    //ArrayList notesUIDSList = new ArrayList(); 
                    for (int rowCount = 1; rowCount <= notesViewCollection.Count; rowCount++)
                    {
                        //Get the nth entry of the selected view according to the iteration.
                        NotesViewEntry viewEntry = notesViewCollection.GetNthEntry(rowCount);
                        //Get the first document of particular entry.
                        NotesDocument document = viewEntry.Document;
                        
                        object documentItems = document.Items;
                        Array itemArray = (System.Array)documentItems;

                        StdContact elem = new StdContact();
                        PersonName name = new PersonName();
                        elem.Name = name;
                        AddressDetail businessAddress = new AddressDetail();
                        elem.BusinessAddressPrimary =businessAddress;
                        AddressDetail address = new AddressDetail();
                        elem.PersonalAddressPrimary = address;

                        ProfileIdInformation information = new ProfileIdInformation(document.NoteID);
                        elem.ExternalIdentifier.SetProfileId(ProfileIdentifierType.LotusNotesId, information);

                        for (int itemCount = 0; itemCount < itemArray.Length; itemCount++)
                        {
                            NotesItem notesItem = (Domino.NotesItem)itemArray.GetValue(itemCount);
                            string itemname = notesItem.Name;
                            string text = notesItem.Text;
                            switch(notesItem.Name)
                            {
                                //Name
                                case "FirstName":
                                    name.FirstName = notesItem.Text;
                                    break;
                                case "LastName":
                                    name.LastName = notesItem.Text;
                                    break;
                                case "Titel":
                                    {
                                        if (notesItem.Text != "0")
                                            name.AcademicTitle = notesItem.Text;
                                    }
                                    break;
                                //Geburtstag
                                case "Birthday":
                                    DateTime dt;
                                    if(DateTime.TryParse(notesItem.Text, out dt))
                                        elem.DateOfBirth = dt;
                                    break;
                                case "Comment":
                                    elem.AdditionalTextData = notesItem.Text;
                                    break;
                                //Business adress
                                case "InternetAddress":
                                    elem.BusinessEmailPrimary = notesItem.Text;
                                    break;
                                case "OfficePhoneNumber":
                                    businessAddress.Phone = new PhoneNumber();
                                    businessAddress.Phone.DenormalizedPhoneNumber = notesItem.Text;
                                    break;
                                case "OfficeStreetAddress":
                                    businessAddress.StreetName = notesItem.Text;
                                    break;
                                case "OfficeState":
                                    businessAddress.StateName = notesItem.Text;
                                    break;
                                case "OfficeCity":
                                    businessAddress.CityName = notesItem.Text;
                                    break;
                                case "OfficeZIP":
                                    businessAddress.PostalCode = notesItem.Text;
                                    break;
                                case "OfficeCountry":
                                     businessAddress.CountryName = notesItem.Text;
                                    break;
                                //Business
                                case "Department":
                                    elem.BusinessDepartment = notesItem.Text;
                                    break;
                                case "CompanyName":
                                    elem.BusinessCompanyName = notesItem.Text;
                                    break;
                                case "JobTitle":
                                    elem.BusinessPosition = notesItem.Text;
                                    break;
                                case "WebSite":
                                    elem.PersonalHomepage = notesItem.Text;
                                    break;
               
                                //Address
                                case "PhoneNumber":
                                    address.Phone = new PhoneNumber();
                                    address.Phone.DenormalizedPhoneNumber = notesItem.Text;
                                    break;
                                case "StreetAddress":
                                    address.StreetName = notesItem.Text;
                                    break;
                                case "State":
                                    address.StateName = notesItem.Text;
                                    break;
                                case "City":
                                    address.CityName = notesItem.Text;
                                    break;
                                case "Zip":
                                    address.PostalCode = notesItem.Text;
                                    break;
                                case "country":
                                    address.CountryName = notesItem.Text;
                                    break;
                                //Mobile 
                                case "CellPhoneNumber":
                                    elem.PersonalPhoneMobile = new PhoneNumber();
                                    elem.PersonalPhoneMobile.DenormalizedPhoneNumber = notesItem.Text;
                                    break;

                                //Categories
                                case "Categories":
                                    elem.Categories = new List<string>(notesItem.Text.Split(';'));
                                    break;

                            }
                        }
                        this.LogProcessingEvent("mapping contact {0} ...", elem.Name.ToString());
                        result.Add(elem);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(
                    string.Format(CultureInfo.CurrentCulture, Resources.uiErrorAtName, currentElementName, ex.Message));
            }
            finally
            {
                //outlookNamespace.Logoff();
                _lotesNotesSession = null;
            }
            return result;
        }

        /// <summary>
        /// Overrides virtual write method for full list of elements
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
            // TODO: implement writing to the Lotus Notes server
            // HINT: To follow best practice, you should try to read each item by using 
            // the property path PersonalProfileIdentifiers.LotusNotesId as a filter
            // to query the Notes server, then update the fields and write back the
            // entry. You should NEVER delete entries to perform an update, because
            // you would loose properties not covered by StdContact.
            throw new NotImplementedException();
        }

        #endregion
    }
}