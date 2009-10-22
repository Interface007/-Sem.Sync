

namespace Sem.Sync.Connector.LinkedIn
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using GenericHelpers;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    #endregion usings
    [ClientStoragePathDescription(Irrelevant = true)]
#if DEBUG
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "LinkedIn", MatchingIdentifier = ProfileIdentifierType.LinkedInId)]
#else
    [ConnectorDescription(CanReadContacts = false, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "LinkedIn", MatchingIdentifier = ProfileIdentifierType.LinkedInId)]
#endif
    public class ContactClient : WebScrapingBaseClient
    {
        /// <summary>
        /// relative URL to query contact links to contact pages
        /// </summary>
        private const string HttpUrlListContent = "/dwr/exec/ConnectionsBrowserService.getMyConnections.dwr";

        /// <summary>
        /// URL to query the profile
        /// </summary>
        private const string HttpUrlProfile = "/profile?goback=%2Econ&viewProfile=&key=46590581&jsstate=";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        public ContactClient()
            : base("http://www.linkedin.com/")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClient"/> class.
        /// </summary>
        /// <param name="httpUrlBaseAddress">
        /// The http url base address.
        /// </param>
        public ContactClient(string httpUrlBaseAddress)
            : base(httpUrlBaseAddress)
        {
        }

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "LinkedIn";
            }
        }

        /// <summary>
        /// Gets the detection string to parse the content of a request if we need to logon
        /// </summary>
        protected override string HttpDetectionStringLogOnNeeded
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the data string to be posted to logon into the site
        /// </summary>
        protected override string HttpDataLogOnRequest
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the regex to extract the form key for the log on
        /// </summary>
        protected override string ExtractorFormKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the regex to extract the iv for the log on
        /// </summary>
        protected override string ExtractorIv
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the regex to extract the iv for the log on
        /// </summary>
        protected override string ExtractorFriendUrls
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the regex to extract additional information
        /// </summary>
        protected override string ContactContentSelector
        {
            get
            {
                throw new NotImplementedException();
            }
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the base address to communicate with the site
        /// </summary>
        protected override string HttpUrlFriendList
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the relative url to log on
        /// </summary>
        protected override string HttpUrlLogOnRequest
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
