namespace Sem.Sync.Test
{
    using GenericHelpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sem.Sync.Test.DataGenerator;

    /// <summary>
    /// Summary description for GenericToolsTest
    /// </summary>
    [TestClass]
    public class GenericToolsTest
    {
        public GenericToolsTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

            Assert.AreEqual(testClass, Tools.GetPropertyValueString(testClass.arr[1], "arr[1]"));
            Assert.AreEqual(testClass, Tools.GetPropertyValueString(testClass.arrayOfArrays[1][2], "arrayOfArrays[1][2]"));
            Assert.AreEqual(testClass, Tools.GetPropertyValueString(testClass.arrayOfClasses[1].a, "arrayOfClasses[1].a"));
        }
    }
}
