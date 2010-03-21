// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Matching.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Defines the Matching type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GenericHelpers.Entities;

    using SyncBase;
    using SyncBase.DetailData;

    /// <summary>
    /// this might be better hosted in a more generic library
    /// </summary>
    public class Matching
    {
        /// <summary>
        /// Reference to the currently active source contact for the call to <see cref="Match"/>.
        /// </summary>
        private StdContact currentSourceElement;

        /// <summary>
        /// Reference to the currently active target contact for the call to <see cref="Match"/>.
        /// </summary>
        private StdContact currentTargetElement;

        /// <summary>
        /// Gets or sets the source list for matching. This is the list of <see cref="StdContact"/> entries that have been read.
        /// </summary>
        public List<StdContact> Source { get; set; }

        /// <summary>
        /// Gets or sets the target list for matching. This is the list of <see cref="StdContact"/> entries that will be written to.
        /// </summary>
        public List<StdContact> Target { get; set; }

        /// <summary>
        /// Gets or sets the BaseLine list of <see cref="MatchingEntry"/>. This list contains entries that will persist
        /// the matching from a source dependend ID (like Xing-URL) to the standard contact ID.
        /// </summary>
        public List<MatchingEntry> BaseLine { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ProfileIdentifierType"/> that should be used for matching in this context.
        /// This is the <see cref="ProfileIdentifierType"/> of the source - matches by other IDs will be 
        /// respected, too, but other IDs will not be written.
        /// <para>
        /// Some sources do include <see cref="ProfileIdentifierType"/>, too. So a match can be made using 
        /// a matching other ID. But the logic in this class will only update the matching entries ID for this
        /// <see cref="ProfileIdentifierType"/> when saving the matching list.
        /// </para>
        /// </summary>
        public ProfileIdentifierType Profile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to filter the property <see cref="SourceAsList"/> by excluding 
        /// the entries already matched to an entry inside <see cref="TargetAsList"/>.
        /// </summary>
        public bool FilterMatchedEntries { get; set; }

        /// <summary>
        /// Sets the reference to the currently active source contact for the call to <see cref="Match"/>.
        /// </summary>
        public StdContact CurrentSourceElement
        {
            set
            {
                this.currentSourceElement = value;
                if (value != null)
                {
                    this.currentTargetElement = (from x in this.Target
                                                 where this.currentSourceElement.ToStringSimple() == x.ToStringSimple()
                                                 select x).FirstOrDefault();
                }
                else
                {
                    this.currentTargetElement = null;
                }
            }
        }

        /// <summary>
        /// Sets the reference to the currently active source contact for the call to <see cref="Match"/>.
        /// </summary>
        public StdContact CurrentTargetElement
        {
            set
            {
                this.currentTargetElement = value;
            }
        }

        /// <summary>
        /// Deletes a matching entry from the <see cref="BaseLine"/>
        /// </summary>
        /// <param name="baseLineId">
        /// The base line id.
        /// </param>
        public void UnMatch(Guid baseLineId)
        {
            // search for the element to unmatch
            var element = this.GetBaselineElementById(baseLineId);

            // simply set the profile id for the current type to null
            element.ProfileId.SetProfileId(this.Profile, null);

            // prevent matching again in case of an now empty list
            this.CurrentSourceElement = null;
        }

        /// <summary>
        /// Adds an entry for the current values of <see cref="CurrentSourceElement"/> and <see cref="CurrentTargetElement"/>
        /// to the matching list <see cref="Matching"/>.
        /// </summary>
        public void Match()
        {
            // todo : handling profile identifiers should be more flexible (if there are multiple, we need to match all of them)
            if (this.currentSourceElement == null || this.currentTargetElement == null)
            {
                return;
            }

            var targetId = this.currentTargetElement.Id;
            var sourceProfileId = this.currentSourceElement.PersonalProfileIdentifiers.GetProfileId(this.Profile);

            // search for the element to match and set the profile id
            var element = this.GetBaselineElementById(targetId);
            if (element == null)
            {
                element = new MatchingEntry { Id = targetId, ProfileId = new ProfileIdentifiers(this.Profile, sourceProfileId) };
                this.BaseLine.Add(element);
            }

            element.ProfileId.SetProfileId(this.Profile, sourceProfileId);

            // check if there is a profile class for the target element
            if (this.currentTargetElement.PersonalProfileIdentifiers == null)
            {
                this.currentTargetElement.PersonalProfileIdentifiers = new ProfileIdentifiers();
            }

            // set the profile id for the target element, too
            this.currentTargetElement.PersonalProfileIdentifiers.SetProfileId(this.Profile, sourceProfileId);
        }

        /// <summary>
        /// Returns a filtered (if <see cref="FilterMatchedEntries"/> is true) list of
        /// view entities for the source list <see cref="Source"/>
        /// </summary>
        /// <returns> A bindable list of view entities </returns>
        public List<MatchCandidateView> SourceAsList()
        {
            return this.FilterMatchedEntries
                ? (from s in this.Source
                   join b in this.BaseLine on
                   s.PersonalProfileIdentifiers equals b.ProfileId into g
                   from y in g.DefaultIfEmpty()
                   where y == null
                   select
                       new MatchCandidateView
                       {
                           ContactName = s.GetFullName(),
                           Element = s
                       }).ToList()
                : (from s in this.Source
                   select
                       new MatchCandidateView
                       {
                           ContactName = s.GetFullName(),
                           Element = s
                       }).ToList();
        }

        /// <summary>
        /// Returns a list of view entities for the target list <see cref="Target"/>
        /// </summary>
        /// <returns> A bindable list of view entities </returns>
        public List<MatchCandidateView> TargetAsList()
        {
            return this.FilterMatchedEntries
                ? (from x in this.Target
                   join y in this.BaseLine on
                       x.Id equals y.Id
                   into g
                   from y in g.DefaultIfEmpty()
                   where y == null || string.IsNullOrEmpty(y.ProfileId.GetProfileId(this.Profile))
                   select
                       new MatchCandidateView
                       {
                           ContactName = x.GetFullName(),
                           Element = x
                       }).ToList()
                : (from x in this.Target
                   select
                       new MatchCandidateView
                       {
                           ContactName = x.GetFullName(),
                           Element = x
                       }).ToList();
        }

        /// <summary>
        /// Returns a list of view entities for the baseline list <see cref="BaseLine"/>
        /// </summary>
        /// <returns> A bindable list of view entities </returns>
        public List<MatchView> BaselineAsList()
        {
            // filtering out entries without matching information is not needed, but helpful for debugging ;-)
            var list = from x in this.BaseLine
                       where !string.IsNullOrEmpty(x.ProfileId.GetProfileId(this.Profile))
                       select x;

            var result = (from b in list
                          join s in this.Source on 
                            b.ProfileId equals s.PersonalProfileIdentifiers 
                          join t in this.Target on b.Id equals t.Id
                          select new MatchView
                                     {
                                         ContactName = s.GetFullName(),
                                         ContactNameMatch = t.GetFullName(),
                                         BaselineId = t.Id,
                                     }).ToList();
            result.Sort();
            return result;
        }

        /// <summary>
        /// Determines a list of property names and values from the current <see cref="CurrentSourceElement"/>.
        /// </summary>
        /// <returns>
        /// A list of <see cref="KeyValuePair"/> with the names and values of the properties.
        /// </returns>
        internal List<KeyValuePair> CurrentSourceProperties()
        {
            return GetPropertyList(this.currentSourceElement);
        }

        /// <summary>
        /// Determines a list of property names and values from the current <see cref="CurrentTargetElement"/>.
        /// </summary>
        /// <returns>
        /// A list of <see cref="KeyValuePair"/> with the names and values of the properties.
        /// </returns>
        internal List<KeyValuePair> CurrentTargetProperties()
        {
            return GetPropertyList(this.currentTargetElement);
        }

        /// <summary>
        /// Performs a match for all source entities. The match may fail for some entities, so it can be incomplete.
        /// <para>Matching is performed by one of the following conditions:
        /// <list type="bullets">
        ///     <item>One matching profile-id</item>
        ///     <item>matching first, middle and last name + matching personal or business address </item>
        /// </list></para>
        /// </summary>
        internal void MatchAll()
        {
            foreach (var sourceItem in this.Source)
            {
                var ppi = sourceItem.PersonalProfileIdentifiers;
                var targetItem = (from x in this.Target
                                  where x.PersonalProfileIdentifiers.Equals(ppi)
                                  select x).FirstOrDefault();

                if (targetItem == null)
                {
                    var name = sourceItem.ToStringSimple();
                    var businessEmail = sourceItem.BusinessEmailPrimary;
                    var personalEmail = sourceItem.BusinessEmailPrimary;
                    targetItem = (from x in this.Target
                                  where x.ToStringSimple() == name
                                        && (x.BusinessEmailPrimary == businessEmail
                                            || x.PersonalEmailPrimary == personalEmail)
                                  select x).FirstOrDefault();
                }

                if (targetItem == null)
                {
                    continue;
                }

                this.CurrentSourceElement = sourceItem;
                this.CurrentTargetElement = targetItem;
                this.Match();
            }
        }

        /// <summary>
        /// Determines a list of property names and values from the <paramref name="objectToInspect"/>.
        /// </summary>
        /// <param name="objectToInspect"> The object to get the properties from. </param>
        /// <typeparam name="T"> The type of the object to be scanned for proprties </typeparam>
        /// <returns> A list of <see cref="KeyValuePair"/> with the names and values of the properties. </returns>
        private static List<KeyValuePair> GetPropertyList<T>(T objectToInspect)
        {
            var resultList = new List<KeyValuePair>();

            var members = typeof(T).GetProperties();

            foreach (var item in members)
            {
                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType.FullName == "System.Enum")
                {
                    typeName = "Enum";
                }

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "Int32":
                        if (item.GetValue(objectToInspect, null) != null)
                        {
                            resultList.Add(
                                new KeyValuePair
                                    {
                                        Key = item.Name,
                                        Value = item.GetValue(objectToInspect, null).ToString()
                                    });
                        }

                        break;

                    case "Byte[]":
                        break;

                    default:
                        resultList.AddRange(GetPropertyList(item.GetValue(objectToInspect, null)));
                        break;
                }
            }

            return resultList;
        }

        /// <summary>
        /// Performs a lookup inside the <see cref="BaseLine"/> for a given <see cref="StdContact"/>-ID.
        /// </summary>
        /// <param name="baseLineId"> The base line StdContact-ID. </param>
        /// <returns> The <see cref="MatchingEntry"/> with the specified ID if it's present - otherwise null</returns>
        private MatchingEntry GetBaselineElementById(Guid baseLineId)
        {
            return (from x in this.BaseLine
                    where x.Id == baseLineId
                    select x).FirstOrDefault();
        }
    }
}
