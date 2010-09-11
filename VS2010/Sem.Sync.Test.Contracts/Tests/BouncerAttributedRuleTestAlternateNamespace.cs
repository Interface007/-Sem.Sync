namespace Sem.Sync.Test.ContractsAlternate
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.Sync.Test.Contracts;

    [TestClass]
    public class BouncerAttributedRuleTestAlternateNamespace
    {
        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFail()
        {
            Bouncer.ForCheckData(() => BouncerAttributedRuleTest.MessageFailNamespace).Assert();
        }

        [TestMethod]
        public void AddRuleForTypeOk()
        {
            Bouncer.ForCheckData(() => BouncerAttributedRuleTest.MessageOneOk).Assert();
        }
    }
}