namespace Sem.Sync.Test.Contracts
{
    using System;
    using System.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.Contracts.Rules;
    using Sem.Sync.Test.Contracts.Entities;
    using Sem.Sync.Test.Contracts.Rules;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerAttributedRuleTest
    {
        #region entities
        public static readonly AttributedSampleClass MessageOneOk = new AttributedSampleClass
            {
                MustBeLengthMin = "6chars",
            };

        private static readonly AttributedSampleClass _MessageOneFailRegEx = new AttributedSampleClass
            {
                MustBeOfRegExPatter = "hello",
            };

        private static readonly AttributedSampleClass _MessageOneFailMin = new AttributedSampleClass
            {
                MustBeLengthMin = "6",
            };

        private static readonly AttributedSampleClass _MessageOneFailMax = new AttributedSampleClass
            {
                MustBeLengthMax = "6charactersAndMore",
            };

        private static readonly AttributedSampleClass _MessageOneFailRegExNull = new AttributedSampleClass
            {
                MustBeOfRegExPatter = null,
            };

        private static readonly AttributedSampleClass _MessageOneFailMinNull = new AttributedSampleClass
            {
                MustBeLengthMin = null,
            };

        private static readonly AttributedSampleClass _MessageOneFailMaxNull = new AttributedSampleClass
            {
                MustBeLengthMax = null,
            };

        private static readonly AttributedSampleClass _MessageOneFailMinMax = new AttributedSampleClass
            {
                MustBeLengthMinMax = "manycharactersarehere",
            };

        public static readonly AttributedSampleClass MessageFailNamespace = new AttributedSampleClass
            {
                MustBeLengthAndNamespace = "m",
            };
        
        #endregion 

        [TestMethod]
        public void AddRuleForTypeOk()
        {
            var x = Bouncer.ForCheckData(() => MessageOneOk).Assert();
            Assert.IsNotNull(x);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect()
        {
            var message = Bouncer.ForMessages(() => _MessageOneFailRegEx).Assert().Results;
            Assert.IsTrue(message[0].Message.Contains("_MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect2()
        {
            var message = Bouncer.ForMessages(_MessageOneFailRegEx, "_MessageOneFailRegEx").Assert().Results;
            Assert.IsTrue(message[0].Message.Contains("_MessageOneFailRegEx.MustBeOfRegExPatter must be  of reg ex '.ell.!'"));
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect3()
        {
            var messages = Bouncer.ForMessages("hello", "theValue").Assert(new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8))).Results;
            var actual = messages[0].Message;
            Assert.AreEqual("The rule Sem.GenericHelpers.Contracts.Rules.ConfigurationValidatorBaseRule`1 did fail for value name >>theValue<<: The validator System.Configuration.StringValidator did throw an exception.", actual);
        }

        [TestMethod]
        public void AddRuleForTypeMustFailRegExCollect4()
        {
            var messages = Bouncer.ForMessages("hello", "theValue").Assert(new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8))).Results;
            var actual = messages[0].Message;
            Assert.AreEqual(messages[0].ToString(), actual);
        }

        [TestMethod]
        public void AddRuleForTypeMustSucceedRegExCollect4()
        {
            var configurationValidatorBaseRule = new ConfigurationValidatorBaseRule<string>(new System.Configuration.StringValidator(8));
            var messages = Bouncer.ForMessages("hello I have more than 8 chars", "theValue").Assert(configurationValidatorBaseRule).Results;
            var actual = messages.Count;
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailRegEx()
        {
            Bouncer.ForCheckData(() => _MessageOneFailRegEx).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMin()
        {
            Bouncer.ForCheckData(() => _MessageOneFailMin).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMax()
        {
            Bouncer.ForCheckData(() => _MessageOneFailMax).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailRegExNull()
        {
            Bouncer.ForCheckData(() => _MessageOneFailRegExNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMinNull()
        {
            Bouncer.ForCheckData(() => _MessageOneFailMinNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMaxNull()
        {
            Bouncer.ForCheckData(() => _MessageOneFailMaxNull).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void AddRuleForTypeMustFailMinMaxNull()
        {
            Bouncer.ForCheckData(() => _MessageOneFailMinMax).Assert();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MethodAttributeInValid()
        {
            var test = new SubscriberOne();
            test.Handle(new MessageOne("hello"));
            Assert.AreEqual("hello0", test.Content);
        }

        //[TestMethod]
        //public void MethodAttributeWithSuccess()
        //{
        //    var test = new SubscriberOne();
        //    test.Handle2(null);
        //    Assert.AreEqual("1", test.Content);
        //}

        [TestMethod]
        public void MethodAttributeWithSuccess2()
        {
            var test = new SubscriberOne();
            test.Handle3(null);
            Assert.AreEqual("1", test.Content);
        }

        [TestMethod]
        public void ClassLevelRuleSetIEnumerable()
        {
            var x = new CustomRuleSet();
            Assert.IsNotNull(((IEnumerable)x).GetEnumerator());
        }
    }
}