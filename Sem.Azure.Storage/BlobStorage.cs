namespace Sem.Azure.Storage
{
    public class BlobStorage
    {
        /// <summary>
        /// Accountinfo for the blob storage.
        /// </summary>
        private readonly AzureAccountInfo accountInfo;

        public BlobStorage(string containerName, string endpointConfigName)
        {
            this.accountInfo = AzureAccountInfo.GetAccountInfoFromConfiguration(
                "AccountName",
                "AccountSharedKey",
                "BlobStorageEndpoint",
                "UsePathStyleUris",
                false);
        }
    }
}
