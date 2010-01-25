// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseCacheItem.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Represents a cached content including cookies
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a cached content including cookies
    /// </summary>
    [Serializable]
    public class ResponseCacheItem
    {
        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets the Cookies.
        /// </summary>
        public List<KeyValuePair> Cookies { get; set; }
    }
}
