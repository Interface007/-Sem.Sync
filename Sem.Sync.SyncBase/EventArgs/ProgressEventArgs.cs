//-----------------------------------------------------------------------
// <copyright file="ProgressEventArgs.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.EventArgs
{
    public class ProgressEventArgs : System.EventArgs
    {
        public int PercentageDone { get; set; }
    }
}