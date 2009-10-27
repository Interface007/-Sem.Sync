// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactClient type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

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

    /// <summary>
    /// This class implements access to the data stored inside the social network LinkedIn.com 
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
#if DEBUG
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "LinkedIn", MatchingIdentifier = ProfileIdentifierType.LinkedInId)]
#else
    [ConnectorDescription(CanReadContacts = true, CanWriteContacts = false, NeedsCredentials = true,
        DisplayName = "LinkedIn", MatchingIdentifier = ProfileIdentifierType.LinkedInId, Internal = true)]
#endif
    public class ContactClient : WebScrapingBaseClient
    {
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
        /// Gets the deterministin part of the placeholder url - the url to a placeholder must match this regex, while
        /// all others must not.
        /// </summary>
        protected override string ImagePlaceholderUrl
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the url pointing to the contact data to be downloaded - contains a single parameter {0} with the ID from the 
        /// profile ids for the contact to be processed.
        /// </summary>
        protected override string HttpUrlContactDownload
        {
            get { return "/profile?goback=%2Econ&viewProfile=&key=46590581&jsstate="; }
        }

        /// <summary>
        /// Gets the <see cref="WebScrapingBaseClient.ProfileIdentifierType"/> of this source.
        /// </summary>
        protected override ProfileIdentifierType ProfileIdentifierType
        {
            get { return ProfileIdentifierType.LinkedInId; }
        }

        /// <summary>
        /// Gets the detection string to parse the content of a request if we need to logon
        /// </summary>
        protected override string HttpDetectionStringLogOnNeeded
        {
            get
            {
                return "login";
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
        /// Gets the regex to extract the picture url from the profile content
        /// </summary>
        protected override string ExtractorProfilePictureUrl
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
            get { return "/dwr/exec/ConnectionsBrowserService.getMyConnections.dwr"; }
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

        /// <summary>
        /// Converts downloaded data into a StdContact structure.
        /// </summary>
        /// <param name="contactUrl"> The contact url. </param>
        /// <param name="content"> The content. </param>
        /// <returns> a new StdClient created from the data provided </returns>
        protected override StdContact ConvertToStdContact(string contactUrl, string content)
        {
            throw new NotImplementedException();
        }
    }
}
