namespace Sem.GenericHelpers.Entities
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class ResponseCacheItem
    {
        public byte[] Content { get; set; }
        public List<KeyValuePair> Cookies { get; set; }
    }
}
