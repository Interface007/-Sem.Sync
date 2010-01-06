// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeInfo.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the ChangeInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.ChangeTracker
{
    using System.Collections.Generic;

    internal class ChangeInfo
    {
        public string DisplayName { get; set; }

        public string TargetSystemName { get; set; }

        public IList<string> ChangedProperties { get; set; }

        public byte[] Image { get; set; }
    }
}