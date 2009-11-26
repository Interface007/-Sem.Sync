// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactSearcher.cs" company="Sven Erik Matzen">
//   (C) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ContactSearcher type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Sem.Sync.Connector.Xing
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase;

    using SyncBase.DetailData;

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
        /// <para>Reads a list of potential contact matches from Xing. Because of limited information exposed to the
        /// public profile, this information can only be used to identify matching profiles - you will have to 
        /// add the matching profiles to your contact list in order to get more information.</para>
        /// </summary>
        /// <param name="clientFolderName"> The client folder name for the memory connector. </param>
        /// <param name="result"> The result list of contacts. </param>
        /// <returns> the search results </returns>
        protected override System.Collections.Generic.List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var connector = new Memory.GenericClient();
            var listToScan = connector.GetAll(clientFolderName);
            this.xingRequester.UiDispatcher = this.UiDispatcher;

            result = new List<StdElement>();

            foreach (StdContact element in listToScan)
            {
                if (!string.IsNullOrEmpty(element.PersonalProfileIdentifiers.XingNameProfileId))
                {
                    continue;
                }

                var profileIds = GuessProfileIds(element);

                foreach (var publicProfileId in profileIds)
                {
                    for (var i = 0; i < 200; i++)
                    {
                        var publicProfile = this.xingRequester.GetContent(publicProfileId + (i == 0 ? string.Empty : i.ToString()));
                        
                        // todo: check for nonexisting profile
                        if (string.IsNullOrEmpty(publicProfile))
                        {
                            break;
                        }

                        var informationName = Regex.Matches(publicProfile, "\\<h1 class=\"name\"\\>.(?<name>[^<]*)\\<", RegexOptions.Singleline);
                        var informationZip = Regex.Matches(publicProfile, "zip_code=\\%22(?<zip>[^%]*)\\%22", RegexOptions.Singleline);
                        var informationBusinessPosition = Regex.Matches(publicProfile, "\\<p class=\"profile-work-descr\"\\>(\\<[^>]*>)*(?<bustitle>[^<]*)\\</", RegexOptions.Singleline);

                        var newContact = new StdContact();
                        newContact.Name = new PersonName(informationName[0].Groups["name"].ToString());
                        newContact.BusinessPosition = informationBusinessPosition[0].Groups["bustitle"].ToString();
                        newContact.BusinessAddressPrimary.PostalCode = informationZip[0].Groups["zip"].ToString();

                        result.Add(newContact);
                    }
                }
                break;
            }

            return result;
        }

        private static List<string> GuessProfileIds(StdContact contact)
        {
            var result = new List<string>();
            if (contact.Name == null
                || string.IsNullOrEmpty(contact.Name.FirstName)
                || string.IsNullOrEmpty(contact.Name.LastName))
            {
                return result;
            }

            const string Xing = "http://www.xing.com/profile/";

            result.Add(Xing + string.Format("{0}_{1}", contact.Name.FirstName, contact.Name.LastName));

            if (string.IsNullOrEmpty(contact.Name.MiddleName))
            {
                return result;
            }

            result.Add(Xing + string.Format("{0}{1}_{2}", contact.Name.FirstName, contact.Name.MiddleName, contact.Name.LastName));
            result.Add(Xing + string.Format("{0}_{1}{2}", contact.Name.FirstName, contact.Name.MiddleName, contact.Name.LastName));

            return result;
        }
    }
}
