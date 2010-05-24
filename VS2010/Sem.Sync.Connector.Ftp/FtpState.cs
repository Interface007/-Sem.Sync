// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FtpState.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the FtpState type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Connector.Ftp
{
    using System;
    using System.Net;
    using System.Threading;

    public class FtpState
    {
        public FtpState()
        {
            this.OperationException = null;
            this.OperationComplete = new ManualResetEvent(false);
        }

        public ManualResetEvent OperationComplete { get; private set; }

        public FtpWebRequest Request { get; set; }

        public string Content { get; set; }

        public Exception OperationException { get; set; }

        public string StatusDescription { get; set; }
    }
}