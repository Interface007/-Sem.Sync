namespace Sem.Sync.Test.FritzTest
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Sync.Connector.FritzBox;
    using Sem.Sync.Connector.FritzBox.Entities;

    /// <summary>
    /// Summary description for FritzApiInteraction
    /// </summary>
    [TestClass]
    public class FritzApiInteraction
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Test whether the write processing inside the connector is correct (mapping to FritzAapi-Entities).
        /// </summary>
        [TestMethod]
        public void WriteSimpleList()
        {
            var apiMock = new Moq.Mock<FritzApi>();
            apiMock
                .Setup(x => x.ClearPhoneBook())
                .Returns(true);
            apiMock
                .Setup(
                    x => x.SetPhoneBook(
                        It.Is<PhoneBook>(
                            p1 => p1.Count == 2)))
                .Returns(true);

            var connector = new Sem.Sync.Connector.FritzBox.ContactClient
                { 
                    FritzApiCreator = clientFolderName => apiMock.Object
                };

            connector.WriteRange(new DataGenerator.Contacts().GetAll("customdata"), "http://fritzbox.fritz");
            apiMock.Verify();
        }

        /// <summary>
        /// Test whether the read processing inside the connector is correct (mapping to FritzAapi-Entities).
        /// </summary>
        [TestMethod]
        public void ReadSimpleList()
        {
            var apiMock = new Moq.Mock<FritzApi>();
            apiMock
                .Setup(x => x.GetPhoneBook())
                .Returns(new PhoneBook
                    {
                        new Contact
                            {
                                Person = new Person
                                    {
                                        ImageUrl = "the image url",
                                        RealName = "first real name"
                                    },
                                Category = PersonCategory.Default,
                                Telephony = new List<PhoneNumber>
                                    {
                                        new PhoneNumber(PhoneNumberType.Home, "012345678"),
                                        new PhoneNumber(PhoneNumberType.Work, "024681357"),
                                    },
                            }
                    });

            var connector = new Sem.Sync.Connector.FritzBox.ContactClient
                { 
                    FritzApiCreator = clientFolderName => apiMock.Object
                };

            var result = connector.GetAll("http://fritzbox.fritz");

            Assert.AreEqual(1, result.Count);
        }
    }
}
