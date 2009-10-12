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
        public void AddressConstructorTest01()
        {
            this.CheckConstructorStreet("Birkenweg 21a", "Birkenweg", 21, "a");
            this.CheckConstructorStreet("Birken Weg 21a", "Birken Weg", 21, "a");
            this.CheckConstructorStreet("Birken-Weg 21a", "Birken-Weg", 21, "a");
            this.CheckConstructorStreet("Birkenweg 21", "Birkenweg", 21, string.Empty);
            this.CheckConstructorStreet("Birken Weg 21", "Birken Weg", 21, string.Empty);
            this.CheckConstructorStreet("Birken-Weg 21", "Birken-Weg", 21, string.Empty);
            this.CheckConstructorStreet("Birkenweg 21a-z", "Birkenweg", 21, "a-z");
            this.CheckConstructorStreet("Birkenweg 21 a-z", "Birkenweg", 21, "a-z");
            this.CheckConstructorStreet("Birkenweg 21 a - z", "Birkenweg", 21, "a - z");
            this.CheckConstructorStreet("Birkenweg 21a -z", "Birkenweg", 21, "a -z");
            this.CheckConstructorStreet("Birkenweg 21a- z", "Birkenweg", 21, "a- z");
            this.CheckConstructorStreet("Birkenweg 21a - z", "Birkenweg", 21, "a - z");
        }

        [TestMethod]
        public void AddressConstructorTest02()
        {
            this.CheckConstructorCity("35586 Wetzlar", "Wetzlar", "35586");
            this.CheckConstructorCity("35586 Frankfurt am Main", "Frankfurt am Main", "35586");
            this.CheckConstructorCity("35586 Frankfurt (am Main)", "Frankfurt (am Main)", "35586");
            this.CheckConstructorCity("35586", string.Empty, "35586");
        }

        [TestMethod]
        public void AddressConstructorTest03()
        {
            this.CheckConstructorCountry("Germany", "Germany");
            this.CheckConstructorCountry("New Zeland", "New Zeland");
            this.CheckConstructorCountry("Korea (Nord)", "Korea (Nord)");
        }

        [TestMethod]
        public void AddressConstructorTest04()
        {
            this.CheckConstructorStreet("Birkenweg 21a\n35586 Wetzlar\nGermany", "Birkenweg", 21, "a");
            this.CheckConstructorCity("Birkenweg 21a\n35586 Wetzlar\nGermany", "Wetzlar", "35586");
            this.CheckConstructorCountry("Birkenweg 21a\n35586 Wetzlar\nGermany", "Germany");

            this.CheckConstructorStreet("Birkenweg 21a\nGermany", "Birkenweg", 21, "a");
            this.CheckConstructorCity("Birkenweg 21a\nGermany", string.Empty, string.Empty);
            this.CheckConstructorCountry("Birkenweg 21a\nGermany", "Germany");

            this.CheckConstructorStreet("Birkenweg 21a\n\n\n\nGermany", "Birkenweg", 21, "a");
            this.CheckConstructorCity("Birkenweg 21a\n\n\n\nGermany", string.Empty, string.Empty);
            this.CheckConstructorCountry("Birkenweg 21a\n\n\n\nGermany", "Germany");
        }

        [TestMethod]
        public void AddressToStringTest01()
        {
            Assert.AreEqual("Birkenweg 21 a / 35586 Wetzlar Germany", new AddressDetail("Birkenweg 21a\n35586 Wetzlar\nGermany").ToString());
        }

        private void CheckConstructorCountry(string checkThis, string countryName)
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

        private void CheckConstructorCity(string checkThis, string city, string postalCode)
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

        private void CheckConstructorStreet(string checkThis, string street, int streetNumber, string extension)
        {
            var address = new AddressDetail(checkThis);
            Assert.AreEqual(street, address.StreetName);
            Assert.AreEqual(streetNumber, address.StreetNumber);
            Assert.AreEqual(extension, address.StreetNumberExtension);
        }
    }
}
