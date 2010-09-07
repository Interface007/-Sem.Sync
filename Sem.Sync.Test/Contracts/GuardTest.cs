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
    public class GuardTest
    {
        #region asserts
        [TestMethod]
        public void CheckIntValid0A()
        {
            Guard.For(0).Assert(x => x == 0);
        }

        [TestMethod]
        public void CheckIntValid1A()
        {
            Guard.For(1).Assert(x => x == 1);
        }

        [TestMethod]
        public void CheckIntValid0B()
        {
            Guard.For(0).Assert(x => x == 0, "ok");
        }

        [TestMethod]
        public void CheckIntValid1B()
        {
            Guard.For(1).Assert(x => x == 1, "ok");
        }
        
        [TestMethod]
        public void CheckIntValid0()
        {
            Guard.For(0, "var0").Assert(x => x == 0, "ok");
        }

        [TestMethod]
        public void CheckIntValid1()
        {
            Guard.For(1, "var1").Assert(x => x == 1, "ok");
        }

        [TestMethod]
        public void CheckIntValidWithParameter1()
        {
            Guard.For(1, "var1").Assert((x, y) => x == 1 && y == 7, 7, "ok");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIntInvalid()
        {
            Guard.For(0, "var0").Assert(x => x == 1, "ok");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIntInvalidWithParameter()
        {
            Guard.For(0, "var0").Assert((x, y) => x == 1, 7, "ok");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIntInvalidWithParameter2()
        {
            Guard.For(0, "var0").Assert((x, y) => y == 8, 7, "ok");
        }
        #endregion

        #region assume
        [TestMethod]
        public void CheckIntValid0AAssume()
        {
            Guard.For(0).Assume(x => x == 0);
        }

        [TestMethod]
        public void CheckIntValid1AAssume()
        {
            Guard.For(1).Assume(x => x == 1);
        }

        [TestMethod]
        public void CheckIntValid0BAssume()
        {
            Guard.For(0).Assume(x => x == 0, "ok");
        }

        [TestMethod]
        public void CheckIntValid1BAssume()
        {
            Guard.For(1).Assume(x => x == 1, "ok");
        }

        public void CheckIntValid0Assume()
        {
            Guard.For(0, "var0").Assume(x => x == 0, "ok");
        }

        [TestMethod]
        public void CheckIntValid1Assume()
        {
            Guard.For(1, "var1").Assume(x => x == 1, "ok");
        }

        [TestMethod]
        public void CheckIntValid1AssumeWithParameter()
        {
            Guard.For(1, "var1").Assume((x, y) => x == 1 && y == 7, 7, "ok");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIntInvalidAssume()
        {
            Guard.For(0, "var0").Assume(x => x == 1, "ok");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckIntInvalidAssumeWithParameter()
        {
            Guard.For(0, "var0").Assume((x, y) => x == 1 && y == 1, 7, "ok");
        }
        #endregion
    }
}
