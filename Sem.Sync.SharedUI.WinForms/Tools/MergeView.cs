using Sem.Sync.SyncBase.Merging;

namespace Sem.Sync.SharedUI.WinForms.Tools
{
    public class MergeView
    {
        public string ContactName { get; set; }
        public string PropertyName { get; set; }
        public string SourceValue { get; set; }
        public string TargetValue { get; set; }

        public MergeConflict Conflict { get; set; }
    }
}