﻿//-----------------------------------------------------------------------
// <copyright file="VCardConverter.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using DetailData;

    /// <summary>
    /// Class to convert StdContacts to/from vCards
    /// This class does NOT fully implement the vCard 2.1 specification, because it does NOT support:
    /// - Property parameters without explicit name
    /// - folding of property values
    /// - grouping of properties
    /// - ONLY Base64-encoding is supported, quoted-printable and 8bit are NOT supported
    /// - value locations - ONLY inline and URL are supported
    /// </summary>
    public class VCardConverter
    {
        /// <summary>
        /// Gets or sets the HttpRequester to download pictures in case of url-references.
        /// </summary>
        public HttpHelper HttpRequester { get; set; }

        /// <summary>
        /// converts a StdContact into a vCard (binary content)
        /// </summary>
        /// <param name="contact"> The contact to be converted. </param>
        /// <returns> a binary vCard representation </returns>
        public static byte[] StdContactToVCard(StdContact contact)
        {
            if (contact == null || contact.Name == null)
            {
                throw new ArgumentNullException("contact");
            }

            var vCard = new StringBuilder();
            vCard.AppendLine("BEGIN:VCARD");
            vCard.AppendLine("VERSION:2.1");
            AddAttributeToStringBuilder(vCard, "N", contact.Name.LastName, contact.Name.FirstName, contact.Name.MiddleName, contact.Name.AcademicTitle);
            AddAttributeToStringBuilder(vCard, "FN", contact.GetFullName());
            AddAttributeToStringBuilder(vCard, "SORT-STRING", contact.Name.LastName);
            AddAttributeToStringBuilder(vCard, "EMAIL;TYPE=INTERNET;TYPE=WORK;TYPE=PREF", contact.BusinessEmailPrimary);
            AddAttributeToStringBuilder(vCard, "EMAIL;TYPE=INTERNET;TYPE=HOME", contact.PersonalEmailPrimary);
            AddAttributeToStringBuilder(vCard, "URL;TYPE=HOME", contact.PersonalHomepage);

            if (contact.DateOfBirth.Year > 1900 && contact.DateOfBirth.Year < 2200)
            {
                AddAttributeToStringBuilder(vCard, "BDAY", contact.DateOfBirth.ToString("yyyyMMdd", CultureInfo.CurrentCulture));
            }

            if (contact.BusinessAddressPrimary != null)
            {
                AddAttributeToStringBuilder(vCard, "ADR;TYPE=WORK", null, null, contact.BusinessAddressPrimary.StreetName, contact.BusinessAddressPrimary.CityName, contact.BusinessAddressPrimary.StateName, contact.BusinessAddressPrimary.PostalCode, contact.BusinessAddressPrimary.CountryName);
                AddAttributeToStringBuilder(vCard, "TEL;TYPE=WORK", contact.BusinessAddressPrimary.Phone);
            }

            if (contact.PersonalAddressPrimary != null)
            {
                AddAttributeToStringBuilder(vCard, "ADR;TYPE=HOME", contact.PersonalAddressPrimary.StreetName, contact.PersonalAddressPrimary.CityName, contact.PersonalAddressPrimary.StateName, contact.PersonalAddressPrimary.PostalCode, contact.PersonalAddressPrimary.CountryName);
                AddAttributeToStringBuilder(vCard, "TEL;TYPE=HOME", contact.PersonalAddressPrimary.Phone);
            }

            AddAttributeToStringBuilder(vCard, "TEL;TYPE=CELL,HOME", contact.PersonalPhoneMobile);
            AddAttributeToStringBuilder(vCard, "TEL;TYPE=CELL,WORK", contact.BusinessPhoneMobile);

            AddAttributeToStringBuilder(vCard, "ORG", contact.BusinessCompanyName);
            AddAttributeToStringBuilder(vCard, "URL;TYPE=WORK", contact.BusinessHomepage);
            AddAttributeToStringBuilder(vCard, "URL;TYPE=HOME", contact.PersonalHomepage);
            AddAttributeToStringBuilder(vCard, "TITLE", contact.BusinessPosition);
            AddAttributeToStringBuilder(vCard, "NOTE", contact.AdditionalTextData);

            AddAttributeToStringBuilder(vCard, "X-MATZEN-GENERATOR", "generated by Sem.Sync - www.svenerikmatzen.info");
            AddAttributeToStringBuilder(vCard, "PRODID", "-//MATZEN//www.svenerikmatzen.info//Sem.Sync//Version 1.0");
            AddAttributeToStringBuilder(vCard, "PHOTO;TYPE=JPEG", contact.PictureData);
            AddAttributeToStringBuilder(vCard, "UID", contact.Id.ToString("N"));

            vCard.AppendLine("END:VCARD");

            return Encoding.UTF8.GetBytes(vCard.ToString());
        }

        /// <summary>
        /// converts a vCard into a standard contact.
        /// </summary>
        /// <param name="vCard"> The vCard. </param>
        /// <returns>a StdContact representation of the vCard</returns>
        public StdContact VCardToStdContact(byte[] vCard)
        {
            return this.VCardToStdContact(vCard, ProfileIdentifierType.Default);
        }

        /// <summary>
        /// converts a vCard into a standard contact.
        /// </summary>
        /// <param name="vCard"> The vCard. </param>
        /// <param name="useIndetifierAs"> This value determines the meaning of the identifier. </param>
        /// <returns>a StdContact representation of the vCard</returns>
        public StdContact VCardToStdContact(byte[] vCard, ProfileIdentifierType useIndetifierAs)
        {
            if (vCard == null)
            {
                throw new ArgumentNullException("vCard");
            }

            var contact = new StdContact
            {
                InternalSyncData = new SyncData
                {
                    DateOfCreation = DateTime.Now,
                    DateOfLastChange = DateTime.Now
                },
                Name = new PersonName(),
            };

            var vCardUTF8 = Encoding.UTF8.GetString(vCard);
            var vCardIso8859 = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("iso8859-1"), Encoding.UTF8, vCard));

            var uid = string.Empty;
            contact.Name = new PersonName();

            var linesUtf8 = vCardUTF8.Split('\n');
            var linesIso8859 = vCardIso8859.Split('\n');
            for (var i = 0; i < linesIso8859.Length; i++)
            {
                var line = linesIso8859[i];

                if (line.Length == 0)
                {
                    continue;
                }

                if (line.ToUpperInvariant().Contains("CHARSET=UTF-8:"))
                {
                    line = linesUtf8[i];
                }

                var propertyDescription = line.Substring(0, line.IndexOf(':')).ToUpperInvariant();
                var value = line.Substring(line.IndexOf(':') + 1).Replace("\r", string.Empty);
                var valueParts = value.Split(';');
                var type = PropertyAttribute(propertyDescription, "TYPE", string.Empty);

                var propertyName = propertyDescription;
                if (propertyDescription.Contains(";"))
                {
                    propertyName = propertyDescription.Substring(0, propertyDescription.IndexOf(';'));
                }

                switch (propertyName)
                {
                    case "TEL":

                        if (type.Contains("CELL"))
                        {
                            if (type.Contains("HOME"))
                            {
                                contact.PersonalPhoneMobile = new PhoneNumber(value);
                                break;
                            }

                            if (type.Contains("WORK"))
                            {
                                contact.BusinessPhoneMobile = new PhoneNumber(value);
                                break;
                            }
                        }

                        if (type.Contains("HOME"))
                        {
                            if (contact.PersonalAddressPrimary == null)
                            {
                                contact.PersonalAddressPrimary = new AddressDetail();
                            }

                            contact.PersonalAddressPrimary.Phone = new PhoneNumber(value);
                            break;
                        }

                        if (type.Contains("WORK"))
                        {
                            if (contact.BusinessAddressPrimary == null)
                            {
                                contact.BusinessAddressPrimary = new AddressDetail();
                            }

                            contact.BusinessAddressPrimary.Phone = new PhoneNumber(value);
                        }

                        break;

                    case "N":
                        contact.Name.LastName = GetNthElement(valueParts, 1);
                        contact.Name.FirstName = GetNthElement(valueParts, 2);
                        contact.Name.MiddleName = GetNthElement(valueParts, 3);
                        contact.Name.AcademicTitle = GetNthElement(valueParts, 4);
                        break;

                    case "EMAIL":
                        if (type.Contains("WORK"))
                        {
                            contact.BusinessEmailPrimary = value;
                            break;
                        }

                        if (type.Contains("HOME"))
                        {
                            contact.PersonalEmailPrimary = value;
                            break;
                        }

                        Console.WriteLine("!!Unhandled email address !!");
                        break;

                    case "URL":
                        if (type.Contains("HOME"))
                        {
                            contact.PersonalHomepage = value;
                        }
                        else
                        {
                            contact.BusinessHomepage = value;
                        }

                        break;

                    case "ORG":
                        contact.BusinessCompanyName = value;
                        break;

                    case "NOTE":
                        contact.AdditionalTextData = value;
                        break;

                    case "ADR":
                        if (line.EndsWith(";;;;;;\r", StringComparison.Ordinal))
                        {
                            // in this case we do not have an address - it's all empty
                            break;
                        }

                        var address = new AddressDetail
                            {
                                CityName = GetNthElement(valueParts, 4),

                                StreetName = GetNthElement(valueParts, 3),
                                StreetNumber = SyncTools.ExtractStreetNumber(GetNthElement(valueParts, 3)),
                                StreetNumberExtension = SyncTools.ExtractStreetNumberExtension(GetNthElement(valueParts, 3)),

                                StateName = GetNthElement(valueParts, 5),

                                PostalCode = GetNthElement(valueParts, 6),

                                CountryName = GetNthElement(valueParts, 7),
                            };

                        if (type.Contains("WORK"))
                        {
                            contact.BusinessAddressPrimary = address;
                        }
                        else
                        {
                            contact.PersonalAddressPrimary = address;
                        }

                        break;

                    case "TITLE":
                        contact.BusinessPosition = value;
                        break;

                    case "PHOTO":
                        if (PropertyAttribute(propertyDescription, "ENCODING", string.Empty).Contains("B"))
                        {
                            contact.PictureData = Convert.FromBase64String(value);
                        }
                        else
                        {
                            var url = value.Replace("URI:", string.Empty);
                            contact.PictureData = this.HttpRequester.GetContentBinary(url, url);
                        }

                        break;

                    case "BDAY":
                        var dateString = value;
                        if (dateString.IndexOf("-", 0, StringComparison.Ordinal) == -1)
                        {
                            dateString = dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" +
                                     dateString.Substring(6, 2);
                        }

                        contact.DateOfBirth = DateTime.Parse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);
                        break;

                    case "UID":
                        if (contact.PersonalProfileIdentifiers == null)
                        {
                            contact.PersonalProfileIdentifiers = new ProfileIdentifiers();
                        }

                        switch (useIndetifierAs)
                        {
                            case ProfileIdentifierType.XingProfileId:
                                uid = value.Substring(13);
                                contact.PersonalProfileIdentifiers.XingProfileId = value;
                                break;

                            case ProfileIdentifierType.FacebookProfileId:
                                contact.PersonalProfileIdentifiers.FacebookProfileId = value;
                                break;

                            default:
                                uid = value;
                                break;
                        }

                        break;

                    case "PRODID":
                    case "BEGIN":
                    case "END":
                    case "SORT-STRING":
                    case "CLASS":
                    case "FN":
                    case "":
                    case "CATEGORIES":
                    case "VERSION:2.1":
                        break;

                    default:
                        Console.WriteLine("unhandled: " + line.Replace("\r", string.Empty));
                        break;
                }
            }

            if (uid.Length == 32)
            {
                contact.Id = new Guid(
                    uid.Substring(0, 8) + "-" +
                    uid.Substring(8, 4) + "-" +
                    uid.Substring(12, 4) + "-" +
                    uid.Substring(16, 4) + "-" +
                    uid.Substring(20, 12));
            }
            else
            {
                contact.Id = uid.Length > 0
                    ? new Guid(int.Parse(uid, CultureInfo.InvariantCulture), 1, 2, 3, 4, 5, 6, 7, 8, 9, 0)
                    : Guid.NewGuid();
            }

            return contact;
        }

        /// <summary>
        /// gets a list of attribute values from a vCard property
        /// </summary>
        /// <param name="property">the vCard property to get the attribute from</param>
        /// <param name="attributeName">the attribute name</param>
        /// <param name="defaultAttributeValue">a default value for the attribute, if there is no value inside the property</param>
        /// <returns>a list of attribute values</returns>
        private static List<string> PropertyAttribute(string property, string attributeName, string defaultAttributeValue)
        {
            var values = new List<string>();
            foreach (var propertyItem in property.Split(';'))
            {
                if (propertyItem.StartsWith(attributeName + "=", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var s in propertyItem.Substring(propertyItem.IndexOf('=') + 1).Split(','))
                    {
                        values.Add(s);
                    }
                }
            }

            if (values.Count == 0)
            {
                foreach (var s in defaultAttributeValue.Split(','))
                {
                    values.Add(s);
                }
            }

            return values;
        }

        /// <summary>
        /// get the n'th element if there is one
        /// </summary>
        /// <param name="inputArray">the array to get the value from</param>
        /// <param name="index">the index of the element to get</param>
        /// <returns>the string element with the desired index, null if there is no such entry</returns>
        private static string GetNthElement(string[] inputArray, int index)
        {
            if (inputArray.Length < index || index < 1)
            {
                return null;
            }

            var returnValue = inputArray[index - 1];
            return returnValue;
        }

        /// <summary>
        /// adds a contact attribute to the vCard. This overload will simply use the ToString method to serialize the object
        /// </summary>
        /// <param name="vCard">the StringBuilder that is currently writing the vCard</param>
        /// <param name="attributeSpecification">the textual attribute specification</param>
        /// <param name="values">the value to write to the attribute</param>
        private static void AddAttributeToStringBuilder(StringBuilder vCard, string attributeSpecification, object values)
        {
            if (values != null)
            {
                AddAttributeToStringBuilder(vCard, attributeSpecification, values.ToString());
            }
        }

        /// <summary>
        /// adds a contact attribute to the vCard. This overload will use base64 encoding
        /// </summary>
        /// <param name="vCard">the StringBuilder that is currently writing the vCard</param>
        /// <param name="attributeSpecification">the textual attribute specification</param>
        /// <param name="values">the value to write to the attribute</param>
        private static void AddAttributeToStringBuilder(StringBuilder vCard, string attributeSpecification, byte[] values)
        {
            if (values != null && values.Length > 0)
            {
                AddAttributeToStringBuilder(vCard, attributeSpecification + ";ENCODING=B", Convert.ToBase64String(values));
            }
        }

        /// <summary>
        /// adds a contact attribute to the vCard. 
        /// </summary>
        /// <param name="vCard">the StringBuilder that is currently writing the vCard</param>
        /// <param name="attributeSpecification">the textual attribute specification</param>
        /// <param name="values">the value to write to the attribute</param>
        private static void AddAttributeToStringBuilder(StringBuilder vCard, string attributeSpecification, params string[] values)
        {
            foreach (var s in values)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    vCard.AppendLine(attributeSpecification + ";CHARSET=UTF-8:" + string.Join(";", values));
                    break;
                }
            }
        }
    }
}
