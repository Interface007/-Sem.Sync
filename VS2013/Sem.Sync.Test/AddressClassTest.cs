// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressClassTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for AddressClassTest
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Summary description for AddressClassTest
    /// </summary>
    [TestClass]
    public class AddressClassTest
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The address constructor test 01.
        /// </summary>
        [TestMethod]
        public void AddressConstructorTest01()
        {
            CheckConstructorStreet("Birkenweg 21a", "Birkenweg 21a");
        }

        /// <summary>
        /// The address constructor test 02.
        /// </summary>
        [TestMethod]
        public void AddressConstructorTest02()
        {
            CheckConstructorCity("35586 Wetzlar", "Wetzlar", "35586");
            CheckConstructorCity("35586 Frankfurt am Main", "Frankfurt am Main", "35586");
            CheckConstructorCity("35586 Frankfurt (am Main)", "Frankfurt (am Main)", "35586");
            CheckConstructorCity("35586", string.Empty, "35586");
        }

        /// <summary>
        /// The address constructor test 03.
        /// </summary>
        [TestMethod]
        public void AddressConstructorTest03()
        {
            CheckConstructorCountry("Germany", "Germany");
            CheckConstructorCountry("New Zeland", "New Zeland");
            CheckConstructorCountry("Korea (Nord)", "Korea (Nord)");
        }

        /// <summary>
        /// The address constructor test 04.
        /// </summary>
        [TestMethod]
        public void AddressConstructorTest04()
        {
            CheckConstructorStreet("Birkenweg 21a\n35586 Wetzlar\nGermany", "Birkenweg 21a");
            CheckConstructorCity("Birkenweg 21a\n35586 Wetzlar\nGermany", "Wetzlar", "35586");
            CheckConstructorCountry("Birkenweg 21a\n35586 Wetzlar\nGermany", "Germany");

            CheckConstructorStreet("Birkenweg 21a\nGermany", "Birkenweg 21a");
            CheckConstructorCity("Birkenweg 21a\nGermany", string.Empty, string.Empty);
            CheckConstructorCountry("Birkenweg 21a\nGermany", "Germany");

            CheckConstructorStreet("Birkenweg 21a\n\n\n\nGermany", "Birkenweg 21a");
            CheckConstructorCity("Birkenweg 21a\n\n\n\nGermany", string.Empty, string.Empty);
            CheckConstructorCountry("Birkenweg 21a\n\n\n\nGermany", "Germany");
        }

        /// <summary>
        /// The address equal test.
        /// </summary>
        [TestMethod]
        public void AddressEqualTest()
        {
            Assert.AreEqual(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain"), 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper disable EqualExpressionComparison
            Assert.IsTrue(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper restore EqualExpressionComparison
            Assert.IsFalse(
                new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(
                new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(
                new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(
                new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(
                new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain") ==
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper disable EqualExpressionComparison
            Assert.IsFalse(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper restore EqualExpressionComparison
            Assert.IsTrue(
                new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(
                new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(
                new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(
                new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(
                new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain") !=
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
        }

        /// <summary>
        /// The address to string test 01.
        /// </summary>
        [TestMethod]
        public void AddressToStringTest01()
        {
            Assert.AreEqual(
                "Birkenweg 21a / 35586 Wetzlar Germany", 
                new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany").ToString());
        }

        #endregion

        #region Methods

        /// <summary>
        /// The check constructor city.
        /// </summary>
        /// <param name="checkThis">
        /// The check this.
        /// </param>
        /// <param name="city">
        /// The city.
        /// </param>
        /// <param name="postalCode">
        /// The postal code.
        /// </param>
        private static void CheckConstructorCity(string checkThis, string city, string postalCode)
        {
            var address = new AddressDetail(checkThis);
            if (string.IsNullOrEmpty(city))
            {
                Assert.IsTrue(string.IsNullOrEmpty(address.CityName));
            }
            else
            {
                Assert.AreEqual(city, address.CityName);
            }

            if (string.IsNullOrEmpty(postalCode))
            {
                Assert.IsTrue(string.IsNullOrEmpty(address.PostalCode));
            }
            else
            {
                Assert.AreEqual(postalCode, address.PostalCode);
            }
        }

        /// <summary>
        /// The check constructor country.
        /// </summary>
        /// <param name="checkThis">
        /// The check this.
        /// </param>
        /// <param name="countryName">
        /// The country name.
        /// </param>
        private static void CheckConstructorCountry(string checkThis, string countryName)
        {
            var address = new AddressDetail(checkThis);
            if (string.IsNullOrEmpty(countryName))
            {
                Assert.IsTrue(string.IsNullOrEmpty(address.CountryName));
            }
            else
            {
                Assert.AreEqual(countryName, address.CountryName);
            }
        }

        /// <summary>
        /// The check constructor street.
        /// </summary>
        /// <param name="checkThis">
        /// The check this.
        /// </param>
        /// <param name="street">
        /// The street.
        /// </param>
        private static void CheckConstructorStreet(string checkThis, string street)
        {
            var address = new AddressDetail(checkThis);
            Assert.AreEqual(street, address.StreetName);
        }

        #endregion
    }
}