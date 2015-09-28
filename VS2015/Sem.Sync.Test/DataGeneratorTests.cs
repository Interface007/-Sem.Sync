using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sem.Sync.Test
{
    /// <summary>
    /// Summary description for DataGeneratorTests
    /// </summary>
    [TestClass]
    public class DataGeneratorTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("LastName.xml")]
        public void GetSomeNamesTest()
        {
            var name = DataGenerator.Contacts.GetRandom("LastName");
            //Assert.IsNotNull(name);
        }
    }
}
