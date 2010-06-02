// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   WebScaping implementation of a FaceBook StdClient
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// WebScaping implementation of a FaceBook StdClient
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true, ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "Facebook (WS)", Internal = false, CanReadContacts = true,
        CanWriteContacts = false, NeedsCredentials = true, NeedsCredentialsDomain = false,
        MatchingIdentifier = ProfileIdentifierType.FacebookProfileId)]
    public class WebScrapingClient : WebScrapingBaseClient, IExtendedReader
    {
        #region Constants and Fields

        /// <summary>
        ///   magic secret for facebook
        /// </summary>
        private readonly string facebookLsd;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "WebScrapingClient" /> class.
        /// </summary>
        public WebScrapingClient()
            : base("http://www.facebook.com/")
        {
            var content = this.HttpRequester.GetContent(string.Empty);
            this.facebookLsd =
                Regex.Match(content, @"name=""lsd"" value=""(?<lsd>.*?)"" autocomplete=""off""").Groups["lsd"].ToString(
                    );
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the WebSideParameters which defines how to deal with the site.
        /// </summary>
        protected override WebSideParameters WebSideParameters
        {
            get
            {
                return new WebSideParameters
                    {
                        ImagePlaceholderUrl = "silhouette.gif",
                        HttpDetectionStringLogOnNeeded = @"""errorSummary"":""Not Logged In""",
                        HttpUrlContactDownload = "/profile.php?id={0}",
                        ProfileIdentifierType = ProfileIdentifierType.FacebookProfileId,
                        HttpUrlLogOnRequest = "https://login.facebook.com/login.php?login_attempt=1",
                        HttpUrlFriendList = "/friends/ajax/superfriends.php?filter=afp&ref=tn&offset={0}&__a=1",
                        HttpDetectionStringLogOnFailed = "no such string available",
                        ExtractorProfilePictureUrl =
                            @"<img src=""(?<pic>[a-z0-9._/:]*)"" alt=""[a-zA-Z0-9._/: ]*"" id=""profile_pic""",
                        ExtractorFriendUrls = @"""members"":\[(""(?<id>\d*)""[,\]])*",
                        HttpDataLogOnRequest =
                            "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&locale=de_DE&non_com_login=&email={0}&pass={1}&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" +
                            this.facebookLsd,
                    };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl">
        /// The contact url. 
        /// </param>
        /// <param name="content">
        /// The content. 
        /// </param>
        /// <returns>
        /// a new StdClient created from the data provided 
        /// </returns>
        protected override StdContact ConvertToStdContact(string contactUrl, string content)
        {
            var result = new StdContact();
            var values = Regex.Matches(
                content, @"\<h1 id=\\""profile_name\\"">(?<value>.*?)<\\/h1>", RegexOptions.Singleline);

            if (values.Count == 0 || values[0].Groups.Count <= 1)
            {
                values = Regex.Matches(content, @"\<h1 id=""profile_name"">(?<value>.*?)</h1>", RegexOptions.Singleline);
            }

            if (values.Count > 0 && values[0].Groups.Count > 1)
            {
                result.Name = new PersonName(GetValue(values[0]));
                this.LogProcessingEvent("processing " + result.Name);
            }
            else
            {
                this.LogProcessingEvent("no name found for this contact");
            }

            values = Regex.Matches(
                content,
                @"<div class=""(?<key>[^""]*)"" style=""[^""]*""><dt>.*?</dt><dd>(?<value>.*?)</dd></div>",
                RegexOptions.Singleline);

            if (values.Count == 0)
            {
                values = Regex.Matches(
                    content,
                    @"<div class=\\""(?<key>[^""]*)\\""( style=\\""[^""]*\\"")?><dt>.*?<\\/dt><dd>(?<value>.*?)<\\/dd><\\/div>",
                    RegexOptions.Singleline);
            }

            foreach (Match match in values)
            {
                var key = match.Groups["key"].ToString();
                var value = GetValue(match);

                result.InternalSyncData = new SyncData();

                switch (key)
                {
                    case "birthday":
                        DateTime dtvalue;
                        if (DateTime.TryParse(
                            value,
                            CultureInfo.CurrentCulture,
                            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                            out dtvalue))
                        {
                            result.DateOfBirth = dtvalue;
                        }

                        break;

                    case "hometown":
                        result.PersonalAddressPrimary = result.PersonalAddressPrimary ?? new AddressDetail();
                        while (value.Contains("  "))
                        {
                            value = value.Replace("  ", " ");
                        }

                        if (Regex.IsMatch(value, "^[0-9]+ "))
                        {
                            result.PersonalAddressPrimary.PostalCode = value.Split(' ')[0];
                            result.PersonalAddressPrimary.CityName = value.Split(' ')[1];
                        }
                        else
                        {
                            result.PersonalAddressPrimary.CityName = value;
                        }

                        break;

                    case "website":
                        result.PersonalHomepage = value;
                        break;

                    case "relationship_status":
                        result.RelationshipStatus = GetRelationshipStatus(key, value);
                        break;

                    case "networks":

                        // TODO: add this to the StdContact object model - contains a separated list of freetext
                        break;

                    case "political_views":

                        // TODO: add this to the StdContact object model
                        break;

                    case "religious_views":

                        // TODO: add this to the StdContact object model
                        break;

                    case "siblings":

                        // TODO: add this to the StdContact object model - contains a name
                        break;

                    case "current_city":

                        // this describes the city the person is currently located (different to the home town) - 
                        // TODO: we might find a usefull way to deal with it.
                        break;

                    default:
                        Tools.DebugWriteLine("new content: {0}  => {1}", key, value);
                        break;
                }
            }

            // TODO: find a better way to extract the identifier from the content, so that we don't need to provide te url inside the method signature
            result.ExternalIdentifier.SetProfileId(
                this.WebSideParameters.ProfileIdentifierType,
                contactUrl.Substring(contactUrl.LastIndexOf("/", StringComparison.Ordinal) + 1));

            var imageurl = Regex.Match(content, @"<img src=\\""(?<url>.*?)\\"" alt=\\""[^""]*"" id=\\""profile_pic\\""");
            if (imageurl.Groups["url"] != null)
            {
                var imageUrl = imageurl.Groups["url"].ToString().Replace(@"\/", @"/");
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    result.PictureData = this.HttpRequester.GetContentBinary(imageUrl);
                }
            }

            return result;
        }

        /// <summary>
        /// Determines the ralationship status by interpreting the value from facebook
        /// </summary>
        /// <param name="key">
        /// The key of the status. 
        /// </param>
        /// <param name="value">
        /// The value of the status. 
        /// </param>
        /// <returns>
        /// the relationship status 
        /// </returns>
        private static RelationshipStatus GetRelationshipStatus(string key, string value)
        {
            var status = RelationshipStatus.Undefined;
            switch (value)
            {
                case "Single":
                    status = RelationshipStatus.Single;
                    break;

                case "In einer Beziehung mit ":
                case "In einer Beziehung":
                    status = RelationshipStatus.InARelationship;
                    break;

                case "Verheiratet mit ":
                case "Verheiratet":
                    status = RelationshipStatus.Married;
                    break;

                default:
                    Tools.DebugWriteLine(key + " = " + value);
                    break;
            }

            return status;
        }

        /// <summary>
        /// Extracts the value from a match.
        /// </summary>
        /// <param name="match">
        /// The match containing the value. 
        /// </param>
        /// <returns>
        /// the extracted value 
        /// </returns>
        private static string GetValue(Match match)
        {
            var value = match.Groups["value"].ToString();
            if (value.Contains(">"))
            {
                value = value.Substring(value.IndexOf('>') + 1);
            }

            if (value.Contains("<"))
            {
                value = value.Substring(0, value.IndexOf('<'));
            }

            value = value.Replace("\\/", "/");

            if (value.Contains("\\u"))
            {
                var code = value.Substring(value.IndexOf("\\u", StringComparison.OrdinalIgnoreCase) + 2, 4);
                var charValue = int.Parse(code, NumberStyles.HexNumber, CultureInfo.CurrentCulture);
                var character = char.ConvertFromUtf32(charValue);

                value = value.Replace("\\u" + code, character);
            }

            return value;
        }

        #endregion

        public StdElement FillContacts(StdElement contactToFill, ICollection<MatchingEntry> baseline)
        {
            var contact = contactToFill as StdContact;
            var webSideParameters = this.WebSideParameters;
            if (contact == null || !contact.ExternalIdentifier.ContainsKey(webSideParameters.ProfileIdentifierType))
            {
                return contactToFill;
            }

            var profileIdInformation = contact.ExternalIdentifier[webSideParameters.ProfileIdentifierType];
            if (profileIdInformation == null || string.IsNullOrWhiteSpace(profileIdInformation.Id))
            {
                return contactToFill;
            }

            var offset = 0;
            var added = 0;
            while (true)
            {
                this.LogProcessingEvent("reading contacts ({0})", offset);
                this.HttpRequester.ContentCredentials = this;

                // get the contact list
                var url = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://www.facebook.com/friends/?id={0}&quickling[version]=252257%3B0&ajaxpipe=1&__a=5",
                    profileIdInformation);

                var profileContent = this.HttpRequester.GetContent(url, string.Format(CultureInfo.InvariantCulture, "FaceBookContent-{0}", offset));
                if (profileContent.Contains("https://login.facebook.com/login.php?login_attempt=1"))
                {
                    if (!this.GetLogon())
                    {
                        return contactToFill;
                    }
                    
                    profileContent = this.HttpRequester.GetContent(url, string.Format(CultureInfo.InvariantCulture, "FaceBookContent-{0}", offset));
                }

                var extracts = Regex.Matches(profileContent, @"Friends.friendClick\(this, event, (?<profileId>[0-9]*)\);", RegexOptions.Singleline);

                // if there is no contact in list, we did reach the end
                if (extracts.Count == 0)
                {
                    break;
                }

                // create a new instance of a list of references if there is none
                contact.Contacts = contact.Contacts ?? new List<ContactReference>(extracts.Count);

                foreach (Match extract in extracts)
                {
                    var profileId = extract.Groups["profileId"].ToString();
                    var stdId =
                        (from x in baseline
                         where x.ProfileId.GetProfileId(webSideParameters.ProfileIdentifierType) == profileId
                         select x.Id).FirstOrDefault();

                    // we ignore contacts we donn't know
                    if (stdId == default(Guid))
                    {
                        continue;
                    }

                    // lookup an existing entry in this contacts contact-list
                    var contactInList = (from x in contact.Contacts where x.Target == stdId select x).FirstOrDefault();

                    if (contactInList == null)
                    {
                        // add a new one if no existing entry has been found
                        contactInList = new ContactReference { Target = stdId };
                        contact.Contacts.Add(contactInList);
                        added++;
                    }

                    // update the flag that this entry is a private contact
                    // todo: private/business contact
                    contactInList.IsPrivateContact = true;
                }

                offset += extracts.Count;
            }
                
            this.LogProcessingEvent(contact, "{0} contacts found, {1} new added", offset, added);

            return contactToFill;
        }
    }
}