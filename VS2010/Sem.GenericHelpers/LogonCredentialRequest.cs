// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogonCredentialRequest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The logon credential request.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers
{
    using Sem.GenericHelpers.Entities;
    using Sem.GenericHelpers.Interfaces;

    /// <summary>
    /// The logon credential request.
    /// </summary>
    public class LogonCredentialRequest
    {
        #region Constants and Fields

        /// <summary>
        /// The software sem sync cachedcredentials.
        /// </summary>
        private const string SoftwareSemSyncCachedcredentials = "Software\\Sem.Sync\\CachedCredentials";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogonCredentialRequest"/> class.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="resourceName">
        /// The resource name.
        /// </param>
        public LogonCredentialRequest(ICredentialAware credentials, string message, string resourceName)
        {
            this.LogOnCredentials = credentials;
            this.MessageForUser = message;
            this.ResourceKey = Tools.GetSha1Hash(resourceName);

            var regValue = Tools.GetRegValue(SoftwareSemSyncCachedcredentials, this.ResourceKey, string.Empty);
            if (!string.IsNullOrEmpty(regValue))
            {
                this.LogOnCredentials = Tools.LoadFromString<Credentials>(regValue);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogonCredentialRequest"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="resourceName">
        /// The resource name.
        /// </param>
        public LogonCredentialRequest(string message, string resourceName)
            : this(new Credentials(), message, resourceName)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets LogOnCredentials.
        /// </summary>
        public ICredentialAware LogOnCredentials { get; set; }

        /// <summary>
        /// Gets or sets MessageForUser.
        /// </summary>
        public string MessageForUser { get; set; }

        /// <summary>
        /// Gets or sets ResourceKey.
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether WriteToCacheAllowed.
        /// </summary>
        public bool WriteToCacheAllowed { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The save credentials.
        /// </summary>
        public void SaveCredentials()
        {
            if (!this.WriteToCacheAllowed)
            {
                return;
            }

            Tools.SetRegValue(
                SoftwareSemSyncCachedcredentials, 
                this.ResourceKey, 
                Tools.SaveToString((Credentials)this.LogOnCredentials));
        }

        #endregion
    }
}