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
            Assert.AreEqual("A", 1.IndexToLetters());
            Assert.AreEqual("B", 2.IndexToLetters());
            Assert.AreEqual("Z", 26.IndexToLetters());
            Assert.AreEqual("AA", 27.IndexToLetters());
            Assert.AreEqual("AB", 28.IndexToLetters());
            Assert.AreEqual("SF", 500.IndexToLetters());
            Assert.AreEqual("VU", 593.IndexToLetters());
            Assert.AreEqual("AQS", 1137.IndexToLetters());
        }

        [TestMethod]
        public void LettersToIndex()
        {
            Assert.AreEqual(1, "A".LettersToIndex());
            Assert.AreEqual(2, "B".LettersToIndex());
            Assert.AreEqual(26, "Z".LettersToIndex());
            Assert.AreEqual(27, "AA".LettersToIndex());
            Assert.AreEqual(28, "AB".LettersToIndex());
            Assert.AreEqual(500, "SF".LettersToIndex());
            Assert.AreEqual(593, "VU".LettersToIndex());
            Assert.AreEqual(1137, "AQS".LettersToIndex());
        }
    }
}
