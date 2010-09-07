namespace Sem.Sync.Test.Contracts
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.MessageAggregation;
    using Sem.Sync.Test.MessageAggregation;

    /// <summary>
    ///This is a test class for GuardTest and is intended
    ///to contain all GuardTest Unit Tests
    ///</summary>
    [TestClass]
    public class GuardImplementsInterfaceTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckParameterImplementsInterfaceMustFail12()
        {
            Guard.ImplementsInterface.AssertFor("name", this, typeof(IHandleThis<GuardTest>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckParameterImplementsInterfaceMustFail1()
        {
            Guard.ImplementsInterface.AssertFor("name", this, typeof(IHandleThis<GuardTest>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckParameterImplementsInterfaceMustFail2()
        {
            Guard.ImplementsInterface.AssertFor("name", this, typeof(IHandleThis<>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckParameterImplementsInterfaceMustFail3()
        {
            Guard.ImplementsInterface.AssertFor("name", null, typeof(IHandleThis<GuardTest>));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckParameterImplementsInterfaceMustFail4()
        {
            Guard.ImplementsInterface.AssertFor("name", null, typeof(IHandleThis<>));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass1()
        {
            Guard.ImplementsInterface.AssertFor("name", new SubscriberOne(), typeof(IHandleThis<>));
        }

        [TestMethod]
        public void CheckParameterImplementsInterfaceMustPass2()
        {
            Guard.ImplementsInterface.AssertFor("name", new SubscriberOne(), typeof(IHandleThis<MessageOne>));
        }
    }
}