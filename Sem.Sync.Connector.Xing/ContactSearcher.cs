// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactSearcher.cs" company="Sven Erik Matzen">
//   (C) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ContactSearcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using Sem.GenericHelpers;

namespace Sem.Sync.Connector.Xing
{
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
        protected override System.Collections.Generic.List<StdElement> ReadFullList(string clientFolderName, System.Collections.Generic.List<StdElement> result)
        {
            var connector = new Memory.GenericClient();
            var listToScan = connector.GetAll(clientFolderName);

            foreach (StdContact element in listToScan)
            {
                foreach (var email in Tools.CombineNonEmpty(element.PersonalEmailPrimary, element.PersonalEmailSecondary, element.BusinessEmailPrimary, element.BusinessEmailSecondary))
                {
                    var searchResult = this.xingRequester.GetContent("");
                }
            }

            return result;
        }
    }
}
