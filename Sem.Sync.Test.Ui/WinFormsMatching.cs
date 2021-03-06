﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinFormsMatching.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Test the matching logic of the winforms matching UI
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.Ui
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.SharedUI.WinForms.ViewModel;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.Test.DataGenerator;

    /// <summary>
    /// Test the matching logic of the winforms matching UI
    /// </summary>
    [TestClass]
    public class WinFormsMatching
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets simple lists of id-only matched/unmachted/orphaned entries, checks
        ///   the filtered view-lists, matches one entry and checks the result.
        /// </summary>
        [TestMethod]
        public void CheckFilteredViewsAndMatchOneEntry()
        {
            var business = new Matching
                {
                    Source = Contacts.GetMatchingSource().ToStdElements(), 
                    Target = Contacts.GetMatchingTarget().ToStdElements(), 
                    BaseLine = Contacts.GetMatchingBaseline(), 
                    FilterMatchedEntriesSource = true, 
                    FilterMatchedEntriesTarget = true, 
                    Profile = ProfileIdentifierType.XingNameProfileId, 
                };

            Assert.IsFalse(business.SourceAsList().Exist("Matched"));
            Assert.IsTrue(business.SourceAsList().Exist("Unmatched"));
            Assert.IsTrue(business.SourceAsList().Exist("SourceOrphan@Source"));

            Assert.IsFalse(business.TargetAsList().Exist("Matched"));
            Assert.IsTrue(business.TargetAsList().Exist("Unmatched"));
            Assert.IsTrue(business.TargetAsList().Exist("TargetOrphan@Target"));

            business.CurrentSourceElement = business.SourceAsList().GetByXingId("Unmatched").Element;
            business.CurrentTargetElement = business.TargetAsList().GetByXingId("Unmatched").Element;
            business.Match();

            Assert.IsFalse(business.SourceAsList().Exist("Matched"));
            Assert.IsFalse(business.SourceAsList().Exist("Unmatched"));
            Assert.IsTrue(business.SourceAsList().Exist("SourceOrphan@Source"));

            Assert.IsFalse(business.TargetAsList().Exist("Matched"));
            Assert.IsFalse(business.TargetAsList().Exist("Unmatched"));
            Assert.IsTrue(business.TargetAsList().Exist("TargetOrphan@Target"));

            Assert.IsTrue(business.BaselineAsList().Exist(business.Target.ToStdContacts().GetByXingId("Unmatched").Id));
        }

        [TestMethod]
        public void CheckSimple1EntityAddition()
        {
            var business = new Matching
            {
                Source = Contacts.GetMatchingSource().ToStdElements(),
                Target = Contacts.GetMatchingTarget().ToStdElements(),
                BaseLine = Contacts.GetMatchingBaseline(),
                FilterMatchedEntriesSource = true,
                FilterMatchedEntriesTarget = true,
                Profile = ProfileIdentifierType.XingNameProfileId,
            };
        }

        #endregion
    }
}