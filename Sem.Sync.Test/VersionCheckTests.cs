// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersionCheckTests.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for VersionCheckTests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;

    /// <summary>
    /// Summary description for VersionCheckTests
    /// </summary>
    [TestClass]
    public class VersionCheckTests
    {
        #region Properties

        ///<summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The check current version.
        /// </summary>
        [TestMethod]
        public void CheckCurrentVersion()
        {
            var version1 = new VersionCheck();
            Assert.IsTrue(version1.Check());
        }

        /// <summary>
        /// The check version comparison.
        /// </summary>
        [TestMethod]
        public void CheckVersionComparison()
        {
            var version1 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };

            var version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };

            Assert.IsTrue(version1.IsLessOrEqual(version2));

            version2 = new VersionCheck
                {
                   Major = 2, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version2 = new VersionCheck
                {
                   Major = 1, Minor = 2, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 2, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 2, Revision = 1, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 2, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 2 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));

            version2 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };
            version1 = new VersionCheck
                {
                   Major = 2, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsFalse(version1.IsLessOrEqual(version2));
            version1 = new VersionCheck
                {
                   Major = 1, Minor = 2, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsFalse(version1.IsLessOrEqual(version2));
            version1 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 2, MinorRevision = 1, Revision = 1, Build = 1 
                };
            Assert.IsFalse(version1.IsLessOrEqual(version2));
            version1 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 2, Revision = 1, Build = 1 
                };
            Assert.IsFalse(version1.IsLessOrEqual(version2));

            // the following two differences will not be recognized, so
            // the check should return TRUE
            version1 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 2, Build = 1 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));
            version1 = new VersionCheck
                {
                   Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 2 
                };
            Assert.IsTrue(version1.IsLessOrEqual(version2));

            this.CheckConstructorVersion();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The check constructor version.
        /// </summary>
        private void CheckConstructorVersion()
        {
            // comparison is server.CompareToVersion(mine) should be true in case if server <= mine
            Assert.IsTrue(new VersionCheck("1.1.1.1.1").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));
            Assert.IsTrue(new VersionCheck("1.1.1.1.1").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));

            Assert.IsFalse(new VersionCheck("3.1.1.1.1").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));
            Assert.IsTrue(new VersionCheck("1.3.1.1.1").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));
            Assert.IsTrue(new VersionCheck("1.1.3.1.1").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));
            Assert.IsTrue(new VersionCheck("1.1.1.3.1").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));
            Assert.IsTrue(new VersionCheck("1.1.1.1.3").IsLessOrEqual(new VersionCheck("2.2.2.2.2")));

            Assert.IsFalse(new VersionCheck("2.1.1.1.1").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));
            Assert.IsFalse(new VersionCheck("1.2.1.1.1").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));
            Assert.IsFalse(new VersionCheck("1.1.2.1.1").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));
            Assert.IsFalse(new VersionCheck("1.1.1.2.1").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));

            // the comparison does not respect the build
            Assert.IsTrue(new VersionCheck("1.1.1.1.2").IsLessOrEqual(new VersionCheck("1.1.1.1.1")));

            Assert.IsTrue(new VersionCheck("2.1.1.1.1").IsLessOrEqual(new VersionCheck("3.1.1.1.1")));
            Assert.IsTrue(new VersionCheck("1.2.1.1.1").IsLessOrEqual(new VersionCheck("3.1.1.1.1")));
            Assert.IsTrue(new VersionCheck("1.1.2.1.1").IsLessOrEqual(new VersionCheck("3.1.1.1.1")));
            Assert.IsTrue(new VersionCheck("1.1.1.2.1").IsLessOrEqual(new VersionCheck("3.1.1.1.1")));
            Assert.IsTrue(new VersionCheck("1.1.1.1.2").IsLessOrEqual(new VersionCheck("3.1.1.1.1")));
        }

        #endregion
    }
}