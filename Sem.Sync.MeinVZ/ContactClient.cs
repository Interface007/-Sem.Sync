// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.MeinVZ
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(
        Irrelevant = true,
        ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(DisplayName = "MeinVZ", 
        CanRead = true,
        CanWrite = false,
        MatchingIdentifier = ProfileIdentifierType.MeinVZ,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        #region string resources for processing the pages

        /// <summary>
        /// Detection string to parse the content of a request if we need to logon
        /// </summary>
        private const string HttpDetectionStringLogonNeeded = "form id=\"Loginbox\"";

        /// <summary>
        /// detection string to detect if we did fail to logon
        /// </summary>
        private const string HttpDetectionStringLogonFailed = "action=\"https://secure.meinvz.net/Login\"";

        /// <summary>
        /// Base address to communicate with the site
        /// </summary>
        private const string HttpUrlBaseAddress = "http://www.meinvz.net";

        /// <summary>
        /// relative url to log on
        /// </summary>
        private const string HttpUrlLogonRequest = "https://secure.meinvz.net/Login";

        /// <summary>
        /// realtive url to get the data of the contacts
        /// </summary>
        private const string HttpUrlListContent = "";

        /// <summary>
        /// data string to be posted to logon into the site
        /// </summary>
        private const string HttpDataLogonRequest 
            = "email={0}&"
            + "password={1}&"
            + "login=Einloggen&jsEnabled=false&"
            + "formkey={2}&"
            + "iv={3}";

        /// <summary>
        /// regex to extract the form key for the log on
        /// </summary>
        private const string ExtractorFormKey = "<input type=\"hidden\" name=\"formkey\" value=\"([0-9a-z]*)\" />";

        /// <summary>
        /// regex to extract the iv for the log on
        /// </summary>
        private const string ExtractorIv = "<input type=\"hidden\" name=\"iv\" value=\"([0-9a-z]*)\" />";

        #endregion

        /// <summary>
        /// http requester object that will read the data from the site
        /// </summary>
        private readonly HttpHelper httpRequester;

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        /// The default constructor will create and configure a new http-requester by reading 
        /// the config file. Use the parametrized constructor to provide a "ready-to-use"
        /// http-requester.
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
        /// Initializes a new instance of the <see cref="ContactClient"/> class. 
        /// This parametrized constructore does accept a "ready to use" http-requester. 
        /// This way you can specify a requester with properties that differ from 
        /// default/config-file properties.
        /// </summary>
        /// <param name="preconfiguredHttpHelper">the preconfigured http-helper class</param>
        public ContactClient(HttpHelper preconfiguredHttpHelper)
        {
            this.httpRequester = preconfiguredHttpHelper;
        }

        #endregion

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "MeinVZ-Connector";
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
            this.httpRequester.UiDispatcher = this.UiDispatcher;
            var site = this.GetUrlList();

            return result;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <returns>a list of urls for the vCards to be downloaded</returns>
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
                    contactListContent = this.httpRequester.GetContent(
                        string.Format(CultureInfo.InvariantCulture, HttpUrlListContent, offsetIndex),
                        "UrlList" + offsetIndex + HttpHelper.CacheHintRefresh);

                    // if we don't find the logon form any more, we did succeed
                    if (!contactListContent.Contains(HttpDetectionStringLogonNeeded))
                    {
                        break;
                    }

                    if (string.IsNullOrEmpty(this.LogOnPassword))
                    {
                        QueryForLogOnCredentials("needs some credentials");
                    }

                    var matches = Regex.Matches(contactListContent, ExtractorFormKey, RegexOptions.Singleline);
                    var formKey = matches[0].Groups[1].Captures[0].ToString();

                    matches = Regex.Matches(contactListContent, ExtractorIv, RegexOptions.Singleline);
                    var iv = matches[0].Groups[1].Captures[0].ToString();

                    // prepare the post data for log on
                    var postData = HttpHelper.PreparePostData(
                        HttpDataLogonRequest,
                        this.LogOnUserId,
                        this.LogOnPassword,
                        formKey,
                        iv);

                    // post to get the cookies
                    var logInResponse = this.httpRequester.GetContentPost(HttpUrlLogonRequest, HttpHelper.CacheHintNoCache, postData);

                    if (!logInResponse.Contains(HttpDetectionStringLogonFailed))
                    {
                        return result;
                    }

                    // we did succeed to log on - tell the user and try reading the data again.
                }

                //// we use regular expressions to extract the urls to the vCards
                ////var vCardExtractor = new Regex(PatternGetVCardUrls, RegexOptions.Singleline);
                ////var matches = vCardExtractor.Matches(contactListContent);

                //// if we don't find more matches, we have finished, or an error did occure
                ////if (matches.Count == 0)
                ////{
                ////    break;
                ////}

                ////// add the matches to the result
                ////foreach (Match match in matches)
                ////{
                ////    result.Add(
                ////        new ContactReference
                ////        {
                ////            Url = match.Groups[1].ToString(),
                ////            Tags = match.Groups[2].ToString(),
                ////        });
                ////}

                ////// we read 10 urls a time
                ////offsetIndex += matches.Count;
            }

            return result;
        }
    }
}
