﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntegrationTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This test class performs requests against a real Exchange service in order to test
//   storing and reading of contacts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.ExchangeWsma
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This test class performs requests against a real Exchange service in order to test 
    ///   storing and reading of contacts.
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        #region Properties

        /// <summary>
        ///   Gets or sets the test context which provides
        ///   information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #endregion

        // todo: the exchange access should be mocked
        /////// <summary>
        /////// Creates and reads a full contact, then it deletes the contact to clean up.
        /////// </summary>
        ////[TestMethod]
        ////public void WriteReadDeleteFullContact()
        ////{
        ////    var connector = new ContactClient();
        ////    var contacts = Contacts.GetStandardContactList(false);

        ////    var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SemSyncManager");
        ////    var workFlow = Tools.LoadFromFile<SyncWorkFlow>(Path.Combine(baseFolder, @"Work\SyncLists\Exchange Write Test.DSyncList"));

        ////    if (workFlow == null)
        ////    {
        ////        Assert.Inconclusive("There is a missing SyncList, so this test cannot be executed.");
        ////    }

        ////    connector.LogOnDomain = workFlow.Target.LogonCredentials.LogOnDomain;
        ////    connector.LogOnPassword = workFlow.Target.LogonCredentials.LogOnPassword;
        ////    connector.LogOnUserId = workFlow.Target.LogonCredentials.LogOnUserId;

        ////    connector.DeleteElements(connector.GetAll(workFlow.Target.Path), workFlow.Target.Path);

        ////    connector.WriteRange(contacts.ToStdElement(), workFlow.Target.Path);
        ////    var contactsRead = connector.GetAll(workFlow.Target.Path).ToContacts();

        ////    var contactWritten = (from x in contacts where x.Name.LastName == "Matzen" select x).FirstOrDefault();
        ////    var contactRead = (from x in contactsRead where x.Name.LastName == "Matzen" select x).FirstOrDefault();

        ////    Assert.AreEqual(contactWritten.Name.FirstName, contactRead.Name.FirstName);
        ////    Assert.AreEqual(contactWritten.Name.MiddleName, contactRead.Name.MiddleName);
        ////    Assert.AreEqual(contactWritten.Name.LastName, contactRead.Name.LastName);

        ////    Assert.AreEqual(contactWritten.DateOfBirth, contactRead.DateOfBirth);

        ////    // todo: the "body" property does not contain the original text, but an html representation.
        ////    ////Assert.AreEqual(contactWritten.AdditionalTextData, contactRead.AdditionalTextData);
        ////    Assert.AreEqual(contactWritten.Categories.Count, contactRead.Categories.NewIfNull().Count);

        ////    Assert.AreEqual(contactWritten.BusinessCompanyName, contactRead.BusinessCompanyName);
        ////    Assert.AreEqual(contactWritten.BusinessDepartment, contactRead.BusinessDepartment);
        ////    Assert.AreEqual(contactWritten.BusinessPosition, contactRead.BusinessPosition);
        ////    Assert.AreEqual(contactWritten.BusinessHomepage, contactRead.BusinessHomepage);

        ////    Assert.AreEqual(contactWritten.BusinessAddressPrimary, contactRead.BusinessAddressPrimary);
        ////    Assert.AreEqual(contactWritten.PersonalAddressPrimary, contactRead.PersonalAddressPrimary);
        ////    Assert.AreEqual(contactWritten.BusinessAddressSecondary, contactRead.BusinessAddressSecondary);
        ////    Assert.AreEqual(contactWritten.BusinessAddressPrimary, contactRead.BusinessAddressPrimary);

        ////    Assert.AreEqual(contactWritten.BusinessPhoneMobile, contactRead.BusinessPhoneMobile);
        ////    Assert.AreEqual(contactWritten.PersonalPhoneMobile, contactRead.PersonalPhoneMobile);

        ////    Assert.AreEqual(contactWritten.BusinessInstantMessengerAddresses.NewIfNull(), contactRead.BusinessInstantMessengerAddresses.NewIfNull());
        ////    Assert.AreEqual(contactWritten.PersonalInstantMessengerAddresses.NewIfNull(), contactRead.PersonalInstantMessengerAddresses.NewIfNull());

        ////    Assert.AreEqual(contactWritten.BusinessEmailPrimary, contactRead.BusinessEmailPrimary);
        ////    Assert.AreEqual(contactWritten.PersonalEmailPrimary, contactRead.PersonalEmailPrimary);
        ////    Assert.AreEqual(contactWritten.PersonalEmailSecondary, contactRead.PersonalEmailSecondary);
        ////}
    }
}