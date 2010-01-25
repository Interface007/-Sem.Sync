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

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class implements access to the data stored inside the social network LinkedIn.com 
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
#if DEBUG
    [ConnectorDescription(
        CanReadContacts = true, 
        CanWriteContacts = false, 
        NeedsCredentials = true,
        NeedsCredentialsDomain = false,
        DisplayName = "LinkedIn", 
        MatchingIdentifier = ProfileIdentifierType.LinkedInId)]
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
        /// Gets the WebSideParameters which defines how to deal with the site.
        /// </summary>
        protected override WebSideParameters WebSideParameters
        {
            get
            {
                return new WebSideParameters
                    {
                        HttpUrlFriendList = "/dwr/exec/ConnectionsBrowserService.getMyConnections.dwr",
                        HttpDetectionStringLogOnNeeded = "login",
                        ProfileIdentifierType = ProfileIdentifierType.LinkedInId,
                        HttpUrlContactDownload = "/profile?goback=%2Econ&viewProfile=&key=46590581&jsstate=",
                    };
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
