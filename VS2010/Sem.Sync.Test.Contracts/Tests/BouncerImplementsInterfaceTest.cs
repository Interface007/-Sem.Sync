namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.Sync.Test.Contracts.Tests;

    /// <summary>
    ///This is a test class for BouncerTest and is intended
    ///to contain all BouncerTest Unit Tests
    ///</summary>
    [TestClass]
    public class BouncerImplementsInterfaceTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail12()
        {
            Assert.IsFalse(Rules.ImplementsInterface<object>().CheckExpression(this, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail1()
        {
            Assert.IsFalse(Rules.ImplementsInterface<object>().CheckExpression(this, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail2()
        {
            Assert.IsFalse(Rules.ImplementsInterface<object>().CheckExpression(this, typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail3()
        {
            Assert.IsFalse(Rules.ImplementsInterface<object>().CheckExpression(null, typeof(IHandleThis<BouncerTest>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustFail4()
        {
            Assert.IsFalse(Rules.ImplementsInterface<object>().CheckExpression(null, typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass1()
        {
            Assert.IsTrue(Rules.ImplementsInterface<object>().CheckExpression(new SubscriberOne(), typeof(IHandleThis<>)));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass2()
        {
            Assert.IsTrue(Rules.ImplementsInterface<object>().CheckExpression(new SubscriberOne(), typeof(IHandleThis<MessageOne>)));
        }
    }

    public interface IHandleThis<T>
    {

    }
}