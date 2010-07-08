// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This class is the client class for handling contacts from Xing.
//   Xing does currently (2009-05-11) not support an official API for
//   reading contact data, so this class will use the http-helper class
//   of Sem.Sync.Helpers.HttpHelper to extract the data from the web pages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Xing
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Interfaces;
    using Sem.Sync.Connector.Xing.Properties;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts from Xing. 
    ///   Xing does currently (2009-05-11) not support an official API for
    ///   reading contact data, so this class will use the http-helper class
    ///   of Sem.Sync.Helpers.HttpHelper to extract the data from the web pages.
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        NeedsCredentialsDomain = false, DisplayName = "Xing",
        MatchingIdentifier = ProfileIdentifierType.XingNameProfileId)]
    public class ContactClient : StdClient, IExtendedReader
    {
        #region Constants and Fields

        /// <summary>
        ///   data string to be posted to logon into Xing
        /// </summary>
        private const string HttpDataLogonRequest = "op=login&dest=%2F&login_user_name={0}&login_password={1}";

        /// <summary>
        ///   detection string to detect if we did fail to logon
        /// </summary>
        private const string HttpDetectionStringLogonFailed = "/app/user?op=lostpassword";

        /// <summary>
        ///   Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "name=\"loginform\"";

        /// <summary>
        ///   Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "https://www.xing.com";

        /// <summary>
        ///   relative URL to query contact links to vCards
        /// </summary>
        private const string HttpUrlListContent =
            "/app/contact?notags_filter=0;card_mode=0;search_filter=;tags_filter=;offset={0}";

        /// <summary>
        ///   relative url to log on
        /// </summary>
        private const string HttpUrlLogonRequest = "/app/user";

        /// <summary>
        ///   URL to query the contacts of another named contact
        /// </summary>
        private const string HttpUrlProfileContacts = "/app/search?op=foaflist&offset={0}";

        /// <summary>
        ///   Extracts the ID and the user profile name from a contacts page
        /// </summary>
        private const string PatternGetContactContacts = @"\<td\>\<a href=./profile/(?<source>[^/]*)(\<[^/][^t][^d][^>]*\>[^\<]*)*(?<targets>.*?)\</tr\>";

        /// <summary>
        ///   regular expression to extract the URLs for the vCards
        /// </summary>
        private const string PatternGetVCardUrls =
            @"name="".*?"" href=""/profile/(?<uname>.*?)/.*?(?<vcardurl>.app.contact.op=vcard;scr_id=[a-zA-Z0-9]+[.][a-zA-Z0-9]*)"".*?(inputField_[0-9]*"" value=""(?<tags>[\w ,]*)"")?";

        ////"(.app.contact.op=vcard;scr_id=[a-zA-Z0-9]+[.][a-zA-Z0-9]*)\".*?inputField_[0-9]*\" value=\"([\\w ,]*)\"";

        /// <summary>
        ///   converter for the vCards downloaded from Xing
        /// </summary>
        private readonly VCardConverter vCardConverter;

        /// <summary>
        ///   http requester object that will read the data from xing
        /// </summary>
        private readonly IHttpHelper xingRequester;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ContactClient" /> class. 
        ///   The default constructor will create and configure a new http-requester by reading 
        ///   the config file. Use the parametrized constructor to provide a "ready-to-use"
        ///   http-requester.
        /// </summary>
        public ContactClient()
        {
            this.xingRequester = Factory.Invoke<IHttpHelper>(() => new HttpHelper(HttpUrlBaseAddress, true)
                {
                    UseCache = this.GetConfigValueBoolean("UseCache"),
                    SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                    UseIeCookies = this.GetConfigValueBoolean("UseIeCookies")
                });

            this.vCardConverter = new VCardConverter { HttpRequester = this.xingRequester };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        ///   This parametrized constructore does accept a "ready to use" http-requester. 
        ///   This way you can specify a requester with properties that differ from 
        ///   default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">
        /// the preconfigured http-helper class
        /// </param>
        public ContactClient(HttpHelper preconfiguredHttpHelper)
        {
            this.xingRequester = preconfiguredHttpHelper;
            this.vCardConverter = new VCardConverter { HttpRequester = this.xingRequester };
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Returns a human readable name of this class.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Xing-Contact-Connector";
            }
        }

        #endregion

        #region Implemented Interfaces

        #region IExtendedReader

        public void FillAllContacts(ICollection<StdElement> contactsToFill, ICollection<MatchingEntry> baseline)
        { 
            var offset = 0;
            while (true)
            {
                var content = this.GetTextContent(string.Format(HttpUrlProfileContacts, offset), string.Empty);
                var results = Regex.Matches(content, PatternGetContactContacts, RegexOptions.Singleline);

                if (results.Count == 0)
                {
                    return;
                }

                foreach (Match item in results)
                {
                    var sourceId = item.Groups["source"].ToString();
                    var sourceContact =
                        baseline.Where(
                            x => x.ProfileId.GetProfileId(ProfileIdentifierType.XingNameProfileId) == sourceId).
                            FirstOrDefault();

                    if (sourceContact == null)
                    {
                        continue;
                    }

                    var source = contactsToFill.Where(x => x.Id == sourceContact.Id).FirstOrDefault() as StdContact;
                    if (source == null)
                    {
                        continue;
                    }

                    var targets = Regex.Matches(item.Groups["targets"].ToString(), @"(\<a href=./profile/(?<target>[^/]*))", RegexOptions.Singleline);
                    foreach (Match target in targets)
                    {
                        var targetId = target.Groups["target"].ToString();

                        var targetEntry =
                            baseline.Where(
                                x =>
                                x.ProfileId.GetProfileId(ProfileIdentifierType.XingNameProfileId) == targetId).
                                FirstOrDefault();

                        if (targetEntry == null)
                        {
                            continue;
                        }

                        var reference = new ContactReference { IsBusinessContact = true, Target = targetEntry.Id };
                        if (!source.Contacts.Contains(reference))
                        {
                            source.Contacts.Add(reference);
                            Debug.Print("adding reference: " + reference);
                        }
                        else
                        {
                            Debug.Print("already existing: " + reference);
                        }
                    }
                }

                offset += results.Count;
            }
        }

        /// <summary>
        /// Implements the interface to get more information - in this case the 
        ///   related contacts from the profile
        /// </summary>
        /// <param name="contactToFill">The contact to fill.</param>
        /// <param name="baseline"> The baseline.</param>
        /// <returns>the contact with more information</returns>
        public StdElement FillContacts(StdElement contactToFill, ICollection<MatchingEntry> baseline)
        {
            return contactToFill;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Reads the full list of contacts from Xing
        /// </summary>
        /// <param name="clientFolderName">
        /// this parameter is not used in this client implementation
        /// </param>
        /// <param name="result">
        /// the list that will be filled with the contacts
        /// </param>
        /// <returns>
        /// the list of contacts that has been read from Xing
        /// </returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            this.xingRequester.UiDispatcher = this.UiDispatcher;
            var xing = this.GetUrlList();
            var itemIndex = 0;
            var itemsToDo = xing.Count;

            this.LogProcessingEvent(Resources.uiDownloadingVCards, xing.Count);

            foreach (var item in xing)
            {
                // https://www.xing.com/app/vcard?op=vcard;scr_id=369754.ab12f8
                var contact = this.DownloadContact(item.VCardUrl, item.VCardUrl.Replace("/", "_").Replace("?", "_"));
                if (contact != null)
                {
                    contact.ExternalIdentifier.SetProfileId(ProfileIdentifierType.XingNameProfileId, item.ProfileUrl);
                    contact.Categories =
                        new List<string>(item.Tags.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                    result.Add(contact);
                }

                this.UpdateProgress(itemIndex++ / itemsToDo * 100);
            }

            return result;
        }

        /// <summary>
        /// downloads a contact (vcard) from xing and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">
        /// url to the xing vcard
        /// </param>
        /// <param name="name">
        /// name that will be used for the cache to store the data for later review
        /// </param>
        /// <returns>
        /// the downloaded contact as a StdContact
        /// </returns>
        private StdContact DownloadContact(string downloadUrl, string name)
        {
            var vCard = this.xingRequester.GetContentBinary(downloadUrl, name + ".txt");
            if (vCard == null)
            {
                return null;
            }

            var contact = this.vCardConverter.VCardToStdContact(vCard, ProfileIdentifierType.XingNameProfileId);
            contact.AdditionalTextData = null;
            this.LogProcessingEvent(contact, Resources.uiDownloaded);
            return contact;
        }

        /// <summary>
        /// Gets the textual content of a URL - this does test for the <see cref="HttpDetectionStringLogonNeeded"/> value
        ///   and asks for credentials. It performs a login request using the provided information and the
        ///   <see cref="HttpUrlLogonRequest"/>.
        /// </summary>
        /// <param name="url">
        /// The url to get the content.  
        /// </param>
        /// <param name="name">
        /// A name of the content requested. 
        /// </param>
        /// <returns>
        /// the content of the url  
        /// </returns>
        private string GetTextContent(string url, string name)
        {
            while (true)
            {
                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                var content = this.xingRequester.GetContent(url, name);

                // if we don't find the logon form any more, we did succeed
                if (!content.Contains(HttpDetectionStringLogonNeeded))
                {
                    return content;
                }

                if (string.IsNullOrEmpty(this.LogOnPassword))
                {
                    this.QueryForLogOnCredentials(Resources.uiXingNeedsCredentials);
                }

                // tell the user that we need to log on
                this.LogProcessingEvent(Resources.uiLogInForUser, this.LogOnUserId);

                // prepare the post data for log on
                var postData = HttpHelper.PreparePostData(HttpDataLogonRequest, this.LogOnUserId, this.LogOnPassword);

                // post to get the cookies
                var logInResponse = this.xingRequester.GetContentPost(
                    HttpUrlLogonRequest, HttpHelper.CacheHintNoCache, postData);

                if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                {
                    this.LogProcessingEvent(Resources.uiLogInFailed, this.LogOnUserId);
                    return string.Empty;
                }

                // we did succeed to log on - tell the user and try reading the data again.
                this.LogProcessingEvent(Resources.uiLogInSucceeded, this.LogOnUserId);
            }
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <returns>
        /// a list of urls for the vCards to be downloaded
        /// </returns>
        private List<XingContactReference> GetUrlList()
        {
            // regular request    : https://www.xing.com/app/contact
            // with offset        : https://www.xing.com/app/contact?notags_filter=0;search_filter=;tags_filter=;offset=10
            // sample of vcard-url: /app/vcard?op=vcard;scr_id=364719.e3db1e
            var result = new List<XingContactReference>();
            var offsetIndex = 0;

            this.LogProcessingEvent(Resources.uiReadingContactList, this.LogOnUserId);

            string contactListContent;
            while (true)
            {
                while (true)
                {
                    // optimistically we try to read the content without explicit logon
                    // this will succeed if we have a valid cookie
                    contactListContent =
                        this.xingRequester.GetContent(
                            string.Format(CultureInfo.InvariantCulture, HttpUrlListContent, offsetIndex),
                            "UrlList" + offsetIndex);

                    // if we don't find the logon form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLogonNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        this.QueryForLogOnCredentials(Resources.uiXingNeedsCredentials);
                    }

                    // tell the user that we need to log on
                    this.LogProcessingEvent(Resources.uiLogInForUser, this.LogOnUserId);

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(HttpDataLogonRequest, this.LogOnUserId, this.LogOnPassword);

                    // post to get the cookies
                    var logInResponse = this.xingRequester.GetContentPost(HttpUrlLogonRequest, HttpHelper.CacheHintNoCache, postData);

                    if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                    {
                        this.LogProcessingEvent(Resources.uiLogInFailed, this.LogOnUserId);
                        return result;
                    }

                    // we did succeed to log on - tell the user and try reading the data again.
                    this.LogProcessingEvent(Resources.uiLogInSucceeded, this.LogOnUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var vCardExtractor = new Regex(PatternGetVCardUrls, RegexOptions.Singleline);
                var matches = vCardExtractor.Matches(contactListContent);

                // if we don't find more matches, we have finished, or an error did occure
                if (matches.Count == 0)
                {
                    break;
                }

                this.LogProcessingEvent(Resources.uiAddingContacts, matches.Count, result.Count);

                // add the matches to the result
                foreach (Match match in matches)
                {
                    result.Add(
                        new XingContactReference
                            {
                                VCardUrl = match.Groups["vcardurl"].ToString(),
                                Tags = match.Groups["tags"].ToString(),
                                ProfileUrl = match.Groups["uname"].ToString()
                            });
                }

                // we read 10 urls a time
                offsetIndex += matches.Count;
            }

            return result;
        }

        #endregion
    }
}