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

    using SyncBase;
    using SyncBase.DetailData;

    public class Matching
    {
        public List<StdContact> Source { get; set; }
        public List<StdContact> Target { get; set; }
        public List<MatchingEntry> BaseLine { get; set; }

        public ProfileIdentifierType Profile { get; set; }

        private StdContact currentSourceElement;
        private StdContact currentTargetElement;

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

        public StdContact CurrentTargetElement
        {
            set
            {
                this.currentTargetElement = value;
            }
        }

        public void UnMatch(Guid baseLineId)
        {
            // search for the element to unmatch
            var element = this.GetBaselineElementById(baseLineId);

            // simply set the profile id for the current type to null
            element.ProfileId.SetProfileId(this.Profile, null);
        }

        private MatchingEntry GetBaselineElementById(Guid baseLineId)
        {
            return (from x in this.BaseLine
                    where
                        x.Id == baseLineId
                    select x).FirstOrDefault();
        }

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

        internal List<KeyValuePair> CurrentSourceProperties()
        {
            return GetPropertyList(this.currentSourceElement);
        }

        internal List<KeyValuePair> CurrentTargetProperties()
        {
            return GetPropertyList(this.currentTargetElement);
        }

        internal List<MatchCandidateView> SourceAsList()
        {
            return (from x in this.Source
                    join y in this.BaseLine on
                    x.PersonalProfileIdentifiers.GetProfileId(this.Profile) equals
                    y.ProfileId.GetProfileId(this.Profile) into g
                    from y in g.DefaultIfEmpty()
                    where y == null
                    select
                        new MatchCandidateView
                        {
                            ContactName = x.GetFullName(),
                            Element = x
                        }).ToList();
        }

        internal List<MatchCandidateView> TargetAsList()
        {
            return (from x in this.Target
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
                        }).ToList();
        }

        internal List<MatchView> BaselineAsList()
        {
            var result = (from b in this.BaseLine
                          join s in this.Source on b.ProfileId.GetProfileId(this.Profile) equals s.PersonalProfileIdentifiers.GetProfileId(this.Profile)
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
    }
}
