// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchByName.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Match internally without user interaction by comparing the names
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GenericHelpers.Interfaces;

    using Interfaces;

    /// <summary>
    /// Match internally without user interaction by comparing the names
    /// </summary>
    public class MatchByName : ISyncCommand
    {
        /// <summary>
        /// Gets or sets UiProvider.
        /// </summary>
        public IUiInteraction UiProvider { get; set; }
        
        /// <summary>
        /// Match internally without user interaction by comparing the names
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> True if the response from the <see cref="UiProvider"/> is "continue" </returns>
        public bool ExecuteCommand(IClientBase sourceClient, IClientBase targetClient, IClientBase baseliClient, string sourceStorePath, string targetStorePath, string baselineStorePath, string commandParameter)
        {
            if (targetClient == null)
            {
                throw new InvalidOperationException("targetClient is null");
            }
            
            if (sourceClient == null)
            {
                throw new InvalidOperationException("sourceClient is null");
            }

            if (sourceStorePath == null)
            {
                throw new InvalidOperationException("sourceStorePath is null");
            }

            if (targetStorePath == null)
            {
                throw new InvalidOperationException("targetStorePath is null");
            }
            
            targetClient.WriteRange(
                MatchThisByName(
                    sourceClient.GetAll(sourceStorePath),
                    targetClient.GetAll(targetStorePath)),
                targetStorePath);
            
            return true;
        }

        /// <summary>
        /// Automatically matches without user interaction entities by <see cref="object.ToString"/> and (with lower 
        /// priority) by <see cref="StdElement.ToStringSimple"/>
        /// </summary>
        /// <param name="source">the list of <see cref="StdElement"/> that contains the source 
        /// (this will not be changed)</param>
        /// <param name="target">the list of <see cref="StdElement"/> that contains the target 
        /// (here the <see cref="StdElement.Id"/> will be changed if a match is found in the source)</param>
        /// <returns>the modified list of elements from the <paramref name="target"/></returns>
        private static List<StdElement> MatchThisByName(IEnumerable<StdElement> source, List<StdElement> target)
        {
            // ReSharper disable AccessToModifiedClosure
            foreach (var item in target)
            {
                var corresponding = (from element in source
                                     where element.Id == item.Id
                                     select element).FirstOrDefault();

                // if there is someone with the same id, we do not need to match
                if (corresponding != null)
                {
                    continue;
                }

                // try it by full name
                // or try it by full name without academic title
                corresponding = (from element in source
                                 where element.ToString() == item.ToString()
                                 select element).FirstOrDefault()
                                 ?? (from element in source
                                     where element.ToStringSimple() == item.ToStringSimple()
                                     select element).FirstOrDefault();

                // if we did find the name, we match using the Id
                if (corresponding != null)
                {
                    item.Id = corresponding.Id;
                }
            }

            // ReSharper restore AccessToModifiedClosure
            return target;
        }
    }
}
