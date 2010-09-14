namespace Sem.Sync.Test.Contracts.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.Test.Contracts;
    using Sem.Sync.Test.Contracts.Entities;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerTestExecution
    {
        [TestMethod]
        public void CheckRuleExecution1()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .Execute(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution1A()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(messageOne, "messageOne")
                .Assert(x => x.Content == "sometext")
                .Execute(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution2()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .Execute(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution3()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "othertext")
                .Execute(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution4()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "sometext")
                .Execute(() => { result = true; });

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckRuleExecution5()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "sometext")
                .Assert(x => x.Content == "sometext")
                .Execute(() => { result = true; });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckRuleExecution6()
        {
            var messageOne = new MessageOne("sometext");
            bool result = false;
            Bouncer
                .ForExecution(() => messageOne)
                .Assert(x => x.Content == "othertext")
                .Assert(x => x.Content == "mytext")
                .Assert(x => x.Content == "yourtext")
                .Execute(() => { result = true; });

            Assert.IsFalse(result);
        }
    }
}
