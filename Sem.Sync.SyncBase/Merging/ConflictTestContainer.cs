//-----------------------------------------------------------------------
// <copyright file="ConflictTestContainer.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Merging
{
    using System;

    public class ConflictTestContainer
    {
        public object SourceProperty { get; set; }
        public object TargetProperty { get; set; }
        public object BaselineProperty { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        public StdElement SourceObject { get; set; }
        public StdElement TargetObject { get; set; }
        public StdElement BaselineObject { get; set; }
    }
}