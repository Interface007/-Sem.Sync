// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonNameClassTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for PersonNameClassTest
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Summary description for PersonNameClassTest
    /// </summary>
    [TestClass]
    public class PersonNameClassTest
    {
        #region Properties

        ///<summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The constructor test.
        /// </summary>
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

        /// <summary>
        /// The to string test.
        /// </summary>
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

        #endregion
    }
}