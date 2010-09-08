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
    public class GuardIsNotOneOfTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckParameterIsNotOneOfMustFail1()
        {
            Rules.IsNotOneOf<string>().AssertFor(new CheckData<string>("name", "2"), new[] { "2", "3" });
        }
        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass0()
        {
            Rules.IsNotOneOf<string>().AssertFor(new CheckData<string>("name", null), new[] { "2", "3" });
        }

        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass1()
        {
            Rules.IsNotOneOf<string>().AssertFor(new CheckData<string>("name", "3"), new[] { "2", "1" });
        }

        [TestMethod]
        public void CheckParameterIsNotOneOfMustPass2()
        {
            Rules.IsNotOneOf<string>().AssertFor(new CheckData<string>("name", "0"), new[] { "1", "2" });
        }
    }
}