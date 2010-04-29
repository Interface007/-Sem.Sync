namespace Sem.Sync.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;

    using DataGenerator;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase.Helpers;

    /// <summary>
    /// Summary description for CommandMatchByProfileTest
    /// </summary>
    [TestClass]
    public class CommandMatchByProfileTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }
            set
            {
                this.testContextInstance = value;
            }
        }

        [TestMethod]
        public void MatchByProfileCheck1()
        {
            var command = new SyncBase.Commands.MatchByProfileId();
            var client = new Contacts();
            
            // matches a test source with undefined (new generated) Ids to the baseline - two of the 3 items can be matched
            command.ExecuteCommand(client, client, client, "matchingtestsource", "matchingtesttarget", "matchingtestbaseline", string.Empty);

            // the target did contain nothing, and now should contain the updated entries
            // two entries should have the know matchable ids
            var target = new Contacts().GetAll("matchingtesttarget").ToStdContacts();
            Assert.AreEqual(3, target.Count, "target count");
            Assert.AreEqual(new Guid("{2191B8BB-40AE-4052-B8AC-89776BB47865}"), target[0].Id, "target match 1");
            Assert.AreEqual(new Guid("{B79B71B6-2FE5-492b-B5B1-8C373D6F4D64}"), target[1].Id, "target match 2");
            
            // the base line must not be changed (still three entries)
            var baseline = new Contacts().GetAll("matchingtestbaseline").ToStdContacts();
            Assert.AreEqual(3, baseline.Count, "baseline count");
        }
    }
}
