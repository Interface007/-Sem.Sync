namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerIsNotNullTest
    {
        [TestMethod]
        public void CheckParameterIsNotNullMustFail()
        {
            Assert.IsFalse(Rules.IsNotNull<object>().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckParameterIsNotNullMustPass1()
        {
            Assert.IsTrue(Rules.IsNotNull<object>().CheckExpression(this, null));
        }
    }
}