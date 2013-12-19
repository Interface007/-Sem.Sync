// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientConfigurationData.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <summary>
//   Defines the ContactClientConfigurationData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;

    /// <summary>
    /// Implements the data structure to save extended configuration data
    /// </summary>
    public class ContactClientConfigurationData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClientConfigurationData"/> class.
        /// </summary>
        public ContactClientConfigurationData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactClientConfigurationData"/> class by 
        /// interpreting a filter list. The filter list is a string representation of the properties 
        /// concatenated with "&amp;&amp;" strings.
        /// </summary>
        /// <param name="filterListDescription"> The filter list description. </param>
        public ContactClientConfigurationData(string filterListDescription)
        {
            this.FilterList = CreateFilterList(filterListDescription);
        }

        /// <summary>
        /// Gets or sets the list of filter key value pairs (both are strings).
        /// </summary>
        public List<KeyValuePair> FilterList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to get all attributes or only the default
        /// set of attributes.
        /// </summary>
        public bool GetAllAttributes { get; set; }

        /// <summary>
        /// Gets or sets the paging number of entities while reading from the services.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore certificate errors while 
        /// communicating with the service via ssl. This is usefull for debugging
        /// the communication with fiddler-tool.
        /// </summary>
        public bool IgnoreCertificateErrors { get; set; }

        /// <summary>
        /// Creates a list of filter expressions, that can be set for a contact object
        /// </summary>
        /// <param name="filter"> The filter string. </param>
        /// <returns> a list of filter expressions </returns>
        private static List<KeyValuePair> CreateFilterList(string filter)
        {
            var result = new List<KeyValuePair>();

            filter.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries)
            .ForEach(x => result.Add(CreateFilterPair(x)));

            return result;
        }

        /// <summary>
        /// Creates a filter key/value-pair from a filter expression
        /// </summary>
        /// <param name="filter"> The filter string. </param>
        /// <returns> a key/value-pair with the filter information </returns>
        private static KeyValuePair CreateFilterPair(string filter)
        {
            var property = GetToken(ref filter);
            return new KeyValuePair(property, filter);
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
