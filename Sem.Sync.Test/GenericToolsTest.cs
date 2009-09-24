﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericToolsTest.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Tests the functionality of the generic (non-project-specific) library
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test
{
    using DataGenerator;
    using GenericHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Net;
    using System.Collections.Generic;
    using System;

    public class RecursiveTestClass
    {
        public string[] s { get; set; }
        public RecursiveTestClass[] t { get; set; }
    }

    public class ComplexTestClass
    {
        public NetworkCredential myProp1 { get; set; }
        public NetworkCredential myProp2 { get; set; }
        public NetworkCredential[] myProp3 { get; set; }
        public List<NetworkCredential> myProp4 { get; set; }
        public Dictionary<string, NetworkCredential> myProp5 { get; set; }

        public DateTime? myProp6 { get; set; }
        public DateTime? myProp7 { get; set; }
        public int? myProp8 { get; set; }

        public ComplexTestClass()
        {
            this.myProp1 = new NetworkCredential("sven", "geheim1", "domain");

            this.myProp2 = null;

            this.myProp3 = new[]
                {
                    new NetworkCredential("sven", "geheim2", "domain"), new NetworkCredential("sven", "geheim3", "domain")
                };

            this.myProp4 = new List<NetworkCredential>
                {
                    new NetworkCredential("sven", "geheim4", "domain"),
                    new NetworkCredential("sven", "geheim5", "domain")
                };

            this.myProp5 = new Dictionary<string, NetworkCredential>
                {
                    { "key1", new NetworkCredential("sven", "geheim6", "domain") },
                    { "key2", new NetworkCredential("sven", "geheim7", "domain") }
                };

            this.myProp6 = new DateTime(2009, 7, 8);
        }

        public DateTime AsString()
        {
            return this.myProp6 ?? new DateTime(1900, 1, 1);
        }

        public DateTime AsString(string defaultValue)
        {
            return this.myProp6 ?? DateTime.Parse(defaultValue);
        }
    }

    /// <summary>
    /// Tests the functionality of the generic (non-project-specific) library
    /// </summary>
    [TestClass]
    public class GenericToolsTest
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
        /// Tests the functionality to read object paths from an object
        /// </summary>
        [TestMethod]
        public void GetPropertyValueTest()
        {
            var testData = Contacts.GetStandardContactList(true);

            Assert.AreEqual(testData[2].Name.FirstName, Tools.GetPropertyValueString(testData[2], "Name.FirstName"));
            Assert.AreEqual(testData[2].PersonalProfileIdentifiers.DefaultProfileId, Tools.GetPropertyValueString(testData, "[2].PersonalProfileIdentifiers.DefaultProfileId"));

            var testClass = new
                {
                    arr = new[] { "str1", "str2", "str3" },
                    arrayOfArrays = new[]
                        {
                            new[] { "str11", "str12", "str13" },
                            new[] { "str21", "str22", "str23" }
                        },
                    arrayOfClasses = new[]
                        {
                            new { a = "str31", b = "str32", c = "str33" },
                            new { a = "str41", b = "str42", c = "str43" },
                        }
                };

            Assert.AreEqual(testClass.arr[1], Tools.GetPropertyValueString(testClass, "arr[1]"));
            //// Assert.AreEqual(testClass.arrayOfArrays[1][2], Tools.GetPropertyValueString(testClass, "arrayOfArrays[1][2]"));
            Assert.AreEqual(testClass.arrayOfClasses[1].a, Tools.GetPropertyValueString(testClass, "arrayOfClasses[1].a"));
        }

        [TestMethod]
        public void GetPropertyValueTestCoBa1()
        {
            var t = new RecursiveTestClass
                {
                    s = new[] { "hallo", "123" }, 
                    t = new RecursiveTestClass[5]
                };

            t.t[0] = new RecursiveTestClass
                {
                    s = new[] { "Ichbineintest", "nocheintest" }
                };

            var s = Tools.GetPropertyValue(t, ".t[0].s[1]") as string;
        }

        [TestMethod]
        public void GetPropertyValueTestCoBa2()
        {
            var x = new ComplexTestClass();

            Assert.AreEqual("geheim1", Tools.GetPropertyValue(x, "myProp1.Password"));
            Assert.IsNull(Tools.GetPropertyValue(x, "myProp2.Password"));
            Assert.AreEqual("geheim3", Tools.GetPropertyValue(x, "myProp3[1].Password"));
            Assert.AreEqual("geheim4", Tools.GetPropertyValue(x, "myProp4[0].Password"));
            Assert.AreEqual("geheim7", Tools.GetPropertyValue(x, "myProp5[key2].Password"));
            
            Assert.AreEqual(new DateTime(2009, 7, 8), Tools.GetPropertyValue(x, "myProp6"));
            Assert.AreEqual("08.07.2009 00:00:00", Tools.GetPropertyValueString(x, "myProp6"));
            Assert.AreEqual("08.07.2009 00:00:00", Tools.GetPropertyValueString(x, "AsString()"));
            Assert.AreEqual("08.07.2009 00:00:00", Tools.GetPropertyValueString(x, "AsString(----)"));
            
            Assert.IsNull(Tools.GetPropertyValue(x, "myProp7"));
            Assert.AreEqual(string.Empty, Tools.GetPropertyValueString(x, "myProp7"));

            Assert.IsNull(Tools.GetPropertyValue(x, "myProp4[20?].Password"));
            Assert.IsNull(Tools.GetPropertyValue(x, "myProp5[nonexistingkey?].Password"));
        }

        [TestMethod]
        public void map()
        {
            var x = new ComplexTestClass();
            var y = new NetworkCredential
                {
                    Domain = x.myProp1.NewIfNull().Domain, 
                    Password = x.myProp3.NewIfNull(5).Password
                };

            Console.WriteLine(y.Domain);
        }
    }

    public static class Ex
    {
        public static T NewIfNull<T>(this T testObject) where T : class, new()
        {
            return testObject ?? new T();
        }

        public static T NewIfNull<T>(this T[] testObject, int index) where T : class, new()
        {
            var x = (testObject ?? new T[0]);
            return x.Length > index ? x[index] : new T();
        }
        
        public static T NewIfNull<T>(this List<T> testObject, int index) where T : class, new()
        {
            var x = (testObject ?? new List<T>());
            return x.Count > index ? x[index] : new T();
        }
        
        public static T NewIfNull<T>(this Dictionary<string, T> testObject, string index) where T : class, new()
        {
            var x = (testObject ?? new Dictionary<string, T>());
            return x.ContainsKey(index) ? x[index] : new T();
        }
    }
}
