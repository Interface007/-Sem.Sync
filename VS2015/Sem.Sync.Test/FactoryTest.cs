namespace Sem.Sync.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers;

    /// <summary>
    /// Summary description for FactoryTest
    /// </summary>
    [TestClass]
    public class FactoryTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void FactoryTestMapCode()
        {
            // setup a mock object to be injected
            var mock = new Moq.Mock<NetworkCredentials>();
            
            // first see if the new operator works as expected with the type
            var x = new NetworkCredentials();
            Assert.IsTrue(x.GetType() == typeof(NetworkCredentials));

            // now use an expression to execute the constructor (should be the same as using the new operator directly)
            x = Factory.Invoke(() => new NetworkCredentials());
            Assert.IsTrue(x.GetType() == typeof(NetworkCredentials));

            // now register a replacement for the constructor call via new by returning the mock object instead
            Factory.Register(() => new NetworkCredentials(), 
                             () => mock.Object);

            // invoke the same code again, now the returned instance should be the mock object as defined for replacement
            x = Factory.Invoke(() => new NetworkCredentials());
            Assert.IsTrue(x.GetType() != typeof(NetworkCredentials));
            Assert.IsTrue(x.GetType().FullName == "Castle.Proxies.NetworkCredentialsProxy");

            // try with two more complex code fragments that only differ in a string constant
            Factory.Register(() => new NetworkCredentials { Passwort = "mock me 01" }, () => new NetworkCredentials { Passwort = "ok 01" });
            Factory.Register(() => new NetworkCredentials { Passwort = "mock me 02" }, () => new NetworkCredentials { Passwort = "ok 02" });
            
            // replacement with one string
            x = Factory.Invoke(() => new NetworkCredentials { Passwort = "mock me 01" });
            Assert.IsTrue(x.Passwort == "ok 01");

            // replacement with the other string
            x = Factory.Invoke(() => new NetworkCredentials { Passwort = "mock me 02" });
            Assert.IsTrue(x.Passwort == "ok 02");
            
            // no replacement defined, so the original string must be returned
            x = Factory.Invoke(() => new NetworkCredentials { Passwort = "mock me 03" });
            Assert.IsTrue(x.Passwort == "mock me 03");

            // replacement with one string
            x = TestSubMethod();
            Assert.IsTrue(x.Passwort == "ok 01");

            // code to be replaced needs to match the destination code exactly - anonymous classes for enclosures will
            // be renamed, so the variable name does matter as a key, not the anonymous class instance. In this example
            // the variable name needs to be "mockMe", because the variable in the expression to be replaced is "mockMe".
            // keep in mind that this will replace all code that looks like this, regardless of the variable content.
            var mockMe = "mock me 01";
            Factory.Register(
                () => new NetworkCredentials { Passwort = mockMe }, 
                () => new NetworkCredentials { Passwort = "ok 01" });
            x = TestSubMethodWithVariables();
            Assert.IsTrue(x.Passwort == "ok 01");
        }

        private static NetworkCredentials TestSubMethod()
        {
            var x = Factory.Invoke(() => new NetworkCredentials { Passwort = "mock me 01" });
            return x;
        }

        private static NetworkCredentials TestSubMethodWithVariables()
        {
            var mockMe = "mock me 01";
            var x = Factory.Invoke(() => new NetworkCredentials { Passwort = mockMe });
            return x;
        }
    }
}
