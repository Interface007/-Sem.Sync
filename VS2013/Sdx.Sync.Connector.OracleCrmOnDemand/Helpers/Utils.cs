// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <summary>
//   Defines the Helpers type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sdx.Sync.Connector.OracleCrmOnDemand.Helpers
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Sem.GenericHelpers.Entities;

    /// <summary>
    /// Implements some "not so interesting" helpers
    /// </summary>
    internal static class Utils
    {

        internal static KeyValuePair SplitToKeyValuePair(string x)
        {
            var i = x.IndexOf(" : ", StringComparison.Ordinal);
            var result = new KeyValuePair(x.Substring(0, i), x.Substring(i + 3));
            return result;
        }

        internal static string ExtractPropertyName(string propertyName, string prefix)
        {
            var result =
                prefix + "." + 
                (propertyName.Contains(".")
                ? propertyName.Substring(0, propertyName.IndexOf('.'))
                : propertyName);

            return result;
        }

        /// <summary>
        /// Converts a simple account name into a query that is supported by Oracle. This call does remove 
        /// some special chars from the account name in order to make the query "oracle-compatible".
        /// </summary>
        /// <param name="criteria"> The string to compare to. </param>
        /// <returns> an expression like <code>LIKE 'H*chst'</code> (in this sample the account name was 
        /// "Höchst" - the "ö" is not compatible with oracle query language, so it has been replaced by
        /// an asterix)</returns>
        internal static string GetLikeQuery(string criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria))
            {
                return criteria;
            }

            criteria = EscapeCharacters(criteria);
            criteria = string.Format(CultureInfo.InvariantCulture, "~LIKE '{0}'", criteria);
            return criteria.Trim();
        }

        /// <summary>
        /// Performs character escaping for the Oracle CRM queries.
        /// </summary>
        /// <param name="unescapedValue"> The value string without escaping. </param>
        /// <returns> the escaped string </returns>
        internal static string EscapeCharacters(string unescapedValue)
        {
            return unescapedValue
                .Replace('ö', '?').Replace('ä', '?').Replace('ü', '?')
                .Replace('Ö', '?').Replace('Ä', '?').Replace('Ü', '?')
                .Replace(@"'", @"''")
                .Replace(@"&", @"\&")
                .Replace(@"(", @"\(")
                .Replace(@")", @"\)");
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
        internal static bool RemoteCertificateValidationCallbackAlwaysTrue(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Sets a value indicating whether to ignore SSL certificate errors. Set this property to "true" if you want to be
        /// able to debug the SSL traffic. Also you might need to set this property to "true" in case of problems with the 
        /// SSL certificate of the web service server.
        /// </summary>
        internal static void IgnoreCertificateError()
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallbackAlwaysTrue;
        }

        /// <summary>
        /// Adds session information to the operation context in order to use them with the request headers
        /// </summary>
        /// <param name="sessionId">the session id to be added to the headers</param>
        internal static void ModifyOperationContext(string sessionId)
        {
            var request = new HttpRequestMessageProperty();
            request.Headers["Cookie"] = "JSESSIONID=" + sessionId;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;
        }

    }
}
