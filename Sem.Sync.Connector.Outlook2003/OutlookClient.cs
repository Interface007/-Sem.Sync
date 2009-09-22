// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutlookClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the OutlookClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Outlook2003
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using GenericHelpers;

    using Microsoft.Office.Interop.Outlook;
    using Microsoft.Win32;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    /// <summary>
    /// This class communicates with an outlook instance.
    /// </summary>
    public static class OutlookClient
    {
        /// <summary>
        /// This is the name of the custom outlook field for the synchronization id
        /// </summary>
        private const string ContactIdOutlookPropertyName = "SemSyncId";

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
        public static List<ContactsItemContainer> GetContactsList(Items contactsEnum)
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
                    contactsList.Add(new ContactsItemContainer { Item = contactItem });
                    GCRelevantCall();
                }
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
        public static bool WriteContactToOutlook(Items contactsEnum, StdContact element, bool skipIfExisting, List<ContactsItemContainer> contactsList)
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
        /// <param name="outlookContact"> The outlook contact to be converted. </param>
        /// <returns> a new standard contact </returns>
        /// <exception cref="ArgumentNullException"> if the outlook contact is null </exception>
        public static StdContact ConvertToStandardContact(_ContactItem outlookContact)
        {
            if (outlookContact == null)
            {
                throw new ArgumentNullException("outlookContact");
            }

            // generate the new id this contact will get in case there is no contact id in outlook
            var newId = GetStandardId(outlookContact);

            // read the picture data and name of this contact
            string pictureName;
            var pictureData = SaveOutlookContactPicture(outlookContact, out pictureName);

            // create a new contact and assign the corresponding values from the outlook contact
            var returnValue = new StdContact
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
        /// <param name="outlookItem"> The outlook item. </param>
        /// <returns> the newly created StdCalendarItem </returns>
        /// <exception cref="ArgumentNullException"> in case of outlookItem being null </exception>
        public static StdCalendarItem ConvertToStandardCalendarItem(_AppointmentItem outlookItem)
        {
            if (outlookItem == null)
            {
                throw new ArgumentNullException("outlookItem");
            }

            var result = new StdCalendarItem
                {
                    Id = Guid.NewGuid(),
                    Subject = outlookItem.Subject,
                    Body = outlookItem.Body,
                    Start = outlookItem.StartUTC,
                    End = outlookItem.EndUTC,
                };

            // TODO: this is a very "incomplete" version of the method
            return result;
        }

        /// <summary>
        /// Opens a MAPI folder from outlook.
        /// </summary>
        /// <param name="outlookNamespace">
        /// The outlook namespace.
        /// </param>
        /// <param name="folderName">
        /// The outlook folder name.
        /// </param>
        /// <param name="defaultFolder">
        /// The default folder.
        /// </param>
        /// <returns>
        /// a reference to the MAPI folder
        /// </returns>
        public static MAPIFolder GetOutlookMapiFolder(_NameSpace outlookNamespace, string folderName, OlDefaultFolders defaultFolder)
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
        /// <returns>
        /// Returns the namespace from outlook.
        /// </returns>
        public static NameSpace GetNamespace()
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
        /// <param name="contactsEnum"> The contacts enum. </param>
        /// <param name="stdCalendarItem"> The std calendar item. </param>
        /// <param name="contactsList"> The contacts list. </param>
        /// <returns> a value indicating whether the element has been written to outlook </returns>
        /// <exception cref="ArgumentNullException"> in case of contactsEnum being null </exception>
        /// <exception cref="NotImplementedException"> always, because the method is not implemented </exception>
        internal static bool WriteCalendarItemToOutlook(Items contactsEnum, StdCalendarItem stdCalendarItem, List<ContactsItemContainer> contactsList)
        {
            if (contactsEnum == null)
            {
                throw new ArgumentNullException("contactsEnum");
            }

            if (stdCalendarItem == null)
            {
                throw new ArgumentNullException("stdCalendarItem");
            }

            if (contactsList == null)
            {
                throw new ArgumentNullException("contactsList");
            }

            throw new NotImplementedException();
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
            if (garbageCollectionRelevantCalls > 100)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Writes the information from a standard contact to a native outlook contact
        /// </summary>
        /// <param name="stdNewContact"> The standard contact to be converted. </param>
        /// <param name="outlookContact"> The outlook contact should be the target of writing. </param>
        /// <returns>true if the outlook contact needs to be saved, false if there was no information altered</returns>
        /// <exception cref="ArgumentNullException"> if one of the parameters is null </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "this is only multiple if-statements - that's not really complex")]
        private static bool ConvertToNativeContact(StdContact stdNewContact, _ContactItem outlookContact)
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
            var stdOldContact = ConvertToStandardContact(outlookContact);
            var gender = stdNewContact.PersonGender ==
                                        Gender.Unspecified ?
                                        OlGender.olUnspecified :
                                            ((stdNewContact.PersonGender == Gender.Male) ?
                                            OlGender.olMale :
                                            OlGender.olFemale);

            if (stdOldContact.DateOfBirth != stdNewContact.DateOfBirth && stdNewContact.DateOfBirth > new DateTime(1900, 1, 2))
            {
                outlookContact.Birthday = stdNewContact.DateOfBirth;
                dirty = true;
            }

            if (stdOldContact.Name.FirstName != stdNewContact.Name.FirstName)
            {
                outlookContact.FirstName = stdNewContact.Name.FirstName;
                dirty = true;
            }

            if (stdOldContact.Name.MiddleName != stdNewContact.Name.MiddleName)
            {
                outlookContact.MiddleName = stdNewContact.Name.MiddleName;
                dirty = true;
            }

            if (stdOldContact.Name.LastName != stdNewContact.Name.LastName)
            {
                outlookContact.LastName = stdNewContact.Name.LastName;
                dirty = true;
            }

            if (stdOldContact.Name.AcademicTitle != stdNewContact.Name.AcademicTitle)
            {
                outlookContact.Title = stdNewContact.Name.AcademicTitle;
                dirty = true;
            }

            if (stdOldContact.PersonGender != stdNewContact.PersonGender)
            {
                outlookContact.Gender = gender;
                dirty = true;
            }

            if (stdOldContact.BusinessEmailPrimary != stdNewContact.BusinessEmailPrimary)
            {
                outlookContact.Email2Address = stdNewContact.BusinessEmailPrimary;
                dirty = true;
            }

            if (stdOldContact.PersonalEmailPrimary != stdNewContact.PersonalEmailPrimary)
            {
                outlookContact.Email1Address = stdNewContact.PersonalEmailPrimary;
                dirty = true;
            }

            if (stdOldContact.BusinessHomepage != stdNewContact.BusinessHomepage)
            {
                outlookContact.BusinessHomePage = stdNewContact.BusinessHomepage;
                dirty = true;
            }

            if (stdOldContact.PersonalHomepage != stdNewContact.PersonalHomepage)
            {
                outlookContact.PersonalHomePage = stdNewContact.PersonalHomepage;
                dirty = true;
            }

            if (stdOldContact.BusinessCompanyName != stdNewContact.BusinessCompanyName)
            {
                outlookContact.CompanyName = stdNewContact.BusinessCompanyName;
                dirty = true;
            }

            if (stdOldContact.BusinessPosition != stdNewContact.BusinessPosition)
            {
                outlookContact.JobTitle = stdNewContact.BusinessPosition;
                dirty = true;
            }

            // if we do not have a business address in the old address, we simply copy from the new one
            if (stdOldContact.BusinessAddressPrimary == null && stdNewContact.BusinessAddressPrimary != null)
            {
                stdOldContact.BusinessAddressPrimary = new AddressDetail();
                dirty = true;
            }

            if (stdOldContact.BusinessAddressPrimary != null && stdNewContact.BusinessAddressPrimary != null)
            {
                dirty = CompareAddress(outlookContact, stdNewContact.BusinessAddressPrimary, stdOldContact.BusinessAddressPrimary, "Business", dirty);
            }

            // if we do not have a business personal in the old address, we simply copy from the new one
            if (stdOldContact.PersonalAddressPrimary == null && stdNewContact.PersonalAddressPrimary != null)
            {
                stdOldContact.PersonalAddressPrimary = new AddressDetail();
                dirty = true;
            }

            if (stdOldContact.PersonalAddressPrimary != null && stdNewContact.PersonalAddressPrimary != null)
            {
                dirty = CompareAddress(outlookContact, stdNewContact.PersonalAddressPrimary, stdOldContact.PersonalAddressPrimary, "Home", dirty);
            }

            if ((stdOldContact.PersonalPhoneMobile == null && stdNewContact.PersonalPhoneMobile != null)
                || (stdNewContact.PersonalPhoneMobile != null && stdOldContact.PersonalPhoneMobile.ToString() != stdNewContact.PersonalPhoneMobile.ToString()))
            {
                outlookContact.MobileTelephoneNumber = (stdNewContact.PersonalPhoneMobile == null) ? null : stdNewContact.PersonalPhoneMobile.ToString();
                dirty = true;
            }

            if ((stdOldContact.BusinessPhoneMobile == null && stdNewContact.BusinessPhoneMobile != null)
                || (stdNewContact.BusinessPhoneMobile != null && stdOldContact.BusinessPhoneMobile.ToString() != stdNewContact.BusinessPhoneMobile.ToString()))
            {
                outlookContact.Business2TelephoneNumber = (stdNewContact.BusinessPhoneMobile == null) ? null : stdNewContact.BusinessPhoneMobile.ToString();
                dirty = true;
            }

            if ((stdOldContact.PersonalInstantMessengerAddresses == null && stdNewContact.PersonalInstantMessengerAddresses != null)
                || (stdNewContact.PersonalInstantMessengerAddresses != null
                    && stdOldContact.PersonalInstantMessengerAddresses.MsnMessenger != stdNewContact.PersonalInstantMessengerAddresses.MsnMessenger))
            {
                outlookContact.IMAddress = (stdNewContact.PersonalInstantMessengerAddresses == null) ? null : stdNewContact.PersonalInstantMessengerAddresses.MsnMessenger;
                dirty = true;
            }

            if ((stdOldContact.AdditionalTextData ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty)
                != (stdNewContact.AdditionalTextData ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty))
            {
                outlookContact.Body = stdNewContact.AdditionalTextData;
                dirty = true;
            }

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

            return dirty;
        }

        /// <summary>
        /// Compares an address of a new contact against an old contact. If the information differs, the outlook 
        /// contact will be updated with the information from the new contact.
        /// </summary>
        /// <param name="outlookContact"> The outlook contact. </param>
        /// <param name="stdNewContactAddress"> The std new contact address. </param>
        /// <param name="stdOldContactAddress"> The std old contact address. </param>
        /// <param name="prefix"> The prefix like "Home" or "Business". </param>
        /// <param name="dirty"> A value that indicates if the outlook contact has already been changed. </param>
        /// <returns>
        /// A value that indicates if the outlook contact has been changed.
        /// </returns>
        private static bool CompareAddress(_ContactItem outlookContact, AddressDetail stdNewContactAddress, AddressDetail stdOldContactAddress, string prefix, bool dirty)
        {
            if (stdOldContactAddress.CityName !=
                stdNewContactAddress.CityName)
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressCity", stdNewContactAddress.CityName);
                dirty = true;
            }

            if (stdOldContactAddress.CountryName !=
                stdNewContactAddress.CountryName)
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressCountry", stdNewContactAddress.CountryName);
                dirty = true;
            }

            if (stdOldContactAddress.PostalCode !=
                stdNewContactAddress.PostalCode)
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressPostalCode", stdNewContactAddress.PostalCode);
                dirty = true;
            }

            if (stdOldContactAddress.StateName !=
                stdNewContactAddress.StateName)
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressState", stdNewContactAddress.StateName);
                dirty = true;
            }

            if (stdOldContactAddress.StreetName !=
                stdNewContactAddress.StreetName)
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressStreet", stdNewContactAddress.StreetName);
                dirty = true;
            }

            if (stdNewContactAddress.Phone != null &&
                (stdOldContactAddress.Phone == null ||
                 stdOldContactAddress.Phone.ToString() != stdNewContactAddress.Phone.ToString()))
            {
                Tools.SetPropertyValue(outlookContact, prefix + "TelephoneNumber", stdNewContactAddress.Phone.ToString());
                dirty = true;
            }

            if ((stdOldContactAddress.StreetName ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty)
                != (stdNewContactAddress.StreetName ?? string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty))
            {
                Tools.SetPropertyValue(outlookContact, prefix + "AddressStreet", stdNewContactAddress.StreetName);
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
        private static byte[] SaveOutlookContactPicture(_ContactItem contact, out string pictureName)
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

                    // save the picture in the temp path
                    var fullName = Path.GetTempFileName();
                    try
                    {
                        attachement.SaveAsFile(fullName);
                        GCRelevantCall();
                    }
                    catch (System.Runtime.InteropServices.COMException)
                    {
                        // we may have a problem if there are too many pictures saved in this session
                        // then we need to clean up the outlook temp path (which is difficult to determine)
                        CleanupTempFolder();

                        // try again
                        attachement.SaveAsFile(fullName);
                    }

                    // read all bytes from the temp file
                    var bytes = File.ReadAllBytes(fullName);

                    // clean up the temp file
                    File.Delete(fullName);

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
        private static void CleanupTempFolder()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\12.0\\Outlook\\Security");
            if (reg == null)
            {
                return;
            }

            var folderReg = reg.GetValue("OutlookSecureTempFolder");
            if (folderReg == null)
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
        /// <param name="outlookContact">the outlook contact to handle</param>
        /// <returns>the corresponding Guid</returns>
        private static Guid GetStandardId(_ContactItem outlookContact)
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
                    // use the formerly generated id if it's not valid
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

            return newId;
        }
    }
}
