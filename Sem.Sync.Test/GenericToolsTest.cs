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
    using DataGenerator;
    using GenericHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            // Assert.AreEqual(testClass.arrayOfArrays[1][2], Tools.GetPropertyValueString(testClass, "arrayOfArrays[1][2]"));
            Assert.AreEqual(testClass.arrayOfClasses[1].a, Tools.GetPropertyValueString(testClass, "arrayOfClasses[1].a"));
        }
    }
}
