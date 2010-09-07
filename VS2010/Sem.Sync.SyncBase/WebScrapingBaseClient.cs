// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingBaseClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the base client class for handling contacts using web scraping technology.
//   It implements handling the basics of login, download of friend list and contacts content.
//   The inherited class needs to provifr information about the web site via properties.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Authentication;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// This class is the base client class for handling contacts using web scraping technology.
    ///   It implements handling the basics of login, download of friend list and contacts content.
    ///   The inherited class needs to provifr information about the web site via properties.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true, ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "WebScraping-Base-Client", 
        CanReadContacts = false, CanWriteContacts = false,
        NeedsCredentials = true, Internal = true, 
        MatchingIdentifier = ProfileIdentifierType.Default)]
    public abstract class WebScrapingBaseClient : StdClient, IExtendedReader
    {
        #region Constants and Fields

        /// <summary>
        ///   http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

        /// <summary>
        /// The source of the web scraping parameters of this instance - it may be a file location or an http web reference.
        /// </summary>
        private readonly string scrapingParameterSource;

        /// <summary>
        /// The compiled regular expression to extract the identifiers from the friend list.
        /// </summary>
        private Regex personIdentifierFromContactsListExtractor;

        /// <summary>
        /// The web scraping parameters of this instance.
        /// </summary>
        private WebSideParameters parameters;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        ///   This parametrized constructore does accept a "ready to use" http-requester. 
        ///   This way you can specify a requester with properties that differ from 
        ///   default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">
        /// the preconfigured http-helper class
        /// </param>
        protected WebScrapingBaseClient(HttpHelper preconfiguredHttpHelper)
        {
            this.httpRequester = preconfiguredHttpHelper;
            this.scrapingParameterSource = this.GetConfigValue("WebScrapingParameterSource", "http://www.svenerikmatzen.info/WebScrapingParameters/");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingBaseClient"/> class. 
        ///   The default constructor will create and configure a new http-requester by reading 
        ///   the config file. Use the parametrized constructor to provide a "ready-to-use"
        ///   http-requester.
        /// </summary>
        /// <param name="httpUrlBaseAddress">
        /// The http Base Address of this connector. 
        /// </param>
        protected WebScrapingBaseClient(string httpUrlBaseAddress)
        {
            this.httpRequester = new HttpHelper(httpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"), 
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"), 
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"), 
                };
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the HttpRequester.
        /// </summary>
        protected HttpHelper HttpRequester
        {
            get
            {
                return this.httpRequester;
            }
        }

        /// <summary>
        ///   Gets the WebSideParameters which defines how to deal with the site.
        /// </summary>
        protected virtual WebSideParameters WebSideParameters
        {
            get
            {
                if (this.parameters == null)
                {
                    var parameterFileName = this.FriendlyClientName + ".xml";
                    if (this.scrapingParameterSource.StartsWith("http", StringComparison.OrdinalIgnoreCase)) 
                    {
                        var parameterFile = this.httpRequester.GetContent(this.scrapingParameterSource + parameterFileName);
                        this.parameters = Tools.LoadFromString<WebSideParameters>(parameterFile);
                    }
                    else
                    {
                        this.parameters = Tools.LoadFromFile<WebSideParameters>(this.scrapingParameterSource);
                    }
                }

                return this.parameters;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs a log on to the web site. This function does not throw an exception, but returns false in case
        /// of an authentication problem.
        /// </summary>
        /// <returns>a value indicating whether the log on was successfull</returns>
        public bool Logon()
        {
            if (string.IsNullOrEmpty(this.LogOnPassword))
            {
                this.QueryForLogOnCredentials("needs some credentials");
            }

            // prepare the post data for log on
            var postData = HttpHelper.PreparePostData(this.WebSideParameters.HttpDataLogOnRequest, this.LogOnUserId, this.LogOnPassword);

            // post to get the cookies
            var logInResponse = this.httpRequester.GetContentPost(this.WebSideParameters.HttpUrlLogOnRequest, "logOn", postData);
            return !logInResponse.Contains(this.WebSideParameters.HttpDetectionStringLogOnFailed);
        }

        /// <summary>
        /// Implements the method to fill the contact with additional links to other contacts.
        /// </summary>
        /// <param name="contactToFill"> The contact to be filled. </param>
        /// <param name="baseline"> The baseline that does contain possible link targets. </param>
        /// <returns> the manipulated contact </returns>
        public StdElement FillContacts(StdElement contactToFill, ICollection<MatchingEntry> baseline)
        {
            var contact = contactToFill as StdContact;
            var webSideParameters = this.WebSideParameters;

            var profileIdentifierType = webSideParameters.ProfileIdentifierType;
            this.personIdentifierFromContactsListExtractor = new Regex(webSideParameters.PersonIdentifierFromContactsListRegex, RegexOptions.Singleline);
            var personIdentifierExtractor = new Regex(webSideParameters.ProfileIdPartExtractor, RegexOptions.Singleline);

            if (contact == null || !contact.ExternalIdentifier.ContainsKey(profileIdentifierType))
            {
                return contactToFill;
            }

            var profileIdInformation = contact.ExternalIdentifier[profileIdentifierType];
            if (profileIdInformation == null || string.IsNullOrWhiteSpace(profileIdInformation.Id))
            {
                return contactToFill;
            }

            var match = personIdentifierExtractor.Match(profileIdInformation.Id);
            if (match.Groups.Count == 0)
            {
                return contactToFill;
            }

            var identifierPart = match.Groups[1];

            var offset = 0;
            var added = 0;
            while (true)
            {
                this.LogProcessingEvent("reading contacts (from offset {0})", offset);
                this.HttpRequester.ContentCredentials = this;

                // get the contact list
                var url = string.Format(
                    CultureInfo.InvariantCulture,
                    webSideParameters.ContactListUrl,
                    identifierPart,
                    offset);

                var profileContent = this.HttpRequester.GetContent(url);
                var loginNeeded = webSideParameters.HttpDetectionStringLogOnNeeded.Where(profileContent.Contains).Count() > 0;
                if (loginNeeded)
                {
                    this.LogProcessingEvent(contact, "log on needed");

                    if (!this.Logon())
                    {
                        this.LogProcessingEvent(contact, "log on failed");
                        return contactToFill;
                    }

                    profileContent = this.HttpRequester.GetContent(url);
                }

                var profileIds = this.ExtractProfileIdsFromFriendsList(profileIdInformation, profileContent);

                // if there is no contact in list, we did reach the end
                if (profileIds.Count == 0)
                {
                    break;
                }

                // create a new instance of a list of references if there is none
                added = this.AddContactIdsToStdContact(contact, profileIds, baseline, added);
                this.LogProcessingEvent(contact, "{0} contacts found, {1} added.", profileIds.Count, added);

                this.LogProcessingEvent(contact, "sleeping some time to not being identifies as a bot...");
                this.ThinkTime();
        
                // todo: facebook uses a page size of 64, wer-kennt-wen 
                offset += 64; // extracts.Count;
            }

            this.LogProcessingEvent(contact, "{0} contacts found, {1} new added", offset, added);

            return contactToFill;
        }

        /// <summary>
        /// If implemented, reads all contact relations for all the !specified! contacts in the <paramref name="contactToFill"/> collection.
        /// Use this to implement connectors that are capable to read multiple contact relation lists in one operation.
        /// This client implementation does not implement reading multiple contacts at once - override the method <see cref="FillContacts"/>, too,
        /// to prevent double processing the contacts.
        /// </summary>
        /// <param name="contactToFill">the list of contacts to be filled</param>
        /// <param name="baseline">the baseline to be used</param>
        public void FillAllContacts(ICollection<StdElement> contactToFill, ICollection<MatchingEntry> baseline)
        {
            return;
        }
        
        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <param name="content"> The content to be converted. </param>
        /// <returns> a new StdClient created from the data provided  </returns>
        protected abstract StdContact ConvertToStdContact(string contactUrl, string content);

        /// <summary>
        /// Abstract read method for full list of elements - this is part of the minimum that needs to be overridden
        /// </summary>
        /// <param name="clientFolderName">
        /// the information from where inside the source the elements should be read - 
        ///   This does not need to be a real "path", but need to be something that can be expressed as a string
        /// </param>
        /// <param name="result"> The list of elements that should get the elements. The elements should be added to the list instead of replacing it. </param>
        /// <returns> The list with the newly added elements </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            this.httpRequester.UiDispatcher = this.UiDispatcher;
            var contactUrls = this.GetUrlList();

            result.AddRange(contactUrls.Select(contactUrl => this.DownloadContact(string.Format(CultureInfo.InvariantCulture, this.WebSideParameters.HttpUrlContactDownload, contactUrl))));

            result.Sort();
            return result;
        }

        /// <summary>
        /// Convert contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <returns> the downloaded information inserted into a <see cref="StdContact"/></returns>
        private StdContact DownloadContact(string contactUrl)
        {
            var content = this.httpRequester.GetContent(contactUrl, contactUrl, string.Empty);
            var result = this.ConvertToStdContact(contactUrl, content);
            if (!string.IsNullOrEmpty(this.WebSideParameters.ExtractorProfilePictureUrl))
            {
                var pictureUrl = Regex.Match(content, this.WebSideParameters.ExtractorProfilePictureUrl);
                if (pictureUrl.Groups.Count > 1)
                {
                    var pictureUrlString = pictureUrl.Groups[1].ToString();
                    if (!pictureUrlString.EndsWith(this.WebSideParameters.ImagePlaceholderUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        result.PictureData = this.httpRequester.GetContentBinary(pictureUrlString, contactUrl, string.Empty);
                    }
                }
            }

            this.ThinkTime(5000);

            return result;
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <exception cref="AuthenticationException">In case of a failing authentication an <see cref="AuthenticationException"/> is thrown.</exception>
        /// <returns> a list of urls for the data to be downloaded </returns>
        private IEnumerable<string> GetUrlList()
        {
            var result = new List<string>();
            this.httpRequester.LogOnFormDetectionString = this.WebSideParameters.HttpDetectionStringLogOnNeeded;
            var extractor = new Regex(this.WebSideParameters.ExtractorFriendUrls, RegexOptions.Singleline | RegexOptions.Compiled);

            while (true)
            {
                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                var theContact = this.httpRequester.GetContent(string.Format(CultureInfo.InvariantCulture, this.WebSideParameters.HttpUrlFriendList, 0));
                var friendIds = extractor.Matches(theContact);

                this.ThinkTime(1000);

                if (friendIds.Count >= 2)
                {
                    result.AddRange(from Match match in friendIds select match.Groups["id"].ToString());
                    return result;
                }

                // we didn't get any valid result, so we assume we have to log in.
                if (!this.Logon())
                {
                    throw new AuthenticationException("Authentication to web site failed.");
                }
            }
        }

        /// <summary>
        /// Adds profile ids to a contacts "contact list" with filtering its own
        /// profile id and preventing doubletts.
        /// </summary>
        /// <param name="contact"> The contact (source of the link). </param>
        /// <param name="profileIds"> The profile ids to be added as link targets. </param>
        /// <param name="baseline"> The baseline. </param>
        /// <param name="added"> The number of targets added so far. </param>
        /// <returns> The number of links added </returns>
        private int AddContactIdsToStdContact(StdContact contact, List<string> profileIds, IEnumerable<MatchingEntry> baseline, int added)
        {
            var profileIdentifierType = this.WebSideParameters.ProfileIdentifierType;
            contact.Contacts = contact.Contacts ?? new List<ContactReference>(profileIds.Count);

            foreach (var extract in profileIds)
            {
                var profileId = string.Format(this.WebSideParameters.ProfileIdFormatter, extract);
                var stdId =
                    (from x in baseline
                     where x.ProfileId.GetProfileId(profileIdentifierType) == profileId
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
                var connectorDescriptionAttribute = this.ConnectorDescription;
                contactInList.IsPrivateContact = connectorDescriptionAttribute.ContentIsPrivate;
                contactInList.IsBusinessContact = connectorDescriptionAttribute.ContentIsBusiness;
            }

            return added;
        }

        protected ConnectorDescriptionAttribute ConnectorDescription
        {
            get
            {
                var myType = this.GetType();
                return myType.GetCustomAttributes(typeof(ConnectorDescriptionAttribute), true).First() as ConnectorDescriptionAttribute;
            }
        }

        /// <summary>
        /// Extracts link targets from a friends-list html page.
        /// </summary>
        /// <param name="currentContactsId">the profile id current of the current contact (the link SOURCE)</param>
        /// <param name="friendListContent">the html content of the friend list html of the current contact (containing the TARGETS)</param>
        /// <returns>the extracted link targets</returns>
        private List<string> ExtractProfileIdsFromFriendsList(ProfileIdInformation currentContactsId, string friendListContent)
        {
            var profileIdCandidates = this.personIdentifierFromContactsListExtractor.Matches(friendListContent);
            var profileIds = new List<string>();

            foreach (var profileIdCandidate in profileIdCandidates)
            {
                var id = profileIdCandidate.ToString();
                if (id == currentContactsId)
                {
                    continue;
                }

                if (profileIds.Contains(id))
                {
                    continue;
                }

                profileIds.Add(id);
            }

            return profileIds;
        }
        #endregion
    }
}