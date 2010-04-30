namespace Sem.GenericHelpers
{
    using Interfaces;

    using Sem.GenericHelpers.Entities;

    public class LogonCredentialRequest
    {
        private const string SoftwareSemSyncCachedcredentials = "Software\\Sem.Sync\\CachedCredentials";

        public ICredentialAware LogOnCredentials { get; set; }

        public string MessageForUser { get; set; }

        public bool WriteToCacheAllowed { get; set; }

        public string ResourceKey { get; set; }

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

        public LogonCredentialRequest(string message, string resourceName)
            : this(new Credentials(), message, resourceName)
        {
        }

        public void SaveCredentials()
        {
            if (!this.WriteToCacheAllowed)
            {
                return;
            }

            Tools.SetRegValue(SoftwareSemSyncCachedcredentials, this.ResourceKey, Tools.SaveToString((Credentials)this.LogOnCredentials));
        }
    }
}