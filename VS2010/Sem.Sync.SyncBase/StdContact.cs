// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StdContact.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class provides an entity to store standardized contact information. It should be capable to
//   store any information that's needed for describing contact relevant information of a person.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// This class provides an entity to store standardized contact information. It should be capable to 
    ///   store any information that's needed for describing contact relevant information of a person.
    /// </summary>
    [Serializable]
    public sealed class StdContact : StdElement
    {
        #region Properties

        /// <summary>
        ///   Gets or sets unstructured text data
        /// </summary>
        public string AdditionalTextData { get; set; }

        /// <summary>
        ///   Gets or sets the primary (first to use) business address. This is the address where the contact is "mostly"
        ///   available.
        /// </summary>
        public AddressDetail BusinessAddressPrimary { get; set; }

        /// <summary>
        ///   Gets or sets the secondary business address. This is the address where the contact is "maybe sometimes"
        ///   available.
        /// </summary>
        public AddressDetail BusinessAddressSecondary { get; set; }

        /// <summary>
        ///   Gets or sets the earned business certificates.
        /// </summary>
        public List<BusinessCertificate> BusinessCertificates { get; set; }

        /// <summary>
        ///   Gets or sets the name of the company this contact is associated with
        /// </summary>
        public string BusinessCompanyName { get; set; }

        /// <summary>
        ///   Gets or sets the name of the department inside the company this contact is associated with
        /// </summary>
        public string BusinessDepartment { get; set; }

        /// <summary>
        ///   Gets or sets the primary business email address. This (in most cases) is not available at non-business time
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessEmailPrimary { get; set; }

        /// <summary>
        ///   Gets or sets the secondary (not checked so often - used when at BusinessAddressSecondary - or similar) 
        ///   business email address. This (in most cases) is not available at non-business time
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessEmailSecondary { get; set; }

        /// <summary>
        ///   Gets or sets the business history (e.g. employment).
        /// </summary>
        public List<BusinessHistoryEntry> BusinessHistoryEntries { get; set; }

        /// <summary>
        ///   Gets or sets the internet address of the companies homepage.
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessHomepage { get; set; }

        /// <summary>
        ///   Gets or sets a collection of business instance messenger addresses where this contact is available.
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public InstantMessengerAddresses BusinessInstantMessengerAddresses { get; set; }

        /// <summary>
        ///   Gets or sets the business mobile phone number. This (in most cases) is not available at non-business time
        /// </summary>
        public PhoneNumber BusinessPhoneMobile { get; set; }

        /// <summary>
        ///   Gets or sets the position in the company. Something like "developer", "CIO" or "Human Resource Manager".
        ///   Typically this is something that's printed on the business card and describes the role inside the company.
        /// </summary>
        public string BusinessPosition { get; set; }

        /// <summary>
        ///   Gets or sets Categories.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2227:CollectionPropertiesShouldBeReadOnly", 
            Justification = "without a setter this cannot be serialized using the xml serializer")]
        public List<string> Categories { get; set; }

        /// <summary>
        ///   Gets or sets the known contacts of this contact
        /// </summary>
        public List<ContactReference> Contacts { get; set; }

        /// <summary>
        ///   Gets or sets the date of birth of a person. This property cannot be set to unusual dates. If the real 
        ///   date of birth is not clear, it should be set to the date with the highest propability.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        ///   Gets or sets a list of image data for this contact.
        /// </summary>
        public List<ImageEntry> ImageEntries { get; set; }

        /// <summary>
        ///   Gets or sets the knowledge levels for different languages.
        /// </summary>
        public List<LanguageKnowledge> LanguageKnowledge { get; set; }

        /// <summary>
        ///   Gets or sets the name description of the person.
        /// </summary>
        public PersonName Name { get; set; }

        /// <summary>
        ///   Gets or sets the gender/sex of a person.
        /// </summary>
        [DefaultValue(Gender.Unspecified)]
        public Gender PersonGender { get; set; }

        /// <summary>
        ///   Gets or sets the primary personal address used by the person (this address is normally 
        ///   shared with some parts of the family).
        /// </summary>
        public AddressDetail PersonalAddressPrimary { get; set; }

        /// <summary>
        ///   Gets or sets the secondary personal address used by the person (this address might be 
        ///   used in case of second address for the time while working).
        /// </summary>
        public AddressDetail PersonalAddressSecondary { get; set; }

        /// <summary>
        ///   Gets or sets the primary (high priority) personal (non-business) email address of this contact.
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalEmailPrimary { get; set; }

        /// <summary>
        ///   Gets or sets the secondary (low priority) personal (non-business) email address of this contact.
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalEmailSecondary { get; set; }

        /// <summary>
        ///   Gets or sets a personal (non-business) internet homepage address
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalHomepage { get; set; }

        /// <summary>
        ///   Gets or sets a personal (non-business) instant messenger address list (with addresses for googke-talk, msn-messenger or similar)
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public InstantMessengerAddresses PersonalInstantMessengerAddresses { get; set; }

        /// <summary>
        ///   Gets or sets the telephone number of the private mobile phone used by the person.
        /// </summary>
        public PhoneNumber PersonalPhoneMobile { get; set; }

        /// <summary>
        ///   Gets or sets the binary data of the associated picture.
        ///   TODO: we currently do not have a gui to compare pictures
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", 
            "CA1819:PropertiesShouldNotReturnArrays", 
            Justification = "this is truely a property, not a collection of bytes and not a method.")]
        [ComparisonModifier(SkipCompare = true, SkipMerge = true)]
        public byte[] PictureData { get; set; }

        /// <summary>
        ///   Gets or sets the name of the image file that is stored inside this contact.
        /// </summary>
        [ComparisonModifier(CaseInsensitive = true)]
        public string PictureName { get; set; }

        /// <summary>
        ///   Gets or sets the personal relationship status of the contact (married/single...).
        /// </summary>
        [DefaultValue(RelationshipStatus.Undefined)]
        public RelationshipStatus RelationshipStatus { get; set; }

        /// <summary>
        ///   Gets or sets a list of image data for this contact.
        /// </summary>
        public SerializableDictionary<string, string> SourceSpecificAttributes { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads a standard contact from the file system.
        /// </summary>
        /// <param name="fileName">
        /// the name of the file that does contain the serialized contact
        /// </param>
        /// <returns>
        /// the deserialized contact
        /// </returns>
        public static StdContact LoadFromFile(string fileName)
        {
            return Tools.LoadFromFile<StdContact>(fileName);
        }

        /// <summary>
        /// determines the full name including academic title, first, last and middle name 
        ///   in a readable representation.
        /// </summary>
        /// <returns>
        /// the full name of the contact
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
            Justification = "this does contain logic, so it's not a property")]
        [AddAsProperty]
        public string GetFullName()
        {
            return this.Name != null ? this.Name.ToString() : string.Empty;
        }

        /// <summary>
        /// Normalizes the information inside the contact by performing data clean up. This includes processing of replacement lists and
        ///   setting dates out of "normal" range to 1900.01.01.
        /// </summary>
        public override void NormalizeContent()
        {
            var dictionary = SyncTools.Replacements;

            if (dictionary != null)
            {
                if (dictionary.BusinessCompanyName != null)
                {
                    foreach (var item in dictionary.BusinessCompanyName)
                    {
                        if (this.BusinessCompanyName == item.Key)
                        {
                            this.BusinessCompanyName = item.Value;
                        }
                    }
                }

                if (dictionary.BusinessHomepage != null)
                {
                    foreach (var item in dictionary.BusinessHomepage)
                    {
                        if (this.BusinessHomepage == item.Key)
                        {
                            this.BusinessHomepage = item.Value;
                        }
                    }
                }

                if (dictionary.City != null)
                {
                    foreach (var item in dictionary.City)
                    {
                        var value = item.Value;
                        var key = item.Key;

                        if (this.PersonalAddressPrimary != null && this.PersonalAddressPrimary.CityName == key)
                        {
                            this.PersonalAddressPrimary.CityName = value;
                        }

                        if (this.BusinessAddressPrimary != null && this.BusinessAddressPrimary.CityName == key)
                        {
                            this.BusinessAddressPrimary.CityName = value;
                        }

                        if (this.PersonalAddressSecondary != null && this.PersonalAddressSecondary.CityName == key)
                        {
                            this.PersonalAddressSecondary.CityName = value;
                        }

                        if (this.BusinessAddressSecondary != null && this.BusinessAddressSecondary.CityName == key)
                        {
                            this.BusinessAddressSecondary.CityName = value;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.BusinessCompanyName))
            {
                this.BusinessCompanyName = this.BusinessCompanyName.Trim();
            }

            if (this.BusinessAddressPrimary != null)
            {
                if (!string.IsNullOrEmpty(this.BusinessAddressPrimary.PostalCode))
                {
                    this.BusinessAddressPrimary.PostalCode = this.BusinessAddressPrimary.PostalCode.Trim();
                }

                if (!string.IsNullOrEmpty(this.BusinessAddressPrimary.StreetName))
                {
                    this.BusinessAddressPrimary.StreetName = this.BusinessAddressPrimary.StreetName.Trim();
                }

                if (!string.IsNullOrEmpty(this.BusinessAddressPrimary.CityName))
                {
                    this.BusinessAddressPrimary.CityName = this.BusinessAddressPrimary.CityName.Trim();
                }
            }

            if (this.Categories != null)
            {
                var newCategories = new List<string>(this.Categories.Count);
                foreach (var category in this.Categories)
                {
                    var newCategory = category.Trim();
                    if (!newCategories.Contains(newCategory))
                    {
                        newCategories.Add(newCategory);
                    }
                }

                this.Categories = newCategories;
            }

            if (this.DateOfBirth.Year < 1900 || this.DateOfBirth.Year > 2200)
            {
                this.DateOfBirth = new DateTime(1900, 1, 1);
            }

            SyncTools.ClearNulls(this, typeof(StdContact));
        }

        /// <summary>
        /// Saves this standard contact to the file system
        /// </summary>
        /// <param name="fileName">
        /// the file name to store the information
        /// </param>
        public void SaveToFile(string fileName)
        {
            Tools.SaveToFile(this, fileName);

            if (!string.IsNullOrEmpty(this.PictureName))
            {
                File.WriteAllBytes(fileName + "-" + this.PictureName, this.PictureData);
            }
        }

        /// <summary>
        /// Determines a string that can be used to sort a list of StdContac elements.
        /// </summary>
        /// <returns>
        /// the sorting string
        /// </returns>
        [AddAsProperty]
        public override string ToSortSimple()
        {
            var name = new StringBuilder();
            if (this.Name != null)
            {
                name.Append(this.Name.LastName);
                name.Append(this.Name.FirstName);
                name.Append(this.Name.MiddleName);
                name.Append(this.Name.AcademicTitle);
            }

            return name.ToString().ToUpperInvariant();
        }

        /// <summary>
        /// Deterimes a string representation of the StdContact.
        /// </summary>
        /// <returns>
        /// a string representation of the StdContact
        /// </returns>
        public override string ToString()
        {
            return this.GetFullName();
        }

        /// <summary>
        /// Implements an overridable SIMPLE string representation in the format "lastname, firstname middlename"
        /// </summary>
        /// <returns>
        /// a dense and simple string representation of the entity
        /// </returns>
        [AddAsProperty]
        public override string ToStringSimple()
        {
            var name = string.Empty;
            if (this.Name != null)
            {
                name += this.Name.LastName ?? string.Empty;
                name += ((name.Length > 0) ? ", " : string.Empty) + this.Name.FirstName + " ";
                name += this.Name.MiddleName;
            }

            return name.Replace("  ", " ").Trim();
        }

        #endregion
    }
}