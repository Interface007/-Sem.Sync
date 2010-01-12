namespace Sem.Sync.Test
{
    using GenericHelpers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for VersionCheckTests
    /// </summary>
    [TestClass]
    public class VersionCheckTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CheckVersionComparison()
        {
            var version1 = new VersionCheck
                {
                    Major = 1,
                    Minor = 1,
                    MajorRevision = 1,
                    MinorRevision = 1,
                    Revision = 1,
                    Build = 1
                };

            var version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };

            Assert.IsTrue(version1.CompareToVersion( version2));

            version2 = new VersionCheck { Major = 2, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version2 = new VersionCheck { Major = 1, Minor = 2, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 2, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 2, Revision = 1, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 2, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 2 };
            Assert.IsTrue(version1.CompareToVersion( version2));

            version2 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };
            version1 = new VersionCheck { Major = 2, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsFalse(version1.CompareToVersion( version2));
            version1 = new VersionCheck { Major = 1, Minor = 2, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsFalse(version1.CompareToVersion( version2));
            version1 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 2, MinorRevision = 1, Revision = 1, Build = 1 };
            Assert.IsFalse(version1.CompareToVersion( version2));
            version1 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 2, Revision = 1, Build = 1 };
            Assert.IsFalse(version1.CompareToVersion( version2));
            
            // the following two differences will not be recognized, so
            // the check should return TRUE
            version1 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 2, Build = 1 };
            Assert.IsTrue(version1.CompareToVersion( version2));
            version1 = new VersionCheck { Major = 1, Minor = 1, MajorRevision = 1, MinorRevision = 1, Revision = 1, Build = 2 };
            Assert.IsTrue(version1.CompareToVersion( version2));
        }
    }
}
