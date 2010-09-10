namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerBackEndNumberBoundariesTest
    {
        [TestMethod]
        public void CheckParameterBackEndNumberBoundariesMustFail()
        {
            Assert.IsFalse(Rules.BackEndNumberBoundaries().CheckExpression(20000, null));
        }

        [TestMethod]
        public void CheckParameterBackEndNumberBoundariesMustPass1()
        {
            Assert.IsTrue(Rules.BackEndNumberBoundaries().CheckExpression(10000, null));
        }
    }
}