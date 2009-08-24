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

        /// <summary>
        /// regex to extract the iv for the log on
        /// </summary>
        private const string ExtractorFriendUrls = "<a href=\"(/Friends/All/[a-z0-9]*/tid/[0-9]*)\" rel=\"nofollow\" title=\"Meine Freunde\">Meine Freunde</a>";

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
            var contactUrls = this.GetUrlList();

            foreach (string contactUrl in contactUrls)
            {
                result.Add(this.GetContactFromUrl(contactUrl));
            }
            
            result.Sort();
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
        /// Convert MeinVZ contact url to <see cref="StdContact"/>
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <returns> the downloaded information inserted into a <see cref="StdContact"/> </returns>
        private StdContact GetContactFromUrl(string contactUrl)
        {
            var result = new StdContact();

            const string ContactImageSelector = "src=\"(http://[-a-z0-9.]*imagevz.net/profile[-/a-z0-9]*.jpg)\" class=\"obj-profileImage\" id=\"profileImage\"";
            const string ContactContentSelector = "<dt>([a-zA-Z: ]*)</dt>.*?<dd>\\s*(.*?)\\s*</dd>";

            var content = this.httpRequester.GetContent(contactUrl);
            var imageUrl = Regex.Match(content, ContactImageSelector, RegexOptions.Singleline);
            if (imageUrl != null)
            {
                var url = imageUrl.Groups[1].ToString();
                result.PictureName = url.Substring(url.LastIndexOf('/') + 1);
                result.PictureData = this.httpRequester.GetContentBinary(url);
            }

            foreach (Match match in Regex.Matches(content, ContactContentSelector, RegexOptions.Singleline))
            {
                var key = match.Groups[1].ToString();
                var value = match.Groups[2].ToString();
                if (value.Contains(">"))
                {
                    value = value.Substring(value.IndexOf('>') + 1);
                }

                if (value.Contains("<"))
                {
                    value = value.Substring(0, value.IndexOf('<'));
                }

                result.InternalSyncData = new SyncData();

                switch (key)
                {
                    case "Name:":
                        result.Name = new PersonName(value);
                        break;

                    case "Mitglied seit:":
                        result.InternalSyncData.DateOfCreation = DateTime.Parse(value);
                        break;

                    case "Letztes Update:":
                        result.InternalSyncData.DateOfLastChange = DateTime.Parse(value);
                        break;

                    case "Geschlecht:":
                        result.PersonGender = value == "männlich" ? Gender.Male : Gender.Female;
                        break;

                    case "Geburtstag:":
                        result.DateOfBirth = DateTime.Parse(value);
                        break;

                    default:
                        Console.WriteLine("new content: " + key + " => " + value);
                        break;
                }
            }

            result.PersonalProfileIdentifiers.SetProfileId(ProfileIdentifierType.MeinVZ, contactUrl);

            return result;
        }

        /// <summary>
        /// Ready a list of vCard locations - this will also establish the logon
        /// </summary>
        /// <returns>a list of urls for the vCards to be downloaded</returns>
        private List<string> GetUrlList()
        {
            var result = new List<string>();

            while (true)
            {
                List<string> extractedData;
                this.httpRequester.LogonFormDetectionString = HttpDetectionStringLogonNeeded;

                // optimistically we try to read the content without explicit logon
                // this will succeed if we have a valid cookie
                if (this.httpRequester.GetExtract(string.Empty, ExtractorFriendUrls, out extractedData))
                {
                    if (this.httpRequester.GetExtract(extractedData[0], "<a href=\"(/Profile/[0-9a-z]*)\"", out result))
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(this.LogOnPassword))
                {
                    QueryForLogOnCredentials("needs some credentials");
                }

                var matches = Regex.Matches(this.httpRequester.LastExtractContent, ExtractorFormKey, RegexOptions.Singleline);
                var formKey = matches[0].Groups[1].Captures[0].ToString();

                matches = Regex.Matches(this.httpRequester.LastExtractContent, ExtractorIv, RegexOptions.Singleline);
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

                if (logInResponse.Contains(HttpDetectionStringLogonFailed))
                {
                    return result;
                }
            }

            return result;
        }
    }
}
