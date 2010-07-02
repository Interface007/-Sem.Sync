namespace Sem.Sync.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        public void FunctionCallToPropertyWithTransformation()
        {
            // register the property mapping of a source and a destination 
            // type with trasformations (in this case we do add a string to
            // the name to generate a calendar title of "Birthday: Riddle, Tom")
            var mapper = new Mapper();
            mapper.Register<StdContact, StdCalendarItem>(contact => contact.Id, (calendar, entity) => calendar.Id = (Guid)entity);
            mapper.Register<StdContact, StdCalendarItem>(contact => contact.GetFullName(), (calendar, entity) => calendar.Title = "Birthday: " + entity);

            // setup a source and a destination object
            var source = new StdContact { Name = new PersonName("Tom Riddle") };
            var target = new StdCalendarItem();

            // perform the mapping
            mapper.Map(source, target);

            // test it
            Assert.AreEqual(source.Id, target.Id);
            Assert.AreEqual("Birthday: " + source.GetFullName(), target.Title);
        }

        [TestMethod]
        public void PropertyPathWithNullToPropertyWithTransformation()
        {
            // register the property mapping of a source and a destination 
            // type with trasformations (in this case we do add a string to
            // the name to generate a calendar title of "Birthday: Riddle, Tom")
            var mapper = new Mapper();
            mapper.Register<StdContact, StdCalendarItem>(
                contact => contact.PersonalAddressPrimary.Phone,
                (calendar, entity) => calendar.Title = entity == null ? "NULL detected" : "Call to " + entity);

            // setup a source and a destination object
            var source = new StdContact { Name = new PersonName("Tom Riddle") };
            var target = new StdCalendarItem();

            // perform the mapping
            mapper.Map(source, target);

            // test it
            Assert.AreEqual("NULL detected", target.Title);
        }

        [TestMethod]
        public void PropertyPathWithNoNullToPropertyWithTransformation()
        {
            // register the property mapping of a source and a destination 
            // type with trasformations (in this case we do add a string to
            // the name to generate a calendar title of "Birthday: Riddle, Tom")
            var mapper = new Mapper();
            mapper.Register<StdContact, StdCalendarItem>(
                contact => contact.PersonalAddressPrimary.Phone,
                (calendar, entity) => calendar.Title = entity == null ? "NULL detected" : "Call to " + entity);

            // setup a source and a destination object
            var source = new StdContact { PersonalAddressPrimary = new AddressDetail{Phone = "0123-456789"} };
            var target = new StdCalendarItem();

            // perform the mapping
            mapper.Map(source, target);

            // test it
            Assert.AreEqual("Call to (123) 456789", target.Title);
        }
        
        [TestMethod]
        public void TestMapping()
        {
            // register the property mapping of a source and a destination 
            // type with trasformations (in this case we do add a string to
            // the name to generate a calendar title of "Birthday: Riddle, Tom")
            var mapper = new Mapper();
            mapper.Register<Source, Target>(x => x.Name, (y, x) => y.FullName = (string)x);
            mapper.Register<Source, Target>(x => x.Phone, (y, x) => y.PhoneNumber = (string)x);
            
            // perform the mapping

            mapper.Register<Source, Target>(
                x => x.Contacts, 
                (y, x) => y.Contacts = 
                    new List<Target>(
                        from c in (List<Source>)x 
                        select new Target
                            {
                            
                            }));

            // setup a source and a destination object
            var source = new StdContact { PersonalAddressPrimary = new AddressDetail{Phone = "0123-456789"} };
            var target = new StdCalendarItem();

            mapper.Map(source, target);

            // test it
            Assert.AreEqual("Call to (123) 456789", target.Title);
        }
    }

    public class Source
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Source> Contacts { get; set; }
    }

    public class Target
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<Target> Contacts { get; set; }
    }
}
