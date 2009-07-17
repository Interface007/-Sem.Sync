namespace Sem.Sync.WerKenntWenConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.DetailData;
    using System.Text;

    #endregion usings

    public class ContactClient : StdClient
    {
        #region string resources for processing wkw pages

        /// Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "id=\"loginform\"";

        /// <summary>
        /// detection string to detect if we did fail to logon
        /// </summary>
        private const string HttpDetectionStringLogonFailed = "/app/user?op=lostpassword";

        /// <summary>
        /// Base address to communicate with Wer-Kennt-Wen
        /// </summary>
        private const string HttpUrlBaseAddress = "http://www.wer-kennt-wen.de";

        /// <summary>
        /// relative url to log on
        /// </summary>
        private const string HttpUrlLogonRequest = "/start.php";

        /// <summary>
        /// relative URL to query contact links to personal pages
        /// </summary>
        private const string HttpUrlListContent = "/people/friends";

        /// <summary>
        /// data string to be posted to logon into wer-kennt-wen
        /// </summary>
        private const string HttpDataLogonRequest = "loginName={0}&pass={1}&x=0&y=0&logIn=1";

        /// <summary>
        /// regular expression to extract the URLs for the personal pages
        /// </summary>
        private const string PatternGetDataUrls = ".div class=\"pl-pic\"..a href=\"([a-zA-Z0-9]+)\"";

        #endregion

        #region private fields

        /// <summary>
        /// http requester object that will read the data from xing
        /// </summary>
        private readonly HttpHelper wkwRequester;

        #endregion

        /// <summary>
        /// cache hint string constant to specify a daily refresh for the cached files
        /// </summary>
        private const string CacheHintRefresh = "[REFRESH=DAILY]";

        /// <summary>
        /// cache hint string constant to specify that this item should not be cached at all
        /// </summary>
        private const string CacheHintNoCache = "[NOCACHE]";

        public override string FriendlyClientName
        {
            get { return "Wer-Kennt-Wen"; }
        }

        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var wkwContacts = this.GetUrlList();

            foreach (var item in wkwContacts)
            {
                var contact = this.DownloadContact(item, item.Replace("/", "_").Replace("?", "_"));
                if (contact != null)
                {
                    result.Add(contact);
                }
            }

            return result;
        }

        private List<string> GetUrlList()
        {
            var result = new List<string>();
            var offsetIndex = 0;

            string contactListContent;
            while (true)
            {
                while (true)
                {
                    // optimistically we try to read the content without explicit logon
                    // this will succeed if we have a valid cookie
                    contactListContent = this.wkwRequester.GetContent(
                        string.Format(CultureInfo.InvariantCulture, HttpUrlListContent, offsetIndex),
                        "UrlList" + offsetIndex + CacheHintRefresh);

                    // if we don't find the logon form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLogonNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        QueryForLogOnCredentials("Wer kennt wen benötigt die Log-In-Daten.");
                    }

                    // tell the user that we need to log on
                    LogProcessingEvent("Wer kennt wen benötigt die Log-In-Daten.", this.LogOnUserId);

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLogonRequest,
                        this.LogOnUserId,
                        this.LogOnPassword);

                    // post to get the cookies
                    var logInResponse = this.wkwRequester.GetContentPost(HttpUrlLogonRequest, CacheHintNoCache, postData);

                    if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                    {
                        LogProcessingEvent("Log-In ist fehlgeschlagen.", this.LogOnUserId);
                        return result;
                    }

                    // we did succeed to log on - tell the user and try reading the data again.
                    LogProcessingEvent("Login erfolgreich", this.LogOnUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var urlExtractor = new Regex(PatternGetDataUrls, RegexOptions.Singleline);
                var matches = urlExtractor.Matches(contactListContent);

                // if we don't find more matches, we have finished, or an error did occure
                if (matches.Count == 0)
                {
                    break;
                }

                LogProcessingEvent("füge Kontakte hinzu...", matches.Count, result.Count);

                // add the matches to the result
                foreach (Match match in matches)
                {
                    result.Add(match.Groups[1].ToString());
                }

                // we read 10 urls a time
                offsetIndex += matches.Count;
            }

            return result;
        }

        /// <summary>
        /// downloads a contact (vcard) from xing and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">url to the xing vcard</param>
        /// <param name="name">name that will be used for the cache to store the data for later review</param>
        /// <returns>the downloaded contact as a StdContact</returns>
        private StdContact DownloadContact(string downloadUrl, string name)
        {
            var data = this.wkwRequester.GetContent(downloadUrl, name + ".txt");
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }

            var extractor = new StringBuilder();
            extractor.Append("/users/firstName/[a-zA-Z0-9 ]*/user/[a-zA-Z0-9 ]*\">(.*?)</a>");

            // we use regular expressions to extract the urls to the vCards
                var dataExtractor = new Regex(PatternGetDataUrls, RegexOptions.Singleline);
                var matches = dataExtractor.Matches(data);

            var contact = new StdContact();
            contact.Name = new PersonName { FirstName = matches[0].Groups[1].ToString() };

            LogProcessingEvent(contact, "downloaded");
            return contact;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }
        
        public ContactClient()
        {
            this.wkwRequester = new HttpHelper(HttpUrlBaseAddress, true)
            {
                UseCache = false,
                SkipNotCached = false,
                UseIeCookies = false,
            };
        }
    }
}
