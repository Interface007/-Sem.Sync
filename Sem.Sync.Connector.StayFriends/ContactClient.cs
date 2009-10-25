// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.StayFriends
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using GenericHelpers;
    using GenericHelpers.Entities;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    /// <summary>
    /// Client implementation for reading information from www.StayFriends.de
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "StayFriends.de", MatchingIdentifier = ProfileIdentifierType.StayFriendsPersonId)]
    public class ContactClient : StdClient
    {
        #region string resources for processing wkw pages

        /// <summary>
        /// Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "action='/sfvc/login/do'";

        /// <summary>
        /// detection string to detect if we did fail to logon
        /// </summary>
        private const string HttpDetectionStringLogonFailed = "/app/user?op=lostpassword";

        /// <summary>
        /// Base address to communicate with StayFriends
        /// </summary>
        private const string HttpUrlBaseAddress = "http://www.StayFriends.de";

        /// <summary>
        /// relative url to log on
        /// </summary>
        private const string HttpUrlLogonRequest = "/sfvc/login/do";

        /// <summary>
        /// relative URL to query contact links to personal pages
        /// </summary>
        private const string HttpUrlListContent = "/j/ViewController?action=contactlist&paging={0}";

        /// <summary>
        /// data string to be posted to logon into StayFriends
        /// </summary>
        private const string HttpDataLogonRequest = "action=login&%3Bnotfirstdisplay=true&%3Baction=edit&%3Bform=HomepageLogin&%3Bformname=login&%3Bsection=main&login.func=&login.go=&login.visit=&login.schoolid=&login.schooltype=&login.gradyear=&login.classphoto=&login.uid={0}&login.password={1}";

        /// <summary>
        /// regular expression to extract the URLs for the personal pages
        /// </summary>
        private const string PatternGetDataUrls = "<a href=\"(.j.ViewController.action=myPage&persid=[0-9]*&visitreferer=18&crc=[0-9]*)\">(.*?)</a>";
        #endregion

        #region private fields

        /// <summary>
        /// http requester object that will read the data from xing
        /// </summary>
        private readonly HttpHelper httpRequester;

        #endregion

        /// <summary>
        /// cache hint string constant to specify a daily refresh for the cached files
        /// </summary>
        private const string CacheHintRefresh = "[REFRESH=DAILY]";

        /// <summary>
        /// cache hint string constant to specify that this item should not be cached at all
        /// </summary>
        private const string CacheHintNoCache = "[NOCACHE]";

        /// <summary>
        /// Extracts the data from the page
        /// </summary>
        private const string PersonDataExtractionPattern1 = "<span class=\"legend\">(?<key>[a-zA-Z ,]*):</span><br>((?<value>[^\\<]*?)<div class=\"spaceXS cl\">&nbsp;</div>)+";

        /// <summary>
        /// Extracts the data from a page (2nd version that may match such data)
        /// </summary>
        private const string PersonDataExtractionPattern2 = "<td class=\"legend\" width=\"[0-9]*%\">(?<key>[a-zA-Z ,]*):</td><td width=\"[0-9]*%\"></td><td class=\"copyLegend\" width=\"[0-9]*%\">(?<value>[a-zA-Z ,]*)</td>";

        /// <summary>
        /// Extracts the url of the picture from the page
        /// </summary>
        private const string PersonPictureUrlPattern = "img src=\"(?<image>http://images.stayfriends.de/img1/([0-9a-z]*).jpg)\"\\sid=\"[0-9]*\"";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
        {
            this.httpRequester = new HttpHelper(HttpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"),
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"),
                };
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "StayFriends";
            }
        }

        /// <summary>
        /// Reads all elements from StayFriends into a list of <see cref="StdContact"/>.
        /// </summary>
        /// <param name="clientFolderName">The parameter clientFolderName is ignored in this implementation.</param>
        /// <param name="result">The list of elements that should get the elements.</param>
        /// <returns>The list with the newly added elements</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var wkwContacts = this.GetUrlList();

            foreach (var item in wkwContacts)
            {
                var contact = this.DownloadContact(item.Key, item.Value);
                if (contact != null)
                {
                    result.Add(contact);
                }
            }

            return result;
        }

        /// <summary>
        /// Wrtire access is not implemented in this connector
        /// </summary>
        /// <param name="elements">The elements parameter is not used.</param>
        /// <param name="clientFolderName">The clientFolderPath is not used.</param>
        /// <param name="skipIfExisting">The skipIfExisting parameter is not used.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the URL encoding assuming "iso8859-1" and removes \t and \n from the text.
        /// </summary>
        /// <param name="toDecode"> The text data to decode. </param>
        /// <returns> The decoded text data. </returns>
        private static string DecodeResultString(string toDecode)
        {
            return HttpUtility.UrlDecode(toDecode, Encoding.GetEncoding("iso8859-1"))
                .Replace("\t", string.Empty)
                .Replace("\n", string.Empty)
                .Trim();
        }

        /// <summary>
        /// Ready a list of html locations of the person data - this will also establish the logon
        /// </summary>
        /// <returns>a list of urls for the data to be downloaded</returns>
        private List<KeyValuePair> GetUrlList()
        {
            var result = new List<KeyValuePair>();

            string contactListContent;
            var paging = 0;

            while (true)
            {
                while (true)
                {
                    // optimistically we try to read the content without explicit logon
                    // this will succeed if we have a valid cookie
                    contactListContent =
                        this.httpRequester.GetContent(
                            string.Format(CultureInfo.InvariantCulture, HttpUrlListContent, paging),
                            "UrlList" + CacheHintRefresh);

                    // if we don't find the logon form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLogonNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        QueryForLogOnCredentials("StayFriends benötigt die Log-In-Daten.");
                    }

                    // tell the user that we need to log on
                    LogProcessingEvent("StayFriends benötigt die Log-In-Daten.", this.LogOnUserId);

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLogonRequest, this.LogOnUserId, this.LogOnPassword);

                    // post to get the cookies
                    var logInResponse = this.httpRequester.GetContentPost(
                        HttpUrlLogonRequest, CacheHintNoCache, postData);

                    if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                    {
                        LogProcessingEvent("Log-In ist fehlgeschlagen.", this.LogOnUserId);
                        return result;
                    }

                    // we did succeed to log on - tell the user and try reading the data again.
                    LogProcessingEvent("Login erfolgreich", this.LogOnUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var matches = Regex.Matches(contactListContent, PatternGetDataUrls, RegexOptions.Singleline);

                LogProcessingEvent("füge Kontakte hinzu...", matches.Count, result.Count);

                // add the matches to the result
                foreach (Match match in matches)
                {
                    result.Add(
                        new KeyValuePair(match.Groups[1].ToString(), DecodeResultString(match.Groups[2].ToString())));
                }

                if (matches.Count < 20)
                {
                    break;
                }

                paging += 20;
            }

            return result;
        }

        /// <summary>
        /// downloads a contact data from wkw and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">url to the wkw page</param>
        /// <param name="name">name that will be used for the cache to store the data for later review</param>
        /// <returns>the downloaded contact as a StdContact</returns>
        private StdContact DownloadContact(string downloadUrl, string name)
        {
            var data = this.httpRequester.GetContent(downloadUrl, name + ".txt");
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var matches = new List<Match>();
            
            // we use regular expressions to extract the urls to the vCards
            var dataExtractor = new Regex(PersonDataExtractionPattern1, RegexOptions.Singleline);
            foreach (Match match in dataExtractor.Matches(data))
            {
                matches.Add(match);
            }

            dataExtractor = new Regex(PersonDataExtractionPattern2, RegexOptions.Singleline);
            foreach (Match match in dataExtractor.Matches(data))
            {
                matches.Add(match);
            }

            var contact = new StdContact
                {
                    Id = Guid.NewGuid(),
                    PersonalAddressPrimary = new AddressDetail(),
                    PersonalInstantMessengerAddresses = new InstantMessengerAddresses(),
                    Name = new PersonName(name),
                };

            foreach (var match in matches)
            {
                var key = DecodeResultString(match.Groups["key"].ToString());

                // the encoding needs to be set to get the correct special chars
                var value = match.Groups["value"];
                switch (key)
                {
                    case "":
                        break;

                    // The following information is provided by StayFriends, but not handled by sem.sync, yet.
                    // It may be handled with one of the next versions.
                    case "Hobbys":
                    case "Fax, privat":
                    case "Messenger, AIM":
                        break;

                    case "Familienstand":
                        switch (value.ToString())
                        {
                            case "ledig":
                                contact.RelationshipStatus = RelationshipStatus.Single;
                                break;

                            case "verheiratet":
                                contact.RelationshipStatus = RelationshipStatus.Married;
                                break;

                            case "zusammenlebend":
                                contact.RelationshipStatus = RelationshipStatus.Engaged;
                                break;

                            default:
                                Console.WriteLine(contact + " : " + key + " = " + value);
                                break;
                        } 
                        
                        break;

                    case "Skype":
                        contact.PersonalInstantMessengerAddresses.Skype = DecodeResultString(value.ToString());
                        break;
                    
                    case "Messenger, MSN":
                        contact.PersonalInstantMessengerAddresses.MsnMessenger = DecodeResultString(value.ToString());
                        break;
                    
                    case "Messenger, ICQ":
                        contact.PersonalInstantMessengerAddresses.ICQ = DecodeResultString(value.ToString());
                        break;
                    
                    case "Beruf":
                        contact.BusinessPosition = DecodeResultString(value.ToString());
                        break;

                    case "Telefon, mobil":
                        contact.PersonalPhoneMobile = new PhoneNumber(DecodeResultString(value.ToString()));
                        contact.PersonalPhoneMobile.CountryCode = CountryCode.Germany;
                        break;

                    case "Telefon, privat":
                        contact.PersonalAddressPrimary.Phone = new PhoneNumber(DecodeResultString(value.ToString()));
                        contact.PersonalAddressPrimary.Phone.CountryCode = CountryCode.Germany;
                        break;

                    case "Wohnort":

                        foreach (Capture capture in value.Captures)
                        {
                            var line = capture.ToString()
                                .Replace("\t", string.Empty)
                                .Replace("\n", string.Empty)
                                .Trim();

                            if (Regex.IsMatch(line, "^[0-9]+ "))
                            {
                                contact.PersonalAddressPrimary.PostalCode = DecodeResultString(line.Split(' ')[0]);
                                contact.PersonalAddressPrimary.CityName = DecodeResultString(line.Split(' ')[1]);
                                Console.WriteLine("PLZ Stadt : " + line);
                                continue;
                            }

                            if (Regex.IsMatch(line, "^[a-zA-Z -.ßäöüÄÖÜ]+[0-9]+"))
                            {
                                contact.PersonalAddressPrimary.StreetName = DecodeResultString(line);
                                Console.WriteLine("Straße : " + line);
                                continue;
                            }

                            if (Regex.IsMatch(line, "^[a-zA-Z -.ßäöüÄÖÜ]+"))
                            {
                                contact.PersonalAddressPrimary.CountryName += DecodeResultString(line);
                                Console.WriteLine("Land : " + line);
                                continue;
                            }

                            Console.WriteLine("??? : " + line);
                        }

                        break;

                    default:
                        Console.WriteLine("unknown attribute: " + key + " - " + value);
                        break;
                }
            }

            contact.PersonalProfileIdentifiers = new ProfileIdentifiers(ProfileIdentifierType.StayFriendsPersonId, Regex.Match(downloadUrl, "persid=([0-9]*)&").Groups[1].ToString());

            dataExtractor = new Regex(PersonPictureUrlPattern, RegexOptions.Singleline);
            var pictureUrlResult = dataExtractor.Matches(data);

            if (pictureUrlResult.Count == 0)
            {
                pictureUrlResult = Regex.Matches(data, "img src=\"(?<image>http://images.stayfriends.de/img1/([0-9a-z]*).jpg)\"");
            }

            if (pictureUrlResult.Count == 1)
            {
                var pictureName = pictureUrlResult[0].Groups["image"].ToString();
                contact.PictureName = pictureName.Substring(pictureName.LastIndexOf('/') + 1);
                contact.PictureData = this.httpRequester.GetContentBinary(pictureName, contact.PictureName);
            }

            if (pictureUrlResult.Count > 1)
            {
                var pictureName = pictureUrlResult[1].Groups["image"].ToString();
                contact.PictureName = pictureName.Substring(pictureName.LastIndexOf('/') + 1);
                contact.PictureData = this.httpRequester.GetContentBinary(pictureName, contact.PictureName);
            }

            LogProcessingEvent(contact, "downloaded");
            return contact;
        }
    }
}