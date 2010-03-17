// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlParsingTests.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for UnitTest1
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.ExcelXmlTest
{
    using Connector.MicrosoftExcelXml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SyncBase;

    /// <summary>
    /// Tests the Excel file parsing functionality of the <see cref="ExcelXml"/> class.
    /// </summary>
    [TestClass]
    public class XmlParsingTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("Data\\ExCel2010-StdContacts.xlsx")]
        public void LoadDataFromExCelOriginalFileOpenXmlDocument()
        {
            var data = ExcelXml.ImportFromFromOpenXmlPackageFile<StdContact>("ExCel2010-StdContacts.xlsx");
            Assert.IsNotNull(data);
        }
    }
}
