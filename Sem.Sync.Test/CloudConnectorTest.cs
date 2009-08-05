using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sem.Sync.CloudStorageConnector;
using Sem.Sync.SyncBase;
using Sem.Sync.Test.DataGenerator;

namespace Sem.Sync.Test
{
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
            

            StdContact contact = new StdContact();
            contact.Id = new Guid();
            List<StdElement> contacts = new List<StdElement>();
            contacts.Add(contact);
            contacts.Add(contact);

            ContactClient connector = new ContactClient();
            connector.WriteRange(contacts,_unitTestBlobId);
            
            Assert.IsTrue(connector.GetContacts(_unitTestBlobId).Count == 2);
          
        }


        #region Private Methods
        /// <summary>
        /// Prepares a folder with files for the tests.
        /// </summary>
        /// <param name="skipNullContact"> skips adding the null contact if True. </param>
        /// <returns> the path to the test folder </returns>
        private static string PrepareFolder(bool skipNullContact)
        {
            var folder = Path.Combine(Path.GetTempPath(), "BasicTests");
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            Directory.CreateDirectory(folder);

            var file1 = new StringBuilder();
            file1.AppendLine("<?xml version=\"1.0\"?>");
            file1.AppendLine("<ArrayOfStdContact xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");

            Contacts.AddContactWithoutPicture(file1);
            Contacts.AddContactWithPicture(file1);
            if (!skipNullContact)
            {
                Contacts.AddContactWithNulls(file1);
            }

            file1.Append("</ArrayOfStdContact>");

            File.WriteAllText(Path.Combine(folder, "file1"), file1.ToString());
            return folder;
        }
        #endregion

    }
}
