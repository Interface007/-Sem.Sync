namespace Sem.Sync.SyncBase.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ComparisonModifierAttribute : Attribute
    {
        public bool SkipMerge { get; set; }
        public bool SkipCompare { get; set; }
        public bool CaseInsensitive { get; set; }
    }
}
