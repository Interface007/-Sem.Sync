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
    using System;
    using System.Collections.Generic;
    using System.Net;

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

        [TestMethod]
        public void TestMapIfExistWith1Step()
        {
            var source = new ComplexTestClass();
            var target = new NetworkCredentials();

            // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            source.myProp2.MapIfExist(y => y.Password, ref target.Passwort);
            Assert.IsNull(target.Passwort);

            // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source.myProp2.MapIfExist(y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
            target.Passwort = "hallo";
            source.myProp1.MapIfExist(y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "geheim1");
        }

        [TestMethod]
        public void TestMapIfExistWith2Steps()
        {
            var source = new ComplexTestClass();
            var target = new NetworkCredentials();

            // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            source.MapIfExist(y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsNull(target.Passwort);

            // source.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source.MapIfExist(y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
            target.Passwort = "hallo";
            source.MapIfExist(y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "geheim1");

            // source is null, so source.myProp1.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source = null;
            source.MapIfExist(y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");
        }

        [TestMethod]
        public void TestMapIfExistWith3Steps()
        {
            var source = new { x = new ComplexTestClass() };
            var source2 = new { x = null as ComplexTestClass };
            var source3 = new { x = new ComplexTestClass { myProp1 = new NetworkCredential("name3", "pass3") } };
            var target = new NetworkCredentials();

            // source.x.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            source.MapIfExist(y => y.x, y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsNull(target.Passwort);

            // source.x.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source.MapIfExist(y => y.x, y => y.myProp2, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source.x.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
            target.Passwort = "hallo";
            source.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "geheim1");

            target.Passwort = "hallo";
            source.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort);  // now it's "geheim1"
            source3.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort); // now it's "pass3"
            Assert.IsTrue(target.Passwort == "pass3");

            // source2.x is null, so source.myProp1.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source2.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source2 is null, so source.myProp1.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source2 = null;
            source2.MapIfExist(y => y.x, y => y.myProp1, y => y.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");
        }

        [TestMethod]
        public void TestMapIfExistWithExpressionTree()
        {
            var source1 = new { x = new ComplexTestClass() };
            var source2 = new { x = null as ComplexTestClass };
            var source3 = new { x = new ComplexTestClass { myProp1 = new NetworkCredential("name3", "pass3") } };
            var target = new NetworkCredentials();

            // source.x.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            source1.MapIfExist2(y => y.x.myProp2.Password, ref target.Passwort);
            Assert.IsNull(target.Passwort);

            // source.x.myProp2 is null, so source.myProp2.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source1.MapIfExist2(y => y.x.myProp2.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source.x.myProp1 is "geheim1", so source.myProp1.Password can be evaluated and x should be updated
            target.Passwort = "hallo";
            source1.MapIfExist2(y => y.x.myProp1.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "geheim1");

            target.Passwort = "hallo";
            source1.MapIfExist2(y => y.x.myProp1.Password, ref target.Passwort);    // now it's "geheim1"
            source3.MapIfExist2(y => y.x.myProp1.Password, ref target.Passwort);    // now it's "pass3"
            Assert.IsTrue(target.Passwort == "pass3");

            // source2.x is null, so source.myProp1.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo";
            source2.MapIfExist2(y => y.x.myProp1.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo");

            // source2 is null, so source.myProp1.Password cannot be evaluated and x should stay the same
            target.Passwort = "hallo2";
            source2 = null;
            source2.MapIfExist2(y => y.x.myProp1.Password, ref target.Passwort);
            Assert.IsTrue(target.Passwort == "hallo2");
        }

        [TestMethod]
        public void MapIfDiffersTest()
        {
            var source1 = new ComplexTestClass();
            var source2 = new ComplexTestClass();
            var target = new NetworkCredentials();

            var dirty = false;

            // both NULL : no mapping
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp2.Password, x => target.Passwort = x);
            Assert.IsFalse(dirty);

            // both "geheim1" : no mapping
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp1.Password, x => target.Passwort = x);
            Assert.IsFalse(dirty);

            // source1 = "geheim1", source2 = "something completely different" : !!mapping!!
            source2.myProp1.Password = "something completely different";
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp1.Password, x => target.Passwort = x);
            Assert.IsTrue(dirty);

            // source1 = NULL, source2 = "something completely different" : !!mapping!!
            source1.myProp1 = null;
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp1.Password, x => target.Passwort = x);
            Assert.IsTrue(dirty);
            Assert.IsNull(target.Passwort);

            // source1 = NULL, source2 = "something completely different" : !!mapping!!
            source1.myProp1 = null;
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp1.Password.ToString(), x => target.Passwort = x);
            Assert.IsTrue(dirty);
            Assert.IsNull(target.Passwort);

            source1.myProp6 = new DateTime(2010, 7, 1);
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp6, x => source2.myProp6 = x);
            Assert.IsTrue(dirty);
            Assert.AreEqual(source2.myProp6, source1.myProp6);

            source1.myProp9 = new DateTime(2010, 7, 1);
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp9, x => source2.myProp9 = x);
            Assert.IsTrue(dirty);
            Assert.AreEqual(source2.myProp9, source1.myProp9);

            source1.myProp9 = new DateTime();
            source1.myProp10 = new ComplexTestClass { myProp9 = new DateTime(2010, 7, 6) };
            MappingHelper.MapIfDiffers(ref dirty, source1, source2, y => y.myProp10.myProp9, x => source2.myProp9 = x);
            Assert.IsTrue(dirty);
            Assert.AreEqual(new DateTime(2010, 7, 6), source2.myProp9);
            Assert.AreEqual(new DateTime(2010, 7, 6), source1.myProp10.myProp9);
        }

        [TestMethod]
        public void SpeedTest()
        {
            for (int i = 0; i < 9; i++)
            {
                this.TestMapIfExistWith3Steps();
            }

            for (int i = 0; i < 9; i++)
            {
                this.TestMapIfExistWithExpressionTree();
            }

            var start = DateTime.Now;
            for (int i = 0; i < 999; i++)
            {
                this.TestMapIfExistWith3Steps();
            }

            Console.WriteLine(start - DateTime.Now);
            start = DateTime.Now;

            for (int i = 0; i < 999; i++)
            {
                this.TestMapIfExistWithExpressionTree();
            }

            Console.WriteLine(start - DateTime.Now);
        }

        /// <summary>
        /// performs a basic test for the "is one of" method
        /// </summary>
        [TestMethod]
        public void IsOneOfBasicTest()
        {
            const string testObject = "check";

            Assert.IsTrue(testObject.IsOneOf("isnot", "no", "check", "test"));
            Assert.IsTrue(testObject.IsOneOf("check", "no", "check", "test"));
            Assert.IsTrue(testObject.IsOneOf("isnot", "no", "test1", "check"));
            Assert.IsTrue(testObject.IsOneOf("check"));
            Assert.IsFalse(testObject.IsOneOf("isnot", "no", "test1", "nocheck"));
            Assert.IsFalse(testObject.IsOneOf("isnot"));
            Assert.IsFalse(testObject.IsOneOf());
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