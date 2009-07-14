namespace Sem.GenericHelpers.Interfaces
{
    /// <summary>
    /// Defines an interface to be able to set log on credentials.
    /// </summary>
    public interface ICredentialAware
    {
        /// <summary>
        /// Gets or sets a domain to log on. In a windows domain this is the domain name, 
        /// in case of a web form this may be omitted. Most client implementation do accept
        /// specifying the domain also as part of the user name: domain\user or user@domain.
        /// </summary>
        string LogOnDomain { get; set; }

        /// <summary>
        /// Gets or sets a user name to log on. Most client implementation do accept
        /// specifying the domain also as part of the user name: domain\user or user@domain.
        /// </summary>
        string LogOnUserId { get; set; }

        /// <summary>
        /// Gets or sets the password to log on. Be aware that this information is READ/WRITE. This
        /// may be a security issue for your user!
        /// </summary>
        string LogOnPassword { get; set; }
    }
}