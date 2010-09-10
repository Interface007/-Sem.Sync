﻿namespace Sem.Sync.Test.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.Test.Contracts;

    /// <summary>
    /// This is a test class for BouncerTest and is intended
    /// to contain all BouncerTest Unit Tests
    /// </summary>
    [TestClass]
    public class BouncerTestMessages
    {
        private static readonly AttributedSampleClass _MessageOneFailRegEx = new AttributedSampleClass
        {
            MustBeOfRegExPatter = "hello",
        };

        [TestMethod]
        public void CheckRuleSet1()
        {
            var messages = Bouncer.ForMessages(() => _MessageOneFailRegEx).Assert();
            Assert.AreEqual(5, messages.Results.Count);
        }
    }
}
