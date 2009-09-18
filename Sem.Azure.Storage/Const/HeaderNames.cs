namespace Sem.Azure.Storage
{
    internal static class HeaderNames
    {
        internal const string PrefixForStorageProperties = "x-ms-prop-";
        internal const string PrefixForMetadata = "x-ms-meta-";
        internal const string PrefixForStorageHeader = "x-ms-";
        internal const string PrefixForTableContinuation = "x-ms-continuation-";

        //
        // Standard headers...
        //
        internal const string ContentLanguage = "Content-Language";
        internal const string ContentLength = "Content-Length";
        internal const string ContentType = "Content-Type";
        internal const string ContentEncoding = "Content-Encoding";
        internal const string ContentMD5 = "Content-MD5";
        internal const string ContentRange = "Content-Range";
        internal const string LastModifiedTime = "Last-Modified";
        internal const string Server = "Server";
        internal const string Allow = "Allow";
        internal const string ETag = "ETag";
        internal const string Range = "Range";
        internal const string Date = "Date";
        internal const string Authorization = "Authorization";
        internal const string IfModifiedSince = "If-Modified-Since";
        internal const string IfUnmodifiedSince = "If-Unmodified-Since";
        internal const string IfMatch = "If-Match";
        internal const string IfNoneMatch = "If-None-Match";
        internal const string IfRange = "If-Range";
        internal const string NextPartitionKey = "NextPartitionKey";
        internal const string NextRowKey = "NextRowKey";
        internal const string NextTableName = "NextTableName";

        //
        // Storage specific custom headers...
        //
        internal const string StorageDateTime = PrefixForStorageHeader + "date";
        internal const string PublicAccess = PrefixForStorageProperties + "publicaccess";
        internal const string StorageRange = PrefixForStorageHeader + "range";

        internal const string CreationTime = PrefixForStorageProperties + "creation-time";
        internal const string ForceUpdate = PrefixForStorageHeader + "force-update";            
        internal const string ApproximateMessagesCount = PrefixForStorageHeader + "approximate-messages-count";
        internal const string Version = PrefixForStorageHeader + "version";     
    }
}