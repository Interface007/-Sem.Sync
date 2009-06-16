// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts from Xing.
//   Xing does currently (2009-05-11) not support an official API for
//   reading contact data, so this class will use the http-helper class
//   of Sem.Sync.Helpers.HttpHelper to extract the data from the web pages.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.XingConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Properties;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts from Xing. 
    /// Xing does currently (2009-05-11) not support an official API for
    /// reading contact data, so this class will use the http-helper class
    /// of Sem.Sync.Helpers.HttpHelper to extract the data from the web pages.
    /// </summary>
    public class ContactClient : StdClient
    {
        #region string resources for processing xing pages
        
        /// <summary>
        /// Detection string to parse the content of a request if we need to login
        /// </summary>
        private const string HttpDetectionStringLoginNeeded = "name=\"loginform\"";

        /// <summary>
        /// detection string to detect if we did fail to log in
        /// </summary>
        private const string HttpDetectionStringLoginFailed = "/app/user?op=lostpassword";

        /// <summary>
        /// Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "https://www.xing.com";

        /// <summary>
        /// relative url to log in
        /// </summary>
        private const string HttpUrlLoginRequest = "/app/user";

        /// <summary>
        /// relative URL to query contact links to vCards
        /// </summary>
        private const string HttpUrlListContent = "/app/contact?notags_filter=0;search_filter=;tags_filter=;offset={0}";
        
        /// <summary>
        /// data string to be posted to login into Xing
        /// </summary>
        private const string HttpDataLoginRequest = "op=login&dest=%2Fapp%2Fuser%3Fop%3Dhome&login_user_name={0}&login_password={1}";

        /// <summary>
        /// regular expression to extract the URLs for the vCards
        /// </summary>
        private const string PatternGetVCardUrls = ".app.vcard.op=vcard;scr_id=.*\" ";

        #endregion

        #region private fields
        
        /// <summary>
        /// http requester object that will read the data from xing
        /// </summary>
        private readonly HttpHelper xingRequester;

        /// <summary>
        /// converter for the vCards downloaded from Xing
        /// </summary>
        private readonly VCardConverter vCardConverter;
        
        #endregion

        private const string CacheHintRefresh = "[REFRESH=DAILY]";

        private const string CacheHintNoCache = "[NOCACHE]";

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        /// The default constructor will create and configure a new http-requester by reading 
        /// the config file. Use the parametrized constructor to provide a "ready-to-use"
        /// http-requester.
        /// </summary>
        public ContactClient()
        {
            this.xingRequester = new HttpHelper(HttpUrlBaseAddress, true)
            {
                UseCache = Convert.ToBoolean(this.GetConfigValue("UseCache"), CultureInfo.InvariantCulture),
                SkipNotCached = Convert.ToBoolean(this.GetConfigValue("SkipNotCached"), CultureInfo.InvariantCulture),
                UseIeCookies = Convert.ToBoolean(this.GetConfigValue("UseIeCookies"), CultureInfo.InvariantCulture),
            };

            this.vCardConverter = new VCardConverter { HttpRequester = this.xingRequester };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        /// This parametrized constructore does accept a "ready to use" http-requester. 
        /// This way you can specify a requester with properties that differ from 
        /// default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">the preconfigured http-helper class</param>
        public ContactClient(HttpHelper preconfiguredHttpHelper)
        {
            this.xingRequester = preconfiguredHttpHelper;
            this.vCardConverter = new VCardConverter { HttpRequester = this.xingRequester };
        }

        #endregion

        /// <summary>
        /// Returns a human readable name of this class.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Xing-Contact-Connector";
            }
        }

        /// <summary>
        /// Because Xing is a read only source, removing duplicates is not implemented.
        /// This method will throw a NotImplementedException
        /// </summary>
        /// <param name="clientFolderName">the method is not implemented - this parameter is not used.</param>
        public override void RemoveDuplicates(string clientFolderName)
        {
            throw new NotImplementedException(Resources.uiNoAlteringImplemented);
        }

        /// <summary>
        /// Reads the full list of contacts from Xing
        /// </summary>
        /// <param name="clientFolderName">this parameter is not used in this client implementation</param>
        /// <param name="result">the list that will be filled with the contacts</param>
        /// <returns>the list of contacts that has been read from Xing</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var xing = this.GetUrlList();

            LogProcessingEvent(Resources.uiDownloadingVCards, xing.Count);

            foreach (var item in xing)
            {
                // https://www.xing.com/app/vcard?op=vcard;scr_id=369754.ab12f8
                var contact = this.DownloadContact(item, item.Replace("/", "_").Replace("?", "_"));
                if (contact != null)
                {
                    result.Add(contact);
                }
            }

            return result;
        }

        /// <summary>
        /// Because Xing is a read only source, writing the list is not implemented.
        /// This method will throw a NotImplementedException
        /// </summary>
        /// <param name="elements">the method is not implemented - the elements parameter is not used.</param>
        /// <param name="clientFolderName">the method is not implemented - the clientFolderName parameter is not used.</param>
        /// <param name="skipIfExisting">the method is not implemented - the skipIfExisting parameter is not used.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException(Resources.uiNoAlteringImplemented);
        }

        #region private implementation

        /// <summary>
        /// downloads a contact (vcard) from xing and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">url to the xing vcard</param>
        /// <param name="name">name that will be used for the cache to store the data for later review</param>
        /// <returns>the downloaded contact as a StdContact</returns>
        private StdElement DownloadContact(string downloadUrl, string name)
        {
            var vCard = this.xingRequester.GetContentBinary(downloadUrl, name + ".txt");
            if (vCard == null)
            {
                return null;
            }

            var contact = this.vCardConverter.VCardToStdContact(vCard, ProfileIdentifierType.XingProfileId);
            contact.AdditionalTextData = null;
            LogProcessingEvent(contact, Resources.uiDownloaded);
            return contact;
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the login
        /// </summary>
        /// <returns>a list of urls for the vCards to be downloaded</returns>
        private List<string> GetUrlList()
        {
            // regular request    : https://www.xing.com/app/contact
            // with offset        : https://www.xing.com/app/contact?notags_filter=0;search_filter=;tags_filter=;offset=10
            // sample of vcard-url: /app/vcard?op=vcard;scr_id=364719.e3db1e
            var result = new List<string>();
            var offsetIndex = 0;

            LogProcessingEvent(Resources.uiReadingContactList, this.LogOnUserId);

            string contactListContent;
            while (true)
            {
                while (true)
                {
                    // optimistically we try to read the content without explicit login
                    // this will succeed if we have a valid cookie
                    contactListContent = this.xingRequester.GetContent(
                        string.Format(CultureInfo.InvariantCulture, HttpUrlListContent, offsetIndex),
                        "UrlList" + offsetIndex + CacheHintRefresh);

                    // if we don't find the login form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLoginNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        QueryForLogOnCredentials(Resources.uiXingNeedsCredentials);
                    }

                    // tell the user that we need to log in
                    LogProcessingEvent(Resources.uiLogInForUser, this.LogOnUserId);

                    // prepare the post data for log in
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLoginRequest,
                        this.LogOnUserId,
                        this.LogOnPassword);

                    // post to get the cookies
                    var logInResponse = this.xingRequester.GetContentPost(HttpUrlLoginRequest, CacheHintNoCache, postData);

                    if (logInResponse.Contains(HttpDetectionStringLoginFailed))
                    {
                        LogProcessingEvent(Resources.uiLogInFailed, this.LogOnUserId);
                        return result;
                    }

                    // we did succeed to log in - tell the user and try reading the data again.
                    LogProcessingEvent(Resources.uiLogInSucceeded, this.LogOnUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var vCardExtractor = new Regex(PatternGetVCardUrls);
                var matches = vCardExtractor.Matches(contactListContent);

                // if we don't find more matches, we have finished, or an error did occure
                if (matches.Count == 0)
                {
                    break;
                }

                LogProcessingEvent(Resources.uiAddingContacts, matches.Count, result.Count);

                // add the matches to the result
                foreach (var match in matches)
                {
                    result.Add(match.ToString().Substring(0, match.ToString().Length - 2));
                }

                // we read 10 urls a time
                offsetIndex += 10;
            }

            return result;
        }

        #endregion
    }
}
