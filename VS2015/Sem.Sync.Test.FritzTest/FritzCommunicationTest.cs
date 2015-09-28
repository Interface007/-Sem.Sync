// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FritzCommunicationTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Summary description for UnitTest1
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#if INTEGRATIONTEST
namespace Sem.Sync.Test.FritzTest
{
    using System;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;
    using Sem.Sync.Connector.FritzBox;
    using Sem.Sync.Connector.FritzBox.Entities;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class FritzCommunicationTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ReadEntries()
        {
            var fritzApi = new FritzApi
                {
                    Host = new Uri("http://192.168.1.9/"),
                    UserPassword = string.Empty
                };

            var x = fritzApi.GetPhoneBook();
            var s = Tools.SaveToString(x);
            Assert.AreNotEqual(0, x.Count);
        }

        [TestMethod, Conditional("INTEGRATIONTEST")]
        public void WriteEntries()
        {
            var fritzApi = new FritzApi
                {
                    Host = new Uri("http://192.168.1.9/"),
                    UserPassword = string.Empty
                };

            var s = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<phonebook xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <contact>\r\n    <category>0</category>\r\n    <person>\r\n      <realName>Hanniball Lector</realName>\r\n    </person>\r\n    <telephony>\r\n      <number type=\"home\" quickdial=\"01\" prio=\"1\">6441390920</number>\r\n      <number type=\"work\" prio=\"0\">6441390922</number>\r\n      <number type=\"mobile\" prio=\"0\">6441390921</number>\r\n    </telephony>\r\n    <services>\r\n      <email />\r\n    </services>\r\n    <setup />\r\n  </contact>\r\n  <contact>\r\n    <category>1</category>\r\n    <person>\r\n      <realName>Tom Riddle</realName>\r\n    </person>\r\n    <telephony>\r\n      <number type=\"home\" quickdial=\"02\" prio=\"1\">0001666999</number>\r\n      <number type=\"work\" prio=\"0\" />\r\n      <number type=\"mobile\" prio=\"0\" />\r\n    </telephony>\r\n    <services>\r\n      <email />\r\n    </services>\r\n    <setup />\r\n  </contact>\r\n</phonebook>";
            var x = Tools.LoadFromString<PhoneBook>(s);
            fritzApi.SetPhoneBook(x);

            Assert.AreNotEqual(0, x.Count);
        }
    }
}
#endif