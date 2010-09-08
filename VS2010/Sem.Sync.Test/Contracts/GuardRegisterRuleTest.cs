namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts;

    /// <summary>
    ///This is a test class for GuardTest and is intended
    ///to contain all GuardTest Unit Tests
    ///</summary>
    [TestClass]
    public class GuardAttributedRuleTest
    {
        private readonly AttributedSampleClass _MessageOneOk = new AttributedSampleClass("Hello");

        private readonly AttributedSampleClass _MessageOneFail = new AttributedSampleClass("hello");

        [TestMethod]
        public void AddRuleForType1()
        {
            Guard.For(() =>_MessageOneOk).Assert();
        }
    }
}