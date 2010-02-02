using Sem.Sync.SyncBase.DetailData;

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

        public static bool Exist(this List<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.PersonalProfileIdentifiers.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                 select x).Count() == 1;
        }

        public static MatchCandidateView GetByXingId(this List<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.PersonalProfileIdentifiers.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).FirstOrDefault();
        }

        public static StdContact GetByXingId(this List<StdContact> list, string xingId)
        {
            return (from x in list
                    where x.PersonalProfileIdentifiers.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).FirstOrDefault();
        }
    }
}