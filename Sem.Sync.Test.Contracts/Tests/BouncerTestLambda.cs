﻿namespace Sem.Sync.Test.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.Test.Contracts;
    using Sem.Sync.Test.Contracts.Entities;
    using Sem.Sync.Test.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTestLambda
    {
        [TestMethod]
        public void CheckRuleSet1()
        {
            var messageOne = new MessageOne("sometext");
            Bouncer.ForCheckData(() => messageOne).Assert(new SampleRuleSet<MessageOne>());
        }
    }
}
