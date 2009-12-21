// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegrationTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for UnitTest1
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.ExchangeWsma
{
    using System.Linq;
    
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    using Sem.GenericHelpers;
    using Sem.Sync.Connector.ExchangeWebServiceManagedApi;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.Test.DataGenerator;

    /// <summary>
    /// This test class performs requests against a real Exchange service in order to test 
    /// storing and reading of contacts.
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

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

        /// <summary>
        /// Creates and reads a full contact, then it deletes the contact to clean up.
        /// </summary>
        [TestMethod]
        public void WriteReadDeleteFullContact()
        {
            var connector = new ContactClient();
            var contacts = Contacts.GetStandardContactList(false);

            var workFlow = Tools.LoadFromFile<SyncWorkFlow>(@"C:\Users\matzensv\AppData\Roaming\SemSyncManager\Work\SyncLists\Exchange Write Test.DSyncList");

            connector.LogOnDomain = workFlow.Target.LogonCredentials.LogOnDomain;
            connector.LogOnPassword = workFlow.Target.LogonCredentials.LogOnPassword;
            connector.LogOnUserId = workFlow.Target.LogonCredentials.LogOnUserId;

            connector.WriteRange(contacts.ToStdElement(), "TestFolder");
            var contactsRead = connector.GetAll("TestFolder").ToContacts();

            var contactWritten = (from x in contacts where x.Name.LastName == "Matzen" select x).FirstOrDefault();
            var contactRead = (from x in contactsRead where x.Name.LastName == "Matzen" select x).FirstOrDefault();

            Assert.AreEqual(contactWritten.Name.FirstName, contactRead.Name.FirstName);
            Assert.AreEqual(contactWritten.Name.MiddleName, contactRead.Name.MiddleName);
            Assert.AreEqual(contactWritten.Name.LastName, contactRead.Name.LastName);

            Assert.AreEqual(contactWritten.DateOfBirth, contactRead.DateOfBirth);
            
            // todo: the "body" property does not contain the original text, but an html representation.
            ////Assert.AreEqual(contactWritten.AdditionalTextData, contactRead.AdditionalTextData);
            Assert.AreEqual(contactWritten.Categories.Count, contactRead.Categories.Count);

            Assert.AreEqual(contactWritten.BusinessCompanyName, contactRead.BusinessCompanyName);
            Assert.AreEqual(contactWritten.BusinessDepartment, contactRead.BusinessDepartment);
            Assert.AreEqual(contactWritten.BusinessPosition, contactRead.BusinessPosition);
            Assert.AreEqual(contactWritten.BusinessHomepage, contactRead.BusinessHomepage);
            
            Assert.AreEqual(contactWritten.BusinessAddressPrimary, contactRead.BusinessAddressPrimary);
            Assert.AreEqual(contactWritten.PersonalAddressPrimary, contactRead.PersonalAddressPrimary);
            Assert.AreEqual(contactWritten.BusinessAddressSecondary, contactRead.BusinessAddressSecondary);
            Assert.AreEqual(contactWritten.BusinessAddressPrimary, contactRead.BusinessAddressPrimary);

            Assert.AreEqual(contactWritten.BusinessPhoneMobile, contactRead.BusinessPhoneMobile);
            Assert.AreEqual(contactWritten.PersonalPhoneMobile, contactRead.PersonalPhoneMobile);

            Assert.AreEqual(contactWritten.BusinessInstantMessengerAddresses.MsnMessenger, contactRead.BusinessInstantMessengerAddresses.MsnMessenger);
            Assert.AreEqual(contactWritten.PersonalInstantMessengerAddresses.MsnMessenger, contactRead.PersonalInstantMessengerAddresses.MsnMessenger);
            Assert.AreEqual(contactWritten.PersonalInstantMessengerAddresses.GoogleTalk, contactRead.PersonalInstantMessengerAddresses.GoogleTalk);

            Assert.AreEqual(contactWritten.BusinessEmailPrimary, contactRead.BusinessEmailPrimary);
            Assert.AreEqual(contactWritten.PersonalEmailPrimary, contactRead.PersonalEmailPrimary);
            Assert.AreEqual(contactWritten.PersonalEmailSecondary, contactRead.PersonalEmailSecondary);
        }
    }
}
