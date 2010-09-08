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
    public class GuardIsOneOfTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckParameterIsOneOfMustFail1()
        {
            Rules.IsOneOf<string>().AssertFor(new CheckData<string>("name", "1"), new[] { "2", "3" });
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckParameterIsOneOfMustFail2()
        {
            Rules.IsOneOf<string>().AssertFor(new CheckData<string>("name", null), new[] { "2", "3" });
        }

        [TestMethod]
        public void CheckParameterIsOneOfMustPass1()
        {
            Rules.IsOneOf<string>().AssertFor(new CheckData<string>("name", "1"), new[] { "2", "1" });
        }

        [TestMethod]
        public void CheckParameterIsOneOfMustPass2()
        {
            Rules.IsOneOf<string>().AssertFor(new CheckData<string>("name", "1"), new[] { "1", "2" });
        }
    }
}