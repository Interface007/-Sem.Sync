namespace Sem.Azure.Storage
{
    internal static class StandardPortalEndpoints
    {
        internal const string BlobStorage = "blob";
        internal const string QueueStorage = "queue";
        internal const string TableStorage = "table";
        internal const string StorageHostSuffix = ".core.windows.net";
        internal const string BlobStorageEndpoint = BlobStorage + StorageHostSuffix;
        internal const string QueueStorageEndpoint = QueueStorage + StorageHostSuffix;
        internal const string TableStorageEndpoint = TableStorage + StorageHostSuffix;
    }
}