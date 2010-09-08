namespace Sem.Sync.Test.Contracts
{
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.Test.MessageAggregation;

    /// <summary>
    ///This is a test class for GuardTest and is intended
    ///to contain all GuardTest Unit Tests
    ///</summary>
    [TestClass]
    public class GuardRegisterRuleTest
    {
        #region preparation
        private readonly MessageOne _MessageOneOk = new MessageOne("Hello");
        
        private readonly MessageOne _MessageOneFail = new MessageOne("hello");

        private readonly MessageTwo _MessageTwoOk = new MessageTwo(7) { SubMessage = new MessageOne("hello") };
        
        private readonly MessageTwo _MessageTwoFail = new MessageTwo(7) { SubMessage = new MessageOne("hell") };

        public static Rule<MessageTwo> TestRule1()
        {
            return new Rule<MessageTwo>
            {
                CheckExpression = parameterValue => parameterValue != null,
            };
        }

        public static Rule<MessageTwo> TestRule2()
        {
            return new Rule<MessageTwo>
            {
                CheckExpression = parameterValue => parameterValue.SubMessage.Content.Length > 4,
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            ((List<TypeRule>)RuleSets.TypeRegisteredRules).Clear();
        }

        [TestInitialize]
        public void InitTest()
        {
            RuleSets.RegisterRule(Rules.IsNotNull<string>());
            RuleSets.RegisterRule(TestRule2());
            RuleSets.RegisterRuleSet(RuleSets.SampleRuleSet<MessageOne>());
        }
        #endregion preparation

        [TestMethod]
        public void AddRuleForType1()
        {
            Guard.For(() => "2").Assert();
        }

        [TestMethod]
        public void AddRuleForType2()
        {
            Guard.For(() => this._MessageTwoOk).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AddRuleForType2MustFail()
        {
            Guard.For(() => this._MessageTwoFail).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void AddRuleForTypeMustFail()
        {
            Guard.For(() => (string)null).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void AddRuleForTypeMustFail2()
        {
            Guard.For(() => this._MessageOneFail).Assert();
        }

        [TestMethod]
        public void AddRuleForType3()
        {
            Guard.For(() => this._MessageOneOk).Assert();
        }
    }
}