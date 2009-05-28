//-----------------------------------------------------------------------
// <copyright file="SyncData.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    public class SyncData
    {
        public DateTime DateOfLastChange { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}