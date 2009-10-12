// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebScrapingClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Facebook
{
    using System;

    using SyncBase.Attributes;
    using SyncBase.DetailData;

    /// <summary>
    /// WebScaping implementation of a FaceBook StdClient
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
#if DEBUG
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "Facebook (WS)", MatchingIdentifier = ProfileIdentifierType.FacebookProfileId)]
#else
    [ConnectorDescription(CanReadContacts = false, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "Facebook (WS)", MatchingIdentifier = ProfileIdentifierType.FacebookProfileId)]
#endif
    public class WebScrapingClient : WebScrapingBaseClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebScrapingClient"/> class.
        /// </summary>
        public WebScrapingClient()
            : base("http://www.facebook.com/")
        {
        }

        protected override string HttpDetectionStringLogOnNeeded
        {
            get { return @"action=""https://login.facebook.com/login.php?login_attempt=1"" id=""login_form"""; }
        }

        protected override string HttpDataLogOnRequest
        {
            get
            {
                return "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&locale=de_DE&login_ab_group=0&email={0}&pass={1}&pass_placeholder=Passwort&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=WTu8R";
            }
        }

        protected override string ExtractorFormKey
        {
            get { return string.Empty; }
        }

        protected override string ExtractorIv
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override string ExtractorFriendUrls
        {
            get { return @"\{""t"":""(?<name>[^""]+?)"",""i"":(?<id>\d*),""ty"":""u"".*?}"; }
        }

        protected override string ContactContentSelector
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Gets the extraction string for the image.
        /// </summary>
        protected override string ContactImageSelector
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the detection string to detect if we did fail to logon
        /// </summary>
        protected override string HttpDetectionStringLogOnFailed
        {
            get
            {
                return "no such string available";
            }
        }

        /// <summary>
        /// Gets the base address to communicate with the site
        /// </summary>
        protected override string HttpUrlFriendList
        {
            get
            {
                return "/friends/ajax/superfriends.php?filter=afp&ref=tn&offset={0}&__a=1 ";
            }
        }

        /// <summary>
        /// Gets or sets the relative url to log on
        /// </summary>
        protected override string HttpUrlLogOnRequest
        {
            get
            {
                return "https://login.facebook.com/login.php?login_attempt=1";
            }
        }

        public override string FriendlyClientName
        {
            get
            {
                return "Facebook-WebScaping";
            }
        }
    }
}
