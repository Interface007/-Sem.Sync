﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleConnectorTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Tests the google connector with a test of standard data and with standard account
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.Google
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.Google;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.Test.DataGenerator;

    /// <summary>
    /// Tests the google connector with a test of standard data and with standard account
    /// </summary>
    [TestClass]
    public class GoogleConnectorTest
    {
        #region Public Methods

        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        ///   Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void GoogleConnectorTestDataCheck()
        {
            var original = Contacts.GetStandardContactList(false);

            var connector = new ContactClient
                {
                    LogOnUserId = "semsynctest@svenerikmatzen.info", 
                    LogOnPassword = Tools.GetRegValue("Software\\Sem.Sync\\Test", "GoogleTestPassword", string.Empty)
                };

            connector.WriteRange(original.ToStdElements(), string.Empty);
            var received = connector.GetAll(string.Empty).ToStdContacts();

            var originalText = Contacts.SerializeList(original);
            var receivedText = Contacts.SerializeList(received);

            connector.DeleteElements(original.ToStdElements(), string.Empty);

            Assert.IsTrue(originalText == receivedText);
        }

        #endregion
    }
}