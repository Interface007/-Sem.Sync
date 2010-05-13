namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.SyncBase;
    using Sem.Sync.SyncBase.Attributes;

    /// <summary>
    ///This is a test class for ConnectorDescriptionAttributeTest and is intended
    ///to contain all ConnectorDescriptionAttributeTest Unit Tests
    ///</summary>
    [TestClass]
    public class ConnectorDescriptionAttributeTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ConnectorDescriptionAttribute Constructor
        ///</summary>
        [TestMethod]
        public void ConnectorDescriptionAttributeConstructorTest()
        {
            var target = new ConnectorDescriptionAttribute();

            Assert.IsTrue(target.CanReadContacts);
            Assert.IsTrue(target.CanWriteContacts);
            Assert.IsTrue(target.CanRead(typeof(StdContact)));
            Assert.IsTrue(target.CanWrite(typeof(StdContact)));

            Assert.IsFalse(target.CanReadCalendarEntries);
            Assert.IsFalse(target.CanWriteCalendarEntries);
            Assert.IsFalse(target.CanRead(typeof(StdCalendarItem)));
            Assert.IsFalse(target.CanWrite(typeof(StdCalendarItem)));
            Assert.IsFalse(target.CanRead(typeof(StdCalendarItem)));
            Assert.IsFalse(target.CanWrite(typeof(StdCalendarItem)));
        }

        /// <summary>
        ///A test for ConnectorDescriptionAttribute Constructor
        ///</summary>
        [TestMethod()]
        public void ConnectorDescriptionAttributePropertySetterTest()
        {
            var target = new ConnectorDescriptionAttribute();

            // READ CONTACTS
            target.CanReadContacts = false;
            Assert.IsFalse(target.CanReadContacts);
            Assert.IsFalse(target.CanRead(typeof(StdContact)));
            target.CanReadContacts = true;
            Assert.IsTrue(target.CanReadContacts);
            Assert.IsTrue(target.CanRead(typeof(StdContact)));
            target.CanReadContacts = false;
            Assert.IsFalse(target.CanReadContacts);
            Assert.IsFalse(target.CanRead(typeof(StdContact)));

            // WRITE CONTACTS
            target.CanWriteContacts = false;
            Assert.IsFalse(target.CanWriteContacts);
            Assert.IsFalse(target.CanWrite(typeof(StdContact)));
            target.CanWriteContacts = true;
            Assert.IsTrue(target.CanWriteContacts);
            Assert.IsTrue(target.CanWrite(typeof(StdContact)));
            target.CanWriteContacts = false;
            Assert.IsFalse(target.CanWriteContacts);
            Assert.IsFalse(target.CanWrite(typeof(StdContact)));

            // READ CALENDAR
            target.CanReadCalendarEntries = false;
            Assert.IsFalse(target.CanReadCalendarEntries);
            Assert.IsFalse(target.CanRead(typeof(StdCalendarItem)));
            target.CanReadCalendarEntries = true;
            Assert.IsTrue(target.CanReadCalendarEntries);
            Assert.IsTrue(target.CanRead(typeof(StdCalendarItem)));
            target.CanReadCalendarEntries = false;
            Assert.IsFalse(target.CanReadCalendarEntries);
            Assert.IsFalse(target.CanRead(typeof(StdCalendarItem)));

            // WRITE CALENDAR
            target.CanWriteCalendarEntries = false;
            Assert.IsFalse(target.CanWriteCalendarEntries);
            Assert.IsFalse(target.CanWrite(typeof(StdCalendarItem)));
            target.CanWriteCalendarEntries = true;
            Assert.IsTrue(target.CanWriteCalendarEntries);
            Assert.IsTrue(target.CanWrite(typeof(StdCalendarItem)));
            target.CanWriteCalendarEntries = false;
            Assert.IsFalse(target.CanWriteCalendarEntries);
            Assert.IsFalse(target.CanWrite(typeof(StdCalendarItem)));
        }
    }
}
