//-----------------------------------------------------------------------
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
        public HttpHelper HttpRequester { get; set; }

        public StdContact VCardToStdContact(byte[] vCard, ProfileIdentifierType useIndetifierAs)
        {
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
                    .Replace("CHARSET=UTF-8:", "")
                    .Replace("CHARSET=ISO-8859-1:", "")
                    .Replace("\r", "")
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
                        Console.WriteLine("!! UNHANDLED TEL !! : " + line.Replace("\r", ""));
                        break;

                    case "N":
                        contact.Name.LastName = parts[1];
                        contact.Name.FirstName = parts[2];
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
                        contact.BusinessHomepage = parts[1].Replace("TYPE=", "").Replace("work:", "").Replace("home:", "");
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
                            CityName = string.IsNullOrEmpty(parts[5]) ? null : parts[5],
                            CountryName = string.IsNullOrEmpty(parts[8]) ? null : parts[8],
                            StateName = string.IsNullOrEmpty(parts[6]) ? null : parts[6],
                            PostalCode = string.IsNullOrEmpty(parts[7]) ? null : parts[7],
                            StreetName = string.IsNullOrEmpty(parts[4]) ? null : parts[4],
                            StreetNumber = SyncTools.ExtractStreetNumber(parts[4]),
                            StreetNumberExtension = SyncTools.ExtractStreetNumberExtension(parts[4]),
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
                        contact.BusinessPosition = parts[1].Replace("CHARSET=ISO-8859-1:", "");
                        break;

                    case "PHOTO":
                        var url = parts[1].Replace("VALUE=URI:", "");
                        contact.PictureData = this.HttpRequester.GetContentBinary(url, url);
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
                            contact.DateOfBirth = DateTime.Parse(parts[0].Substring(5), CultureInfo.CurrentCulture);
                            break;
                        }
                        if (parts[0].StartsWith("UID:", StringComparison.Ordinal))
                        {
                            switch (useIndetifierAs)
                            {
                                case ProfileIdentifierType.XingProfileId:
                                    contact.PersonalProfileIdentifiers.XingProfileId = parts[0].Substring(4);
                                    break;

                                case ProfileIdentifierType.FacebookProfileId:
                                    contact.PersonalProfileIdentifiers.FacebookProfileId = parts[0].Substring(4);
                                    break;

                                default:
                                    break;
                            }

                            uid = parts[0].Substring(13);
                            break;
                        }
                        if (parts[0].StartsWith("SORT-STRING:", StringComparison.Ordinal))
                        {
                            break;
                        }
                        Console.WriteLine("unhandled: " + line.Replace("\r", ""));
                        break;
                }
            }

            contact.Id = uid.Length > 0
                ? new Guid(int.Parse(uid, CultureInfo.InvariantCulture), 1, 2, 3, 4, 5, 6, 7, 8, 9, 0)
                : Guid.NewGuid();
            return contact;
        }

        public static byte[] StdContactToVCard(StdContact contact)
        {
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
            if (contact.BusinessPhoneMobile != null && !string.IsNullOrEmpty(contact.BusinessPhoneMobile.ToString())) vCard.AppendLine("TEL;TYPE=cell,work:" + contact.BusinessPhoneMobile);
            if (!string.IsNullOrEmpty(contact.BusinessCompanyName)) vCard.AppendLine("ORG;CHARSET=ISO-8859-1:" + contact.BusinessCompanyName);
            if (!string.IsNullOrEmpty(contact.BusinessHomepage)) vCard.AppendLine("URL;TYPE=work:" + contact.BusinessHomepage);
            if (!string.IsNullOrEmpty(contact.BusinessPosition)) vCard.AppendLine("TITLE;CHARSET=UTF-8:" + contact.BusinessPosition);
            vCard.AppendLine("NOTE;CHARSET=UTF-8:generated by Sem.Sync - www.svenerikmatzen.info");
            vCard.AppendLine("PRODID:-//MATZEN//www.svenerikmatzen.info//Sem.Sync//Version 1.0");
            if (!string.IsNullOrEmpty(contact.PictureName)) vCard.AppendLine("PHOTO;ENCODING=b;TYPE=JPEG:" + Convert.ToBase64String(contact.PictureData));
            vCard.AppendLine("UID:" + contact.Id.ToString("N"));
            vCard.AppendLine("END:VCARD");

            return Encoding.UTF8.GetBytes(vCard.ToString());
        }
    }
}
