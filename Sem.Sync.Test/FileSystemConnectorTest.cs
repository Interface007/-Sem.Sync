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

    using FilesystemConnector;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase;
    using SyncBase.DetailData;
    using SyncBase.Helpers;

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

        /// <summary>
        /// contact element id with null values
        /// </summary>
        private const string ContactWithNulls = "21C3586A-BB96-4a3a-9B05-D40F1125BFB9";

        // public FileSystemConnectorTest()
        // {
        //    // TODO: Add constructor logic here
        // }

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

            Assert.AreEqual(3, listWithTwoContacts.Count);
            Assert.AreEqual(0, listWithTwoContacts.GetContactById(ContactWithoutPicture).PictureData.Length);
            Assert.AreEqual(2090, listWithTwoContacts.GetContactById(ContactWithPicture).PictureData.Length);
            Assert.AreEqual("Sven", listWithTwoContacts.GetContactById(ContactWithPicture).Name.FirstName);
            Assert.AreEqual(Gender.Male, listWithTwoContacts.GetContactById(ContactWithPicture).PersonGender);
            Assert.AreEqual(Gender.Unspecified, listWithTwoContacts.GetContactById(ContactWithoutPicture).PersonGender);
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

            AssertOriginalAndCopyCompare(originalList, copyList);

            vcardConnector = new ContactClientVCards(true);
            vcardConnector.WriteRange(originalList, path1);

            Assert.IsTrue(File.Exists(Path.Combine(tempFolder, "vCards\\" + SyncTools.NormalizeFileName(originalList.GetContactById(ContactWithPicture).ToStringSimple())) + "-ContactPicture.jpg"));
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

            AssertOriginalAndCopyCompare(originalList, copyList);

            vcardConnector = new ContactClientVCards(true);
            vcardConnector.WriteRange(originalList, path1);

            Assert.IsTrue(File.Exists(Path.Combine(tempFolder, "vCards\\" + SyncTools.NormalizeFileName(originalList.GetContactById(ContactWithPicture).ToStringSimple())) + "-ContactPicture.jpg"));
        }

        /// <summary>
        /// Performs a copy from one file system to a vCard and back. This executes read and write.
        /// Then both files will be compared to validate that all data has been copied.
        /// </summary>
        [TestMethod]
        public void CopyTests()
        {
            var connector = new ContactClient();
            var tempFolder = PrepareFolder(true);
            var file1 = Path.Combine(tempFolder, "file1");
            var file2 = Path.Combine(tempFolder, "file2");

            connector.WriteRange(connector.GetAll(file1), file2);

            File.WriteAllText(file1, File.ReadAllText(file1).Replace(" ", string.Empty));
            File.WriteAllText(file2, File.ReadAllText(file2).Replace(" ", string.Empty));

            var content1 = File.ReadAllText(file1);
            var content2 = File.ReadAllText(file2);

            Assert.AreEqual(content1, content2);
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

            AssertOriginalAndCopyCompare(originalList, copyList);
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

            AssertOriginalAndCopyCompare(originalList, copyList);
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

            AddContactWithoutPicture(file1);
            AddContactWithPicture(file1);
            if (!skipNullContact)
            {
                AddContactWithNulls(file1);
            }

            file1.Append("</ArrayOfStdContact>");

            File.WriteAllText(Path.Combine(folder, "file1"), file1.ToString());
            return folder;
        }

        /// <summary>
        /// Adds a contact to a StringBuilder that does not contain a picture.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        private static void AddContactWithoutPicture(StringBuilder content)
        {
            content.AppendLine("  <StdContact Id=\"9c8a9b29-2fda-44f3-8324-62b983468a7e\">");
            content.AppendLine("    <InternalSyncData>");
            content.AppendLine("      <DateOfLastChange>2009-05-18T18:03:29.162</DateOfLastChange>");
            content.AppendLine("      <DateOfCreation>2009-04-23T21:37:44.281</DateOfCreation>");
            content.AppendLine("    </InternalSyncData>");
            content.AppendLine("    <PersonGender>Unspecified</PersonGender>");
            content.AppendLine("    <DateOfBirth>1900-01-01T00:00:00</DateOfBirth>");
            content.AppendLine("    <Name>");
            content.AppendLine("      <FirstName>Auskunft</FirstName>");
            content.AppendLine("      <LastName>11880</LastName>");
            content.AppendLine("    </Name>");
            content.AppendLine("    <PersonalPhoneMobile>");
            content.AppendLine("      <DenormalizedPhoneNumber>11880</DenormalizedPhoneNumber>");
            content.AppendLine("      <CountryCode>unspecified</CountryCode>");
            content.AppendLine("      <AreaCode>0</AreaCode>");
            content.AppendLine("    </PersonalPhoneMobile>");
            content.AppendLine("    <PictureData />");
            content.AppendLine("  </StdContact>");
        }

        /// <summary>
        /// Adds a contact to a StringBuilder that does not contain a picture.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        private static void AddContactWithNulls(StringBuilder content)
        {
            content.AppendLine("  <StdContact Id=\"21C3586A-BB96-4a3a-9B05-D40F1125BFB9\">");
            content.AppendLine("    <InternalSyncData>");
            content.AppendLine("      <DateOfLastChange>2009-05-18T18:03:29.162</DateOfLastChange>");
            content.AppendLine("      <DateOfCreation>2009-04-23T21:37:44.281</DateOfCreation>");
            content.AppendLine("    </InternalSyncData>");
            content.AppendLine("  </StdContact>");
        }

        /// <summary>
        /// Adds a contact with a picture to the string builder.
        /// </summary>
        /// <param name="content">StringBuilder that will get the information.</param>
        private static void AddContactWithPicture(StringBuilder content)
        {
            content.AppendLine(" <StdContact Id=\"929e2981-ee94-4e1f-adb0-240cb8a9afd6\">");
            content.AppendLine("   <InternalSyncData>");
            content.AppendLine("     <DateOfLastChange>2009-05-18T18:57:41.865</DateOfLastChange>");
            content.AppendLine("     <DateOfCreation>2009-04-23T21:51:09.146</DateOfCreation>");
            content.AppendLine("   </InternalSyncData>");
            content.AppendLine("   <PersonGender>Male</PersonGender>");
            content.AppendLine("   <DateOfBirth>1971-01-18T00:00:00</DateOfBirth>");
            content.AppendLine("   <Name>");
            content.AppendLine("     <AcademicTitle>Dipl. Biol.</AcademicTitle>");
            content.AppendLine("     <FirstName>Sven</FirstName>");
            content.AppendLine("     <MiddleName>Erik</MiddleName>");
            content.AppendLine("     <LastName>Matzen</LastName>");
            content.AppendLine("   </Name>");
            content.AppendLine("   <PersonalAddressPrimary>");
            content.AppendLine("     <CountryName>Germany</CountryName>");
            content.AppendLine("     <PostalCode>12345</PostalCode>");
            content.AppendLine("     <CityName>Wetzlar, Hessen</CityName>");
            content.AppendLine("     <StreetName>My Street 12a</StreetName>");
            content.AppendLine("     <StreetNumber>12</StreetNumber>");
            content.AppendLine("     <Phone>");
            content.AppendLine("       <CountryCode>Germany</CountryCode>");
            content.AppendLine("       <AreaCode>6441</AreaCode>");
            content.AppendLine("       <Number>123456</Number>");
            content.AppendLine("     </Phone>");
            content.AppendLine("   </PersonalAddressPrimary>");
            content.AppendLine("   <PersonalPhoneMobile>");
            content.AppendLine("     <CountryCode>Germany</CountryCode>");
            content.AppendLine("     <AreaCode>234</AreaCode>");
            content.AppendLine("     <Number>567890</Number>");
            content.AppendLine("   </PersonalPhoneMobile>");
            content.AppendLine("   <PersonalEmailPrimary>sven.erik.matzen@web.de</PersonalEmailPrimary>");
            content.AppendLine("   <PersonalHomepage>http://www.svenerikmatzen.info</PersonalHomepage>");
            content.AppendLine("   <BusinessCompanyName>SDX AG</BusinessCompanyName>");
            content.AppendLine("   <BusinessAddressPrimary>");
            content.AppendLine("     <CountryName>Deutschland</CountryName>");
            content.AppendLine("     <PostalCode>60388</PostalCode>");
            content.AppendLine("     <CityName>Frankfurt am Main</CityName>");
            content.AppendLine("     <StreetName>Borsigallee 19</StreetName>");
            content.AppendLine("     <StreetNumber>19</StreetNumber>");
            content.AppendLine("     <Phone>");
            content.AppendLine("       <CountryCode>Germany</CountryCode>");
            content.AppendLine("       <AreaCode>123</AreaCode>");
            content.AppendLine("       <Number>7654321</Number>");
            content.AppendLine("     </Phone>");
            content.AppendLine("   </BusinessAddressPrimary>");
            content.AppendLine("   <BusinessHomepage>http://www.sdx-ag.de</BusinessHomepage>");
            content.AppendLine("   <AdditionalTextData>LinkedIn Profile: http://www.linkedin.com/profile?viewProfile=&amp;key=someKey</AdditionalTextData>");
            content.AppendLine("   <PictureName>ContactPicture.jpg</PictureName>");
            content.AppendLine("   <PictureData>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</PictureData>");
            content.AppendLine(" </StdContact>");
        }

        /// <summary>
        /// Compares the original with the copy
        /// </summary>
        /// <param name="originalList"> The original list. </param>
        /// <param name="copyList"> The copy list. </param>
        private static void AssertOriginalAndCopyCompare(List<StdElement> originalList, List<StdElement> copyList)
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
            Assert.AreEqual(withPictureOriginal.PictureData.Length, withPictureCopy.PictureData.Length);
            Assert.AreEqual(withPictureOriginal.PersonalPhoneMobile.Number, withPictureCopy.PersonalPhoneMobile.Number);
        }
    }
}
