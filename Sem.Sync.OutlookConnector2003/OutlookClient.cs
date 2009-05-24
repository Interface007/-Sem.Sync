namespace Sem.Sync.OutlookConnector2003
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    
    using Microsoft.Office.Interop.Outlook;
    using Microsoft.Win32;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    public static class OutlookClient
    {
        private const string ContactIdOutlookPropertyName = "SemSyncId";

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
                        continue;

                    // extract the file name
                    pictureName = attachement.FileName;

                    // save the picture in the temp path
                    var fullName = Path.GetTempFileName();
                    try
                    {
                        attachement.SaveAsFile(fullName);
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
            pictureName = "";
            return new byte[0];
        }

        /// <summary>
        /// Clean up the outlook exclusive temp folder (who knows why they cannot simply use 
        /// the normal temp folder with standard temp file handling...)
        /// </summary>
        private static void CleanupTempFolder()
        {
            var reg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Office\\11.0\\Outlook\\Security");
            if (reg == null) return;

            var folderReg = reg.GetValue("OutlookSecureTempFolder");
            if (folderReg == null) return;

            foreach (var picturePath in Directory.GetFiles(folderReg.ToString(), "Contact*.jpg"))
            {
                try
                {
                    File.Delete(picturePath);
                }
                catch (IOException)
                {
                    // this.LogProcessingEvent("problem to clean the outlook temp folder " + picturePath + ": " + ex.Message);
                }
            }
        }

        public static MAPIFolder GetOutlookMAPIFolder(_NameSpace outlookNamespace, string folderName, OlDefaultFolders defaultFolder)
        {
            // Get all the Contacts Folder
            var pathPart = GetNextPathPart(folderName, out folderName);

            var contacts =
                (pathPart == "ask") ? outlookNamespace.PickFolder() :
                (string.IsNullOrEmpty(pathPart)) ? outlookNamespace.GetDefaultFolder(defaultFolder) :
                (from x in outlookNamespace.Folders.OfType<MAPIFolder>()
                 where x.Name == pathPart
                 select x).FirstOrDefault();

            if (folderName.Length > 0 && contacts != null)
            {
                contacts = GetOutlookContactsFolder(contacts, folderName);
            }

            return contacts;
        }

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

        public static NameSpace GetNameSpace()
        {
            var outlookApplication = new Application();

            // Get NameSpace.
            var outlookNamespace = outlookApplication.GetNamespace("mapi");

            // Logon. If an outlook app is already open, then it will reuse that session. Else
            // it will perform a fresh logon. If you have profiles and passwords for the same, 
            // you need to enter the passwords in the dialogbox when they are shown
            outlookNamespace.Logon("SR", "", true, true);

            return outlookNamespace;
        }

        private static string GetNextPathPart(string path, out string returnPath)
        {
            var result = "";
            if (path != null)
            {
                while (path.StartsWith(@"\"))
                {
                    path = path.Substring(1);
                }

                var idx = path.IndexOf('\\');
                idx = (idx == -1) ? path.Length : idx;

                result = path.Substring(0, idx);
                returnPath = (idx < path.Length) ? path.Substring(idx + 1) : "";
            }
            else
            {
                returnPath = "";
            }

            return result;
        }

        /// <summary>
        /// Generate a new
        /// </summary>
        /// <param name="outlookContact"></param>
        /// <returns></returns>
        private static Guid GetStandardId(_ContactItem outlookContact)
        {
            if (outlookContact == null) throw new ArgumentNullException("outlookContact");

            var newId = Guid.NewGuid();
            try
            {
                // try to read the contact id property - generate one if it's not there
                var contactIdObject = outlookContact.UserProperties[ContactIdOutlookPropertyName] ??
                                      outlookContact.UserProperties.Add(ContactIdOutlookPropertyName,
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

        public static List<ContactsItemContainer> GetContactsList(Items contactsEnum)
        {
            if (contactsEnum == null) throw new ArgumentNullException("contactsEnum");

            var contactsList = new List<ContactsItemContainer>();
            foreach (var item in contactsEnum)
            {
                if (item is ContactItem)
                {
                    contactsList.Add(new ContactsItemContainer { Item = (ContactItem)item });
                }
            }
            return contactsList;
        }

        public static bool WriteContactToOutlook(Items contactsEnum, StdContact element, bool skipIfExisting, List<ContactsItemContainer> contactsList)
        {
            if (contactsEnum == null) throw new ArgumentNullException("contactsEnum");
            if (element == null) throw new ArgumentNullException("element");

            var outlookContact = (from x in contactsList
                                  where x.Id == element.Id.ToString()
                                  select x.Item).FirstOrDefault();

            if (skipIfExisting && outlookContact != null)
                return false;

            if (outlookContact == null)
                outlookContact = (ContactItem)contactsEnum.Add(OlItemType.olContactItem);

            // convert StdContact to Outlook contact
            if (ConvertToNativeContact(element, outlookContact))
            {
                outlookContact.Save();
                return true;
            }

            return false;
        }

        #region conversion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "this is only multiple if-statements - that's not really complex")]
        private static bool ConvertToNativeContact(StdContact stdNewContact, _ContactItem outlookContact)
        {
            if (stdNewContact == null) throw new ArgumentNullException("stdNewContact");
            if (outlookContact == null) throw new ArgumentNullException("outlookContact");

            var dirty = false;
            var stdOldContact = ConvertToStandardContact(outlookContact);
            var gender = stdNewContact.PersonGender ==
                                        Gender.Unspecified ?
                                        OlGender.olUnspecified :
                                            ((stdNewContact.PersonGender == Gender.Male) ?
                                            OlGender.olMale :
                                            OlGender.olFemale);

            if (stdOldContact.DateOfBirth != stdNewContact.DateOfBirth) { outlookContact.Birthday = stdNewContact.DateOfBirth; dirty = true; }
            if (stdOldContact.Name.FirstName != stdNewContact.Name.FirstName) { outlookContact.FirstName = stdNewContact.Name.FirstName; dirty = true; }
            if (stdOldContact.Name.MiddleName != stdNewContact.Name.MiddleName) { outlookContact.MiddleName = stdNewContact.Name.MiddleName; dirty = true; }
            if (stdOldContact.Name.LastName != stdNewContact.Name.LastName) { outlookContact.LastName = stdNewContact.Name.LastName; dirty = true; }
            if (stdOldContact.Name.AcademicTitle != stdNewContact.Name.AcademicTitle) { outlookContact.Title = stdNewContact.Name.AcademicTitle; dirty = true; }

            if (stdOldContact.PersonGender != stdNewContact.PersonGender) { outlookContact.Gender = gender; dirty = true; }
            if (stdOldContact.BusinessEmailPrimary != stdNewContact.BusinessEmailPrimary) { outlookContact.Email2Address = stdNewContact.BusinessEmailPrimary; dirty = true; }
            if (stdOldContact.PersonalEmailPrimary != stdNewContact.PersonalEmailPrimary) { outlookContact.Email1Address = stdNewContact.PersonalEmailPrimary; dirty = true; }
            if (stdOldContact.BusinessHomepage != stdNewContact.BusinessHomepage) { outlookContact.BusinessHomePage = stdNewContact.BusinessHomepage; dirty = true; }
            if (stdOldContact.PersonalHomepage != stdNewContact.PersonalHomepage) { outlookContact.PersonalHomePage = stdNewContact.PersonalHomepage; dirty = true; }
            if (stdOldContact.BusinessCompanyName != stdNewContact.BusinessCompanyName) { outlookContact.CompanyName = stdNewContact.BusinessCompanyName; dirty = true; }
            if (stdOldContact.BusinessPosition != stdNewContact.BusinessPosition) { outlookContact.JobTitle = stdNewContact.BusinessPosition; dirty = true; }

            // if we do not have a business address in the old address, we simply copy from the new one
            if (stdOldContact.BusinessAddressPrimary == null && stdNewContact.BusinessAddressPrimary != null)
            {
                stdOldContact.BusinessAddressPrimary = stdNewContact.BusinessAddressPrimary;
                dirty = true;
            }
            else
            {
                if (stdOldContact.BusinessAddressPrimary != null && stdNewContact.BusinessAddressPrimary != null)
                {
                    if (stdOldContact.BusinessAddressPrimary.CityName != stdNewContact.BusinessAddressPrimary.CityName) { outlookContact.BusinessAddressCity = stdNewContact.BusinessAddressPrimary.CityName; dirty = true; }
                    if (stdOldContact.BusinessAddressPrimary.CountryName != stdNewContact.BusinessAddressPrimary.CountryName) { outlookContact.BusinessAddressCountry = stdNewContact.BusinessAddressPrimary.CountryName; dirty = true; }
                    if (stdOldContact.BusinessAddressPrimary.PostalCode != stdNewContact.BusinessAddressPrimary.PostalCode) { outlookContact.BusinessAddressPostalCode = stdNewContact.BusinessAddressPrimary.PostalCode; dirty = true; }
                    if (stdOldContact.BusinessAddressPrimary.StateName != stdNewContact.BusinessAddressPrimary.StateName) { outlookContact.BusinessAddressState = stdNewContact.BusinessAddressPrimary.StateName; dirty = true; }
                    if (stdNewContact.BusinessAddressPrimary.Phone != null && (stdOldContact.BusinessAddressPrimary.Phone == null
                        || stdOldContact.BusinessAddressPrimary.Phone.ToString() != stdNewContact.BusinessAddressPrimary.Phone.ToString())) { outlookContact.BusinessTelephoneNumber = stdNewContact.BusinessAddressPrimary.Phone.ToString(); dirty = true; }

                    if ((stdOldContact.BusinessAddressPrimary.StreetName ?? "").Replace("\r", "").Replace("\n", "")
                        != (stdNewContact.BusinessAddressPrimary.StreetName ?? "").Replace("\r", "").Replace("\n", ""))
                    {
                        outlookContact.BusinessAddressStreet = stdNewContact.BusinessAddressPrimary.StreetName;
                        dirty = true;
                    }
                }
            }

            // if we do not have a business personal in the old address, we simply copy from the new one
            if (stdOldContact.PersonalAddressPrimary == null && stdNewContact.PersonalAddressPrimary != null)
            {
                stdOldContact.PersonalAddressPrimary = stdNewContact.PersonalAddressPrimary;
                dirty = true;
            }
            else
            {
                if (stdOldContact.PersonalAddressPrimary != null && stdNewContact.PersonalAddressPrimary != null)
                {
                    if (stdOldContact.PersonalAddressPrimary.CityName != stdNewContact.PersonalAddressPrimary.CityName) { outlookContact.HomeAddressCity = stdNewContact.PersonalAddressPrimary.CityName; dirty = true; }
                    if (stdOldContact.PersonalAddressPrimary.CountryName != stdNewContact.PersonalAddressPrimary.CountryName) { outlookContact.HomeAddressCountry = stdNewContact.PersonalAddressPrimary.CountryName; dirty = true; }
                    if (stdOldContact.PersonalAddressPrimary.PostalCode != stdNewContact.PersonalAddressPrimary.PostalCode) { outlookContact.HomeAddressPostalCode = stdNewContact.PersonalAddressPrimary.PostalCode; dirty = true; }
                    if (stdOldContact.PersonalAddressPrimary.StateName != stdNewContact.PersonalAddressPrimary.StateName) { outlookContact.HomeAddressState = stdNewContact.PersonalAddressPrimary.StateName; dirty = true; }
                    if (stdOldContact.PersonalAddressPrimary.StreetName != stdNewContact.PersonalAddressPrimary.StreetName) { outlookContact.HomeAddressStreet = stdNewContact.PersonalAddressPrimary.StreetName; dirty = true; }
                    if (stdNewContact.PersonalAddressPrimary.Phone != null
                        && (stdOldContact.PersonalAddressPrimary.Phone == null || stdOldContact.PersonalAddressPrimary.Phone.ToString() != stdNewContact.PersonalAddressPrimary.Phone.ToString()))
                    {
                        outlookContact.HomeTelephoneNumber = stdNewContact.PersonalAddressPrimary.Phone.ToString();
                        dirty = true;
                    }

                    if ((stdOldContact.PersonalAddressPrimary.StreetName ?? "").Replace("\r", "").Replace("\n", "")
                        != (stdNewContact.PersonalAddressPrimary.StreetName ?? "").Replace("\r", "").Replace("\n", ""))
                    {
                        outlookContact.HomeAddressStreet = stdNewContact.PersonalAddressPrimary.StreetName;
                        dirty = true;
                    }
                }
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
                outlookContact.IMAddress = (stdNewContact.PersonalInstantMessengerAddresses == null)? null : stdNewContact.PersonalInstantMessengerAddresses.MsnMessenger; 
                dirty = true; 
            }

            if ((stdOldContact.AdditionalTextData ?? "").Replace("\r", "").Replace("\n", "")
                != (stdNewContact.AdditionalTextData ?? "").Replace("\r", "").Replace("\n", ""))
            {
                outlookContact.Body = stdNewContact.AdditionalTextData;
                dirty = true;
            }

            if (stdOldContact.Id != stdNewContact.Id)
            {
                outlookContact.UserProperties.Add(ContactIdOutlookPropertyName,
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

        public static StdContact ConvertToStandardContact(_ContactItem outlookContact)
        {
            if (outlookContact == null) throw new ArgumentNullException("outlookContact");

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
                                : (outlookContact.Title.IsOneOf("Herr", "Mr."))
                                        ? Gender.Male
                                        : (outlookContact.Title.IsOneOf("Frau", "Mrs."))
                                                ? Gender.Female
                                                : Gender.Unspecified,

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
                    StreetNumberExtension =
                        SyncTools.ExtractStreetNumberExtension(outlookContact.HomeAddressStreet),
                },

                PersonalHomepage = outlookContact.PersonalHomePage,
                PersonalEmailPrimary = outlookContact.Email1Address,
                PersonalInstantMessengerAddresses = (string.IsNullOrEmpty(outlookContact.IMAddress))? null : new InstantMessengerAddresses(outlookContact.IMAddress),
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
                    StreetNumberExtension =
                        SyncTools.ExtractStreetNumberExtension(outlookContact.BusinessAddressStreet),
                },

                BusinessHomepage = outlookContact.BusinessHomePage,
                BusinessEmailPrimary = outlookContact.Email2Address,
                BusinessPhoneMobile = (!string.IsNullOrEmpty(outlookContact.Business2TelephoneNumber)) ? new PhoneNumber(outlookContact.Business2TelephoneNumber) : null,

                AdditionalTextData = outlookContact.Body,
                PictureName = pictureName,
                PictureData = pictureData
            };

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
        
        public static StdCalendarItem ConvertToStandardCalendarItem(_AppointmentItem outlookItem)
        {
            if (outlookItem == null) throw new ArgumentNullException("outlookItem");

            var result = new StdCalendarItem { Subject = outlookItem.Subject };
            // TODO: this is a very "incomplete" version of the method
            return result;
        }

        #endregion

        internal static bool WriteCalendarItemToOutlook(Items contactsEnum, StdCalendarItem stdCalendarItem, bool skipIfExisting, List<ContactsItemContainer> contactsList)
        {
            if (contactsEnum == null) throw new ArgumentNullException("contactsEnum");
            if (stdCalendarItem == null) throw new ArgumentNullException("stdCalendarItem");

            throw new NotImplementedException();
        }
    }
}
