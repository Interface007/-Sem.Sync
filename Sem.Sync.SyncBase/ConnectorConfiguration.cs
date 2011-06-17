// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectorConfiguration.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Base class for connector configuration.
    /// </summary>
    [Serializable]
    public abstract class ConnectorConfiguration
    {

        /// <summary>
        /// Storage class for logon information.
        /// </summary>
        public class LogonStore
        {
            /// <summary>
            /// Gets or sets the user id to connect to an information store.
            /// </summary>
            [XmlAttribute]
            public string UserId { get; set; }

            /// <summary>
            /// Gets or sets the password to connect to an information store.
            /// </summary>
            [XmlAttribute]
            public string Password { get; set; }
        }

        /// <summary>
        /// Storage class for http helper parameters
        /// </summary>
        public class HttpConnectionStore
        {
            /// <summary>
            /// Gets or sets a value indicating whether the non-cached information 
            /// should be skipped while retrieving information via the http helper.
            /// </summary>
            [XmlAttribute]
            public bool SkipNotCached { get; set; }

            [XmlAttribute]
            public bool UseIeCookies { get; set; }

            [XmlAttribute]
            public bool ReadCache { get; set; }

            [XmlAttribute]
            public bool WriteCache { get; set; }
        }

        public LogonStore Logon { get; set; }

        public HttpConnectionStore HttpConnection { get; set; }
    }
}
