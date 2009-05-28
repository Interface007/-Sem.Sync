//-----------------------------------------------------------------------
// <copyright file="MatchingEntry.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System;

    public class MatchingEntry : StdElement
    {
        public override void NormalizeContent()
        {
            throw new NotImplementedException();
        }

        public ProfileIdentifiers ProfileId { get; set; }
    }
}
