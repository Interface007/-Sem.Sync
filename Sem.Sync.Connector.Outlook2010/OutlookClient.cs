// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutlookClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the OutlookClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook2010
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Microsoft.Office.Interop.Outlook;
    using Microsoft.Win32;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// This class communicates with an outlook instance.
    /// </summary>
    internal static class OutlookClient
    {
        /// <summary>
        /// This is the name of the custom outlook field for the synchronization id
        /// </summary>
        private const string ContactIdOutlookPropertyName = "SemSyncId";

        /// <summary>
        /// This is the name of the custom outlook field for the synchronization id
        /// </summary>
        private const string AppointmentIdOutlookPropertyName = "SemSyncId";

        /// <summary>
        /// Counts calls that may allocate but not free Runtime Callable Wrappers (COM objects) but not free them in time
        /// </summary>
        private static int garbageCollectionRelevantCalls;

        /// <summary>
        /// creates a list of ContactsItemContainer from a contacts enumeration
        /// </summary>
        /// <param name="contactsEnum"> The contacts enum. </param>
        /// <returns> a list of ContactsItemContainer </returns>
        /// <exception cref="ArgumentNullException"> in case of contactsEnum being null </exception>
        internal static IEnumerable<ContactsItemContainer> GetContactsList(Items contactsEnum)
        {
            if (contactsEnum == null)
            {
                throw new ArgumentNullException("contactsEnum");
            }

            var contactsList = new List<ContactsItemContainer>();
            foreach (var item in contactsEnum)
            {
                var contactItem = item as ContactItem;
                if (contactItem == null)
                {
                    continue;
                }

                contactsList.Add(new ContactsItemContainer { Item = contactItem });
                GCRelevantCall();
            }

            return contactsList;
        }
        
        /// <summary>
        /// creates a list of AppointmentItemContainer from a contacts enumeration
        /// </summary>
        /// <param name="appointmentsEnum"> The contacts enum. </param>
        /// <returns> a list of AppointmentItemContainer </returns>
        /// <exception cref="ArgumentNullException"> in case of contactsEnum being null </exception>
        internal static IEnumerable<AppointmentItemContainer> GetAppointmentsList(Items appointmentsEnum)
        {
            if (appointmentsEnum == null)
            {
                throw new ArgumentNullException("appointmentsEnum");
            }

            var contactsList = new List<AppointmentItemContainer>();
            foreach (var item in appointmentsEnum)
            {
                var contactItem = item as AppointmentItem;
                if (contactItem == null)
                {
                    continue;
                }

                contactsList.Add(new AppointmentItemContainer { Item = contactItem });
                GCRelevantCall();
            }

            return contactsList;
        }

        /// <summary>
        /// writes a contact to outlook
        /// </summary>
        /// <param name="contactsEnum"> The contacts enum. </param>
        /// <param name="element"> The element. </param>
        /// <param name="skipIfExisting"> The skip if existing. </param>
        /// <param name="contactsList"> The contacts list. </param>
        /// <returns> a value indicating if the contact has been saved </returns>
        /// <exception cref="ArgumentNullException">in case of contactsEnum or element being null</exception>
        internal static bool WriteContactToOutlook(Items contactsEnum, StdContact element, bool skipIfExisting, IEnumerable<ContactsItemContainer> contactsList)
        {
            if (contactsEnum == null)
            {
                throw new ArgumentNullException("contactsEnum");
            }

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var outlookContact = (from x in contactsList
                                  where x.Id == element.Id.ToString()
                                  select x.Item).FirstOrDefault();

            if (skipIfExisting && outlookContact != null)
            {
                return false;
            }

            if (outlookContact == null)
            {
                outlookContact = (ContactItem)contactsEnum.Add(OlItemType.olContactItem);
            }

            // convert StdContact to Outlook contact
            if (ConvertToNativeContact(element, outlookContact))
            {
                outlookContact.Save();
                GCRelevantCall();
                return true;
            }

            GCRelevantCall();
            return false;
        }

        #region conversion

        /// <summary>
        /// Converts an outlook contact to a standard contact.
        /// </summary>
        /// <param name="outlookContact"> The outlook contact to be converted.  </param>
        /// <param name="contactList"> The contact List to lookup duplicates. </param>
        /// <returns> a new standard contact  </returns>
        /// <exception cref="ArgumentNullException"> if the outlook contact is null  </exception>
        internal static StdContact ConvertToStandardContact(ContactItem outlookContact, IEnumerable<StdContact> contactList)
        {
            if (outlookContact == null)
            {
                throw new ArgumentNullException("outlookContact");
            }

            // generate the new id this contact will get in case there is no contact id in outlook
            var newId = GetStandardContactId(outlookContact, contactList);

            // read the picture data and name of this contact
            string pictureName;
            var pictureData = SaveOutlookContactPicture(outlookContact, out pictureName);

            StdContact returnValue;

            try
            {
                // create a new contact and assign the corresponding values from the outlook contact
                returnValue = new StdContact
                {
                    Id = newId,
                    InternalSyncData = new SyncData
                    {
                        DateOfLastChange = outlookContact.LastModificationTime,
                        DateOfCreation = outlookContact.CreationTime
                    },
                    PersonGender =
                        (outlookContact.Gender == OlGender.olMale)
                            ? Gender.Male
                            : (outlookContact.Gender == OlGender.olFemale)
                                    ? Gender.Female
                                    : SyncTools.GenderByText(outlookContact.Title),

                    DateOfBirth = outlookContact.Birthday,

                    Name = new PersonName
                    {
                        FirstName = outlookContact.FirstName,
                        LastName = outlookContact.LastName,
                        MiddleName = outlookContact.MiddleName,
                        AcademicTitle =
                           outlookContact.Title.IsOneOf("Herr", "Mr.", "Frau", "Mrs.") ? null :
                           outlookContact.Title,
                    },

                    PersonalAddressPrimary = new AddressDetail
                    {
                        Phone = (!string.IsNullOrEmpty(outlookContact.HomeTelephoneNumber)) ? new PhoneNumber(outlookContact.HomeTelephoneNumber) : null,
                        CountryName = outlookContact.HomeAddressCountry,
                        PostalCode = outlookContact.HomeAddressPostalCode,
                        CityName = outlookContact.HomeAddressCity,
                        StateName = outlookContact.HomeAddressState,
                        StreetName = outlookContact.HomeAddressStreet,
                        StreetNumber = SyncTools.ExtractStreetNumber(outlookContact.HomeAddressStreet),
                        StreetNumberExtension = SyncTools.ExtractStreetNumberExtension(),
                    },

                    PersonalHomepage = outlookContact.PersonalHomePage,
                    PersonalEmailPrimary = outlookContact.Email1Address,
                    PersonalInstantMessengerAddresses = string.IsNullOrEmpty(outlookContact.IMAddress) ? null : new InstantMessengerAddresses(outlookContact.IMAddress),
                    PersonalPhoneMobile = (!string.IsNullOrEmpty(outlookContact.MobileTelephoneNumber)) ? new PhoneNumber(outlookContact.MobileTelephoneNumber) : null,

                    BusinessCompanyName = outlookContact.CompanyName,
                    BusinessPosition = outlookContact.JobTitle,

                    BusinessAddressPrimary = new AddressDetail
                    {
                        Phone = (!string.IsNullOrEmpty(outlookContact.BusinessTelephoneNumber)) ? new PhoneNumber(outlookContact.BusinessTelephoneNumber) : null,
                        CountryName = outlookContact.BusinessAddressCountry,
                        PostalCode = outlookContact.BusinessAddressPostalCode,
                        CityName = outlookContact.BusinessAddressCity,
                        StateName = outlookContact.BusinessAddressState,
                        StreetName = outlookContact.BusinessAddressStreet,
                        StreetNumber = SyncTools.ExtractStreetNumber(outlookContact.BusinessAddressStreet),
                        StreetNumberExtension = SyncTools.ExtractStreetNumberExtension(),
                    },

                    BusinessHomepage = outlookContact.BusinessHomePage,
                    BusinessEmailPrimary = outlookContact.Email2Address,
                    BusinessPhoneMobile = (!string.IsNullOrEmpty(outlookContact.Business2TelephoneNumber)) ? new PhoneNumber(outlookContact.Business2TelephoneNumber) : null,

                    AdditionalTextData = outlookContact.Body,
                    PictureName = pictureName,
                    PictureData = pictureData
                };
            }
            catch (COMException ex)
            {
                if (ex.ErrorCode == -2147467260)
                {
                    return null;
                }

                throw;
            }

            if (!string.IsNullOrEmpty(outlookContact.Categories))
            {
                returnValue.Categories = MergeStrings(returnValue.Categories, outlookContact.Categories);
            }

            if (string.IsNullOrEmpty(returnValue.PersonalAddressPrimary.ToString()))
            {
                returnValue.PersonalAddressPrimary = null;
            }

            if (string.IsNullOrEmpty(returnValue.BusinessAddressPrimary.ToString()))
            {
                returnValue.PersonalAddressPrimary = null;
            }

            // return the newly generated standard contact
            return returnValue;
        }

        /// <summary>
        /// converts an outlook appointment element to a StdCalendarItem
        /// </summary>
        /// <param name="outlookItem"> The outlook item to be converted.  </param>
        /// <param name="appointmentList">list of std calendar entries to suppress duplicates</param>
        /// <returns> the newly created StdCalendarItem </returns>
        /// <exception cref="ArgumentNullException"> in case of outlookItem being null </exception>
        internal static StdCalendarItem ConvertToStandardCalendarItem(AppointmentItem outlookItem, IEnumerable<StdCalendarItem> appointmentList)
        {
            if (outlookItem == null)
            {
                throw new ArgumentNullException("outlookItem");
            }

            // generate the new id this contact will get in case there is no contact id in outlook
            var newId = GetStandardAppointmentId(outlookItem, appointmentList);

            var result = new StdCalendarItem
                {
                    Id = newId,
                    Title = outlookItem.Subject,
                    Description = outlookItem.Body,
                    Start = outlookItem.StartUTC,
                    End = outlookItem.EndUTC,
                    BusyStatus = outlookItem.BusyStatus.ToBusyStatus(),
                    InternalSyncData = new SyncData { DateOfLastChange = outlookItem.LastModificationTime },
                    Location = outlookItem.Location,
                    ExternalIdentifier =
                        new List<CalendarIdentifier>
                            {
                                new CalendarIdentifier
                                    { 
                                        Identifier = outlookItem.GlobalAppointmentID, 
                                        IdentifierType = CalendarIdentifierType.Outlook, 
                                    }
                            },
                    RecurrenceState = outlookItem.RecurrenceState.ToRecurrenceState(),
                    ReminderBeforeStart = TimeSpan.FromMinutes(outlookItem.ReminderMinutesBeforeStart),
                    ResponseRequested = outlookItem.ResponseRequested,
                    ResponseStatus = outlookItem.ResponseStatus.ToResponseStatus(),
                };

            return result;
        }

        /// <summary>
        /// Opens a MAPI folder from outlook.
        /// </summary>
        /// <param name="outlookNamespace"> The outlook namespace. </param>
        /// <param name="folderName"> The outlook folder name. </param>
        /// <param name="defaultFolder"> The default folder. </param>
        /// <returns> a reference to the MAPI folder </returns>
        internal static MAPIFolder GetOutlookMapiFolder(NameSpace outlookNamespace, string folderName, OlDefaultFolders defaultFolder)
        {
            if (outlookNamespace == null)
            {
                throw new ArgumentNullException("outlookNamespace");
            }

            if (folderName == null)
            {
                throw new ArgumentNullException("folderName");
            }

            // Get all the Contacts Folder
            var pathPart = GetNextPathPart(folderName, out folderName);

            var contacts =
                (pathPart == "ask") ? outlookNamespace.PickFolder() :
                string.IsNullOrEmpty(pathPart) ? outlookNamespace.GetDefaultFolder(defaultFolder) :
                (from x in outlookNamespace.Folders.OfType<MAPIFolder>()
                 where x.Name == pathPart
                 select x).FirstOrDefault();

            if (folderName.Length > 0 && contacts != null)
            {
                contacts = GetOutlookContactsFolder(contacts, folderName);
            }

            return contacts;
        }

        /// <summary>
        /// Get the namespace from outlook.
        /// </summary>
        /// <returns> Returns the namespace from outlook. </returns>
        internal static NameSpace GetNamespace()
        {
            var outlookApplication = new Application();

            // Get NameSpace.
            var outlookNamespace = outlookApplication.GetNamespace("mapi");

            // Logon. If an outlook app is already open, then it will reuse that session. Else
            // it will perform a fresh logon. If you have profiles and passwords for the same, 
            // you need to enter the passwords in the dialogbox when they are shown
            outlookNamespace.Logon(string.Empty, string.Empty, true, true);

            return outlookNamespace;
        }

        #endregion

        /// <summary>
        /// this method is still not implemented
        /// </summary>
        /// <param name="appointmentEnum"> The appointment enum.  </param>
        /// <param name="stdCalendarItem"> The std calendar item.  </param>
        /// <param name="appointmentList"> The appointment List. </param>
        /// <returns> a value indicating whether the element has been written to outlook  </returns>
        /// <exception cref="ArgumentNullException"> in case of contactsEnum being null  </exception>
        internal static bool WriteCalendarItemToOutlook(Items appointmentEnum, StdCalendarItem stdCalendarItem, IEnumerable<AppointmentItemContainer> appointmentList)
        {
            if (appointmentEnum == null)
            {
                throw new ArgumentNullException("appointmentEnum");
            }

            if (stdCalendarItem == null)
            {
                throw new ArgumentNullException("stdCalendarItem");
            }

            var outlookAppointment = (from x in appointmentList
                                      where x.Id == stdCalendarItem.Id.ToString()
                                      select x.Item).FirstOrDefault();

            if (outlookAppointment == null)
            { 
                outlookAppointment = (AppointmentItem)appointmentEnum.Add(OlItemType.olAppointmentItem); 
            }

            // convert StdContact to Outlook contact
            if (ConvertToNativeAppointment(stdCalendarItem, outlookAppointment))
            {
                outlookAppointment.Save();
                GCRelevantCall();
                return true;
            }

            GCRelevantCall();
            return false;
        }

        /// <summary>
        /// Converts a <see cref="StdCalendarItem"/> to an Outlook-Appointment.
        /// </summary>
        /// <param name="stdNewAppointment"> The <see cref="StdCalendarItem"/> to be converted. </param>
        /// <param name="appointment"> The appointment to be updated. </param>
        /// <returns> True if there have been updates in the target </returns>
        /// <exception cref="ArgumentNullException"> in case of the <paramref name="stdNewAppointment"/> or <paramref name="appointment"/> being null.</exception>
        private static bool ConvertToNativeAppointment(StdCalendarItem stdNewAppointment, AppointmentItem appointment)
        {
            if (stdNewAppointment == null)
            {
                throw new ArgumentNullException("stdNewAppointment");
            }

            if (appointment == null)
            {
                throw new ArgumentNullException("appointment");
            }

            var stdOldAppointment = ConvertToStandardCalendarItem(appointment, null);

            SyncTools.ClearNulls(stdNewAppointment, typeof(StdCalendarItem));
            SyncTools.ClearNulls(stdOldAppointment, typeof(StdCalendarItem));

            var dirty = false;
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.Title, x => appointment.Subject = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.Description, x => appointment.Body = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.Start, x => appointment.StartUTC = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.End, x => appointment.EndUTC = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.BusyStatus.ToOutlook(), x => appointment.BusyStatus = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.Location, x => appointment.Location = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.ReminderBeforeStart.Minutes, x => appointment.ReminderMinutesBeforeStart = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewAppointment, stdOldAppointment, x => x.ResponseRequested, x => appointment.ResponseRequested = x);

            // todo: how can se set the RecurrenceState property?
            // todo: how to set the ResponseStatus property
            return dirty;
        }

        /// <summary>
        /// Merges a semicolon seperated sequence of strings into a list of strings.
        /// </summary>
        /// <param name="list">The list of strings that should get the new entries.</param>
        /// <param name="semicolonSeperatedStrings">The new strings to be added seperated by a semicolon.</param>
        /// <returns>The list with added strings</returns>
        private static List<string> MergeStrings(List<string> list, string semicolonSeperatedStrings)
        {
            if (list == null)
            {
                list = new List<string>();
            }

            if (string.IsNullOrEmpty(semicolonSeperatedStrings))
            {
                return list;
            }

            foreach (var category in semicolonSeperatedStrings.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var currentCategory = category;
                if (!list.Exists(x => x.Equals(currentCategory, StringComparison.OrdinalIgnoreCase)))
                {
                    list.Add(currentCategory);
                }
            }

            return list;
        }

        /// <summary>
        /// Counts GC relevant calls and executes the garbage collection each 100 calls. This is in 
        /// hope of preventing exceptions caused by known outlook memory leaks.
        /// </summary>
        private static void GCRelevantCall()
        {
            garbageCollectionRelevantCalls++;
            if (garbageCollectionRelevantCalls <= 100)
            {
                return;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Writes the information from a standard contact to a native outlook contact
        /// </summary>
        /// <param name="stdNewContact"> The standard contact to be converted. </param>
        /// <param name="outlookContact"> The outlook contact should be the target of writing. </param>
        /// <returns>true if the outlook contact needs to be saved, false if there was no information altered</returns>
        /// <exception cref="ArgumentNullException"> if one of the parameters is null </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "this is only multiple if-statements - that's not really complex")]
        private static bool ConvertToNativeContact(StdContact stdNewContact, ContactItem outlookContact)
        {
            if (stdNewContact == null)
            {
                throw new ArgumentNullException("stdNewContact");
            }

            if (outlookContact == null)
            {
                throw new ArgumentNullException("outlookContact");
            }

            var dirty = false;
            var stdOldContact = ConvertToStandardContact(outlookContact, null);
            var gender = stdNewContact.PersonGender ==
                                        Gender.Unspecified ?
                                        OlGender.olUnspecified :
                                            ((stdNewContact.PersonGender == Gender.Male) ?
                                            OlGender.olMale :
                                            OlGender.olFemale);

            SyncTools.ClearNulls(stdNewContact, typeof(StdContact));
            SyncTools.ClearNulls(stdOldContact, typeof(StdContact));

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.DateOfBirth, x => outlookContact.Birthday = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonGender, x => outlookContact.Gender = gender);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.Name.FirstName, x => outlookContact.FirstName = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.Name.MiddleName, x => outlookContact.MiddleName = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.Name.LastName, x => outlookContact.LastName = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.Name.AcademicTitle, x => outlookContact.Title = x);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessEmailPrimary, x => outlookContact.Email2Address = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalEmailPrimary, x => outlookContact.Email1Address = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessHomepage, x => outlookContact.BusinessHomePage = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalHomepage, x => outlookContact.PersonalHomePage = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessCompanyName, x => outlookContact.CompanyName = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessPosition, x => outlookContact.JobTitle = x);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.CityName, x => outlookContact.BusinessAddressCity = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.CountryName, x => outlookContact.BusinessAddressCountry = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.PostalCode, x => outlookContact.BusinessAddressPostalCode = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.StateName, x => outlookContact.BusinessAddressState = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.StreetName, x => outlookContact.BusinessAddressStreet = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessAddressPrimary.Phone.ToString(), x => outlookContact.BusinessTelephoneNumber = x);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.CityName, x => outlookContact.HomeAddressCity = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.CountryName, x => outlookContact.HomeAddressCountry = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.PostalCode, x => outlookContact.HomeAddressPostalCode = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.StateName, x => outlookContact.HomeAddressState = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.StreetName, x => outlookContact.HomeAddressStreet = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalAddressPrimary.Phone.ToString(), x => outlookContact.HomeTelephoneNumber = x);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalPhoneMobile.ToString(), x => outlookContact.MobileTelephoneNumber = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.BusinessPhoneMobile.ToString(), x => outlookContact.Business2TelephoneNumber = x);
            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.PersonalInstantMessengerAddresses.MsnMessenger, x => outlookContact.IMAddress = x);

            MappingHelper.MapIfDiffers(ref dirty, stdNewContact, stdOldContact, x => x.AdditionalTextData, x => outlookContact.Body = x);

            if (stdOldContact.Id != stdNewContact.Id)
            {
                outlookContact.UserProperties.Add(
                    ContactIdOutlookPropertyName,
                    OlUserPropertyType.olText,
                    true,
                    OlFormatText.olFormatTextText).Value = stdNewContact.Id.ToString();
                dirty = true;
            }

            // import pictures if we have more data inside the new contact
            if (stdNewContact.PictureData != null && stdNewContact.PictureData.Length > stdOldContact.PictureData.Length)
            {
                var fullName = Path.GetTempFileName() + ".jpg";
                File.WriteAllBytes(fullName, stdNewContact.PictureData);
                outlookContact.AddPicture(fullName);
                File.Delete(fullName);
                dirty = true;
            }

            if (stdNewContact.Categories != null &&
                (stdOldContact.Categories == null ||
                stdNewContact.Categories.Count != stdOldContact.Categories.Count))
            {
                outlookContact.Categories = stdNewContact.Categories.MergeList(stdOldContact.Categories).ConcatElementsToString(";");
                dirty = true;
            }

            return dirty;
        }

        /// <summary>
        /// Reads the contact picture and its name
        /// </summary>
        /// <param name="contact">the contact to process</param>
        /// <param name="pictureName">returns the name of the picture from the mime object</param>
        /// <returns>an array of bytes representing the picture</returns>
        private static byte[] SaveOutlookContactPicture(ContactItem contact, out string pictureName)
        {
            // check if we have a picture inside the outlook contact
            if (contact.HasPicture)
            {
                // iterate the attachements to find the correct one
                foreach (Attachment attachement in contact.Attachments)
                {
                    // the picture attachement has a defined name, skip if not that name
                    if (attachement.DisplayName != "ContactPicture.jpg" &&
                        attachement.DisplayName != "ContactPhoto")
                    {
                        continue;
                    }

                    // extract the file name
                    pictureName = attachement.FileName;
                    var bytes = new byte[] { };

                    // save the picture in the temp path
                    var fullName = Path.GetTempFileName();
                    try
                    {
                        try
                        {
                            attachement.SaveAsFile(fullName);

                            // read all bytes from the temp file
                            bytes = File.ReadAllBytes(fullName);

                            GCRelevantCall();
                        }
                        catch (COMException)
                        {
                            // we may have a problem if there are too many pictures saved in this session
                            // then we need to clean up the outlook temp path (which is difficult to determine)
                            CleanupTempFolder("11.0");
                            CleanupTempFolder("12.0");
                            CleanupTempFolder("14.0");

                            // try again
                            attachement.SaveAsFile(fullName);

                            // read all bytes from the temp file
                            bytes = File.ReadAllBytes(fullName);
                        }

                        // clean up the temp file
                        File.Delete(fullName);
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        // TODO: log this error
                    }

                    // that's it
                    return bytes;
                }
            }

            // sorry, but we don't have such a picture ;-)
            pictureName = string.Empty;
            return new byte[0];
        }

        /// <summary>
        /// Clean up the outlook exclusive temp folder (who knows why they cannot simply use 
        /// the normal temp folder with standard temp file handling...)
        /// </summary>
        /// <param name="identifier"> The identifier of the office version (e.g. 11.0). </param>
        private static void CleanupTempFolder(string identifier)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\" + identifier + "\\Outlook\\Security");
            if (reg == null)
            {
                return;
            }

            var folderReg = reg.GetValue("OutlookSecureTempFolder");
            if (folderReg == null || !Directory.Exists(folderReg.ToString()))
            {
                return;
            }

            foreach (var picturePath in Directory.GetFiles(folderReg.ToString(), "Contact*.jpg"))
            {
                try
                {
                    File.Delete(picturePath);
                }
                catch (IOException)
                {
                    // LogProcessingEvent("problem to clean the outlook temp folder " + picturePath + ": " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens a contact folder by name by iterating through the path.
        /// </summary>
        /// <param name="outlookFolder">The outlook folder.</param>
        /// <param name="folderName">The folder name.</param>
        /// <returns>Returns a MAPI folder specified by the folder name.</returns>
        private static MAPIFolder GetOutlookContactsFolder(MAPIFolder outlookFolder, string folderName)
        {
            // Get all the Contacts Folder
            var pathPart = GetNextPathPart(folderName, out folderName);

            // Get all the Contacts Folder
            var contacts =
                (from x in outlookFolder.Folders.OfType<MAPIFolder>()
                 where x.Name == pathPart
                 select x).FirstOrDefault();

            if (folderName.Length > 0)
            {
                contacts = GetOutlookContactsFolder(contacts, pathPart);
            }

            return contacts;
        }

        /// <summary>
        /// helper funtion to parse a path.
        /// </summary>
        /// <param name="path">The path to be parsed.</param>
        /// <param name="returnPath">The remaining part of the path.</param>
        /// <returns>the next part of the path</returns>
        private static string GetNextPathPart(string path, out string returnPath)
        {
            var result = string.Empty;
            if (path != null)
            {
                while (path.StartsWith(@"\", StringComparison.Ordinal))
                {
                    path = path.Substring(1);
                }

                var idx = path.IndexOf('\\');
                idx = (idx == -1) ? path.Length : idx;

                result = path.Substring(0, idx);
                returnPath = (idx < path.Length) ? path.Substring(idx + 1) : string.Empty;
            }
            else
            {
                returnPath = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Returns a syncronization id for a given contact. If there is no syncronization id, a new one will be 
        /// created and saved to outlook. If saving the contact does fail because of authorization, NO exception 
        /// will be thrown.
        /// </summary>
        /// <param name="outlookContact"> the outlook contact to handle </param>
        /// <param name="contactList"> The contact List to lookup duplicates. </param>
        /// <returns> the corresponding Guid </returns>
        private static Guid GetStandardContactId(ContactItem outlookContact, IEnumerable<StdContact> contactList)
        {
            if (outlookContact == null)
            {
                throw new ArgumentNullException("outlookContact");
            }

            var newId = Guid.NewGuid();
            try
            {
                // try to read the contact id property - generate one if it's not there
                var contactIdObject = outlookContact.UserProperties[ContactIdOutlookPropertyName] ??
                                      outlookContact.UserProperties.Add(
                                          ContactIdOutlookPropertyName,
                                          OlUserPropertyType.olText,
                                          true,
                                          OlFormatText.olFormatTextText);

                // test if the value is a valid id
                if (contactIdObject.Value.ToString().Length != 36)
                {
                    // use the formerly generated id if the one from outlook is not valid
                    contactIdObject.Value = newId.ToString();
                    outlookContact.Save();
                }

                // finally read the id from the property
                newId = new Guid(contactIdObject.Value.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                // if we are not authorized to write back the id, we will assume a new id
            }

            var guid = newId;
            if (contactList != null && contactList.Where(x => x.Id == guid).Count() > 0)
            {
                newId = Guid.NewGuid();
            }

            return newId;
        }
        
        /// <summary>
        /// Returns a syncronization id for a given appointment. If there is no syncronization id, a new one will be 
        /// created and saved to outlook. If saving the appointment does fail because of authorization, NO exception 
        /// will be thrown.
        /// </summary>
        /// <param name="outlookAppointment"> the outlook appointment to handle </param>
        /// <param name="appointmentList"> The appointment List to lookup duplicates. </param>
        /// <returns> the corresponding Guid </returns>
        private static Guid GetStandardAppointmentId(AppointmentItem outlookAppointment, IEnumerable<StdCalendarItem> appointmentList)
        {
            if (outlookAppointment == null)
            {
                throw new ArgumentNullException("outlookAppointment");
            }

            var newId = Guid.NewGuid();
            try
            {
                // try to read the contact id property - generate one if it's not there
                var contactIdObject = outlookAppointment.UserProperties[AppointmentIdOutlookPropertyName] ??
                                      outlookAppointment.UserProperties.Add(
                                          ContactIdOutlookPropertyName,
                                          OlUserPropertyType.olText,
                                          true,
                                          OlFormatText.olFormatTextText);

                // test if the value is a valid id
                if (contactIdObject.Value.ToString().Length != 36)
                {
                    // use the formerly generated id if the one from outlook is not valid
                    contactIdObject.Value = newId.ToString();
                    outlookAppointment.Save();
                }

                // finally read the id from the property
                newId = new Guid(contactIdObject.Value.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                // if we are not authorized to write back the id, we will assume a new id
            }

            var guid = newId;
            if (appointmentList != null && appointmentList.Where(x => x.Id == guid).Count() > 0)
            {
                newId = Guid.NewGuid();
            }

            return newId;
        }
    }
}
