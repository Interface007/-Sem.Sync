namespace Sem.Sync.SyncBase.DetailData
{
    using Interfaces;

    /// <summary>
    /// Minimal implementation of the <see cref="ICredentialAware"/> interface to represent 
    /// log on credentials.
    /// </summary>
    public class Credentials : ICredentialAware
    {

        #region ICredentialAware Members

        /// <summary>
        /// Gets of sets an identifier that groups user names in one authentication system. In windows
        /// environments this may be an NT-Domain, a Computer name or an Active Directory name.
        /// </summary>
        public string LogOnDomain { get; set;}

        /// <summary>
        /// Gets or sets the user name of the principal that wants to establish the log on. In windows
        /// environmenmts this is a domain- or computer-user name.
        /// </summary>
        public string LogOnUserId { get; set; }

        /// <summary>
        /// Gets or sets the "shared secret" to authenticate the user. The user log on password is
        /// used in windows environment.
        /// </summary>
        public string LogOnPassword { get; set; }

        #endregion
    }
}
