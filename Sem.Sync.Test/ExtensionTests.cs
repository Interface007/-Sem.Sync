// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionTests.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Tests the Extension Methods from the generic helpers
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test
{
    using System.Collections.Generic;

    using GenericHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SyncBase.Helpers;

    /// <summary>
    /// Tests the Extension Methods from the generic helpers
    /// </summary>
    [TestClass]
    public class ExtensionTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
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
        #endregion

        /// <summary>
        /// performs a basic test for the "is one of" method
        /// </summary>
        [TestMethod]
        public void IsOneOfBasicTest()
        {
            const string TestObject = "check";

            Assert.IsTrue(TestObject.IsOneOf("isnot", "no", "check", "test"));
            Assert.IsTrue(TestObject.IsOneOf("check", "no", "check", "test"));
            Assert.IsTrue(TestObject.IsOneOf("isnot", "no", "test1", "check"));
            Assert.IsTrue(TestObject.IsOneOf("check"));
            Assert.IsFalse(TestObject.IsOneOf("isnot", "no", "test1", "nocheck"));
            Assert.IsFalse(TestObject.IsOneOf("isnot"));
            Assert.IsFalse(TestObject.IsOneOf());
        }

        /// <summary>
        /// performs a basic test for the "is one of" method
        /// </summary>
        [TestMethod]
        public void ConcatElementsToStringTest()
        {
            Assert.AreEqual("hello world", (new List<string> { "hello", "world" }).ConcatElementsToString(" "));
            Assert.AreEqual("hello world", (new List<string> { "hello world" }).ConcatElementsToString(" "));
            Assert.AreEqual("hello world !", (new List<string> { "hello", "world", "!" }).ConcatElementsToString(" "));
            Assert.AreEqual("hello-world-!", (new List<string> { "hello", "world", "!" }).ConcatElementsToString("-"));
            Assert.AreEqual(string.Empty, (new List<string>()).ConcatElementsToString("-"));
        }
    }
}