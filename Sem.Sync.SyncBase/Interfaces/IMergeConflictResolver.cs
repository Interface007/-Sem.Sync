//-----------------------------------------------------------------------
// <copyright file="IMergeConflictResolver.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase.Interfaces
{
    using System.Collections.Generic;
    
    using Merging;

    public interface IMergeConflictResolver
    {
        List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList);
        List<StdElement> PerformEntityMerge(List<StdElement> sourceList, List<StdElement> targetList, List<StdElement> baselineList);
    }
}
