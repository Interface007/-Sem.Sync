﻿namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SyncBase.DetailData;

    /// <summary>
    /// Summary description for PersonNameClassTest
    /// </summary>
    [TestClass]
    public class PersonNameClassTest
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
        public void ConstructorTest()
        {
            PersonName name;

            name = new PersonName("Sven Erik Matzen");
            Assert.IsTrue(string.IsNullOrEmpty(name.AcademicTitle));
            Assert.AreEqual("Sven", name.FirstName);
            Assert.AreEqual("Erik", name.MiddleName);
            Assert.AreEqual("Matzen", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.Suffix));
            Assert.IsTrue(string.IsNullOrEmpty(name.FormerName));
            Assert.AreEqual("Matzen, Sven Erik", name.ToString());

            name = new PersonName("Sven Matzen");
            Assert.IsTrue(string.IsNullOrEmpty(name.AcademicTitle));
            Assert.AreEqual("Sven", name.FirstName);
            Assert.IsTrue(string.IsNullOrEmpty(name.MiddleName));
            Assert.AreEqual("Matzen", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.Suffix));
            Assert.IsTrue(string.IsNullOrEmpty(name.FormerName));
            Assert.AreEqual("Matzen, Sven", name.ToString());

            name = new PersonName("Matzen, Sven Erik");
            Assert.IsTrue(string.IsNullOrEmpty(name.AcademicTitle));
            Assert.AreEqual("Sven", name.FirstName);
            Assert.AreEqual("Erik", name.MiddleName);
            Assert.AreEqual("Matzen", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.Suffix));
            Assert.IsTrue(string.IsNullOrEmpty(name.FormerName));
            Assert.AreEqual("Matzen, Sven Erik", name.ToString());

            name = new PersonName("Matzen, Sven");
            Assert.IsTrue(string.IsNullOrEmpty(name.AcademicTitle));
            Assert.AreEqual("Sven", name.FirstName);
            Assert.IsTrue(string.IsNullOrEmpty(name.MiddleName));
            Assert.AreEqual("Matzen", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.Suffix));
            Assert.IsTrue(string.IsNullOrEmpty(name.FormerName));
            Assert.AreEqual("Matzen, Sven", name.ToString());

            name = new PersonName("Matzen (Dr.), Sven Erik");
            Assert.AreEqual("Dr.", name.AcademicTitle);
            Assert.AreEqual("Sven", name.FirstName);
            Assert.AreEqual("Erik", name.MiddleName);
            Assert.AreEqual("Matzen", name.LastName);
            Assert.IsTrue(string.IsNullOrEmpty(name.Suffix));
            Assert.IsTrue(string.IsNullOrEmpty(name.FormerName));
            Assert.AreEqual("Matzen (Dr.), Sven Erik", name.ToString());
        }

        [TestMethod]
        public void ToStringTest()
        {
            PersonName name;

            name = new PersonName("Sven Erik Matzen");
            Assert.AreEqual("Matzen, Sven Erik", name.ToString());

            name = new PersonName("Sven Matzen");
            Assert.AreEqual("Matzen, Sven", name.ToString());

            name = new PersonName("Matzen, Sven Erik");
            Assert.AreEqual("Matzen, Sven Erik", name.ToString());

            name = new PersonName("Matzen, Sven");
            Assert.AreEqual("Matzen, Sven", name.ToString());

            name = new PersonName("Matzen (Dr.), Sven Erik");
            Assert.AreEqual("Matzen (Dr.), Sven Erik", name.ToString());
        }
    }
}
