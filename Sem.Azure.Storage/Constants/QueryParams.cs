namespace Sem.Azure.Storage
{
    internal static class QueryParams
    {
        internal const string SeparatorForParameterAndValue = "=";
        internal const string QueryParamTimeout = "timeout";
        internal const string QueryParamComp = "comp";

        // Other query string parameter names
        internal const string QueryParamBlockId = "blockid";
        internal const string QueryParamPrefix = "prefix";
        internal const string QueryParamMarker = "marker";
        internal const string QueryParamMaxResults = "maxresults";
        internal const string QueryParamDelimiter = "delimiter";
        internal const string QueryParamModifiedSince = "modifiedsince";
    }
}