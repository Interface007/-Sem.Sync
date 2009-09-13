﻿namespace Sem.Sync.Test
{
    using DataGenerator;
    using GenericHelpers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SyncBase.Helpers;

    /// <summary>
    /// Tests the google connector with a 
    /// </summary>
    [TestClass]
    public class GoogleConnectorTest
    {
        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void GoogleConnectorTestDataCheck()
        {
            var original = Contacts.GetStandardContactList(false);

            var connector = new GoogleClient.ContactClient 
                {
                    LogOnUserId = "semsynctest@svenerikmatzen.info",
                    LogOnPassword =
                        Tools.GetRegValue(
                        "Software\\Sem.Sync\\Test", "GoogleTestPassword", "{71AFF4AF-01B9-4f28-83AA-94C15EB39857}")
                };

            connector.WriteRange(original.ToStdElement(), string.Empty);
            var received = connector.GetAll(string.Empty).ToContacts();

            var originalText = Contacts.SerializeList(original);
            var receivedText = Contacts.SerializeList(received);
            
            Assert.IsTrue(originalText == receivedText);
        }
    }
}
