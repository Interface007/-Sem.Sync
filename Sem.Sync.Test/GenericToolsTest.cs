// --------------------------------------------------------------------------------------------------------------------
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using GenericHelpers.Entities;
    using GenericHelpers.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.Sync.Test.DataGenerator;

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
            Assert.AreEqual("nocheintest", s);
            
            s = Tools.GetPropertyValue(t, ".s[0]") as string;
            Assert.AreEqual("hallo", s);
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
        public void GetPropertyNulls()
        {
            var x = new ComplexTestClass();
            Assert.AreEqual("default", x.NewIfNull().myProp2.NewIfNull().Domain.DefaultIfNullOrEmpty("default"));
        }

        [TestMethod]
        public void CheckCachePathGeneration()
        {
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", Tools.GetSha1Hash(string.Empty));
            Assert.AreEqual("da39a3ee5e6b4b0d3255bfef95601890afd80709", Tools.GetSha1Hash(null));
            Assert.AreEqual("48af14dbae9dd04377e61d02ac07126e7e75e65e", Tools.GetSha1Hash("a little test"));
        }

        [TestMethod]
        public void CheckQuotedPrintableTest()
        {
            Assert.AreEqual(string.Empty, Tools.EncodeToQuotedPrintable(string.Empty));
            Assert.AreEqual(string.Empty, Tools.EncodeToQuotedPrintable(null));
            Assert.AreEqual("a little test", Tools.EncodeToQuotedPrintable("a little test"));
            Assert.AreEqual("g&#=dc=e4=d6(?%&j8?=e4=f6=fc", Tools.EncodeToQuotedPrintable("g&#ÜäÖ(?%&j8?äöü"));

            Assert.AreEqual("g&#ÜäÖ(?%&j8?äöü", Tools.DecodeFromQuotedPrintable("g&#=dc=e4=d6(?%&j8?=e4=f6=fc"));
            Assert.AreEqual("a little test", Tools.DecodeFromQuotedPrintable("a little test"));
            Assert.AreEqual(string.Empty, Tools.DecodeFromQuotedPrintable(null));
            Assert.AreEqual(string.Empty, Tools.DecodeFromQuotedPrintable(string.Empty));
        }

        [TestMethod]
        public void SimpleExceptionTest()
        {
            try
            {
                File.WriteAllText("invalid Path:\\\\\\\\::?!", "test");
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        [TestMethod]
        public void TechnicalExceptionTest()
        {
            var myValueToLog = new Triple<string, string, string>
                {
                    Value1 = "hello", 
                    Value2 = "world", 
                    Value3 = "!" 
                };

            var exceptionText = string.Empty;

            try
            {
                try
                {
                    File.WriteAllText("invalid Path:\\\\\\\\::?!", "test");
                }
                catch (Exception ex)
                {
                    throw new TechnicalException(
                        "bad file name",
                        ex,
                        new KeyValuePair<string, object>("myTriple", myValueToLog),
                        new KeyValuePair<string, object>("myString", "hello world"),
                        new KeyValuePair<string, object>("myInt", 49));
                }
            }
            catch (Exception ex)
            {
                exceptionText = ExceptionHandler.HandleException(ex);
            }

            Assert.IsTrue(exceptionText.Contains("<string name=\"myString\">hello world</string>"));
            Assert.IsTrue(exceptionText.Contains("<int name=\"myInt\">49</int>"));
            Assert.IsTrue(exceptionText.Contains("<TripleOfStringStringString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" name=\"myTriple\">"));
            Assert.IsTrue(exceptionText.Contains("<Value1>hello</Value1>"));
            Assert.IsTrue(exceptionText.Contains("<Timestamp>" + DateTime.Now.Year));
            Assert.IsTrue(exceptionText.Contains("vstesthost.exe</ExecutingMainModule>"));
            Assert.IsTrue(exceptionText.Contains("<SpecificInformation>Sem.GenericHelpers.Exceptions.TechnicalException: bad file name"));
            Assert.IsTrue(exceptionText.Contains("<SpecificInformation>System.ArgumentException: Illegales Zeichen im Pfad."));
        }

        [TestMethod]
        public void CryptoTest()
        {
            var input = "11234567890kjhgfdsaqwertzuiomnbvcxywertzuio8ztrfdxy34r5tzhuj!%&WDFCVZUJKOIJHNk098u7ztrfdä#ölkjh";
            Assert.AreEqual(input, Tools.DecryptString(Tools.EncryptString(input, 2048, ""), 2048, ""));
        }
    }
}
