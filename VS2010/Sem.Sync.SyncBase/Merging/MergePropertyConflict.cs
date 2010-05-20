// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MergePropertyConflict.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The merge conflict description - it describes what kind of conflict exists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Merging
{
    using System;

    /// <summary>
    /// The merge conflict description - it describes what kind of conflict exists.
    /// </summary>
    [Flags]
    public enum MergePropertyConflicts
    {
        /// <summary>
        ///   No conflict at all
        /// </summary>
        None = 0, 

        /// <summary>
        ///   The source has been changed in comparison to the baseline, but the target has not
        /// </summary>
        SourceChanged = 1, 

        /// <summary>
        ///   The target has been changed in comparison to the baseline, but the source has not
        /// </summary>
        TargetChanged = 2, 

        /// <summary>
        ///   Both source and target have changed in comparison to the baseline
        /// </summary>
        BothChanged = 3, 

        /// <summary>
        ///   Both source and target have changed in comparison to the baseline, but both have been changed identical, so there's no difference any more.
        ///   This item is taged as obsolete, because BothChangedIdentically should be used instead. This enum is an enum of Flags and if this flag is true,
        ///   both flags <see cref = "TargetChanged" /> and <see cref = "SourceChanged" /> need also be true.
        /// </summary>
        [Obsolete("use BothChangedIdentically instead")]
        IdenticallyChanged = 4, 

        /// <summary>
        ///   Both source and target have changed in comparison to the baseline, but both have been changed identical, so there's no difference any more
        /// </summary>
        BothChangedIdentically = 7
    }
}