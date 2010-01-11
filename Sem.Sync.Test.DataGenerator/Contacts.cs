// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contacts.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the Contacts type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.DataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    /// <summary>
    /// Defines contact test data
    /// </summary>
    [ClientStoragePathDescriptionAttribute(ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Test Data connector",
        CanWriteContacts = true, CanReadContacts = true)]
    public class Contacts : StdClient
    {
        /// <summary>
        /// random number generator for the OneOf function
        /// </summary>
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// memory storage for the matching
        /// </summary>
        private static List<StdElement> matchingTarget = new List<StdElement>();
        
        /// <summary>
        /// Gets a list of variable contacts. Some do have a constant name, some have a constant Id, some oave a constant DefaultProfileId
        /// </summary>
        /// <returns>a list of standard contacts with some variations </returns>
        public static List<StdContact> VariableContactList
        {
            get
            {
                return new List<StdContact>
                    {
                        new StdContact
                            {
                                Name = new PersonName("Susanne Mustermann"),
                                Id = new Guid("{F4A4A250-D5EA-4413-93DB-9E040510C766}"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 21\n56545 Irgendwo\nGermany",
                                    "Obere Galle 44a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich\nDeutschland"))
                            },
                        new StdContact
                            {
                                Name = new PersonName("Kati Katze"),
                                Id = new Guid("{7EAA8009-BBF6-4adf-8F6B-1275F2CA52AE}"),
                                PersonalProfileIdentifiers =
                                    new ProfileIdentifiers(ProfileIdentifierType.Default, Guid.NewGuid().ToString()),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 22\n56545 Irgendwo\nGermany",
                                    "Untere Galle 34a\n78631 SomeWhere",
                                    "Meine Strasse unterm Deich\nDeutschland"))
                            },
                        new StdContact
                            {
                                Name = new PersonName("Maria Klaus Hartwig"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "An der B46 78s\n85365 Ober Unter Schleißheim\nGermany",
                                    "Obere Untertasse 44a\n89273 Krotzen",
                                    "Deine Strasse Überm Deich\nDeutschland"))
                            },
                        new StdContact
                            {
                                Name = new PersonName("Meier, Arnold"),
                                Id = new Guid("{3611AB68-CE1C-4a8e-919E-59E07ABC7CD7}"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 21\n56545 Irgendwo\nGermany",
                                    "Obere Galle 44a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich\nDeutschland")),
                                AdditionalTextData = "ich bin auch noch da",
                                Categories = new List<string> { "gut", "böse", "no category" }
                            },
                        new StdContact
                            {
                                Name = new PersonName("Meier, Marcel"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 21\n56545 Irgendwo\nGermany",
                                    "Obere Galle 44a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich\nDeutschland")),
                                AdditionalTextData = "ich bin auch noch da",
                                Categories = new List<string> { "gut", "böse", "no category" }
                            },
                        new StdContact
                            {
                                Name = new PersonName("Meier, Maria"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 21\n56545 Irgendwo\nGermany",
                                    "Obere Galle 44a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich\nDeutschland")),
                                AdditionalTextData = "ich bin auch noch da",
                                Categories = new List<string> { "gut", "böse", "no category" }
                            },
                        new StdContact
                            {
                                Name = new PersonName("Meier, Andrea"),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Hirtenweg 21\n56545 Irgendwo\nGermany",
                                    "Obere Galle 44a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich\nDeutschland")),
                                AdditionalTextData = "ich bin auch noch da",
                                Categories = new List<string> { "gut", "böse", "no category" }
                            },
                        new StdContact
                            {
                                Name = new PersonName(OneOf("Lässig, Harry", "Patricia Müller", "Frahm, Manuela")),
                                PersonalAddressPrimary =
                                    new AddressDetail(
                                    OneOf(
                                    "Babylon Weg 67\n57845 Irgendwo\nDeutschland",
                                    "Obere Galle 24a\n78631 SomeWhere",
                                    "Meine Strasse Überm Deich 21857\nDeutschland")),
                                AdditionalTextData = "ich bin auch noch da",
                                Categories = new List<string> { "gut", "böse", "no category" }
                            }
                    };
            }
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Test data store";
            }
        }

        /// <summary>
        /// Adds a contact to a StringBuilder that does not contain a picture.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        public static void AddContactWithoutPicture(StringBuilder content)
        {
            content.AppendLine(@"  <StdContact Id=""9c8a9b29-2fda-44f3-8324-62b983468a7e"">");
            content.AppendLine(@"    <PersonGender>Female</PersonGender>");
            content.AppendLine(@"    <DateOfBirth>1971-01-18T00:00:00</DateOfBirth>");
            content.AppendLine(@"    <Name>");
            content.AppendLine(@"      <AcademicTitle>Dr.</AcademicTitle>");
            content.AppendLine(@"      <FirstName>Andrea</FirstName>");
            content.AppendLine(@"      <MiddleName>Alexandra</MiddleName>");
            content.AppendLine(@"      <LastName>Bauer</LastName>");
            content.AppendLine(@"      <LastNamePrefix>von</LastNamePrefix>");
            content.AppendLine(@"      <Suffix>sen.</Suffix>");
            content.AppendLine(@"      <FormerName>Kalback</FormerName>");
            content.AppendLine(@"    </Name>");
            content.AppendLine(@"    <PersonalAddressPrimary>");
            content.AppendLine(@"      <CountryName>Germany</CountryName>");
            content.AppendLine(@"      <StateName>Hessen</StateName>");
            content.AppendLine(@"      <PostalCode>78631</PostalCode>");
            content.AppendLine(@"      <CityName>SomeWhere</CityName>");
            content.AppendLine(@"      <StreetName>Obere Galle</StreetName>");
            content.AppendLine(@"      <StreetNumber>44</StreetNumber>");
            content.AppendLine(@"      <Room>R 5.6.7.8</Room>");
            content.AppendLine(@"      <StreetNumberExtension>a</StreetNumberExtension>");
            content.AppendLine(@"      <Phone>");
            content.AppendLine(@"        <CountryCode>Germany</CountryCode>");
            content.AppendLine(@"        <AreaCode>6558</AreaCode>");
            content.AppendLine(@"        <Number>987654</Number>");
            content.AppendLine(@"      </Phone>");
            content.AppendLine(@"    </PersonalAddressPrimary>");
            content.AppendLine(@"    <PersonalPhoneMobile>");
            content.AppendLine(@"      <CountryCode>Germany</CountryCode>");
            content.AppendLine(@"      <AreaCode>234</AreaCode>");
            content.AppendLine(@"      <Number>567890</Number>");
            content.AppendLine(@"    </PersonalPhoneMobile>");
            content.AppendLine(@"    <PersonalEmailPrimary>recycler@svenerikmatzen.info</PersonalEmailPrimary>");
            content.AppendLine(@"    <PersonalEmailSecondary>recycler2@svenerikmatzen.info</PersonalEmailSecondary>");
            content.AppendLine(@"    <PersonalHomepage>http://www.svenerikmatzen.info</PersonalHomepage>");
            content.AppendLine(@"    <PersonalInstantMessengerAddresses>");
            content.AppendLine(@"      <MsnMessenger>http://www.svenerikmatzen.com</MsnMessenger>");
            content.AppendLine(@"      <GoogleTalk>google@svenerikmatzen.info</GoogleTalk>");
            content.AppendLine(@"      <YahooMessenger>yahoo@svenerikmatzen.info</YahooMessenger>");
            content.AppendLine(@"      <Skype>skype@svenerikmatzen.info</Skype>");
            content.AppendLine(@"      <Icq>icq@svenerikmatzen.info</Icq>");
            content.AppendLine(@"    </PersonalInstantMessengerAddresses>");
            content.AppendLine(@"    <PersonalProfileIdentifiers>");
            content.AppendLine(@"      <DefaultProfileId>{67a5163c-de74-447f-83ca-c64b8ff372f2}</DefaultProfileId>");
            content.AppendLine(@"      <XingNameProfileId>xingprofid</XingNameProfileId>");
            content.AppendLine(@"      <FacebookProfileId>faceprofid</FacebookProfileId>");
            content.AppendLine(@"      <ActiveDirectoryId>adprofid</ActiveDirectoryId>");
            content.AppendLine(@"      <MicrosoftAccessId>msaccprofid</MicrosoftAccessId>");
            content.AppendLine(@"      <WerKenntWenUrl>wkwprofid</WerKenntWenUrl>");
            content.AppendLine(@"      <StayFriendsPersonId>stayprofid</StayFriendsPersonId>");
            content.AppendLine(@"      <MeinVZPersonId>meinprofid</MeinVZPersonId>");
            content.AppendLine(@"      <GoogleId>googleprofid</GoogleId>");
            content.AppendLine(@"    </PersonalProfileIdentifiers>");
            content.AppendLine(@"    <BusinessCompanyName>SDX AG</BusinessCompanyName>");
            content.AppendLine(@"    <BusinessDepartment>Development</BusinessDepartment>");
            content.AppendLine(@"    <BusinessAddressPrimary>");
            content.AppendLine(@"      <CountryName>Germany</CountryName>");
            content.AppendLine(@"      <StateName>Hessen</StateName>");
            content.AppendLine(@"      <PostalCode>60388</PostalCode>");
            content.AppendLine(@"      <CityName>Frankfurt am Main</CityName>");
            content.AppendLine(@"      <StreetName>Borsigallee 19</StreetName>");
            content.AppendLine(@"      <StreetNumber>19</StreetNumber>");
            content.AppendLine(@"      <Room>Room 2.4.5</Room>");
            content.AppendLine(@"      <StreetNumberExtension>z</StreetNumberExtension>");
            content.AppendLine(@"      <Phone>");
            content.AppendLine(@"        <CountryCode>Germany</CountryCode>");
            content.AppendLine(@"        <AreaCode>123</AreaCode>");
            content.AppendLine(@"        <Number>7654321</Number>");
            content.AppendLine(@"      </Phone>");
            content.AppendLine(@"    </BusinessAddressPrimary>");
            content.AppendLine(@"    <BusinessHomepage>http://www.sdx-ag.de</BusinessHomepage>");
            content.AppendLine(@"    <BusinessInstantMessengerAddresses>");
            content.AppendLine(@"      <MsnMessenger>busimmsn</MsnMessenger>");
            content.AppendLine(@"      <GoogleTalk>busimgoogle</GoogleTalk>");
            content.AppendLine(@"      <YahooMessenger>busimyahoo</YahooMessenger>");
            content.AppendLine(@"      <Skype>busimskype</Skype>");
            content.AppendLine(@"      <Icq>busimicq</Icq>");
            content.AppendLine(@"    </BusinessInstantMessengerAddresses>");
            content.AppendLine(@"    <AdditionalTextData>additional text</AdditionalTextData>");
            content.AppendLine(@"    <PictureData />");
            content.AppendLine(@"    <Categories>");
            content.AppendLine(@"      <string>family</string>");
            content.AppendLine(@"      <string>business</string>");
            content.AppendLine(@"      <string>enemy</string>");
            content.AppendLine(@"    </Categories>");
            content.AppendLine(@"  </StdContact>");
        }

        /// <summary>
        /// Adds a contact to a StringBuilder that does not contain a picture.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        public static void AddContactWithNulls(StringBuilder content)
        {
            content.AppendLine("  <StdContact Id=\"21C3586A-BB96-4a3a-9B05-D40F1125BFB9\">");
            content.AppendLine("    <InternalSyncData>");
            content.AppendLine("      <DateOfLastChange>2009-05-18T18:03:29.162</DateOfLastChange>");
            content.AppendLine("      <DateOfCreation>2009-04-23T21:37:44.281</DateOfCreation>");
            content.AppendLine("    </InternalSyncData>");
            content.AppendLine("  </StdContact>");
        }

        /// <summary>
        /// Adds a contact with a picture to the string builder.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        public static void AddContactWithPicture(StringBuilder content)
        {
            content.AppendLine(" <StdContact Id=\"929e2981-ee94-4e1f-adb0-240cb8a9afd6\">");
            content.AppendLine("   <InternalSyncData>");
            content.AppendLine("     <DateOfLastChange>2009-05-18T18:57:41.865</DateOfLastChange>");
            content.AppendLine("     <DateOfCreation>2009-04-23T21:51:09.146</DateOfCreation>");
            content.AppendLine("   </InternalSyncData>");
            content.AppendLine("   <PersonGender>Male</PersonGender>");
            content.AppendLine("   <DateOfBirth>1971-01-18T00:00:00</DateOfBirth>");
            content.AppendLine("   <Name>");
            content.AppendLine("     <AcademicTitle>Dipl. Biol.</AcademicTitle>");
            content.AppendLine("     <FirstName>Sven</FirstName>");
            content.AppendLine("     <MiddleName>Erik</MiddleName>");
            content.AppendLine("     <LastName>Matzen</LastName>");
            content.AppendLine("   </Name>");
            content.AppendLine("   <PersonalAddressPrimary>");
            content.AppendLine("     <CountryName>Germany</CountryName>");
            content.AppendLine("     <PostalCode>12345</PostalCode>");
            content.AppendLine("     <CityName>Wetzlar, Hessen</CityName>");
            content.AppendLine("     <StreetName>My Street</StreetName>");
            content.AppendLine("     <StreetNumber>12</StreetNumber>");
            content.AppendLine("     <StreetNumberExtension>a</StreetNumberExtension>");
            content.AppendLine("     <Phone>");
            content.AppendLine("       <CountryCode>Germany</CountryCode>");
            content.AppendLine("       <AreaCode>6441</AreaCode>");
            content.AppendLine("       <Number>123456</Number>");
            content.AppendLine("     </Phone>");
            content.AppendLine("   </PersonalAddressPrimary>");
            content.AppendLine("   <PersonalPhoneMobile>");
            content.AppendLine("     <CountryCode>Germany</CountryCode>");
            content.AppendLine("     <AreaCode>234</AreaCode>");
            content.AppendLine("     <Number>567890</Number>");
            content.AppendLine("   </PersonalPhoneMobile>");
            content.AppendLine("   <PersonalEmailPrimary>sven.erik.matzen@web.de</PersonalEmailPrimary>");
            content.AppendLine("   <PersonalHomepage>http://www.svenerikmatzen.info</PersonalHomepage>");
            content.AppendLine("   <PersonalProfileIdentifiers>");
            content.AppendLine("   <DefaultProfileId>{929e2981-ee94-4e1f-adb0-240cb8a9afd6}</DefaultProfileId>");
            content.AppendLine("   </PersonalProfileIdentifiers>");
            content.AppendLine("   <BusinessCompanyName>SDX AG</BusinessCompanyName>");
            content.AppendLine("   <BusinessAddressPrimary>");
            content.AppendLine("     <CountryName>Deutschland</CountryName>");
            content.AppendLine("     <PostalCode>60388</PostalCode>");
            content.AppendLine("     <CityName>Frankfurt am Main</CityName>");
            content.AppendLine("     <StreetName>Borsigallee</StreetName>");
            content.AppendLine("     <StreetNumber>19</StreetNumber>");
            content.AppendLine("     <Phone>");
            content.AppendLine("       <CountryCode>Germany</CountryCode>");
            content.AppendLine("       <AreaCode>123</AreaCode>");
            content.AppendLine("       <Number>7654321</Number>");
            content.AppendLine("     </Phone>");
            content.AppendLine("   </BusinessAddressPrimary>");
            content.AppendLine("   <BusinessHomepage>http://www.sdx-ag.de</BusinessHomepage>");
            content.AppendLine("   <AdditionalTextData>LinkedIn Profile: http://www.linkedin.com/profile?viewProfile=&amp;key=someKey</AdditionalTextData>");
            content.AppendLine("   <PictureName>ContactPicture.jpg</PictureName>");
            content.AppendLine("   <PictureData>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</PictureData>");
            content.AppendLine(" </StdContact>");
        }

        /// <summary>
        /// Generates a standard contact list containing multiple contacts with different aspects for testing.
        /// if <paramref name="includeNulls"/> is true, this includes a contact with mainly null values.
        /// </summary>
        /// <param name="includeNulls"> A value indicating if a contact with only null values is wanted. </param>
        /// <returns> a list of contacts </returns>
        public static List<StdContact> GetStandardContactList(bool includeNulls)
        {
            var container = new StringBuilder();

            container.AppendLine("<?xml version=\"1.0\"?>");
            container.AppendLine("<ArrayOfStdContact xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            if (includeNulls)
            {
                AddContactWithNulls(container);
            }

            AddContactWithoutPicture(container);
            AddContactWithPicture(container);

            container.Append("</ArrayOfStdContact>");

            var contactListFormatter = new XmlSerializer(typeof(List<StdContact>));
            var textStream = new MemoryStream(Encoding.UTF8.GetBytes(container.ToString()));
            return (List<StdContact>)contactListFormatter.Deserialize(textStream);
        }

        /// <summary>
        /// Serializes a list of contacts into a string
        /// </summary>
        /// <param name="list"> The list to be serialized. </param>
        /// <returns> the serialized object as an xml string </returns>
        public static string SerializeList(List<StdContact> list)
        {
            var contactListFormatter = new XmlSerializer(typeof(List<StdContact>));

            var textStream = new MemoryStream();
            contactListFormatter.Serialize(textStream, list);

            TextReader reader = new StreamReader(textStream);
            textStream.Seek(0, 0);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Creates a list of contacts with the artificial Xing-IDs "Unmatched", "Matched" and "New". 
        /// The entities can be used together with the lists returned by <see cref="GetMatchingTarget"/> 
        /// and <see cref="GetMatchingBaseline"/> to simulate matching actions.
        /// </summary>
        /// <returns> The created list </returns>
        public static List<StdContact> GetMatchingSource()
        {
            return new List<StdContact>
                 {
                     CreateIdOnlyContact("Unmatched"),
                     CreateIdOnlyContact("Matched"),
                     CreateIdOnlyContact("New"),
                 };
        }

        /// <summary>
        /// Creates a list of contacts with the artificial Xing-IDs "Unmatched", "Matched" and "TargetOrphan". 
        /// The entities can be used together with the lists returned by <see cref="GetMatchingSource"/> 
        /// and <see cref="GetMatchingBaseline"/> to simulate matching actions.
        /// </summary>
        /// <returns> The created list </returns>
        public static List<StdContact> GetMatchingTarget()
        {
            return new List<StdContact>
                       {
                            CreateIdOnlyContact("Unmatched", "{1048893D-6550-494f-A696-41849103174B}"),
                            CreateIdOnlyContact("Matched", "{03652E94-05F4-4410-95C6-BAF38925A368}"),
                            CreateIdOnlyContact("TargetOrphan", "{CA3585EC-A110-4eaf-932B-BB155B2430D1}"),
                       };
        }

        /// <summary>
        /// Creates a list of contacts with one single artificial Xing-ID "Matched". 
        /// The entity can be used together with the lists returned by <see cref="GetMatchingTarget"/> 
        /// and <see cref="GetMatchingSource"/> to simulate matching actions.
        /// </summary>
        /// <returns> The created list </returns>
        public static List<MatchingEntry> GetMatchingBaseline()
        {
            return new List<MatchingEntry>
                       {
                            new MatchingEntry
                                {
                                    Id = new Guid("{03652E94-05F4-4410-95C6-BAF38925A368}"),
                                    ProfileId = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "Matched")
                                }  
                       };
        }

        /// <summary>
        /// Abstract write method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="elements">the list of elements that should be written to the target system.</param>
        /// <param name="clientFolderName">the information to where inside the source the elements should be written - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="skipIfExisting">specifies whether existing elements should be updated or simply left as they are</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            switch (clientFolderName)
            {
                case "exception":
                    throw new NotSupportedException("Test-Exception");

                case "matchingtesttarget":
                    matchingTarget = elements;
                    return;
            }
        }

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">the information from where inside the source the elements should be read - 
        /// This does not need to be a real "path", but need to be something that can be expressed as a string</param>
        /// <param name="result">The list of elements that should get the elements. The elements should be added to
        /// the list instead of replacing it.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            switch (clientFolderName)
            {
                case "matchingtestsource":
                    result.AddRange(
                        new List<StdElement>
                            {
                                new StdContact
                                    {
                                        Name = "matchable1",
                                        PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "matchable1")
                                    },
                                new StdContact
                                    {
                                        Name = "matchable2",
                                        PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "matchable2")
                                    },
                                new StdContact
                                    {
                                        Name = "unmatchable",
                                        PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "orphan1")
                                    },
                            });
                    break;

                case "matchingtesttarget":
                    return matchingTarget;

                case "exception":
                    throw new NotSupportedException("Test-Exception");

                case "matchingtestbaseline":
                    result.AddRange(
                        new List<StdElement>
                            {
                                new MatchingEntry
                                    {
                                        Id = new Guid("{A1445F74-6C24-47a3-97E9-9A3E2FA35B17}"),
                                        ProfileId = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "orphan1bl")
                                    },
                                new MatchingEntry
                                    {
                                        Id = new Guid("{2191B8BB-40AE-4052-B8AC-89776BB47865}"),
                                        ProfileId = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "matchable1")
                                    },
                                new MatchingEntry
                                    {
                                        Id = new Guid("{B79B71B6-2FE5-492b-B5B1-8C373D6F4D64}"),
                                        ProfileId = new ProfileIdentifiers(ProfileIdentifierType.XingNameProfileId, "matchable2")
                                    }
                            });
                    break;

                default:
                    if (VariableContactList != null)
                    {
                        result = VariableContactList.ToStdElement();
                        result.AddRange(GetStandardContactList(false).ToStdElement());
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Selects a random string from the params array
        /// </summary>
        /// <param name="candidates"> The string candidates. </param>
        /// <returns> the random string </returns>
        private static string OneOf(params string[] candidates)
        {
            return candidates[Rnd.Next(candidates.Length)];
        }

        /// <summary>
        /// Creates a <see cref="StdContact"/> entity with only the Xing profile ID specified for matching simulation.
        /// The <see cref="StdContact.Id"/> will be a newly generated Guid.
        /// </summary>
        /// <param name="profileId"> The profile id. </param>
        /// <returns> The entity created with a Xing profile id </returns>
        private static StdContact CreateIdOnlyContact(string profileId)
        {
            return CreateIdOnlyContact(profileId, Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Creates a <see cref="StdContact"/> entity with only the Xing profile ID specified for matching simulation.
        /// </summary>
        /// <param name="profileId"> The profile id.  </param>
        /// <param name="id"> The <see cref="StdContact.Id"/> . </param>
        /// <returns> The entity created with a Xing profile id  </returns>
        private static StdContact CreateIdOnlyContact(string profileId, string id)
        {
            var result = new StdContact
                             {
                                 Name = new PersonName(profileId),
                                 Id = new Guid(id),
                                 PersonalProfileIdentifiers = { XingNameProfileId = profileId },
                             };
            return result;
        }
    }
}
