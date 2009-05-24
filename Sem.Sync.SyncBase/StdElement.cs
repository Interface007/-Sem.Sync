using Sem.Sync.SyncBase.Attributes;

namespace Sem.Sync.SyncBase
{
    using System;
    using System.Xml.Serialization;

    using DetailData;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "overriding IComparable is just for sorting!"),
    Serializable]
    public abstract class StdElement : IComparable<StdElement>
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        
        [ComparisonModifier(SkipCompare = true, SkipMerge = true)]
        public SyncData InternalSyncData { get; set; }

        public virtual int CompareTo(StdElement other)
        {
            return string.Compare(this.ToSortSimple(), other.ToSortSimple(), StringComparison.OrdinalIgnoreCase);
        }

        public virtual string ToStringSimple()
        {
            return this.ToString();
        }

        public virtual string ToSortSimple()
        {
            return this.ToString();
        }
        
        public abstract void NormalizeContent();
    }
}