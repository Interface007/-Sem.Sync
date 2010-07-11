// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseGeneratorTest.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   This is a test class for DatabaseGeneratorTest and is intended
//   to contain all DatabaseGeneratorTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sync.Test.MsSqlDatabase
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.Sync.Connector.MsSqlDatabase;
    using Sem.Sync.SyncBase.DetailData;

    /// <summary>
    /// This is a test class for DatabaseGeneratorTest and is intended
    /// to contain all DatabaseGeneratorTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DatabaseGeneratorTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// A test for CreateFromEntity
        /// </summary>
        [TestMethod()]
        public void CreateFromEntityTest()
        {
            var target = new DatabaseGenerator{ConnectionString = ""};
            var tables = new List<Table>();
            target.CreateFromEntityType(typeof(StdContact), "Contacts", tables);
            string script = "-- Table generation script\n\r";
            script += string.Join("\n\r", from x in tables select x.ToScript());
            script += string.Join("\n\r", from x in tables select x.ToScriptReferences());
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
