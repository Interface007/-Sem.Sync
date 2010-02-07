﻿// --------------------------------------------------------------------------------------------------------------------
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

        [TestMethod]
        public void TestAccessor()
        {
            var source = new ComplexTestClass();
            var target = new NetworkCredentials();

            // testClass.myProp2 is null, so testClass.myProp2.Password cannot be evaluated and x should stay the same
            source.MapIfExist(y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsNull(target.Passwort);

            // testClass.myProp2 is null, so testClass.myProp2.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source.MapIfExist(y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // testClass.myProp1 is "", so testClass.myProp2.Password can be evaluated and x should be updated
            target.Passwort = "hallo";
            source.MapIfExist(y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "geheim1");
        }

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

        /// <summary>
        /// Tests if the extension method NewIfNull does run correctly
        /// </summary>
        [TestMethod]
        public void NewIfNullTest()
        {
            var x = new ComplexTestClass();

            // simple reference type
            Assert.AreEqual("geheim1", x.myProp1.NewIfNull().Password);
            Assert.AreEqual(string.Empty, x.myProp2.NewIfNull().Domain);

            // array
            Assert.AreEqual("domain", x.myProp3.NewIfNull(0).Domain);
            Assert.AreEqual(string.Empty, x.myProp3.NewIfNull(20).Domain);

            // list
            Assert.AreEqual("domain", x.myProp4.NewIfNull(0).Domain);
            Assert.AreEqual(string.Empty, x.myProp4.NewIfNull(70).Domain);

            // dictionary
            Assert.AreEqual("domain", x.myProp5.NewIfNull("key2").Domain);
            Assert.AreEqual(string.Empty, x.myProp5.NewIfNull("nonexistingKey").Domain);
        }
    }

    public class NetworkCredentials
    {
        public string Passwort;
    }
}