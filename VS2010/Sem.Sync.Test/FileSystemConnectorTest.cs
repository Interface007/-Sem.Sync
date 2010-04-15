// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemConnectorTest.cs" company="Sven Erik Matzen">
//     Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <author>Sven Erik Matzen</author>
// <summary>
//   Test class for the file system connectors
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Sem.Sync.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.Filesystem;
    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.DetailData;
    using Sem.Sync.SyncBase.Helpers;
    using Sem.Sync.Test.DataGenerator;

    /// <summary>
    /// Test class for the file system connector
    /// </summary>
    [TestClass]
    public class FileSystemConnectorTest
    {
        /// <summary>
        /// contact element with picture
        /// </summary>
        private const string ContactWithPicture = "929e2981-ee94-4e1f-adb0-240cb8a9afd6";

        /// <summary>
        /// contact element id without a picture (empty array, not null!)
        /// </summary>
        private const string ContactWithoutPicture = "9c8a9b29-2fda-44f3-8324-62b983468a7e";

        /////// <summary>
        /////// contact element id with null values
        /////// </summary>
        ////private const string ContactWithNulls = "21C3586A-BB96-4a3a-9B05-D40F1125BFB9";

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
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
        #endregion

        /// <summary>
        /// performs some basic tests for the file system connector by reading a hard coded xml file and testing for some data.
        /// </summary>
        [TestMethod]
        public void BasicTests()
        {
            var connector = new ContactClient();
            Assert.AreEqual("FileSystem Contact Connector - one file for all contacts", connector.FriendlyClientName);

            var tempFolder = PrepareFolder(false);
            var listWithTwoContacts = connector.GetAll(Path.Combine(tempFolder, "file1")).ToContacts();

            Assert.AreEqual(2, listWithTwoContacts.Count);
            Assert.AreEqual(0, listWithTwoContacts.GetContactById(ContactWithoutPicture).PictureData.Length);
            Assert.AreEqual(2090, listWithTwoContacts.GetContactById(ContactWithPicture).PictureData.Length);
            Assert.AreEqual("Sven", listWithTwoContacts.GetContactById(ContactWithPicture).Name.FirstName);
            Assert.AreEqual(Gender.Male, listWithTwoContacts.GetContactById(ContactWithPicture).PersonGender);
            Assert.AreEqual(Gender.Female, listWithTwoContacts.GetContactById(ContactWithoutPicture).PersonGender);
        }

        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void CopyTestsvCard()
        {
            var connector = new ContactClient();
            var vcardConnector = new ContactClientVCards();
            var tempFolder = PrepareFolder(false);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");
            var path1 = Path.Combine(tempFolder, "vCards");

            var originalList = connector.GetAll(file1);
            originalList.Add(new StdContact());
            vcardConnector.WriteRange(originalList, path1);
            var copyList = vcardConnector.GetAll(path1);
            copyList.Add(new StdContact());
            connector.WriteRange(copyList, file2);

            AssertOriginalAndCopyCompare(originalList, copyList, false);

            vcardConnector = new ContactClientVCards(true);
            vcardConnector.WriteRange(originalList, path1);

            Assert.IsTrue(File.Exists(Path.Combine(tempFolder, "vCards\\" + SyncTools.NormalizeFileName(originalList.GetContactById(ContactWithPicture).ToStringSimple())) + "-ContactPicture.jpg"));
        }

        /// <summary>
        /// Performs a copy test of StdContacts from CSV file to CSV file
        /// </summary>
        [TestMethod]
        public void CopyTestsCSV()
        {
            var connector = new ContactClient();
            var csvConnector = new GenericClientCsv<StdContact>();
            var tempFolder = PrepareFolder(false);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");
            var path1 = Path.Combine(tempFolder, "csv.csv");

            var originalList = connector.GetAll(file1);
            originalList.Add(new StdContact());
            csvConnector.WriteRange(originalList, path1);
            var copyList = csvConnector.GetAll(path1);
            copyList.Add(new StdContact());
            connector.WriteRange(copyList, file2);

            AssertOriginalAndCopyCompare(originalList, copyList, true);
        }

        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void CopyTestsvCardExternal()
        {
            var connector = new ContactClient();
            var vcardConnector = new ContactClientVCards(true);
            var tempFolder = PrepareFolder(false);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");
            var path1 = Path.Combine(tempFolder, "vCards");

            var originalList = connector.GetAll(file1);
            vcardConnector.WriteRange(originalList, path1);
            var copyList = vcardConnector.GetAll(path1);
            connector.WriteRange(copyList, file2);

            AssertOriginalAndCopyCompare(originalList, copyList, false);

            vcardConnector = new ContactClientVCards(true);
            vcardConnector.WriteRange(originalList, path1);

            Assert.IsTrue(File.Exists(Path.Combine(tempFolder, "vCards\\" + SyncTools.NormalizeFileName(originalList.GetContactById(ContactWithPicture).ToStringSimple())) + "-ContactPicture.jpg"));
        }

        // todo: This test must be rewritten because of the way the data is now handled
        ///// <summary>
        ///// Performs a copy from one file system to a vCard and back. This executes read and write.
        ///// Then both files will be compared to validate that all data has been copied.
        ///// </summary>
        //[TestMethod]
        //public void CopyTests()
        //{
        //    var connector = new ContactClient();
        //    var tempFolder = PrepareFolder(true);
        //    var file1 = Path.Combine(tempFolder, "file1");
        //    var file2 = Path.Combine(tempFolder, "file2");

        //    connector.WriteRange(connector.GetAll(file1), file2);

        //    File.WriteAllText(file1, File.ReadAllText(file1).Replace(" ", string.Empty));
        //    File.WriteAllText(file2, File.ReadAllText(file2).Replace(" ", string.Empty));

        //    var content1 = File.ReadAllText(file1);
        //    var content2 = File.ReadAllText(file2);

        //    Assert.AreEqual(content1, content2);
        //}

        [TestMethod]
        public void TestCategorySelectorRead()
        {
            var contact = new StdContact { Categories = new List<string> { "cat1", "category 1", "!important!" } };

            Assert.AreEqual("cat1", Tools.GetPropertyValueString(contact, "Categories[0]"));
            Assert.AreEqual("category 1", Tools.GetPropertyValueString(contact, "Categories[1]"));
            Assert.AreEqual("!important!", Tools.GetPropertyValueString(contact, "Categories[2]"));
            Assert.AreEqual("cat1|category 1|!important!", Tools.GetPropertyValueString(contact, "Categories"));
        }

        [TestMethod]
        public void TestCategorySelectorWrite()
        {
            var contact = new StdContact();
            Tools.SetPropertyValue(contact, "Categories", "cat1|category 1|!important!");

            Assert.AreEqual("cat1|category 1|!important!", Tools.GetPropertyValueString(contact, "Categories"));
            Assert.AreEqual("cat1", contact.Categories[0]);
            Assert.AreEqual("category 1", contact.Categories[1]);
            Assert.AreEqual("!important!", contact.Categories[2]);
            
        }

        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void CopyTestsIndividualFiles()
        {
            var connector = new ContactClient();
            var individualFilesConnector = new ContactClientIndividualFiles();
            var tempFolder = PrepareFolder(false);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");
            var path1 = Path.Combine(tempFolder, "vCards");

            var originalList = connector.GetAll(file1);
            originalList.Add(new StdContact());
            individualFilesConnector.WriteRange(originalList, path1);
            var copyList = individualFilesConnector.GetAll(path1);
            copyList.Add(new StdContact());
            connector.WriteRange(copyList, file2);

            AssertOriginalAndCopyCompare(originalList, copyList, false);
        }

        /// <summary>
        /// Performs a copy from one file system store to another. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void CopyTestsGenericConnector()
        {
            var connector = new ContactClient();
            var genericConnector = new GenericClient<StdContact>();
            var tempFolder = PrepareFolder(false);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");
            var path1 = Path.Combine(tempFolder, "vCards");

            var originalList = connector.GetAll(file1);
            originalList.Add(new StdContact());
            genericConnector.WriteRange(originalList, path1);
            var copyList = genericConnector.GetAll(path1);
            copyList.Add(new StdContact());
            connector.WriteRange(copyList, file2);

            AssertOriginalAndCopyCompare(originalList, copyList, false);
        }

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

        /// <summary>
        /// Compares the original with the copy
        /// </summary>
        /// <param name="originalList"> The original list.  </param>
        /// <param name="copyList"> The copy list.  </param>
        /// <param name="skipPicture"> A value specifying whether to skip the picture comparison or not. </param>
        private static void AssertOriginalAndCopyCompare(List<StdElement> originalList, List<StdElement> copyList, bool skipPicture)
        {
            var withoutPictureOriginal = originalList.GetContactById("9c8a9b29-2fda-44f3-8324-62b983468a7e");
            var withoutPictureCopy = copyList.GetContactById("9c8a9b29-2fda-44f3-8324-62b983468a7e");

            var withPictureOriginal = originalList.GetContactById("929e2981-ee94-4e1f-adb0-240cb8a9afd6");
            var withPictureCopy = copyList.GetContactById("929e2981-ee94-4e1f-adb0-240cb8a9afd6");

            Assert.AreEqual(withoutPictureOriginal.AdditionalTextData, withoutPictureCopy.AdditionalTextData);
            Assert.AreEqual(withoutPictureOriginal.Name.FirstName, withoutPictureCopy.Name.FirstName);
            Assert.AreEqual(withoutPictureOriginal.Name.LastName, withoutPictureCopy.Name.LastName);

            Assert.AreEqual(withPictureOriginal.AdditionalTextData, withPictureCopy.AdditionalTextData);
            Assert.AreEqual(withPictureOriginal.Name.AcademicTitle, withPictureCopy.Name.AcademicTitle);
            Assert.AreEqual(withPictureOriginal.Name.FirstName, withPictureCopy.Name.FirstName);
            Assert.AreEqual(withPictureOriginal.Name.LastName, withPictureCopy.Name.LastName);
            Assert.AreEqual(withPictureOriginal.Name.MiddleName, withPictureCopy.Name.MiddleName);
            Assert.AreEqual(withPictureOriginal.PersonalPhoneMobile.Number, withPictureCopy.PersonalPhoneMobile.Number);

            Assert.AreEqual(withPictureOriginal.Categories.NewIfNull(0), withPictureCopy.Categories.NewIfNull(0));
            Assert.AreEqual(withPictureOriginal.Categories.NewIfNull(1), withPictureCopy.Categories.NewIfNull(1));
            Assert.AreEqual(withPictureOriginal.Categories.NewIfNull(2), withPictureCopy.Categories.NewIfNull(2));
            Assert.AreEqual(withPictureOriginal.Categories.NewIfNull(3), withPictureCopy.Categories.NewIfNull(3));

            if (!skipPicture)
            {
                Assert.AreEqual(withPictureOriginal.PictureData.Length, withPictureCopy.PictureData.Length);
            }
        }
    }
}
