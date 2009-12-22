namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase.DetailData;

    /// <summary>
    /// Summary description for AddressClassTest
    /// </summary>
    [TestClass]
    public class AddressClassTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AddressConstructorTest01()
        {
            CheckConstructorStreet("Birkenweg 21a", "Birkenweg", 21, "a");
            CheckConstructorStreet("Birken Weg 21a", "Birken Weg", 21, "a");
            CheckConstructorStreet("Birken-Weg 21a", "Birken-Weg", 21, "a");
            CheckConstructorStreet("Birkenweg 21", "Birkenweg", 21, string.Empty);
            CheckConstructorStreet("Birken Weg 21", "Birken Weg", 21, string.Empty);
            CheckConstructorStreet("Birken-Weg 21", "Birken-Weg", 21, string.Empty);
            CheckConstructorStreet("Birkenweg 21a-z", "Birkenweg", 21, "a-z");
            CheckConstructorStreet("Birkenweg 21 a-z", "Birkenweg", 21, "a-z");
            CheckConstructorStreet("Birkenweg 21 a - z", "Birkenweg", 21, "a - z");
            CheckConstructorStreet("Birkenweg 21a -z", "Birkenweg", 21, "a -z");
            CheckConstructorStreet("Birkenweg 21a- z", "Birkenweg", 21, "a- z");
            CheckConstructorStreet("Birkenweg 21a - z", "Birkenweg", 21, "a - z");
        }

        [TestMethod]
        public void AddressConstructorTest02()
        {
            CheckConstructorCity("35586 Wetzlar", "Wetzlar", "35586");
            CheckConstructorCity("35586 Frankfurt am Main", "Frankfurt am Main", "35586");
            CheckConstructorCity("35586 Frankfurt (am Main)", "Frankfurt (am Main)", "35586");
            CheckConstructorCity("35586", string.Empty, "35586");
        }

        [TestMethod]
        public void AddressConstructorTest03()
        {
            CheckConstructorCountry("Germany", "Germany");
            CheckConstructorCountry("New Zeland", "New Zeland");
            CheckConstructorCountry("Korea (Nord)", "Korea (Nord)");
        }

        [TestMethod]
        public void AddressConstructorTest04()
        {
            CheckConstructorStreet("Birkenweg 21a\n35586 Wetzlar\nGermany", "Birkenweg", 21, "a");
            CheckConstructorCity("Birkenweg 21a\n35586 Wetzlar\nGermany", "Wetzlar", "35586");
            CheckConstructorCountry("Birkenweg 21a\n35586 Wetzlar\nGermany", "Germany");

            CheckConstructorStreet("Birkenweg 21a\nGermany", "Birkenweg", 21, "a");
            CheckConstructorCity("Birkenweg 21a\nGermany", string.Empty, string.Empty);
            CheckConstructorCountry("Birkenweg 21a\nGermany", "Germany");

            CheckConstructorStreet("Birkenweg 21a\n\n\n\nGermany", "Birkenweg", 21, "a");
            CheckConstructorCity("Birkenweg 21a\n\n\n\nGermany", string.Empty, string.Empty);
            CheckConstructorCountry("Birkenweg 21a\n\n\n\nGermany", "Germany");
        }

        [TestMethod]
        public void AddressToStringTest01()
        {
            Assert.AreEqual("Birkenweg 21 a / 35586 Wetzlar Germany", new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany").ToString());
        }

        [TestMethod]
        public void AddressEqualTest()
        {
            Assert.AreEqual(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.AreNotEqual(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain"), new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper disable EqualExpressionComparison
            Assert.IsTrue(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            
            // ReSharper restore EqualExpressionComparison
            Assert.IsFalse(new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsFalse(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain") == new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));

            // ReSharper disable EqualExpressionComparison
            Assert.IsFalse(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            
            // ReSharper restore EqualExpressionComparison
            Assert.IsTrue(new AddressDetail("Birkenweg 21\n35586 Wetzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(new AddressDetail("Birkenweg 22a\n35586 Wetzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(new AddressDetail("Birkenweck 21a\n35586 Wetzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(new AddressDetail("Birkenweg 21a\n34586 Wetzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(new AddressDetail("Birkenweg 21a\n35586 Wätzlar\nGermany") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
            Assert.IsTrue(new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nSpain") != new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany"));
        }

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

        private static void CheckConstructorStreet(string checkThis, string street, int streetNumber, string extension)
        {
            var address = new AddressDetail(checkThis);
            Assert.AreEqual(street, address.StreetName);
            Assert.AreEqual(streetNumber, address.StreetNumber);
            Assert.AreEqual(extension, address.StreetNumberExtension);
        }
    }
}
