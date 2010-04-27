
namespace Sem.Sync.Test.Ui
{
    using DataGenerator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.SharedUI.WinForms.ViewModel;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    
    /// <summary>
    /// Test the matching logic of the winforms matching UI
    /// </summary>
    [TestClass]
    public class WinFormsMatching
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Gets simple lists of id-only matched/unmachted/orphaned entries, checks
        /// the filtered view-lists, matches one entry and checks the result.
        /// </summary>
        [TestMethod]
        public void CheckFilteredViewsAndMatchOneEntry()
        {
            var business = new Matching
                               {
                                   Source = Contacts.GetMatchingSource().ToStdElement(),
                                   Target = Contacts.GetMatchingTarget().ToStdElement(),
                                   BaseLine = Contacts.GetMatchingBaseline(),
                                   FilterMatchedEntriesSource = true,
                                   FilterMatchedEntriesTarget = true,
                                   Profile = ProfileIdentifierType.XingNameProfileId,
                               };

            Assert.IsFalse(business.SourceAsList().Exist("Matched"));
            Assert.IsTrue(business.SourceAsList().Exist("Unmatched"));
            Assert.IsTrue(business.SourceAsList().Exist("New"));
            
            Assert.IsFalse(business.TargetAsList().Exist("Matched"));
            Assert.IsTrue(business.TargetAsList().Exist("Unmatched"));
            Assert.IsTrue(business.TargetAsList().Exist("TargetOrphan"));

            business.CurrentSourceElement = business.SourceAsList().GetByXingId("Unmatched").Element;
            business.CurrentTargetElement = business.TargetAsList().GetByXingId("Unmatched").Element;
            business.Match();

            Assert.IsFalse(business.SourceAsList().Exist("Matched"));
            Assert.IsFalse(business.SourceAsList().Exist("Unmatched"));
            Assert.IsTrue(business.SourceAsList().Exist("New"));

            Assert.IsFalse(business.TargetAsList().Exist("Matched"));
            Assert.IsFalse(business.TargetAsList().Exist("Unmatched"));
            Assert.IsTrue(business.TargetAsList().Exist("TargetOrphan"));

            Assert.IsTrue(business.BaselineAsList().Exist(business.Target.ToContacts().GetByXingId("Unmatched").Id));
        }
    }
}
