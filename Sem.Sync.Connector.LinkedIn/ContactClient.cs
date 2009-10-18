

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
    public class ContactClient : StdClient, IExtendedReader
    {
        /// <summary>
        /// Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "http://www.linkedin.com/";

        /// <summary>
        /// relative URL to query contact links to contact pages
        /// </summary>
        private const string HttpUrlListContent = "/dwr/exec/ConnectionsBrowserService.getMyConnections.dwr";

        /// <summary>
        /// URL to query the profile
        /// </summary>
        private const string HttpUrlProfile = "/profile?goback=%2Econ&viewProfile=&key=46590581&jsstate=";

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public StdElement FillContacts(StdElement contactToFill, List<MatchingEntry> baseline)
        {
            throw new NotImplementedException();
        }
    }
}
