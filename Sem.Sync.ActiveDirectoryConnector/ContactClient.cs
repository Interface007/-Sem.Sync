namespace Sem.Sync.ActiveDirectoryConnector
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.IO;
    using System.Text;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

    public class ContactClient : StdClient
    {
        private const string RegBasePath = "software\\Sem.Sync.ActiveDirectoryConnector";
        private string DumpPath { get; set;}

        public ContactClient()
        {
            this.DumpPath = this.GetConfigValue("DumpPath");
        }

        private static string GetPropString(ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0 && thePropertyCollection[propName].Count > 0)
                return thePropertyCollection[propName][0].ToString();

            return null;
        }

        private static DateTime GetPropDate(ResultPropertyCollection thePropertyCollection, string propName)
        {
            if (thePropertyCollection != null && thePropertyCollection.Count > 0 && thePropertyCollection[propName].Count > 0)
                return (DateTime)thePropertyCollection[propName][0];

            return new DateTime();
        }
        
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            try
            {
                // default from registry (if there is something, we will use it)
                var domainName = SyncTools.GetRegValue(RegBasePath, "domainName", "");
                this.LoginUserId = SyncTools.GetRegValue(RegBasePath, "username", "{default}");
                this.LoginPassword = SyncTools.GetRegValue(RegBasePath, "password", "");

                // check if the user an empty string, in this case ask the user for credentials
                if (string.IsNullOrEmpty(this.LoginUserId) || this.LoginPassword == "{ask}")
                {
                    this.LoginPassword = "";
                    this.QueryForLogOnCredentials("Please provide login credentials for LDAP query.\nPress cancle to use current user.");
                }

                // if we have a user name and it does contain a backslash, we need to split the domain from the user name
                if (!string.IsNullOrEmpty(this.LoginUserId) && this.LoginUserId.Contains("\\"))
                {
                    domainName = this.LoginUserId.Split('\\')[0];
                    this.LoginUserId = this.LoginUserId.Split('\\')[1];
                }

                // if the user id is {default} we need to get the domain from the currently logged in user
                if (string.IsNullOrEmpty(this.LoginUserId) || this.LoginUserId == "{default}")
                {
                    var currentIdentity = System.Security.Principal.WindowsIdentity.GetCurrent();
                    if (currentIdentity != null)
                        domainName = currentIdentity.Name;

                    if (!string.IsNullOrEmpty(domainName) && domainName.Contains("\\"))
                        domainName = domainName.Split('\\')[0];
                }

                // open the directory using explicit or implicit credentials
                var entry =
                    (string.IsNullOrEmpty(this.LoginPassword))
                    ? new DirectoryEntry("LDAP://" + domainName)
                    : new DirectoryEntry("LDAP://" + domainName, this.LoginUserId, this.LoginPassword);

                var search = new DirectorySearcher(entry)
                                               {
                                                   Filter = clientFolderName
                                               };

                var resultList = search.FindAll();

                foreach (SearchResult searchItem in resultList)
                {
                    var newContact = new StdContact
                                         {
                                             Id = Guid.NewGuid(),
                                             InternalSyncData = new SyncData
                                                                    {
                                                                        DateOfCreation = GetPropDate(searchItem.Properties, "whencreated"),
                                                                        DateOfLastChange = GetPropDate(searchItem.Properties, "whenchanged"),
                                                                    },
                                             BusinessAddressPrimary = new AddressDetail
                                                                          {
                                                                              CountryName = GetPropString(searchItem.Properties, "co"),
                                                                              StateName = GetPropString(searchItem.Properties, "st"),
                                                                              PostalCode = GetPropString(searchItem.Properties, "postalcode"),
                                                                              CityName = GetPropString(searchItem.Properties, "l"),
                                                                              StreetName = GetPropString(searchItem.Properties, "streetaddress"),
                                                                              Phone = new PhoneNumber(GetPropString(searchItem.Properties, "telephonenumber")),
                                                                              Room = GetPropString(searchItem.Properties, "roomnumber"),
                                                                          },
                                             BusinessPhoneMobile = new PhoneNumber(GetPropString(searchItem.Properties, "mobile")),
                                             BusinessPosition = GetPropString(searchItem.Properties, "title"),
                                             BusinessCompanyName = GetPropString(searchItem.Properties, "company"),
                                             BusinessDepartment = GetPropString(searchItem.Properties, "department"),
                                             BusinessEmailPrimary = GetPropString(searchItem.Properties, "mail"),

                                             PersonalAddressPrimary = new AddressDetail
                                                                          {
                                                                              Phone = new PhoneNumber(GetPropString(searchItem.Properties, "homephone")),
                                                                          },
                                             Name = new PersonName
                                                        {
                                                            FirstName = GetPropString(searchItem.Properties, "givenname"),
                                                            LastName = GetPropString(searchItem.Properties, "sn"),

                                                        },

                                             PersonGender =
                                                 SyncTools.GenderByText(GetPropString(searchItem.Properties, "personaltitle")),

                                             AdditionalTextData = GetPropString(searchItem.Properties, "info"),
                                         };

                    if (!string.IsNullOrEmpty(newContact.ToStringSimple()))
                    {
                        if (!string.IsNullOrEmpty(this.DumpPath))
                        {
                            DumpUserInformation(searchItem,
                                                Path.Combine(this.DumpPath,
                                                             SyncTools.NormalizeFileName(newContact.ToStringSimple())));
                        }

                        result.Add(newContact);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogProcessingEvent(ex.Message);
            }
            return result;
        }

        private static void DumpUserInformation(SearchResult searchItem, string path)
        {
            SyncTools.EnsurePathExist(Path.GetDirectoryName(path));

            var content = new StringBuilder();

            foreach (var name in searchItem.Properties.PropertyNames)
            {
                foreach (var propItem in searchItem.Properties[name.ToString()])
                {
                    content.AppendLine(name + " ... " + propItem);
                }
            }

            File.WriteAllText(path, content.ToString());
        }

        protected override void WriteFullList(List<StdElement> elements, string clientFolderName, bool skipIfExisting)
        {
            throw new NotImplementedException();
        }

        public override string FriendlyClientName
        {
            get { return "Active-Directory-Connector"; }
        }
    }
}
