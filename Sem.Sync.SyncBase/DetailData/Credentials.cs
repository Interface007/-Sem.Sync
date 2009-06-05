namespace Sem.Sync.SyncBase.DetailData
{
    using Sem.Sync.SyncBase.Interfaces;

    public class Credentials : ICredentialAware
    {

        #region ICredentialAware Members

        public string LogOnDomain { get; set;}
        public string LogOnUserId { get; set; }
        public string LogOnPassword { get; set; }

        #endregion
    }
}
