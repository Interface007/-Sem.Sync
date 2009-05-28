//-----------------------------------------------------------------------
// <copyright file="ReplacementLists.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.DetailData
{
    using System.Collections.Generic;

    public class ReplacementLists
    {
        public List<KeyValuePair> BusinessCompanyName { get; set; }
        public List<KeyValuePair> BusinessHomepage { get; set; }

        public ReplacementLists()
        {
            this.BusinessCompanyName = new List<KeyValuePair>();
            this.BusinessHomepage = new List<KeyValuePair>();
        }
    }
}
