// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.GoogleClient
{
    #region usings

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    using Google.GData.Contacts;
    using Google.GData.Extensions;

    using SyncBase;
    using SyncBase.Attributes;
    using SyncBase.DetailData;

    #endregion usings

    /// <summary>
    /// This class is the client class for handling contacts persisted to the file system
    /// </summary>
    [ClientStoragePathDescription(Irrelevant = true)]
    [ConnectorDescription(
        DisplayName = "Google Mail Contacts Client",
        CanRead = true,
        CanWrite = false,
        MatchingIdentifier = ProfileIdentifierType.Google,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// defines the uri to the contacts feed. this need to be processed to contain the google user id
        /// </summary>
        private const string FeedUri = "http://www.google.com/m8/feeds/contacts/{0}/base";

        /// <summary>
        /// Gets the user readable name of the client implementation. This name should
        /// be specific enough to let the user know what element store will be accessed.
        /// </summary>
        public override string FriendlyClientName
        {
            get
            {
                return "Google-Contact-Client";
            }
        }

        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">the full name including path of the file that does contain the contacts.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            var service = new ContactsService("Sem.Sync.GoogleClient");
            var userName = this.LogOnUserId;
            var passWord = this.LogOnPassword;

            if (!string.IsNullOrEmpty(userName))
            {
                service.setUserCredentials(userName, passWord);
            }

            var query = new ContactsQuery
                {
                    Uri =
                        new Uri(
                        string.Format(FeedUri, HttpUtility.UrlEncode(userName, Encoding.GetEncoding("iso8859-1"))))
                };

            var contactFeed = service.Query(query);

            foreach (ContactEntry entry in contactFeed.Entries)
            {
                var stdEntry = new StdContact
                    {
                        Name = new PersonName(entry.Title.Text)
                    };

                if (entry.PostalAddresses.Count > 0)
                {
                    foreach (var address in entry.PostalAddresses)
                    {
                        if (address.Home)
                        {
                            var parts = address.Value.Split('\n');
                            stdEntry.PersonalAddressPrimary = new AddressDetail();
                            if (parts.Length == 2)
                            {
                                stdEntry.PersonalAddressPrimary.StreetName = parts[0];
                                stdEntry.PersonalAddressPrimary.CityName = parts[1];
                            }
                        }
                    }

                    foreach (var extensionElement in entry.ExtensionElements)
                    {
                        switch (extensionElement.XmlName)
                        {
                            case "organization":
                                var value = extensionElement as Organization;
                                if (value != null)
                                {
                                    stdEntry.BusinessPosition = value.Title;
                                    stdEntry.BusinessCompanyName = value.Name;
                                }

                                break;

                            case "email":
                                break;

                            case "phoneNumber":
                                break;

                            case "postalAddress":
                                break;

                            default:
                                Console.WriteLine(extensionElement.XmlName + " - " + extensionElement);
                                break;
                        }
                    }
                }

                result.Add(stdEntry);
            }

            return result;
        }

        /// <summary>
        /// Overrides the method to write the full list of data.
        /// </summary>
        /// <param name="elements"> The elements to be exported. </param>
        /// <param name="clientFolderName">the full name including path of the file that will get the contacts while exporting data.</param>
        /// <param name="skipIfExisting">this value is not used in this client.</param>
        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
        }
    }
}
