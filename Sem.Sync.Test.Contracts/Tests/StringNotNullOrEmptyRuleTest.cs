namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.SemRules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class StringNotNullOrEmptyRuleTest
    {
        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail()
        {
            Assert.IsFalse(new StringNotNullOrEmptyRule().CheckExpression(string.Empty, null));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustFail2()
        {
            Assert.IsFalse(new StringNotNullOrEmptyRule().CheckExpression(null, null));
        }

        [TestMethod]
        public void CheckStringNotNullOrEmptyRuleMustPass1()
        {
            Assert.IsTrue(new StringNotNullOrEmptyRule().CheckExpression("hello", null));
        }
    }
}