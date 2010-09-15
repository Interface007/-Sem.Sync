namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Rules;

    /// <summary>
    /// This is a test class for BouncerTest and is intended
    /// to contain all BouncerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BouncerIsNotNullTest
    {
        [TestMethod]
        public void CheckParameterIsNotNullMustFail()
        {
            Assert.IsFalse(new IsNotNullRule<object>().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckParameterIsNotNullMustPass1()
        {
            Assert.IsTrue(new IsNotNullRule<object>().CheckExpression(this, null));
        }
    }
}