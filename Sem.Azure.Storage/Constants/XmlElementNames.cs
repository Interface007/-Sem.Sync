namespace Sem.Azure.Storage
{
    internal static class XmlElementNames
    {
        internal const string BlockList = "BlockList";
        internal const string Block = "Block";
        internal const string EnumerationResults = "EnumerationResults";
        internal const string Prefix = "Prefix";
        internal const string Marker = "Marker";
        internal const string MaxResults = "MaxResults";
        internal const string Delimiter = "Delimiter";
        internal const string NextMarker = "NextMarker";
        internal const string Containers = "Containers";
        internal const string Container = "Container";
        internal const string ContainerName = "Name";
        internal const string ContainerNameAttribute = "ContainerName";
        internal const string AccountNameAttribute = "AccountName";
        internal const string LastModified = "LastModified";
        internal const string Etag = "Etag";
        internal const string Url = "Url";
        internal const string CommonPrefixes = "CommonPrefixes";
        internal const string ContentType = "ContentType";
        internal const string ContentEncoding = "ContentEncoding";
        internal const string ContentLanguage = "ContentLanguage";
        internal const string Size = "Size";
        internal const string Blobs = "Blobs";
        internal const string Blob = "Blob";
        internal const string BlobName = "Name";
        internal const string BlobPrefix = "BlobPrefix";
        internal const string BlobPrefixName = "Name";
        internal const string Name = "Name";
        internal const string Queues = "Queues";
        internal const string Queue = "Queue";
        internal const string QueueName = "QueueName";
        internal const string QueueMessagesList = "QueueMessagesList";
        internal const string QueueMessage = "QueueMessage";
        internal const string MessageId = "MessageId";
        internal const string PopReceipt = "PopReceipt";
        internal const string InsertionTime = "InsertionTime";
        internal const string ExpirationTime = "ExpirationTime";
        internal const string TimeNextVisible = "TimeNextVisible";
        internal const string MessageText = "MessageText";

        // Error specific constants
        internal const string ErrorRootElement = "Error";
        internal const string ErrorCode = "Code";
        internal const string ErrorMessage = "Message";
        internal const string ErrorException = "ExceptionDetails";
        internal const string ErrorExceptionMessage = "ExceptionMessage";
        internal const string ErrorExceptionStackTrace = "StackTrace";
        internal const string AuthenticationErrorDetail = "AuthenticationErrorDetail";

        //The following are for table error messages
        internal const string DataWebMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        internal const string TableErrorCodeElement = "code";
        internal const string TableErrorMessageElement = "message";
    }
}