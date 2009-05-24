using System.Collections.Generic;
using Sem.Sync.SyncBase.Merging;

namespace Sem.Sync.SyncBase.Interfaces
{
    public interface IMergeConflictResolver
    {
        List<StdElement> PerformMerge(List<MergeConflict> toMerge, List<StdElement> targetList);
    }
}
