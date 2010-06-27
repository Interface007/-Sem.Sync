namespace Sem.Sync.Test
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// Summary description for MapperTests
    /// </summary>
    [TestClass]
    public class MapperTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var mapper = new Mapper();
            mapper.Register<StdContact, StdCalendarItem>(contact => contact.Id, (calendar, entity) => calendar.Id = (Guid)entity);
            mapper.Register<StdContact, StdCalendarItem>(contact => contact.GetFullName(), (calendar, entity) => calendar.Title = "Birthday: " + entity);

            var source = new StdContact { Name = new PersonName("Tom Riddle") };
            var target = new StdCalendarItem();

            mapper.Map(source, target);

            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual("Birthday: " + source.GetFullName(), target.Title);
        }
    }
}
