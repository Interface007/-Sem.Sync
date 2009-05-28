//-----------------------------------------------------------------------
// <copyright file="MergePropertyConflict.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Merging
{
    using System;

    [Flags]
    public enum MergePropertyConflict
    {
        None = 0,
        SourceChanged = 1,
        TargetChanged = 2,
        BothChanged   = 3,
        IdenticallyChanged = 4,
        BothChangedIdentically = 7
    }

    public enum MergePropertyAction
    {
        Default = 0,
        NoAction = 0,
        CopySourceToTarget = 1,
        KeepCurrentTarget = 2,
        SolveConflict = 3,
    }

    public class MergeConflict
    {
        public StdElement SourceElement { get; set; }
        public StdElement TargetElement { get; set; }
        public StdElement BaselineElement { get; set; }

        public string SourcePropertyValue { get; set; }
        public string TargetPropertyValue { get; set; }
        public string BaselinePropertyValue { get; set; }

        public string PathToProperty { get; set; }
        public MergePropertyConflict PropertyConflict { get; set; }
        public MergePropertyAction ActionToDo { get; set; }
    }
}