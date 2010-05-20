// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   The test helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sem.Sync.SharedUI.WinForms.ViewModel;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// The test helper.
    /// </summary>
    public static class TestHelper
    {
        #region Public Methods

        /// <summary>
        /// The exist.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The exist.
        /// </returns>
        public static bool Exist(this IList<MatchView> list, Guid id)
        {
            return (from x in list where x.BaselineId == id select x).Count() == 1;
        }

        /// <summary>
        /// The exist.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="xingId">
        /// The xing id.
        /// </param>
        /// <returns>
        /// The exist.
        /// </returns>
        public static bool Exist(this IEnumerable<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).Count() == 1;
        }

        /// <summary>
        /// The get by xing id.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="xingId">
        /// The xing id.
        /// </param>
        /// <returns>
        /// </returns>
        public static MatchCandidateView GetByXingId(this IEnumerable<MatchCandidateView> list, string xingId)
        {
            return (from x in list
                    where x.Element.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                    select x).FirstOrDefault();
        }

        /// <summary>
        /// The get by xing id.
        /// </summary>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="xingId">
        /// The xing id.
        /// </param>
        /// <returns>
        /// </returns>
        public static StdContact GetByXingId(this IEnumerable<StdContact> list, string xingId)
        {
            return
                (from x in list
                 where x.ExternalIdentifier.GetProfileId(ProfileIdentifierType.XingNameProfileId) == xingId
                 select x).FirstOrDefault();
        }

        #endregion
    }
}