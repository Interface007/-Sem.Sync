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
    public class GuardBackEndNumberBoundariesTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CheckParameterBackEndNumberBoundariesMustFail()
        {
            Rules.BackEndNumberBoundaries().AssertFor(new CheckData<int>("name", 20000));
        }

        [TestMethod]
        public void CheckParameterBackEndNumberBoundariesMustPass1()
        {
            Rules.BackEndNumberBoundaries().AssertFor(new CheckData<int>("name", 10000));
        }
    }
}