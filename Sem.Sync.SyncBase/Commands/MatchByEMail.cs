// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MatchByEMail.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Match internally without user interaction by comparing the email addresses
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SyncBase.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.SyncBase.Interfaces;

    /// <summary>
    /// Match internally without user interaction by comparing the email addresses
    /// </summary>
    public class MatchByEMail : SyncComponent, ISyncCommand
    {
        /// <summary>
        /// defines a regular expression to test an email
        /// address for invalid parts
        /// </summary>
        private static readonly Regex CheckRegEx = new Regex(@"([^a-zA-Z]?(test)|(promo.?)|(info)|(support)|(redaktion)|(hilfe)[^a-zA-Z])|(abteilung)|(service)@", RegexOptions.IgnoreCase);

        /// <summary>
        /// Match internally without user interaction by comparing the email addresses
        /// </summary>
        /// <param name="sourceClient">The source client.</param>
        /// <param name="targetClient">The target client.</param>
        /// <param name="baseliClient">The baseline client.</param>
        /// <param name="sourceStorePath">The source storage path.</param>
        /// <param name="targetStorePath">The target storage path.</param>
        /// <param name="baselineStorePath">The baseline storage path.</param>
        /// <param name="commandParameter">The command parameter.</param>
        /// <returns> returns always true </returns>
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
                this.MatchThisByEMail(
                    sourceClient.GetAll(sourceStorePath).ToContacts(),
                    targetClient.GetAll(targetStorePath).ToContacts()),
                targetStorePath);
            
            return true;
        }

        /// <summary>
        /// Performs a match by comparing email addresses
        /// </summary>
        /// <param name="element1"> The first element to test. </param>
        /// <param name="element2"> The second element to test. </param>
        /// <returns> true in case of a match in one or more email addresses </returns>
        private static bool IsEmailMatch(StdContact element1, StdContact element2)
        {
            if (IsValidEmailAddress(element1.PersonalEmailPrimary) && element1.PersonalEmailPrimary.IsOneOf(element2.PersonalEmailPrimary, element2.PersonalEmailSecondary, element2.BusinessEmailPrimary, element2.PersonalEmailSecondary))
            {
                return true;
            }

            if (IsValidEmailAddress(element1.PersonalEmailSecondary) && element1.PersonalEmailPrimary.IsOneOf(element2.PersonalEmailPrimary, element2.PersonalEmailSecondary, element2.BusinessEmailPrimary, element2.PersonalEmailSecondary))
            {
                return true;
            }

            if (IsValidEmailAddress(element1.BusinessEmailPrimary) && element1.PersonalEmailPrimary.IsOneOf(element2.PersonalEmailPrimary, element2.PersonalEmailSecondary, element2.BusinessEmailPrimary, element2.PersonalEmailSecondary))
            {
                return true;
            }

            if (IsValidEmailAddress(element1.BusinessEmailSecondary) && element1.PersonalEmailPrimary.IsOneOf(element2.PersonalEmailPrimary, element2.PersonalEmailSecondary, element2.BusinessEmailPrimary, element2.PersonalEmailSecondary))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether the email address is valid for comparing with other email addresses
        /// </summary>
        /// <param name="emailAddress"> The email address. </param>
        /// <returns> true if comparison is allowed </returns>
        private static bool IsValidEmailAddress(string emailAddress)
        {
            return !string.IsNullOrEmpty(emailAddress)
                && !CheckRegEx.IsMatch(emailAddress);
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
        private List<StdElement> MatchThisByEMail(IEnumerable<StdContact> source, List<StdContact> target)
        {
            foreach (var item in target)
            {
                var testItem = item;

                var corresponding = (from element in source
                                     where element.Id == testItem.Id
                                     select element).FirstOrDefault();

                // if there is someone with the same id, we do not need to match
                if (corresponding != null)
                {
                    continue;
                }

                // try it by full name
                // or try it by full name without academic title
                corresponding = (from element in source
                                 where IsEmailMatch(element, testItem)
                                 select element).FirstOrDefault();

                // if we did find the name, we match using the Id
                if (corresponding == null)
                {
                    continue;
                }

                this.LogProcessingEvent(item, "Match found.");
                item.Id = corresponding.Id;
            }

            return target.ToStdElement();
        }
    }
}
