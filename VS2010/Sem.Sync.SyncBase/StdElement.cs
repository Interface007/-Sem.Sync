//-----------------------------------------------------------------------
// <copyright file="StdElement.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
//-----------------------------------------------------------------------
namespace Sem.Sync.SyncBase
{
    using System;
    using System.Xml.Serialization;

    using DetailData;

    using GenericHelpers.Attributes;

    /// <summary>
    /// This is the base class for contacts and calendar entries. It should contain everything that 
    /// is needed to successfully sync all kind of the entities.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1036:OverrideMethodsOnComparableTypes", Justification = "overriding IComparable is just for sorting!"),
    Serializable]
    public abstract class StdElement : IComparable<StdElement>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity. Optimally you will sync
        /// entities in a way that one physical entity (person / event in time and 
        /// space / whatever) will have only one ID.
        /// </summary>
        [XmlAttribute]
        public virtual Guid Id { get; set; }
        
        /// <summary>
        /// Gets or sets some internal synchronization data that does not need to (but 
        /// might) match to any real property of the entity.
        /// </summary>
        [ComparisonModifier(SkipCompare = true, SkipMerge = true)]
        public SyncData InternalSyncData { get; set; }

        /// <summary>
        /// compares two entities
        /// </summary>
        /// <param name="other"> The other instance to compare to. </param>
        /// <returns> a value indicating whether the other is "greater", "euqal" or "less" than this entity </returns>
        public virtual int CompareTo(StdElement other)
        {
            return string.Compare(this.ToSortSimple(), other.ToSortSimple(), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Implements an overridable SIMPLE string representation.
        /// </summary>
        /// <returns>a dense and simple string representation of the entity</returns>
        public virtual string ToStringSimple()
        {
            return this.ToString();
        }

        /// <summary>
        /// Implements a overridable sortable string representation of the entity. This 
        /// is NOT intended to be shown in any UI and should strictly be used only for 
        /// sorting entities.
        /// </summary>
        /// <returns>a string that provides a "weight"/"rank" of the entity</returns>
        public virtual string ToSortSimple()
        {
            return this.ToString();
        }
        
        /// <summary>
        /// This method must be overridden and should be implemented in a way that it 
        /// performs "clean up" operation to the properties of the entity. E.g. by
        /// removing leading and tailing whitespace or pretty formatting other data.
        /// </summary>
        public abstract void NormalizeContent();
    }
}