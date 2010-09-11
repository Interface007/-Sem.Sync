namespace Sem.Sync.Test.Contracts
{
    using System.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.Exceptions;

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
            Bouncer.ForCheckData(() => MessageOneOk).Assert();
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
        public void ClassLevelRuleSetIEnumerable()
        {
            var x = new CustomRuleSet();
            Assert.IsNotNull(((IEnumerable)x).GetEnumerator());
        }
    }
}