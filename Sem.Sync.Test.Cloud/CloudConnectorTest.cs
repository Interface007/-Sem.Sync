// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloudConnectorTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Tests the loud connector
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.Cloud
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase;

    /// <summary>
    /// Tests the loud connector
    /// </summary>
    [TestClass]
    public class CloudConnectorTest
    {
        /// <summary>
        /// Blob-Id to use
        /// </summary>
        private const string UnitTestBlobId = "unitTestBlobId";
        
        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void TestCloudStorage()
        {
            var contact = new StdContact { Id = new Guid() };
            var contacts = new List<StdElement> { contact, contact };
            var connector = new Connector.OnlineStorage.CloudClient
                {
                    BindingAddress = "http://localhost:50643/Storage.svc" 
                };

            connector.WriteRange(contacts, UnitTestBlobId);
            Assert.IsTrue(connector.GetAll(UnitTestBlobId).Count == 2);
        }
    }
}