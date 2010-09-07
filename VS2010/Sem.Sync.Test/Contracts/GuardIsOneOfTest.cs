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
        public void CheckParameterIsNotNullMustFail1()
        {
            Guard.IsOneOf.AssertFor("name", "1", new[] { "2", "3" });
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckParameterIsNotNullMustFail2()
        {
            Guard.IsOneOf.AssertFor("name", null, new[] { "2", "3" });
        }

        [TestMethod]
        public void CheckParameterIsNotNullMustPass1()
        {
            Guard.IsOneOf.AssertFor("name", "1", new[] { "2", "1" });
        }

        [TestMethod]
        public void CheckParameterIsNotNullMustPass2()
        {
            Guard.IsOneOf.AssertFor("name", "1", new[] { "1", "2" });
        }
    }
}