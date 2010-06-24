namespace Sem.Sync.Test.FritzTest
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Sem.Sync.Connector.FritzBox;
    using Sem.Sync.Connector.FritzBox.Entities;
    using Sem.Sync.SyncBase.DetailData;

    using PhoneNumber = Sem.Sync.Connector.FritzBox.Entities.PhoneNumber;

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
                                        new PhoneNumber(PhoneNumberType.Mobile, "09871234"),
                                    },
                            }
                    });

            var connector = new Sem.Sync.Connector.FritzBox.ContactClient
                { 
                    FritzApiCreator = clientFolderName => apiMock.Object
                };

            var result = connector.GetAll("http://fritzbox.fritz");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            
            var resultContact1 = result[0] as StdContact;
            Assert.IsNotNull(resultContact1);
            Assert.AreEqual("09871234", resultContact1.PersonalPhoneMobile);
            Assert.IsNotNull(resultContact1.BusinessAddressPrimary);
            Assert.IsNotNull(resultContact1.BusinessAddressPrimary.Phone);
            Assert.AreEqual("024681357", resultContact1.BusinessAddressPrimary.Phone);
            Assert.IsNotNull(resultContact1.PersonalAddressPrimary);
            Assert.IsNotNull(resultContact1.PersonalAddressPrimary.Phone);
            Assert.AreEqual("012345678", resultContact1.PersonalAddressPrimary.Phone);
            Assert.AreEqual("the image url", resultContact1.SourceSpecificAttributes["FritzBox.ImageUrl"]);
            
            // the name string will be split into last name and first name while transformation Fritz=>StdContact
            // the ToString of the name does return "{lastname}, {firstname}"
            Assert.AreEqual("name, first real", resultContact1.Name.ToString());
        }
    }
}
