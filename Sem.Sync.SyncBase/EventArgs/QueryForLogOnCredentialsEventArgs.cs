//-----------------------------------------------------------------------
// <copyright file="QueryForLogOnCredentialsEventArgs.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.EventArgs
{
    public class QueryForLogOnCredentialsEventArgs : System.EventArgs
    {
        public string MessageForUser { get; set; }
        public string LoginUserId { get; set; }
        public string LoginPassword { get; set; }
    }
}