namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerObjectNotNullTest
    {
        [TestMethod]
        public void CheckObjectNotNullTestMustFail()
        {
            Assert.IsFalse(Rules.ObjectNotNullRule<string>().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckObjectNotNullTestMustPass1()
        {
            Assert.IsTrue(Rules.ObjectNotNullRule<string>().CheckExpression(string.Empty, null));
        }

        [TestMethod]
        public void CheckObjectNotNullTestMustPass1A()
        {
            Assert.IsTrue(Rules.NotNull().CheckExpression(string.Empty, null));
        }

        [TestMethod]
        public void CheckObjectNotNullTestMustFail1A()
        {
            Assert.IsFalse(Rules.NotNull().CheckExpression(null, null));
        }
    }
}