namespace Sem.Sync.Test.Contracts
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    ///This is a test class for GuardTest and is intended
    ///to contain all GuardTest Unit Tests
    ///</summary>
    [TestClass]
    public class GuardIsNotNullTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckParameterIsNotNullMustFail()
        {
            Rules.IsNotNull<object>().AssertFor(new CheckData<object>("name", null));
        }

        [TestMethod]
        public void CheckParameterIsNotNullMustPass1()
        {
            Rules.IsNotNull<object>().AssertFor(new CheckData<object>("name", this));
        }
    }
}