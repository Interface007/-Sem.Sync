namespace Sem.Sync.Test.Contracts.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.RuleExecuters;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sync.Test.Contracts.Entities;
    using Sem.Sync.Test.Contracts.Executors;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTestRuleExecutionErrors
    {
        [TestMethod]
        public void CheckRuleWithExpressionCausingNullRefException()
        {
            var executor = new MessageCollection<string>(() => (string)null);
            var results = executor.Assert(x => x.Length > 2).Results;
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void CheckRuleWithNullRule()
        {
            var executor = new MessageCollection<string>(() => (string)null);
            var result = executor.ExecuteRuleExpression(null, "hello", "you");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleWithExecutorVetoYes()
        {
            var nonveto = string.Empty;
            var executor = new VetoExecutor<string>(() => nonveto);
            var result = executor.ExecuteRuleExpression(new StringMaxLengthRule(), 1, "you");
            Assert.IsTrue(executor.LastValidation);
        }

        [TestMethod]
        public void CheckRuleWithExecutorVetoNo()
        {
            var nonveto = string.Empty;
            var executor = new VetoExecutor<string>(() => nonveto);
            var result = executor.ExecuteRuleExpression(new StringMaxLengthRule(), 1, "veto");
            Assert.IsTrue(executor.LastValidation);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void CheckRuleWithExpressionCausingFormatException()
        {
            var executor = new MessageCollection<string>(() => "Hello");
            var result = executor.Assert(x => bool.Parse(x.Length.ToString())).Results;
        }

        [TestMethod]
        public void CheckRuleWithExceptionHandled()
        {
            var executor = new ExceptionHandlerExecutor<string>(() => "just me");
            var result = executor.ExecuteRuleExpression(new StringRegexMatchRule(), "{[(", "handle");
            Assert.IsTrue(executor.ExceptionHandled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckRuleWithExceptionNotHandled()
        {
            var executor = new ExceptionHandlerExecutor<string>(() => "just me");
            var result = executor.ExecuteRuleExpression(new StringRegexMatchRule(), "{[(", "donthandle");
        }
    }
}
