namespace Sem.Sync.XingConnector
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    using Properties;

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
        private const string DetectLoginNeeded = "name=\"loginform\"";
        private const string XingBaseAddress = "https://www.xing.com";
        private const string ListContentUrl = "/app/contact?notags_filter=0;search_filter=;tags_filter=;offset={0}";
        #endregion

        #region private implementation

        /// <summary>
        /// http requester object that will read the data from xing
        /// </summary>
        private readonly HttpHelper xingRequester;
        
        /// <summary>
        /// converter for the vCards doenloaded from Xing
        /// </summary>
        private readonly VCardConverter vCardConverter;

        public ContactClient()
        {
            xingRequester = new HttpHelper(XingBaseAddress, true)
                                {
                                    UseCache = Convert.ToBoolean(this.GetConfigValue("UseCache")),
                                    SkipNotCached = Convert.ToBoolean(this.GetConfigValue("SkipNotCached")),
                                    UseIeCookies = Convert.ToBoolean(this.GetConfigValue("UseIeCookies")),
                                };

            vCardConverter = new VCardConverter { HttpRequester = this.xingRequester };
        }

        /// <summary>
        /// downloads a contact (vcard) from xing and converts the data into a standard contact
        /// </summary>
        /// <param name="downloadUrl">url to the xing vcard</param>
        /// <param name="name">name that will be used for the cache to store the data for later review</param>
        /// <returns></returns>
        private StdElement DownloadContact(string downloadUrl, string name)
        {
            var vCard = xingRequester.GetContentBinary(downloadUrl, name + ".txt");
            if (vCard == null)
            {
                return null;
            }

            var contact = vCardConverter.VCardToStdContact(vCard, ProfileIdentifierType.Default);
            LogProcessingEvent(contact, Resources.uiDownloaded);
            return contact;
        }


        private List<string> GetUrlList()
        {
            // regular request    : https://www.xing.com/app/contact
            // with offset        : https://www.xing.com/app/contact?notags_filter=0;search_filter=;tags_filter=;offset=10
            // sample of vcard-url: /app/vcard?op=vcard;scr_id=364719.e3db1e

            var result = new List<string>();
            var offsetIndex = 0;

            LogProcessingEvent(Resources.uiReadingContactList, this.LoginUserId);
            
            string contactListContent;
            while (true)
            {
                while (true)
                {
                    // optimistically we try to read the content without explicit login
                    // this will succeed if we have a valid cookie
                    contactListContent = xingRequester.GetContent(string.Format(ListContentUrl , offsetIndex), offsetIndex + "urllist.txt");

                    // if we don't find the login form any more, we did succeed
                    if (!contactListContent.Contains(DetectLoginNeeded))
                        break;

                    if (string.IsNullOrEmpty(this.LoginPassword))
                    {
                        QueryForLogOnCredentials(Resources.uiXingNeedsCredentials);
                    }

                    // tell the user that we need to log in
                    LogProcessingEvent(Resources.uiLogInForUser, this.LoginUserId);

                    // prepare the post data for log in
                    var postData = string.Format(
                        "op=login&dest=%2Fapp%2Fuser%3Fop%3Dhome&login_user_name={0}&login_password={1}", 
                        HttpHelper.EncodeForPost(this.LoginUserId), 
                        HttpHelper.EncodeForPost(this.LoginPassword));

                    // post to get the cookies
                    var logInResponse = xingRequester.GetContentPost("/app/user", "[NOCACHE]", postData);

                    if (logInResponse.Contains("/app/user?op=lostpassword"))
                    {
                        LogProcessingEvent(Resources.uiLogInFailed, this.LoginUserId);
                        return result;
                    }
                    
                    // we did succeed to log in - tell the user and try reading the data again.
                    LogProcessingEvent(Resources.uiLogInSucceeded, this.LoginUserId);
                }

                // we use regular expressions to extract the urls to the vCards
                var vCardExtractor = new Regex(".app.vcard.op=vcard;scr_id=.*\" ");
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

        public override void RemoveDuplicates(string clientFolderName)
        {
            throw new NotImplementedException(Resources.uiNoAlteringImplemented);
        }

        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var xing = GetUrlList();

            LogProcessingEvent(Resources.uiDownloadingVCards, xing.Count);

            foreach (var item in xing)
            {
                //https://www.xing.com/app/vcard?op=vcard;scr_id=369754.ab12f8
                var contact = DownloadContact(item, item.Replace("/", "_").Replace("?", "_"));
                if (contact != null)
                {
                    result.Add(contact);
                }
            }

            return result;
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException(Resources.uiNoAlteringImplemented);
        }

        public override string FriendlyClientName
        {
            get
            {
                return "Xing-Contact-Connector";
            }
        }
    }
}
