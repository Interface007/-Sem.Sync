namespace Sem.Azure.Storage
{
    internal static class HeaderValues
    {
        internal const string ContentTypeXml = "application/xml";

        /// <summary>
        /// This is the default content-type xStore uses when no content type is specified
        /// </summary>
        internal const string DefaultContentType = "application/octet-stream";

        // The Range header value is "bytes=start-end", both start and end can be empty
        internal const string RangeHeaderFormat = "bytes={0}-{1}";

    }
}