namespace Sem.Sync.Test.MsExcelOpenXml
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sem.Sync.Connector.MsExcelOpenXml;

    [TestClass]
    public class HelperTests
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CellIndexTest()
        {
            Assert.AreEqual("A", XmlContactClient.GetColSelectorChars(1));
            Assert.AreEqual("B", XmlContactClient.GetColSelectorChars(2));
            Assert.AreEqual("Z", XmlContactClient.GetColSelectorChars(26));
            Assert.AreEqual("AA", XmlContactClient.GetColSelectorChars(27));
            Assert.AreEqual("AB", XmlContactClient.GetColSelectorChars(28));
            Assert.AreEqual("SF", XmlContactClient.GetColSelectorChars(500));
            Assert.AreEqual("VU", XmlContactClient.GetColSelectorChars(593));
            Assert.AreEqual("AQS", XmlContactClient.GetColSelectorChars(1137));
        }
    }
}
