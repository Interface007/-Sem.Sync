namespace Sem.Sync.Test
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase;
    
    [TestClass]
    public class CloudConnectorTest
    {
        private const string _unitTestBlobId = "unitTestBlobId";
        
        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void TestCloudStorage()
        {
            var contact = new StdContact { Id = new Guid() };
            var contacts = new List<StdElement> { contact, contact };
            var connector = new OnlineStorageConnector.CloudClient();
            connector.BindingAddress = "http://localhost:50643/Storage.svc";

            connector.WriteRange(contacts,_unitTestBlobId);
            Assert.IsTrue(connector.GetAll(_unitTestBlobId).Count == 2);
        }
    }
}
