using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sem.Sync.Test.Contracts.Tests
{
    using Sem.GenericHelpers.Contracts;
    using Sem.GenericHelpers.Contracts.RuleExecuters;

    /// <summary>
    /// Summary description for BouncerAfterInvokeActionTest
    /// </summary>
    [TestClass]
    public class BouncerAfterInvokeActionTest
    {
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AfterInvokeTest01()
        {
            var ok = true;
            var isNotNull = Rules.IsNotNull<object>();
            
            // we will have one failing test, so "&= false" should set this variable to "false"
            CheckData<object>.AfterInvokeAction.Add(x => { ok &= x.Result; });
            try
            {
                new CheckData<object>(() => null).Assert(isNotNull);
            }
            catch (Exception)
            {
            }
            
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public void AfterInvokeTest02()
        {
            var ok = false;

            // we should have one successfull test, so "|= x.Result" should set the variable to true
            CheckData<object>.AfterInvokeAction.Add(x => { ok |= x.Result; });
            var isNotNull = Rules.IsNotNull<object>();
            try
            {
                new CheckData<object>(() => this).Assert(isNotNull);
            }
            catch (Exception)
            {
            }
            
            Assert.IsTrue(ok);
        }
    }
}
