// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactAccess.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactAccess type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;

    using Sdx.Sync.Connector.OracleCrmOnDemand.AccountSR;
    using Sdx.Sync.Connector.OracleCrmOnDemand.ActivitySR;
    using Sdx.Sync.Connector.OracleCrmOnDemand.ContactSR;
    using Sdx.Sync.Connector.OracleCrmOnDemand.Helpers;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using queryType = Sdx.Sync.Connector.OracleCrmOnDemand.ContactSR.queryType;

    /// <summary>
    /// This class implements access to the web services including the authentication handling.
    /// </summary>
    internal class ContactAccess : SyncComponent
    {
        #region constants

        /// <summary>
        /// This is the number of contacts the web service can handle per request. The documentations
        /// stats "The maximum number of objects that can be sent in a single SOAP request is 20." but 
        /// to be we had problems with more than 10 per request.
        /// </summary>
        private const int MaximumNumberOfContactsTheWsCanHandle = 10;

        /// <summary>
        /// The filter name prefix for account filters
        /// </summary>
        private const string PrefixAccount = "Account.";

        /// <summary>
        /// The filter name prefix for contact filters
        /// </summary>
        private const string PrefixContact = "Contact.";

        /// <summary>
        /// The filter name prefix for contact filters
        /// </summary>
        private const string PrefixActivity = "Activity.";

        #endregion

        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAccess"/> class. 
        /// </summary>
        public ContactAccess()
        {
            this.Configuration.PageSize = 100;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAccess"/> class.
        /// </summary>
        /// <param name="serverName"> The name of the CRM server to use. </param>
        /// <param name="configurationData"> The configuration data structure with extended configuration data. </param>
        public ContactAccess(string serverName, ContactClientConfigurationData configurationData)
        {
            this.ServerName = serverName;
            this.Configuration = configurationData;
            if (this.Configuration.PageSize == 0)
            {
                this.Configuration.PageSize = 100;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the SessionId after log on.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the ServerName. The server name is configures on a per Oracle CRM customer.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets or sets extended configuration data.
        /// </summary>
        public ContactClientConfigurationData Configuration { get; set; }

        #endregion

        /// <summary>
        /// Gets a list of contacts from the Oracle CRM on Demand web service according to the <paramref name="filterList"/>.
        /// </summary>
        /// <param name="filterList"> The filter list. This includes information about the fields to be filtered. </param>
        /// <returns> a list of Oracle CRM contacts </returns>
        internal IEnumerable<ContactData> QueryContactsByFilter(IEnumerable<KeyValuePair> filterList)
        {
            if (filterList == null)
            {
                throw new ArgumentNullException("filterList");
            }

            var accounts = this.QueryAccountsByFilter(
                from x in filterList
                where x.Key.StartsWith(PrefixAccount, StringComparison.OrdinalIgnoreCase)
                select x);

            var activities = this.QueryActivityByFilter(
                from x in filterList
                where x.Key.StartsWith(PrefixActivity, StringComparison.OrdinalIgnoreCase)
                select x);

            var contactClient = this.GetClient<ContactSR.ContactClient>();
            var numAccounts = accounts.Count();
            var resultList = new List<ContactData>();

            this.LogProcessingEvent("preparing filters");
            var filterContact = GetFilterEntity<ContactQuery>(filterList);

            foreach (var account in accounts)
            {
                var currentAccountCaption = string.IsNullOrWhiteSpace(account) ? "'all'" : account;
                this.LogProcessingEvent("processing account name {0}...", currentAccountCaption);
                using (new OperationContextScope(contactClient.InnerChannel))
                {
                    Utils.ModifyOperationContext(this.SessionId);

                    var result = new List<ContactData>();
                    var rowsFound = this.Configuration.PageSize;
                    var rowsStart = 0;

                    // apply current account name filter - if we don't have an account filter, the filterContact is
                    // string.Empty, so we do not change the filterContact by calling => no "if" needed
                    filterContact.AccountName = new queryType { Value = Utils.GetLikeQuery(account) };
                    Tools.SetPropertyValue(filterContact, "AccountName", Utils.GetLikeQuery(account));

                    try
                    {
                        while (rowsFound == this.Configuration.PageSize)
                        {
                            OraclePause();
                            var rows = contactClient.ContactQueryPage(
                                new ContactQueryPage_Input
                                    {
                                        ListOfContact = new ListOfContactQuery
                                            {
                                                startrownum = rowsStart.ToString(CultureInfo.InvariantCulture),
                                                pagesize = this.Configuration.PageSize.ToString(CultureInfo.InvariantCulture),
                                                Contact = filterContact
                                            }
                                    }).ListOfContact;
                            
                            rowsFound = 0;
                            if (rows.Contact != null)
                            {
                                rowsFound = rows.Contact.Length;
                                rowsStart += rowsFound;
                                result.AddRange(rows.Contact);
                            }

                            if (rowsFound != this.Configuration.PageSize)
                            {
                                break;                                
                            }

                            this.LogProcessingEvent("{0} rows added for {1} ...", result.Count, currentAccountCaption);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetType() == typeof(ProcessAbortException))
                        {
                            throw;
                        }

                        this.LogProcessingEvent("an exception has been recorded while reading data: " + ex.Message);
                        resultList.AddRange(result);
                    }

                    resultList.AddRange(result);
                }
            }

            return resultList;
        }

        private IEnumerable<string> QueryActivityByFilter(IEnumerable<KeyValuePair> filterList)
        {
            if (filterList == null || filterList.Count() == 0)
            {
                return new List<string> { string.Empty };
            }

            var activityClient = this.GetClient<ActivitySR.Default_Binding_ActivityNWSClient>();
            using (new OperationContextScope(activityClient.InnerChannel))
            {
                Utils.ModifyOperationContext(this.SessionId);

                var result = new List<Activity>();
                var pageSize = this.Configuration.PageSize;
                var rowsFound = pageSize;
                var rowsStart = 0;

                this.LogProcessingEvent("preparing filters Activity");

                var filterActivity = GetFilterEntity<Activity>(filterList);

                try
                {
                    while (rowsFound == pageSize)
                    {
                        this.LogProcessingEvent("reading Activity data with page size {0} rows...", pageSize);

                        var rows = activityClient.Activity_QueryPage(new ActivityNWS_Activity_QueryPage_Input
                            {
                                StartRowNum = rowsStart.ToString(CultureInfo.InvariantCulture),
                                PageSize = pageSize.ToString(CultureInfo.InvariantCulture),
                                ListOfActivity = new[]
                                    {
                                        filterActivity, 
                                    }
                            }).ListOfActivity;

                        if (rows != null)
                        {
                            rowsFound = rows.Length;
                            rowsStart += rowsFound;
                            result.AddRange(rows);
                        }
                        else
                        {
                            rowsFound = 0;
                        }

                        this.LogProcessingEvent("{0} Activity rows added - {1} Activity rows downloaded ...", rowsFound, result.Count);
                    }

                    this.LogProcessingEvent("read Activity operations finished");
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ProcessAbortException))
                    {
                        throw;
                    }

                    this.LogProcessingEvent("an exception has been recorded while reading Activity data: " + ex.Message);
                }

                return from activityList in result
                       from activity in activityList.ListOfContact
                       select activity.ContactId;
            }
        }

        /// <summary>
        /// Updates the contact information for the contacts of the <paramref name="contactList"/>
        /// </summary>
        /// <param name="contactList"> The list of contacts to be updated. </param>
        /// <returns> true in case of a successfull update </returns>
        internal bool UpdateContacts(IEnumerable<ContactData> contactList)
        {
            if (contactList == null || contactList.Count() == 0)
            {
                return false;
            }

            var numContactsForUpdate = contactList.Count();
            var contactClient = this.GetClient<ContactSR.ContactClient>();

            using (new OperationContextScope(contactClient.InnerChannel))
            {
                Utils.ModifyOperationContext(this.SessionId);

                var numUpdatedContacts = 0;

                // the web-service cannot handle an arbitrary number of contacts per request so you have
                // to divide the full list into smaller pieces (with a number of this.PageSize contacts)
                ContactData[] chunkOfContacts;

                // as long as there are contacts we have not sent to the service for update...
                // skip those contacts that were already sent and take the next chunk
                while (
                    (chunkOfContacts =
                        contactList.Skip(numUpdatedContacts)
                        .Take(MaximumNumberOfContactsTheWsCanHandle)
                        .ToArray())
                    .Length > 0)
                {
                    this.LogProcessingEvent("sending {0} contacts for update ...", numContactsForUpdate);

                    OraclePause();
                    try
                    {
                        // fire the update request and collect the response from the service
                        var response = contactClient.ContactUpdate(
                            new ContactUpdate_Input { ListOfContact = new ListOfContactData { Contact = chunkOfContacts } });

                        // if no contacts were updated the response is empty (response.ListOfContact.Contact == null)
                        // otherwise it contains the contacts that were updated by the service
                        numUpdatedContacts += response.ListOfContact.Contact == null ? 0 : response.ListOfContact.Contact.Length;
                    }
                    catch (Exception ex)
                    {
                        this.LogException(new TechnicalException("web-service exception", ex, new KeyValuePair<string, object>("contacts", chunkOfContacts)));
                        return false;
                    }

                    this.LogProcessingEvent("{0} of {1} contacts updated ...", numUpdatedContacts, numContactsForUpdate);
                }

                this.LogProcessingEvent("update operations finished");

                // different number of contacts between query/result?
                return numUpdatedContacts == numContactsForUpdate;
            }
        }

        /// <summary>
        /// Logs off from the server - this will invalidate the session token.
        /// </summary>
        /// <exception cref="WebException"> in case of a problem logging off - in this case the validity of 
        /// the session id is undefined. </exception>
        internal void LogOff()
        {
            this.LogProcessingEvent("logging off ...");

            var logoffUrlString = new Uri(string.Format(CultureInfo.InstalledUICulture, "https://{0}/Services/Integration?command=logoff", this.ServerName));
            var request = (HttpWebRequest)WebRequest.Create(logoffUrlString);
            request.CookieContainer = new CookieContainer(1);
            request.CookieContainer.Add(new Cookie("JSESSIONID", this.SessionId, "/Services/Integration", this.ServerName));

            // make the HTTP call
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Logout failed");
            }

            this.LogProcessingEvent("log of finished");
        }

        /// <summary>
        /// Performs a log on request and stored the session id into the property <see cref="SessionId"/>.
        /// </summary>
        /// <param name="userName"> The user name. </param>
        /// <param name="password"> The password. </param>
        /// <returns> true in case of a successfull log on </returns>
        internal bool LogOn(string userName, string password)
        {
            this.LogProcessingEvent("log on started ...");

            try
            {
                var uri = new Uri(string.Format(CultureInfo.InvariantCulture, "https://{0}/Services/Integration?command=login&isEncoded=Y", this.ServerName));

                // create a http request and set the headers for authentication
                var myRequest = (HttpWebRequest)WebRequest.Create(uri);
                myRequest.Method = "POST";

                // passing username and password in the http header
                // username format if it includes slash should be the forward slash /
                myRequest.Headers["UserName"] = userName.Replace("\\", "/");
                myRequest.Headers["Password"] = password;

                using (var myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    // todo: why do we get the response stream here?
                    using (var sr = myResponse.GetResponseStream())
                    {
                        char[] sep = { ';' };

                        var headers = myResponse.Headers["Set-Cookie"].Split(sep);
                        for (var i = 0; i <= headers.Length - 1; i++)
                        {
                            if (!headers[i].StartsWith("JSESSIONID", StringComparison.Ordinal))
                            {
                                continue;
                            }

                            sep[0] = '=';
                            this.SessionId = headers[i].Split(sep)[1];
                            break;
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                this.LogProcessingEvent("exception while log on process: " + webException.Message);
                return false;
            }

            // send back the session id that should be passed 
            // to subsequent calls to webservices
            this.LogProcessingEvent("log on succeeded");
            return true;
        }

        /// <summary>
        /// Ocare limits the number of requests per second the client can do. We need to 
        /// check if this limit has exceeded and wait some time.
        /// </summary>
        private static void OraclePause()
        {
            Thread.Sleep(60);
        }

        /// <summary>
        /// Create a new filter entity instance and fills the properties according to the
        /// provided filter list (<paramref name="filterList"/>).
        /// </summary>
        /// <param name="filterList"> The list with filter entries. </param>
        /// <typeparam name="TEntity"> The type of filter entity to be created. </typeparam>
        /// <returns> The new filter entity with the filter properties set. </returns>
        private TEntity GetFilterEntity<TEntity>(IEnumerable<KeyValuePair> filterList) where TEntity : new()
        {
            var filterEntity = new TEntity();

            // according to the GetAllAttributes configuration value set
            // all properties or only the predefined to string.Empty
            (this.Configuration.GetAllAttributes
                ? Tools.GetPropertyList(string.Empty, typeof(TEntity))
                : Constants.PropertiesToQueryForContacts)
            .ForEach(x => Tools.SetPropertyValue(filterEntity, x, "{emptystring}"));

            // now set the filter values
            var entityFilterPrefix =
                typeof(TEntity) == typeof(ContactQuery)
                ? PrefixContact 
                : PrefixAccount;

            filterList.ForEach(x => Tools.SetPropertyValue(filterEntity, x.Key.Substring(entityFilterPrefix.Length), x.Value));

            // entity prepared - return it
            return filterEntity;
        }

        /// <summary>
        /// Prepares a web service client of the type <typeparamref name="TClient"/> with the correct
        /// server address and binding.
        /// </summary>
        /// <typeparam name="TClient">the type of the client</typeparam>
        /// <returns>the new client instance</returns>
        private TClient GetClient<TClient>() where TClient : class
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport) { MaxBufferSize = 6553600, MaxReceivedMessageSize = 6553600 };
            var address = new EndpointAddress(string.Format(CultureInfo.InvariantCulture, "https://{0}/Services/Integration", this.ServerName));

            // disable automatic cookie handling - this enables us to explicitly set cookies
            binding.AllowCookies = false;
            TClient result = null;

            if (typeof(TClient) == typeof(ContactSR.ContactClient))
            {
                result = new ContactSR.ContactClient(binding, address) as TClient;
            }

            if (typeof(TClient) == typeof(AccountClient))
            {
                result = new AccountClient(binding, address) as TClient;
            }

            if (typeof(TClient) == typeof(ActivitySR.Default_Binding_ActivityNWSClient))
            {
                result = new ActivitySR.Default_Binding_ActivityNWSClient(binding, address) as TClient;
            }

            return result;
        }

        /// <summary>
        /// Queries the accounts that do have the "CustomBoolean14" (is MAP account) set.
        /// </summary>
        /// <param name="filterList">filter list for accounts</param>
        /// <returns>A list of accounts with the boolean set.</returns>
        private IEnumerable<string> QueryAccountsByFilter(IEnumerable<KeyValuePair> filterList)
        {
            if (filterList == null || filterList.Count() == 0)
            {
                return new List<string> { string.Empty };
            }

            var accountClient = this.GetClient<AccountClient>();
            using (new OperationContextScope(accountClient.InnerChannel))
            {
                Utils.ModifyOperationContext(this.SessionId);

                var result = new List<AccountData>();
                var pageSize = this.Configuration.PageSize;
                var rowsFound = pageSize;
                var rowsStart = 0;

                this.LogProcessingEvent("preparing filters Account");

                var filterAccount = GetFilterEntity<AccountQuery>(filterList);

                try
                {
                    while (rowsFound == pageSize)
                    {
                        this.LogProcessingEvent("reading Account data with page size {0} rows...", pageSize);

                        var rows = accountClient.AccountQueryPage(
                            new AccountQueryPage_Input
                                {
                                    ListOfAccount = new ListOfAccountQuery
                                        {
                                            startrownum = rowsStart.ToString(CultureInfo.InvariantCulture), 
                                            pagesize = pageSize.ToString(CultureInfo.InvariantCulture), 
                                            Account = filterAccount
                                        }
                                }).ListOfAccount;

                        if (rows.Account != null)
                        {
                            rowsFound = rows.Account.Length;
                            rowsStart += rowsFound;
                            result.AddRange(rows.Account);
                        }
                        else
                        {
                            rowsFound = 0;
                        }

                        this.LogProcessingEvent("{0} Account rows added - {1} Account rows downloaded ...", rowsFound, result.Count);
                    }

                    this.LogProcessingEvent("read Account operations finished");
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ProcessAbortException))
                    {
                        throw;
                    }

                    this.LogProcessingEvent("an exception has been recorded while reading Account data: " + ex.Message);
                }

                return from account in result where !string.IsNullOrEmpty(account.AccountName) select account.AccountName;
            }
        }
    }
}