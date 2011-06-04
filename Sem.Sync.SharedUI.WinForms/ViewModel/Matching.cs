// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Matching.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   this might be better hosted in a more generic library
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.SharedUI.WinForms.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.GenericHelpers.Entities;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;

    /// <summary>
    /// this might be better hosted in a more generic library
    /// </summary>
    public class Matching
    {
        #region Constants and Fields

        /// <summary>
        ///   Reference to the currently active source contact for the call to <see cref = "Match" />.
        /// </summary>
        private StdElement currentSourceElement;

        /// <summary>
        ///   Reference to the currently active target contact for the call to <see cref = "Match" />.
        /// </summary>
        private StdElement currentTargetElement;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the BaseLine list of <see cref = "MatchingEntry" />. This list contains entries that will persist
        ///   the matching from a source dependend ID (like Xing-URL) to the standard contact ID.
        /// </summary>
        public List<MatchingEntry> BaseLine { get; set; }

        /// <summary>
        ///   Sets the reference to the currently active source contact for the call to <see cref = "Match" />.
        /// </summary>
        public StdElement CurrentSourceElement
        {
            set
            {
                this.currentSourceElement = value;
                if (value != null)
                {
                    this.currentTargetElement =
                        (from x in this.Target
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
        ///   Sets the reference to the currently active source contact for the call to <see cref = "Match" />.
        /// </summary>
        public StdElement CurrentTargetElement
        {
            set
            {
                this.currentTargetElement = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether to filter the property <see cref = "SourceAsList" /> by excluding 
        ///   the entries already matched to an entry inside <see cref = "TargetAsList" />.
        /// </summary>
        public bool FilterMatchedEntriesSource { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to filter the property <see cref = "SourceAsList" /> by excluding 
        ///   the entries already matched to an entry inside <see cref = "TargetAsList" />.
        /// </summary>
        public bool FilterMatchedEntriesTarget { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref = "ProfileIdentifierType" /> that should be used for matching in this context.
        ///   This is the <see cref = "ProfileIdentifierType" /> of the source - matches by other IDs will be 
        ///   respected, too, but other IDs will not be written.
        ///   <para>
        ///     Some sources do include <see cref = "ProfileIdentifierType" />, too. So a match can be made using 
        ///     a matching other ID. But the logic in this class will only update the matching entries ID for this
        ///     <see cref = "ProfileIdentifierType" /> when saving the matching list.
        ///   </para>
        /// </summary>
        public ProfileIdentifierType Profile { get; set; }

        /// <summary>
        ///   Gets or sets the source list for matching. This is the list of <see cref = "StdContact" /> entries that have been read.
        /// </summary>
        public List<StdElement> Source { get; set; }

        /// <summary>
        ///   Gets or sets the target list for matching. This is the list of <see cref = "StdContact" /> entries that will be written to.
        /// </summary>
        public List<StdElement> Target { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of view entities for the baseline list <see cref="BaseLine"/>
        /// </summary>
        /// <returns>
        /// A bindable list of view entities 
        /// </returns>
        public List<MatchView> BaselineAsList()
        {
            // filtering out entries without matching information is not needed, but helpful for debugging ;-)
            var profileIdentifierType = this.Profile;
            var result = new List<MatchView>();

            foreach (var baselineEntry in this.BaseLine)
            {
                if (!baselineEntry.ProfileId.ContainsKey(profileIdentifierType))
                {
                    continue;
                }

                var id = baselineEntry.ProfileId[profileIdentifierType];
                var source = (from x in this.Source where x.ExternalIdentifier[profileIdentifierType] == id select x).FirstOrDefault();
                var target = this.Target.GetElementById<StdElement>(baselineEntry.Id);

                if (source != null && target != null)
                {
                    result.Add(new MatchView(source, target));
                }
            }

            ////var result = (from b in this.BaseLine
            ////              join s in this.Source on b.ProfileId.GetProfileId(profileIdentifierType) equals
            ////                  s.ExternalIdentifier.GetProfileId(profileIdentifierType)
            ////              join t in this.Target on b.Id equals t.Id
            ////              select new MatchView(s, t)).ToList();
            result.Sort();
            return result;
        }

        /// <summary>
        /// Adds an entry for the current values of <see cref="CurrentSourceElement"/> and <see cref="CurrentTargetElement"/>
        ///   to the matching list <see cref="Matching"/>.
        /// </summary>
        public void Match()
        {
            // todo : handling profile identifiers should be more flexible (if there are multiple, we need to match all of them)
            if (this.currentSourceElement == null || this.currentTargetElement == null)
            {
                return;
            }

            var targetId = this.currentTargetElement.Id;
            var sourceProfileId = this.currentSourceElement.ExternalIdentifier.GetProfileId(this.Profile);

            // search for the element to match and set the profile id
            var element = this.GetBaselineElementById(targetId);
            if (element == null)
            {
                element = new MatchingEntry
                    {
                       Id = targetId, ProfileId = new ProfileIdentifierDictionary(this.Profile, sourceProfileId) 
                    };
                this.BaseLine.Add(element);
            }

            element.ProfileId.SetProfileId(this.Profile, sourceProfileId);

            // check if there is a profile class for the target element
            if (this.currentTargetElement.ExternalIdentifier == null)
            {
                this.currentTargetElement.ExternalIdentifier = new ProfileIdentifierDictionary();
            }

            // set the profile id for the target element, too
            this.currentTargetElement.ExternalIdentifier.SetProfileId(this.Profile, sourceProfileId);
        }

        /// <summary>
        /// Returns a filtered (if <see cref="FilterMatchedEntriesSource"/> is true) list of
        ///   view entities for the source list <see cref="Source"/>
        /// </summary>
        /// <returns>
        /// A bindable list of view entities 
        /// </returns>
        public List<MatchCandidateView> SourceAsList()
        {
            return this.FilterMatchedEntriesSource
                       ? (from s in this.Source
                          join b in this.BaseLine on s.ExternalIdentifier equals b.ProfileId into g
                          from y in g.DefaultIfEmpty()
                          where y == null
                          select new MatchCandidateView(s)).ToList()
                       : (from s in this.Source select new MatchCandidateView(s)).ToList();
        }

        /// <summary>
        /// Returns a filtered (if <see cref="FilterMatchedEntriesTarget"/> is true) list of
        ///   view entities for the source list <see cref="Source"/>
        /// </summary>
        /// <returns>
        /// A bindable list of view entities 
        /// </returns>
        public IEnumerable<MatchCandidateView> SourceAsList2()
        {
            var result = (from x in this.Source select new MatchCandidateView(x)).ToList();

            if (!this.FilterMatchedEntriesSource)
            {
                return result;
            }

            var profileType = this.Profile;

            this.BaseLine.ForEach(
                x =>
                    {
                        var id = x.ProfileId.GetProfileId(profileType);
                        var entries =
                            (from y in result
                             where y.Element.ExternalIdentifier.GetProfileId(profileType) == id
                             select y).ToList();
                        foreach (var entry in entries)
                        {
                            result.Remove(entry);
                        }
                    });

            return result;
        }

        /// <summary>
        /// Returns a list of view entities for the target list <see cref="Target"/>
        /// </summary>
        /// <returns>
        /// A bindable list of view entities 
        /// </returns>
        public List<MatchCandidateView> TargetAsList()
        {
            return this.FilterMatchedEntriesTarget
                       ? (from x in this.Target
                          join y in this.BaseLine on x.Id equals y.Id into g
                          from y in g.DefaultIfEmpty()
                          where y == null || string.IsNullOrEmpty(y.ProfileId.GetProfileId(this.Profile))
                          select new MatchCandidateView(x)).ToList()
                       : (from x in this.Target select new MatchCandidateView(x)).ToList();
        }

        /// <summary>
        /// Returns a list of view entities for the target list <see cref="Target"/>
        /// </summary>
        /// <returns>
        /// A bindable list of view entities 
        /// </returns>
        public IEnumerable<MatchCandidateView> TargetAsList2()
        {
            var result = (from x in this.Target select new MatchCandidateView(x)).ToList();

            if (!this.FilterMatchedEntriesTarget)
            {
                return result;
            }

            this.BaseLine.ForEach(
                x =>
                    {
                        var id = x.Id;
                        var entries = (from y in result where y.Element.Id == id select y).ToList();
                        foreach (var entry in entries)
                        {
                            if (x.ProfileId.GetProfileId(this.Profile) != null)
                            {
                                result.Remove(entry);
                            }
                        }
                    });

            return result;
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

        #endregion

        #region Methods

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
        ///   <para>
        /// Matching is performed by one of the following conditions:
        ///     <list type="bullets">
        /// <item>
        /// One matching profile-id
        /// </item>
        /// <item>
        /// matching first, middle and last name + matching personal or business address 
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        internal void MatchAll()
        {
            foreach (var sourceItem in this.Source)
            {
                var ppi = sourceItem.ExternalIdentifier;
                var targetItem =
                    (from x in this.Target where x.ExternalIdentifier.Equals(ppi) select x).FirstOrDefault();

                var sourceContact = sourceItem as StdContact;
                if (targetItem == null && sourceContact != null)
                {
                    var name = sourceItem.ToStringSimple();
                    var businessEmail = sourceContact.BusinessEmailPrimary;
                    var personalEmail = sourceContact.PersonalEmailPrimary;
                    targetItem = (from x in this.Target.ToStdContacts()
                                  where
                                      x.ToStringSimple() == name &&
                                      (x.BusinessEmailPrimary == businessEmail ||
                                       x.PersonalEmailPrimary == personalEmail)
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
        /// Performs a match for all source entities. The match may fail for some entities, so it can be incomplete.
        ///   <para>
        /// Matching is performed by one of the following conditions:
        ///     <list type="bullets">
        /// <item>
        /// One matching profile-id
        /// </item>
        /// <item>
        /// matching first, middle and last name + matching personal or business address 
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        internal void UnMatchAll()
        {
            this.BaseLine.ForEach(x => x.ProfileId.Remove(this.Profile));
        }

        /// <summary>
        /// Determines a list of property names and values from the <paramref name="objectToInspect"/>.
        /// </summary>
        /// <param name="objectToInspect">
        /// The object to get the properties from. 
        /// </param>
        /// <typeparam name="T">
        /// The type of the object to be scanned for proprties 
        /// </typeparam>
        /// <returns>
        /// A list of <see cref="KeyValuePair"/> with the names and values of the properties. 
        /// </returns>
        private static List<KeyValuePair> GetPropertyList<T>(T objectToInspect) where T : class
        {
            var resultList = new List<KeyValuePair>();

            var members = (objectToInspect != null)
                              ? objectToInspect.GetType().GetProperties()
                              : typeof(T).GetProperties();

            foreach (var item in members)
            {
                var typeName = item.PropertyType.Name;
                if (item.PropertyType.BaseType != null && item.PropertyType.BaseType.FullName == "System.Enum")
                {
                    typeName = "Enum";
                }

                switch (typeName)
                {
                    case "Enum":
                    case "Guid":
                    case "String":
                    case "DateTime":
                    case "TimeSpan":
                    case "Int32":
                        if (item.GetValue(objectToInspect, null) != null)
                        {
                            resultList.Add(
                                new KeyValuePair
                                    {
                                       Key = item.Name, Value = item.GetValue(objectToInspect, null).ToString() 
                                    });
                        }

                        break;

                    case "SerializableDictionary`2":
                    case "ProfileIdentifierDictionary":
                    case "List`1":
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
        /// <param name="baseLineId">
        /// The base line StdContact-ID. 
        /// </param>
        /// <returns>
        /// The <see cref="MatchingEntry"/> with the specified ID if it's present - otherwise null
        /// </returns>
        private MatchingEntry GetBaselineElementById(Guid baseLineId)
        {
            return (from x in this.BaseLine where x.Id == baseLineId select x).FirstOrDefault();
        }

        #endregion
    }
}