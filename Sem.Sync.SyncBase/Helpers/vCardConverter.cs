﻿//-----------------------------------------------------------------------
// <copyright file="VCardConverter.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Helpers
{
    using System;
    using System.Globalization;
    using System.Text;

    using DetailData;

    /// <summary>
    /// Class to convert StdContacts to/from vCards
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
            if (contact == null)
            {
                throw new ArgumentNullException("contact");
            }

            var vCard = new StringBuilder();
            vCard.AppendLine("BEGIN:VCARD");
            vCard.AppendLine("VERSION:2.1");
            vCard.AppendLine(string.Format(CultureInfo.CurrentCulture, "N;CHARSET=UTF-8:{0};{1};{2};{3}", contact.Name.LastName, contact.Name.FirstName, contact.Name.MiddleName, contact.Name.AcademicTitle));
            vCard.AppendLine("FN;CHARSET=UTF-8:" + contact.GetFullName());
            vCard.AppendLine("SORT-STRING:" + contact.Name.LastName);
            vCard.AppendLine("CLASS:PRIVATE");

            if (!string.IsNullOrEmpty(contact.BusinessEmailPrimary)) vCard.AppendLine("EMAIL;TYPE=internet;type=WORK;type=pref:" + contact.BusinessEmailPrimary);
            if (!string.IsNullOrEmpty(contact.PersonalEmailPrimary)) vCard.AppendLine("EMAIL;TYPE=internet;type=HOME:" + contact.PersonalEmailPrimary);
            if (contact.DateOfBirth.Year > 1900 && contact.DateOfBirth.Year < 2200) vCard.AppendLine("BDAY:" + contact.DateOfBirth.ToString("yyyyMMdd", CultureInfo.CurrentCulture));
            if (!string.IsNullOrEmpty(contact.PersonalHomepage)) vCard.AppendLine("URL;TYPE=home:" + contact.PersonalHomepage);
            if (contact.BusinessAddressPrimary != null)
            {
                if (!string.IsNullOrEmpty(contact.BusinessAddressPrimary.StreetName + contact.BusinessAddressPrimary.CityName + contact.BusinessAddressPrimary.StateName + contact.BusinessAddressPrimary.PostalCode + contact.BusinessAddressPrimary.CountryName)) vCard.AppendLine(string.Format(CultureInfo.CurrentCulture, "ADR;TYPE=work;CHARSET=UTF-8:;;{0};{1};{2};{3};{4}", contact.BusinessAddressPrimary.StreetName, contact.BusinessAddressPrimary.CityName, contact.BusinessAddressPrimary.StateName, contact.BusinessAddressPrimary.PostalCode, contact.BusinessAddressPrimary.CountryName));
                if (contact.BusinessAddressPrimary.Phone != null && !string.IsNullOrEmpty(contact.BusinessAddressPrimary.Phone.ToString())) vCard.AppendLine("TEL;TYPE=work:" + contact.BusinessAddressPrimary.Phone);
            }
            if (contact.PersonalAddressPrimary != null)
            {
                if (!string.IsNullOrEmpty(contact.PersonalAddressPrimary.StreetName + contact.PersonalAddressPrimary.CityName + contact.PersonalAddressPrimary.StateName + contact.PersonalAddressPrimary.PostalCode + contact.PersonalAddressPrimary.CountryName)) vCard.AppendLine(string.Format(CultureInfo.CurrentCulture, "ADR;TYPE=home;CHARSET=UTF-8:;;{0};{1};{2};{3};{4}", contact.PersonalAddressPrimary.StreetName, contact.PersonalAddressPrimary.CityName, contact.PersonalAddressPrimary.StateName, contact.PersonalAddressPrimary.PostalCode, contact.PersonalAddressPrimary.CountryName));
                if (contact.PersonalAddressPrimary.Phone != null && !string.IsNullOrEmpty(contact.PersonalAddressPrimary.Phone.ToString())) vCard.AppendLine("TEL;TYPE=home:" + contact.PersonalAddressPrimary.Phone);
            }
            if (contact.PersonalPhoneMobile != null && !string.IsNullOrEmpty(contact.PersonalPhoneMobile.ToString())) vCard.AppendLine("TEL;TYPE=cell,home:" + contact.PersonalPhoneMobile);
            if (contact.BusinessPhoneMobile != null && !string.IsNullOrEmpty(contact.BusinessPhoneMobile.ToString())) vCard.AppendLine("TEL;TYPE=cell,work:" + contact.BusinessPhoneMobile);
            if (!string.IsNullOrEmpty(contact.BusinessCompanyName)) vCard.AppendLine("ORG;CHARSET=ISO-8859-1:" + contact.BusinessCompanyName);
            if (!string.IsNullOrEmpty(contact.BusinessHomepage)) vCard.AppendLine("URL;TYPE=work:" + contact.BusinessHomepage);
            if (!string.IsNullOrEmpty(contact.PersonalHomepage)) vCard.AppendLine("URL;TYPE=home:" + contact.PersonalHomepage);
            if (!string.IsNullOrEmpty(contact.BusinessPosition)) vCard.AppendLine("TITLE;CHARSET=UTF-8:" + contact.BusinessPosition);
            vCard.AppendLine("NOTE;CHARSET=UTF-8:generated by Sem.Sync - www.svenerikmatzen.info");
            vCard.AppendLine("PRODID:-//MATZEN//www.svenerikmatzen.info//Sem.Sync//Version 1.0");
            if (!string.IsNullOrEmpty(contact.PictureName)) vCard.AppendLine("PHOTO;ENCODING=b;TYPE=JPEG:" + Convert.ToBase64String(contact.PictureData));
            vCard.AppendLine("UID:" + contact.Id.ToString("N"));
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

                if (line.Contains("CHARSET=UTF-8:"))
                {
                    line = linesUtf8[i];
                }

                var parts = line
                    .Replace("CHARSET=UTF-8:", string.Empty)
                    .Replace("CHARSET=ISO-8859-1:", string.Empty)
                    .Replace("\r", string.Empty)
                    .Split(';');

                switch (parts[0])
                {
                    case "TEL":
                        if (parts[1].StartsWith("TYPE=cell,home:", StringComparison.Ordinal))
                        {
                            contact.PersonalPhoneMobile = new PhoneNumber(parts[1].Substring(15));
                            break;
                        }

                        if (parts[1].StartsWith("TYPE=cell,work:", StringComparison.Ordinal))
                        {
                            contact.BusinessPhoneMobile = new PhoneNumber(parts[1].Substring(15));
                            break;
                        }

                        if (parts[1].StartsWith("TYPE=home:", StringComparison.Ordinal))
                        {
                            if (contact.PersonalAddressPrimary == null)
                            {
                                contact.PersonalAddressPrimary = new AddressDetail();
                            }

                            contact.PersonalAddressPrimary.Phone = new PhoneNumber(parts[1].Substring(10));
                            break;
                        }

                        if (parts[1].StartsWith("TYPE=work:", StringComparison.Ordinal))
                        {
                            if (contact.BusinessAddressPrimary == null)
                            {
                                contact.BusinessAddressPrimary = new AddressDetail();
                            }

                            contact.BusinessAddressPrimary.Phone = new PhoneNumber(parts[1].Substring(10));
                            break;
                        }

                        if (parts[1].StartsWith("TYPE=fax", StringComparison.Ordinal))
                        {
                            break;
                        }

                        Console.WriteLine("!! UNHANDLED TEL !! : " + line.Replace("\r", string.Empty));
                        break;

                    case "N":
                        contact.Name.LastName = parts[1];
                        contact.Name.FirstName = (parts.Length > 2) ? parts[2] : null;
                        contact.Name.MiddleName = (parts.Length > 3) ? parts[3] : null;
                        contact.Name.AcademicTitle = (parts.Length > 4) ? parts[4] : null;
                        break;

                    case "EMAIL":
                        if (parts[2] == "type=WORK")
                        {
                            contact.BusinessEmailPrimary = parts[3].StartsWith("type=pref:", StringComparison.Ordinal) ? parts[3].Substring(10) : parts[3];
                            break;
                        }

                        if (parts[2].StartsWith("type=WORK:", StringComparison.Ordinal))
                        {
                            contact.BusinessEmailPrimary = parts[2].Substring(10);
                            break;
                        }

                        if (parts[2] == "type=HOME")
                        {
                            contact.PersonalEmailPrimary = parts[3];
                            break;
                        }

                        if (parts[2].StartsWith("type=HOME:", StringComparison.Ordinal))
                        {
                            contact.PersonalEmailPrimary = parts[2].Substring(10);
                            break;
                        }

                        Console.WriteLine("!!Unhandled email address !!");
                        break;

                    case "URL":
                        if (parts[1].Contains("=home:"))
                        {
                            contact.PersonalHomepage = parts[1].Replace("TYPE=", string.Empty)
                                                               .Replace("home:", string.Empty);
                        }
                        else
                        {
                            contact.BusinessHomepage = parts[1].Replace("TYPE=", string.Empty)
                                                               .Replace("work:", string.Empty);
                        }

                        break;

                    case "ORG":
                        contact.BusinessCompanyName = parts[1];
                        break;

                    case "ADR":
                        if (line.EndsWith(";;;;;;\r", StringComparison.Ordinal))
                        {
                            break;
                        }

                        var address = new AddressDetail
                            {
                                CityName = (parts.Length < 6 || string.IsNullOrEmpty(parts[5])) ? null : parts[5],
                                CountryName = (parts.Length < 9 || string.IsNullOrEmpty(parts[8])) ? null : parts[8],
                                StateName = (parts.Length < 7 || string.IsNullOrEmpty(parts[6])) ? null : parts[6],
                                PostalCode = (parts.Length < 8 || string.IsNullOrEmpty(parts[7])) ? null : parts[7],
                                StreetName = (parts.Length < 5 || string.IsNullOrEmpty(parts[4])) ? null : parts[4],
                                StreetNumber = parts.Length < 5 ? 0 : SyncTools.ExtractStreetNumber(parts[4]),
                                StreetNumberExtension = parts.Length < 5 ? null : SyncTools.ExtractStreetNumberExtension(parts[4]),
                            };
                        if (parts[1] == "TYPE=work")
                        {
                            contact.BusinessAddressPrimary = address;
                        }
                        else
                        {
                            contact.PersonalAddressPrimary = address;
                        }

                        break;

                    case "TITLE":
                        contact.BusinessPosition = parts[1].Replace("CHARSET=ISO-8859-1:", string.Empty);
                        break;

                    case "PHOTO":
                        if (parts[1] == "ENCODING=b")
                        {
                            contact.PictureData =
                                Convert.FromBase64String(
                                    parts[2].Substring(parts[2].IndexOf(":", 0, StringComparison.Ordinal) + 1));
                        }
                        else
                        {
                            var url = parts[1].Replace("VALUE=URI:", string.Empty);
                            contact.PictureData = this.HttpRequester.GetContentBinary(url, url);
                        }

                        break;

                    case "PRODID:-//XING//www.xing.com//epublica//www.epublica.de//Version 1.3":
                    case "BEGIN:VCARD":
                    case "END:VCARD":
                    case "SORT-STRING":
                    case "CLASS:PRIVATE":
                    case "CLASS:PUBLIC":
                    case "FN":
                    case "":
                    case "CATEGORIES":
                    case "VERSION:2.1":
                    case "NOTE":
                        break;

                    default:
                        if (parts[0].StartsWith("BDAY:", StringComparison.Ordinal))
                        {
                            var dateString = parts[0].Substring(5);
                            if (dateString.IndexOf("-", 0, StringComparison.Ordinal) == -1)
                            {
                                dateString = dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" +
                                         dateString.Substring(6, 2);
                            }

                            contact.DateOfBirth = DateTime.Parse(dateString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal);
                            break;
                        }

                        if (parts[0].StartsWith("UID:", StringComparison.Ordinal))
                        {
                            if (contact.PersonalProfileIdentifiers == null)
                            {
                                contact.PersonalProfileIdentifiers = new ProfileIdentifiers();
                            }

                            switch (useIndetifierAs)
                            {
                                case ProfileIdentifierType.XingProfileId:
                                    uid = parts[0].Substring(13);
                                    contact.PersonalProfileIdentifiers.XingProfileId = parts[0].Substring(4);
                                    break;

                                case ProfileIdentifierType.FacebookProfileId:
                                    contact.PersonalProfileIdentifiers.FacebookProfileId = parts[0].Substring(4);
                                    break;

                                default:
                                    uid = parts[0].Substring(4);
                                    break;
                            }

                            break;
                        }

                        if (parts[0].StartsWith("SORT-STRING:", StringComparison.Ordinal))
                        {
                            break;
                        }

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
    }
}
