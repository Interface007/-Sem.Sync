// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AzureAccountInfo.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Access to the azure account information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Azure.Storage
{
    using System;
    using System.Configuration;

    using Microsoft.ServiceHosting.ServiceRuntime;

    /// <summary>
    /// Access to the azure account information
    /// </summary>
    public class AzureAccountInfo
    {
        /// <summary>
        /// a message for the developer in case of an error
        /// </summary>
        private const string GeneralAccountConfigurationExceptionString =
            "If the portal defines http://test.blob.core.windows.net as your blob storage endpoint, the string \"test\" " +
            "is your account name, and you can specify http://blob.core.windows.net as the BlobStorageEndpoint in your " +
            "service's configuration file(s).";

        /// <summary>
        /// The default prefix string in application config and Web.config files to indicate that this setting should be looked up 
        /// in the fabric's configuration system.
        /// </summary>
        private const string CSConfigStringPrefix = "CSConfigName";

        /// <summary>
        /// a value indicating usage of path style uris
        /// </summary>
        private bool? usePathStyleUris;

        /// <summary>
        /// Gets or sets the base URI of the account.
        /// </summary>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets the account name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the account's key.
        /// </summary>
        public string Base64Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use path style uris. If the property has not been explicitly set, 
        /// the implementation tries to derive the correct value from the base URI.
        /// </summary>
        public bool UsePathStyleUris
        {
            get
            {
                if (this.usePathStyleUris != null)
                {
                    return this.usePathStyleUris.Value;
                }
                
                return this.BaseUri != null && Utilities.StringIsIPAddress(this.BaseUri.Host);
            }

            set
            {
                this.usePathStyleUris = value;
            }
        }

        /// <summary>
        /// Retrieves account information settings from configuration settings. First, the implementation checks for 
        /// settings in an application config section of an app.config or Web.config file. These values are overwritten 
        /// if the same settings appear in a .csdef file.
        /// The implementation also supports indirect settings. In this case, indirect settings overwrite all other settings.
        /// </summary>        
        /// <param name="accountNameConfiguration">Configuration string for the account name.</param>
        /// <param name="accountSharedKeyConfiguration">Configuration string for the key.</param>
        /// <param name="endpointConfiguration">Configuration string for the endpoint.</param>
        /// <param name="usePathStyleUrisConfiguration">Configuration string for the path style.</param>
        /// <param name="allowIncompleteSettings">If false, an exception is thrown if not all settings are available.</param>
        /// <returns>StorageAccountInfo object containing the retrieved settings.</returns>        
        public static AzureAccountInfo GetAccountInfoFromConfiguration(
            string accountNameConfiguration,
            string accountSharedKeyConfiguration,
            string endpointConfiguration,
            string usePathStyleUrisConfiguration,
            bool allowIncompleteSettings)
        {
            if (string.IsNullOrEmpty(endpointConfiguration))
            {
                throw new ArgumentException("Endpoint configuration is missing", "endpointConfiguration");
            }

            var name = TryGetAppSetting(accountNameConfiguration);
            var key = TryGetAppSetting(accountSharedKeyConfiguration);
            var endpoint = TryGetAppSetting(endpointConfiguration);
            var pathStyle = TryGetAppSetting(usePathStyleUrisConfiguration);

            // settings in the csc file overload settings in Web.config
            if (RoleManager.IsRoleManagerRunning)
            {
                // get config settings from the csc file
                name = TryGetConfigurationSetting(accountNameConfiguration, name);
                key = TryGetConfigurationSetting(accountSharedKeyConfiguration, key);
                endpoint = TryGetConfigurationSetting(endpointConfiguration, endpoint);
                pathStyle = TryGetConfigurationSetting(usePathStyleUrisConfiguration, pathStyle);

                // the Web.config can have references to csc setting strings
                // these count event stronger than the direct settings in the csc file
                name = TryGetConfigurationSetting(TryGetAppSetting(CSConfigStringPrefix + accountNameConfiguration), name);
                key = TryGetConfigurationSetting(TryGetAppSetting(CSConfigStringPrefix + accountSharedKeyConfiguration), key);
                endpoint = TryGetConfigurationSetting(TryGetAppSetting(CSConfigStringPrefix + endpointConfiguration), endpoint);
                pathStyle = TryGetConfigurationSetting(TryGetAppSetting(CSConfigStringPrefix + usePathStyleUrisConfiguration), pathStyle);
            }

            if (string.IsNullOrEmpty(key) && !allowIncompleteSettings)
            {
                throw new ArgumentException("No account key specified!");
            }

            if (string.IsNullOrEmpty(endpoint) && !allowIncompleteSettings)
            {
                throw new ArgumentException("No endpoint specified!");
            }

            if (string.IsNullOrEmpty(name))
            {
                // in this case let's try to derive the account name from the Uri
                string newAccountName;
                Uri newBaseUri;
                if (!string.IsNullOrEmpty(endpoint) && IsStandardStorageEndpoint(new Uri(endpoint), out newAccountName, out newBaseUri))
                {
                    if (newAccountName != null && newBaseUri != null)
                    {
                        endpoint = newBaseUri.AbsoluteUri;
                        name = newAccountName;
                    }
                }

                if (string.IsNullOrEmpty(name) && !allowIncompleteSettings)
                {
                    throw new ArgumentException("No account name specified.");
                }
            }

            bool? usePathStyleUris = null;
            if (!string.IsNullOrEmpty(pathStyle))
            {
                bool b;
                if (!bool.TryParse(pathStyle, out b))
                {
                    throw new ConfigurationErrorsException("Cannot parse value of setting UsePathStyleUris as a boolean");
                }

                usePathStyleUris = b;
            }

            Uri tmpBaseUri = null;
            if (!string.IsNullOrEmpty(endpoint))
            {
                tmpBaseUri = new Uri(endpoint);
            }

            return new AzureAccountInfo(tmpBaseUri, usePathStyleUris, name, key, allowIncompleteSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAccountInfo"/> class. 
        /// Constructor for creating account info objects.
        /// </summary>
        /// <param name="baseUri"> The account's base URI. </param>
        /// <param name="usePathStyleUris">
        /// If true, path-style URIs (http://baseuri/accountname/containername/objectname) are used.
        /// If false host-style URIs (http://accountname.baseuri/containername/objectname) are used,
        /// where baseuri is the URI of the service..
        /// If null, the choice is made automatically: path-style URIs if host name part of base URI is an 
        /// IP addres, host-style otherwise.
        /// </param>
        /// <param name="accountName"> The account name. </param>
        /// <param name="base64Key"> The account's shared key. </param>
        public AzureAccountInfo(Uri baseUri, bool? usePathStyleUris, string accountName, string base64Key)
            : this(baseUri, usePathStyleUris, accountName, base64Key, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAccountInfo"/> class. 
        /// Constructor for creating account info objects.
        /// </summary>
        /// <param name="baseUri">The account's base URI.</param>
        /// <param name="usePathStyleUris">If true, path-style URIs (http://baseuri/accountname/containername/objectname) are used.
        /// If false host-style URIs (http://accountname.baseuri/containername/objectname) are used,
        /// where baseuri is the URI of the service.
        /// If null, the choice is made automatically: path-style URIs if host name part of base URI is an 
        /// IP addres, host-style otherwise.</param>
        /// <param name="accountName">The account name.</param>
        /// <param name="base64Key">The account's shared key.</param>
        /// <param name="allowIncompleteSettings">true if it shall be allowed to only set parts of the StorageAccountInfo properties.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        public AzureAccountInfo(Uri baseUri, bool? usePathStyleUris, string accountName, string base64Key, bool allowIncompleteSettings)
        {
            if (baseUri == null && !allowIncompleteSettings)
            {
                throw new ArgumentNullException("baseUri");
            }

            if (string.IsNullOrEmpty(base64Key) && !allowIncompleteSettings)
            {
                throw new ArgumentNullException("base64Key");
            }

            if (baseUri != null)
            {
                string newAccountName;
                Uri newBaseUri;
                if (IsStandardStorageEndpoint(baseUri, out newAccountName, out newBaseUri))
                {
                    if (!string.IsNullOrEmpty(newAccountName) &&
                        !string.IsNullOrEmpty(accountName) &&
                        string.Compare(accountName, newAccountName, StringComparison.Ordinal) != 0)
                    {
                        throw new ArgumentException("The configured base URI " + baseUri.AbsoluteUri + " for the storage service is incorrect. " +
                                                    "The configured account name " + accountName + " must match the account name " + newAccountName +
                                                    " as specified in the storage service base URI." +
                                                     GeneralAccountConfigurationExceptionString);
                    }
                    
                    if (newAccountName != null && newBaseUri != null)
                    {
                        accountName = newAccountName;
                        baseUri = newBaseUri;
                    }
                }
            }

            if (string.IsNullOrEmpty(accountName) && !allowIncompleteSettings)
            {
                throw new ArgumentNullException("accountName");
            }

            if (!string.IsNullOrEmpty(accountName) && accountName.ToLowerInvariant() != accountName)
            {
                throw new ArgumentException("The account name must not contain upper-case letters. " +
                                "The account name is the first part of the URL for accessing the storage services as presented to you by the portal or " +
                                "the predefined storage account name when using the development storage tool. " +
                                GeneralAccountConfigurationExceptionString);
            }

            this.BaseUri = baseUri;
            this.AccountName = accountName;
            this.Base64Key = base64Key;
            if (usePathStyleUris == null && baseUri == null && !allowIncompleteSettings)
            {
                throw new ArgumentException("Cannot determine setting from empty URI.");
            }
            
            if (usePathStyleUris == null)
            {
                this.usePathStyleUris = 
                    baseUri != null 
                    ? (bool?)Utilities.StringIsIPAddress(baseUri.Host) 
                    : null;
            }
            else
            {
                this.UsePathStyleUris = usePathStyleUris.Value;
            }
        }

        private static string TryGetAppSetting(string configName)
        {
            return TryGetAppSetting(configName, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
                                                                  Justification = "Make sure that nothing prevents us to read from the fabric's configuration envrionment.")]
        private static string TryGetAppSetting(string configName, string defaultValue)
        {
            string ret;
            try
            {
                ret = ConfigurationManager.AppSettings[configName];
            }
            catch (Exception)
            {
                // some exception happened when accessing the app settings section
                // most likely this is because there is no app setting file
                // we assume that this is because there is no app settings file; this is not an error
                // and explicitly all exceptions are captured here
                return defaultValue;
            }

            return string.IsNullOrEmpty(ret) ? defaultValue : ret;
        }

        private static string TryGetConfigurationSetting(string configName, string defaultValue)
        {
            string ret;
            try
            {
                ret = RoleManager.GetConfigurationSetting(configName);
            }
            catch (RoleException)
            {
                return defaultValue;
            }

            return string.IsNullOrEmpty(ret) ? defaultValue : ret;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        private static bool IsStandardStorageEndpoint(Uri baseUri, out string newAccountName, out Uri newBaseUri)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("baseUri");
            }

            newAccountName = null;
            newBaseUri = null;

            var host = baseUri.Host;
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("The host part of the Uri " + baseUri.AbsoluteUri + " must not be null or empty.");
            }

            if (host != host.ToLowerInvariant())
            {
                throw new ArgumentException("The specified host string " + host + " must not contain upper-case letters.");
            }

            const string Suffix = StandardPortalEndpoints.BlobStorageEndpoint;

            var index = host.IndexOf(Suffix, StringComparison.Ordinal);
            if (index > 0)
            {
                var first = host.Substring(0, index);
                if (first[first.Length - 1] != ConstChars.Dot[0])
                {
                    return false;
                }

                first = first.Substring(0, first.Length - 1);
                if (string.IsNullOrEmpty(first))
                {
                    throw new ArgumentException("The configured base URI " + baseUri.AbsoluteUri + " for the storage service is incorrect. " +
                                                 GeneralAccountConfigurationExceptionString);
                }

                if (first.Contains(ConstChars.Dot))
                {
                    throw new ArgumentException("The configured base URI " + baseUri.AbsoluteUri + " for the storage service is incorrect. " +
                                                 GeneralAccountConfigurationExceptionString);
                }

                newAccountName = first;
                newBaseUri = new Uri(baseUri.Scheme + Uri.SchemeDelimiter + Suffix + baseUri.PathAndQuery);
            }

            return true;
        }
    }
}
