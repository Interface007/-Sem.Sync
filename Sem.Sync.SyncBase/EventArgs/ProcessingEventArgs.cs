//-----------------------------------------------------------------------
// <copyright file="ProcessingEventArgs.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.EventArgs
{
    public class ProcessingEventArgs : System.EventArgs
    {
        public object Item { get; set; }
        public string Message { get; set; }
    }
}