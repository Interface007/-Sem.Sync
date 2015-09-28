// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUiSyncInteraction.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Interface for objects that are able to route UI access to the correct objects
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Interfaces
{
    using System.Collections.Generic;

    using Sem.GenericHelpers.Interfaces;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Merging;

    /// <summary>
    /// Interface for objects that are able to route UI access to the correct objects
    /// </summary>
    public interface IUiSyncInteraction : IUiInteraction
    {
        #region Public Methods

        /// <summary>
        /// Requests a merge action for attributes of conflicting entities
        /// </summary>
        /// <param name="toMerge">
        /// the list of merge conflicts to reslove
        /// </param>
        /// <param name="targetList">
        /// the list of elements that should be changed
        /// </param>
        /// <returns>
        /// the target list with merged properties
        /// </returns>
        List<StdElement> PerformAttributeMerge(List<MergeConflict> toMerge, List<StdElement> targetList);

        /// <summary>
        /// Requests an entity merge of elements
        /// </summary>
        /// <param name="sourceList">
        /// the source entity list
        /// </param>
        /// <param name="targetList">
        /// the list that will be changed
        /// </param>
        /// <param name="baselineList">
        /// a baseline list that helps merging
        /// </param>
        /// <param name="identifierToUse">
        /// The identifier To Use.
        /// </param>
        /// <returns>
        /// the target list with additional merged elements
        /// </returns>
        List<StdElement> PerformEntityMerge(
            List<StdElement> sourceList, 
            List<StdElement> targetList, 
            List<StdElement> baselineList, 
            ProfileIdentifierType identifierToUse);

        #endregion
    }
}