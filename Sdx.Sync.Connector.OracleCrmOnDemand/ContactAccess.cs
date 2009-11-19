// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactAccess.cs" company="SDX-AG">
//   (c) 2009 by SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the ContactAccess type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Reflection;

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Exceptions;
    using Sem.Sync.SyncBase;
    using System.Globalization;
    using System.Configuration;

    /// <summary>
    /// This class implements access to the web services including the authentication handling.
    /// </summary>
    public class ContactAccess : SyncComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAccess"/> class. Initializes the
        /// <see cref="PageSize"/> to 100 rows per request.
        /// </summary>
        public ContactAccess()
        {
            this.PageSize = 100;
        }

        /// <summary>
        /// Sets a value indicating whether to ignore SSL certificate errors. Set this property to "true" if you want to be
        /// able to debug the SSL traffic. Also you might need to set this property to "true" in case of problems with the 
        /// SSL certificate of the web service server.
        /// </summary>
        public static bool IgnoreCertificateError
        {
            set
            {
                if (value)
                {
                    ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallbackAlwaysTrue;
                }
                else
                {
                    ServicePointManager.ServerCertificateValidationCallback -= RemoteCertificateValidationCallbackAlwaysTrue;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of rows allowed to be returned in one web response.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the SessionId after log on.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the ServerName. The server name is configures on a per Oracle CRM customer.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Gets a list of contacts from the Oracle CRM on Demand web service according to the <paramref name="filterList"/>.
        /// </summary>
        /// <param name="filterList"> The filter list. This includes information about the fields to be filtered. </param>
        /// <returns> a list of Oracle CRM contacts </returns>
        public IEnumerable<Contact> QueryContactsByFilter(IEnumerable<KeyValuePair<string, string>> filterList)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport) { MaxBufferSize = 6553600, MaxReceivedMessageSize = 6553600 };
            var address = new EndpointAddress(string.Format(CultureInfo.InvariantCulture, "https://{0}/Services/Integration", this.ServerName));

            // disable automatic cookie handling - this enables us to explicitly set cookies
            binding.AllowCookies = false;
            var contactClient = new Default_Binding_ContactClient(binding, address);

            using (new OperationContextScope(contactClient.InnerChannel))
            {
                var contact = (Default_Binding_Contact)contactClient;

                // Embeds the extracted cookie in the next web service request
                // Note that we manually have to create the request object since
                // it doesn't exist yet at this stage 
                var request = new HttpRequestMessageProperty();
                request.Headers["Cookie"] = "JSESSIONID=" + this.SessionId;
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                var result = new List<Contact>();
                var rowsFound = 100;
                var rowsStart = 0;

                this.LogProcessingEvent("preparing filters");

                var filterContact = new Contact();
                if (this.GetAllAttributes)
                {
                    Tools.GetPropertyList(string.Empty, typeof(Contact)).ForEach(x => Tools.SetPropertyValue(filterContact, x, "{emptystring}"));
                }
                else
                {
                    Constants.PropertiesToQuery.ForEach(x => Tools.SetPropertyValue(filterContact, x.Key, x.Value));
                }

                filterList.ForEach(x => Tools.SetPropertyValue(filterContact, x.Key, x.Value));

                try
                {
                    while (rowsFound == 100)
                    {
                        this.LogProcessingEvent("reading data with page size {0} rows...", this.PageSize);

                        var rows =
                            contact.ContactQueryPage(
                                new ContactQueryPageRequest(
                                    new ContactWS_ContactQueryPage_Input
                                    {
                                        StartRowNum = rowsStart.ToString(CultureInfo.InvariantCulture),
                                        PageSize = this.PageSize.ToString(CultureInfo.InvariantCulture),
                                        ListOfContact = new[] { filterContact }
                                    })).ContactWS_ContactQueryPage_Output.ListOfContact;

                        rowsFound = rows.Length;
                        rowsStart += rowsFound;
                        result.AddRange(rows);

                        this.LogProcessingEvent("{0} rows added - {1} rows downloaded ...", rowsFound, result.Count);
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(ProcessAbortException))
                    {
                        throw;
                    }

                    this.LogProcessingEvent("an exception has been recorded while reading data: " + ex.Message);
                    return result;
                }

                this.LogProcessingEvent("read operations finished");
                return result;
            }
        }

        public bool GetAllAttributes { get; set; }

        /// <summary>
        /// Logs off from the server - this will invalidate the session token.
        /// </summary>
        /// <exception cref="WebException"> in case of a problem logging off - in this case the validity of 
        /// the session id is undefined. </exception>
        public void LogOff()
        {
            this.LogProcessingEvent("logging off ...");

            var logoffUrlString = new Uri(string.Format(CultureInfo.InstalledUICulture, "https://{0}/Services/Integration?command=logoff", this.ServerName));
            var req = (HttpWebRequest)WebRequest.Create(logoffUrlString);
            req.CookieContainer = new CookieContainer(1);
            req.CookieContainer.Add(new Cookie("JSESSIONID", this.SessionId, "/Services/Integration", this.ServerName));

            // make the HTTP call
            var resp = (HttpWebResponse)req.GetResponse();
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException("Logout failed");
            }
            
            this.LogProcessingEvent("log of finished");
        }

        /// <summary>
        /// Performs a löog on request and stored the session id into the property <see cref="SessionId"/>.
        /// </summary>
        /// <param name="userName"> The user name. </param>
        /// <param name="password"> The password. </param>
        /// <returns> true in case of a successfull log on </returns>
        public bool LogOn(string userName, string password)
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

                        sr.Close();
                    }

                    myResponse.Close();
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
        /// Returns true - this method is the handler for <see cref="ServicePointManager.ServerCertificateValidationCallback"/> if
        /// the <see cref="IgnoreCertificateError"/> has been set to "true".
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="certificate"> The certificate. </param>
        /// <param name="chain"> The chain. </param>
        /// <param name="sslPolicyErrors"> The ssl policy errors. </param>
        /// <returns>
        /// true (yes, always true)
        /// </returns>
        private static bool RemoteCertificateValidationCallbackAlwaysTrue(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
