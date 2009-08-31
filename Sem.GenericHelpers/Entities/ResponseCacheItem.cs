namespace Sem.GenericHelpers.Entities
{
    using System.Net;

    public class ResponseCacheItem
    {
        public byte[] Content { get; set; }
        public CookieCollection Cookies { get; set; }
    }
}
