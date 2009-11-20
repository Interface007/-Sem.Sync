// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactSearcher.cs" company="Sven Erik Matzen">
//   (C) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ContactSearcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Xing
{
    using System.Collections.Generic;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;

    /// <summary>
    /// performs lookups for contacts in Xing based on a list of contacts stored inside the memory connector
    /// </summary>
    public class ContactSearcher : StdClient
    {
        /// <summary>
        /// Base address to communicate with Xing
        /// </summary>
        private const string HttpUrlBaseAddress = "https://www.xing.com";

        private readonly HttpHelper xingRequester;

        public ContactSearcher()
        {
            this.xingRequester = new HttpHelper(HttpUrlBaseAddress, true)
            {
                UseCache = this.GetConfigValueBoolean("UseCache"),
                SkipNotCached = this.GetConfigValueBoolean("SkipNotCached"),
                UseIeCookies = this.GetConfigValueBoolean("UseIeCookies"),
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the memory connector. </param>
        /// <param name="result"> The result list of contacts. </param>
        /// <returns> the search results </returns>
        protected override System.Collections.Generic.List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var connector = new Memory.GenericClient();
            var listToScan = connector.GetAll(clientFolderName);
            xingRequester.UiDispatcher = this.UiDispatcher;

            foreach (StdContact element in listToScan)
            {
                var eMailAddresses = Tools.CombineNonEmpty(element.PersonalEmailPrimary, element.PersonalEmailSecondary, element.BusinessEmailPrimary, element.BusinessEmailSecondary);
                var query = string.Format("http://www.google.de/search?q=site%3Awww.xing.com+{0}&btnG=Suche&meta=&aq=f&oq=", HttpHelper.EncodeForPost(element.Name.ToString()));
                var searchResult = this.xingRequester.GetContent(query);

                var matches = System.Text.RegularExpressions.Regex.Matches(searchResult, "<a href=\"(http://www.xing.com/profile/.*?)\" class=l onmousedown");

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    var publicProfile = this.xingRequester.GetContent(match.Groups[1].ToString());
                }
                
                foreach (var email in eMailAddresses)
                {
                    
                }
            }

            return result;
        }
    }
}
