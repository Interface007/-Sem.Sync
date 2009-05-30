using System.Collections.Generic;
using Sem.Sync.SharedUI.WinForms.UI;
using Sem.Sync.SyncBase;
using Sem.Sync.SyncBase.Interfaces;
using Sem.Sync.SyncBase.Merging;

namespace Sem.Sync.SharedUI.WinForms.Tools
{
    public class UiDispatcher : IMergeConflictResolver
    {
        public List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList)
        {
            var ui = new MergeEntities();
            return ui.PerformMerge(toMerge, targetList);
        }

        public List<StdElement> PerformEntityMerge(List<StdElement> toMerge, List<StdElement> targetList, List<StdElement> baselineList)
        {
            var ui = new MatchEntities();
            return ui.PerformMerge(toMerge, targetList, baselineList);
        }
    }
}
