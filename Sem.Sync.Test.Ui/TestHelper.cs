﻿using Sem.Sync.SyncBase.DetailData;

namespace Sem.Sync.Test.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SharedUI.WinForms.ViewModel;

    using SyncBase;

    public static class TestHelper
    {
        public static bool Exist(this List<MatchView> list, Guid id)
        {
            return (from x in list
                 where x.BaselineId == id
                 select x).Count() == 1;
        }

        public static bool Exist(this IEnumerable<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                 select x).Count() == 1;
        }

        public static MatchCandidateView GetByXingId(this IEnumerable<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).FirstOrDefault();
        }

        public static StdContact GetByXingId(this IEnumerable<StdContact> list, string xingId)
        {
            return (from x in list
                    where x.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).FirstOrDefault();
        }
    }
}