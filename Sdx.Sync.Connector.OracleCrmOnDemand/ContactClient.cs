// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClient.cs" company="SDX-AG">
//   (c) 2009 by SDX-AG
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   This class is the client class for handling contacts
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    
    using Sem.GenericHelpers;
    
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    /// Implements a read/write connector to "Oracle CRM on Demand"
    /// </summary>
    [ClientStoragePathDescription(ReferenceType = ClientPathType.Undefined)]
    [ConnectorDescription(
        DisplayName = "Oracle CRM On Demand",
        CanReadContacts = true,
        NeedsCredentials = true)]
    public class ContactClient : StdClient
    {
        /// <summary>
        /// Overrides the method to read the full list of data.
        /// </summary>
        /// <param name="clientFolderName">The clientFolderName is currently ignored by the connector.</param>
        /// <param name="result">A list of StdElements that will get the new imported entries.</param>
        /// <returns>The list with the added contacts</returns>
        protected override List<StdElement> ReadFullList(string clientFolderName, List<StdElement> result)
        {
            this.LogProcessingEvent("starting read operation");
            Tools.DebugWriteLine("{0} Starting Read process ...", DateTime.Now);

            ContactAccess.IgnoreCertificateError = true;
            var contactClient = new ContactAccess 
                { 
                    ServerName = this.LogOnDomain, 
                    GetAllAttributes = false //// set this to true in order to get all properties
                };

            contactClient.ProcessingEvent += this.LogProcessingEvent;
            var filterList = CreateFilterList(clientFolderName);

            this.LogProcessingEvent("login using credantials for {0}...", this.LogOnUserId);
            if (contactClient.LogOn(this.LogOnUserId, this.LogOnPassword))
            {
                this.LogProcessingEvent("login succeeded");

                this.LogProcessingEvent("reading data ...");
                var contacts = contactClient.QueryContactsByFilter(filterList);

                this.LogProcessingEvent("log off ...");
                contactClient.LogOff();

                this.LogProcessingEvent("converting data ...");
                contacts.ForEach(x => result.Add(x.ToStdContact(contactClient.GetAllAttributes)));

                this.LogProcessingEvent("cleaning entities ...");
                CleanUpEntities(result);
            }

            contactClient.ProcessingEvent -= this.LogProcessingEvent;

            Tools.DebugWriteLine("{0} Finished Read process ({1} entries)...", DateTime.Now, result.Count); 
            return result;
        }

        /// <summary>
        /// Creates a list of filter expressions, that can be set for a contact object
        /// </summary>
        /// <param name="filter"> The filter string. </param>
        /// <returns> a list of filter expressions </returns>
        private static List<KeyValuePair<string, string>> CreateFilterList(string filter)
        {
            var result = new List<KeyValuePair<string, string>>();
            
            filter.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries)
            .ForEach(x => result.Add(CreateFilterPair(x)));

            return result;
        }

        /// <summary>
        /// Creates a filter key/value-pair from a filter expression
        /// </summary>
        /// <param name="filter"> The filter string. </param>
        /// <returns> a key/value-pair with the filter information </returns>
        private static KeyValuePair<string, string> CreateFilterPair(string filter)
        {
            var property = GetToken(ref filter);
            return new KeyValuePair<string, string>(property, filter);
        }

        /// <summary>
        /// Parses the next token from the string and cuts that token off.
        /// </summary>
        /// <param name="filter"> The filter by reference. The token returned will be cut from this string. </param>
        /// <returns> the next token </returns>
        private static string GetToken(ref string filter)
        {
            filter = filter.Replace("=", " = ").Replace("  ", " ").Trim(); 
            
            var whiteSpacePosition = filter.IndexOf(' ');
            if (whiteSpacePosition == -1)
            {
                whiteSpacePosition = filter.Length;
            }

            var token = filter.Substring(0, whiteSpacePosition + 1);
            filter = filter.Substring(whiteSpacePosition).Trim();
            return token.Trim();
        }
    }
}