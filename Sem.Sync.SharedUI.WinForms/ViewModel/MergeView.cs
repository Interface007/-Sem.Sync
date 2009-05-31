namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using SyncBase.Merging;

    public class MergeView
    {
        public string ContactName { get; set; }
        public string PropertyName { get; set; }
        public string SourceValue { get; set; }
        public string TargetValue { get; set; }

        public MergeConflict Conflict { get; set; }
    }
}