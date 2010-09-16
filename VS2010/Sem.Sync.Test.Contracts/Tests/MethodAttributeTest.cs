namespace Sem.Sync.Test.Contracts.Tests
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sync.Test.Contracts.Entities;

    /// <summary>
    /// Summary description for MethodAttributeTest
    /// </summary>
    [TestClass]
    public class MethodAttributeTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var results = new MyBusinessComponentSave().InsertCustomer(new MyCustomer { FullName = "Sven", EMailAddress = "don't@spam.me" }).ToList();
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(typeof(StringRegexMatchRule), results[0].RuleType);
            Assert.AreEqual(typeof(Rules.CanNotEnterRule), results[1].RuleType);
        }
    }
}
