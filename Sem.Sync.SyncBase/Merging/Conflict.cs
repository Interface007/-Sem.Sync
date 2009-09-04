// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Conflict.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Merging
{
    using System;

    /// <summary>
    /// The merge conflict description - it describes what kind of conflict exists.
    /// </summary>
    [Flags]
    public enum MergePropertyConflict
    {
        /// <summary>
        /// No conflict at all
        /// </summary>
        None = 0,

        /// <summary>
        /// The source has been changed in comparison to the baseline, but the target has not
        /// </summary>
        SourceChanged = 1,

        /// <summary>
        /// The target has been changed in comparison to the baseline, but the source has not
        /// </summary>
        TargetChanged = 2,

        /// <summary>
        /// Both source and target have changed in comparison to the baseline
        /// </summary>
        BothChanged   = 3,

        /// <summary>
        /// Both source and target have changed in comparison to the baseline, but both have been changed identical, so there's no difference any more.
        /// This item is taged as obsolete, because BothChangedIdentically should be used instead. This enum is an enum of Flags and if this flag is true,
        /// both flags <see cref="TargetChanged"/> and <see cref="SourceChanged"/> need also be true.
        /// </summary>
        [Obsolete("use BothChangedIdentically instead")]
        IdenticallyChanged = 4,

        /// <summary>
        /// Both source and target have changed in comparison to the baseline, but both have been changed identical, so there's no difference any more
        /// </summary>
        BothChangedIdentically = 7
    }

    /// <summary>
    /// This value describes what to do with the conflict as a result of a conflict solution.
    /// </summary>
    public enum MergePropertyAction
    {
        /// <summary>
        /// As a default nothing is done with that conflict, so the items will stay unchanged.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Nothing is done with that conflict, so the items will stay unchanged.
        /// </summary>
        NoAction = 0,

        /// <summary>
        /// The source item will be copied to the target property, so the value of the target property is lost.
        /// </summary>
        CopySourceToTarget = 1,

        /// <summary>
        /// The target property will be accepted as the new value. This will cause the baseline to be updated.
        /// </summary>
        KeepCurrentTarget = 2,

        /// <summary>
        /// There is a conflict that still need to be resolved, because no solution could be found - so the 
        /// conflict needs to be solved manually
        /// </summary>
        SolveConflict = 3,
    }

    /// <summary>
    /// Describes a merge conflict in a way that UI can handle the conflict and store information for conflict 
    /// solution inside this entity.
    /// </summary>
    public class MergeConflict
    {
        /// <summary>
        /// Gets or sets a reference to the source element of the merge action (this element will not be changed).
        /// </summary>
        public StdElement SourceElement { get; set; }

        /// <summary>
        /// Gets or sets a reference to the target element of the merge action (this is the element that potentially should be updated).
        /// </summary>
        public StdElement TargetElement { get; set; }

        /// <summary>
        /// Gets or sets a reference to the baseline element of the merge action (this is the element that contains information 
        /// about the version, that is considered as a baseline for source and target - ideally this should contain the version
        /// of the last merged source and target).
        /// </summary>
        public StdElement BaselineElement { get; set; }

        /// <summary>
        /// Gets or sets the value of the conflicting property of <see cref="SourceElement"/>.
        /// </summary>
        public string SourcePropertyValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the conflicting property of <see cref="TargetElement"/>.
        /// </summary>
        public string TargetPropertyValue { get; set; }

        /// <summary>
        /// Gets or sets the value of the conflicting property of <see cref="BaselineElement"/>.
        /// </summary>
        public string BaselinePropertyValue { get; set; }

        /// <summary>
        /// Gets or sets the object path to the property in question (e.g. "myObject.Address.Street") in a string representation.
        /// </summary>
        public string PathToProperty { get; set; }

        /// <summary>
        /// Gets or sets the description of the conflict - source or target changes or both.
        /// </summary>
        public MergePropertyConflict PropertyConflict { get; set; }

        /// <summary>
        /// Gets or sets the description of the action to be done to solve the conflict.
        /// </summary>
        public MergePropertyAction ActionToDo { get; set; }

        public override string ToString()
        {
            return this.SourceElement + " vs. " + this.TargetElement + " : " + this.PathToProperty;
        }
    }
}