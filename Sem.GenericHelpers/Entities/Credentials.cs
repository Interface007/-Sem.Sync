// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Credentials.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Minimal implementation of the &lt;see cref="ICredentialAware" /&gt; interface to represent
//   log on credentials.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.GenericHelpers.Entities
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml.Serialization;

    using Interfaces;

    /// <summary>
    /// Minimal implementation of the <see cref="ICredentialAware"/> interface to represent 
    /// log on credentials.
    /// </summary>
    [Serializable]
    public class Credentials : ICredentialAware
    {
        /// <summary>
        /// Initializes static members of the <see cref="Credentials"/> class. 
        /// </summary>
        static Credentials()
        {
            Entropy = Encoding.UTF8.GetBytes("Sem.Sync");
        }

        /// <summary>
        /// Gets or sets some entropy to not share the encryption with other local programs.
        /// This is not serialized when using the XmlSerializer, so it needs to be known by the 
        /// process ("Sem.Sync" is the standard value for the entropy).
        /// </summary>
        [XmlIgnore]
        public static byte[] Entropy { get; set; }

        /// <summary>
        /// Gets or sets an identifier that groups user names in one authentication system. In windows
        /// environments this may be an NT-Domain, a Computer name or an Active Directory name.
        /// </summary>
        public string LogOnDomain { get; set; }

        /// <summary>
        /// Gets or sets the user name of the principal that wants to establish the log on. In windows
        /// environmenmts this is a domain- or computer-user name.
        /// </summary>
        public string LogOnUserId { get; set; }

        /// <summary>
        /// Gets or sets the "shared secret" to authenticate the user. The user log on password is
        /// used in windows environment.
        /// </summary>
        [XmlIgnore]
        public string LogOnPassword { get; set; }

        /// <summary>
        /// Gets or sets the protected password. The password is protected by the user scope encryption key
        /// of the DPAPI (Data Protection API).
        /// </summary>
        public byte[] LogOnPasswordProtected
        {
            get
            {
                byte[] protectedString = null;
                if (this.LogOnPassword != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(this.LogOnPassword);
                    protectedString = ProtectedData.Protect(bytes, Credentials.Entropy, DataProtectionScope.CurrentUser);
                }

                return protectedString;
            }

            set
            {
                var unprotectedString = ProtectedData.Unprotect(value, Credentials.Entropy, DataProtectionScope.CurrentUser);
                var dataString = Encoding.UTF8.GetString(unprotectedString);
                this.LogOnPassword = dataString;
            }
        }
    }
}