//-----------------------------------------------------------------------
// <copyright file="StdContact.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    using System;
    using System.IO;
    using System.Text;

    using Attributes;
    using DetailData;
    using Helpers;

    public class StdContact : StdElement
    {
        public Gender PersonGender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public PersonName Name { get; set; }

        public AddressDetail PersonalAddressPrimary { get; set; }
        public AddressDetail PersonalAddressSecondary { get; set; }
        public PhoneNumber PersonalPhoneMobile { get; set; }
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalEmailPrimary { get; set; }
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalEmailSecondary { get; set; }
        [ComparisonModifier(CaseInsensitive = true)]
        public string PersonalHomepage { get; set; }
        public InstantMessengerAddresses PersonalInstantMessengerAddresses { get; set; }
        public ProfileIdentifiers PersonalProfileIdentifiers { get; set; }

        public string BusinessCompanyName { get; set; }
        public string BusinessDepartment { get; set; }
        public AddressDetail BusinessAddressPrimary { get; set; }
        public AddressDetail BusinessAddressSecondary { get; set; }
        public PhoneNumber BusinessPhoneMobile { get; set; }

        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessEmailPrimary { get; set; }
        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessEmailSecondary { get; set; }
        public string BusinessPosition { get; set; }
        [ComparisonModifier(CaseInsensitive = true)]
        public string BusinessHomepage { get; set; }
        public InstantMessengerAddresses BusinessInstantMessengerAddresses { get; set; }

        public string AdditionalTextData { get; set; }

        [ComparisonModifier(CaseInsensitive = true)]
        public string PictureName { get; set; }

        /// <summary>
        /// Gets or sets the binary data of the associated picture.
        /// TODO: we currently do not have a gui to compare pictures
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "this is truely a property, not a collection of bytes and not a method."),
        ComparisonModifier(SkipCompare = true, SkipMerge = true)]
        public byte[] PictureData { get; set; }

        /// <summary>
        /// determines the full name including academic title, first, last and middle name 
        /// in a readable representation.
        /// </summary>
        /// <returns>the full name of the contact</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "this does contain logic, so it's not a property")]
        public string GetFullName()
        {
            var name = string.Empty;
            if (this.Name != null)
            {
                name += this.Name.LastName;
                name += string.IsNullOrEmpty(this.Name.AcademicTitle) ? string.Empty : " (" + this.Name.AcademicTitle + ")";
                name += ((name.Length > 0) ? ", " : string.Empty) + this.Name.FirstName + " ";
                name += this.Name.MiddleName + " ";
            }

            return name.Replace("()", string.Empty).Replace("  ", " ").Trim();
        }

        /// <summary>
        /// Determines a string that can be used to sort a list of StdContac elements.
        /// </summary>
        /// <returns>the sorting string</returns>
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

            return name.ToString();
        }

        /// <summary>
        /// Deterimes a string representation of the StdContact.
        /// </summary>
        /// <returns>a string representation of the StdContact</returns>
        public override string ToString()
        {
            return this.GetFullName();
        }

        public override string ToStringSimple()
        {
            var name = string.Empty;
            if (this.Name != null)
            {
                name += this.Name.LastName ?? string.Empty;
                name += ((name.Length > 0) ? ", " : string.Empty) + this.Name.FirstName + " ";
                name += this.Name.MiddleName + " ";
            }

            return name.Replace("()", string.Empty).Replace("  ", " ").Trim();
        }

        public static StdContact LoadFromFile(string fileName)
        {
            return SyncTools.LoadFromFile<StdContact>(fileName);
        }

        public void SaveToFile(string fileName)
        {
            SyncTools.SaveToFile(this, fileName);

            if (!string.IsNullOrEmpty(this.PictureName))
            {
                File.WriteAllBytes(fileName + "-" + this.PictureName, this.PictureData);
            }
        }

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
            }

            if (!string.IsNullOrEmpty(this.BusinessCompanyName))
            {
                this.BusinessCompanyName = this.BusinessCompanyName.Trim();
            }

            if (this.DateOfBirth.Year < 1900 || this.DateOfBirth.Year > 2200)
            {
                this.DateOfBirth = new DateTime(1900, 1, 1);
            }
        }


    }
}