namespace Sem.Sync.Test.Contracts
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sem.GenericHelpers.Contracts.Exceptions;
    using Sem.GenericHelpers.MessageAggregation;

    /// <summary>
    /// This is a test class for AggregatorTest and is intended
    /// to contain all AggregatorTest Unit Tests
    /// </summary>
    [TestClass]
    public class AggregatorTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            MessageAggregator.Instance.Reset();
        }

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SubscribeToMessage()
        {
            MessageAggregator.Instance.Subscribe(new SubscriberOne());
            Assert.AreEqual(1, MessageAggregator.Instance.Subscriptions.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(RuleValidationException))]
        public void SubscribeIllegalToMessage()
        {
            MessageAggregator.Instance.Subscribe(new AggregatorTest());
        }

        [TestMethod]
        public void UnSubscribeToMessage()
        {
            var subscriber = new SubscriberOne();

            MessageAggregator.Instance.Subscribe(subscriber);
            MessageAggregator.Instance.UnSubscribe(subscriber);

            Assert.AreEqual(0, MessageAggregator.Instance.Subscriptions.Count);
        }

        [TestMethod]
        public void DoubleSubscribeToMessage()
        {
            var subscriberOne = new SubscriberOne();
            MessageAggregator.Instance.Subscribe(subscriberOne);
            MessageAggregator.Instance.Subscribe(subscriberOne);
            Assert.AreEqual(1, MessageAggregator.Instance.Subscriptions.Count);
        }

        [TestMethod]
        public void DoubleUnSubscribeToMessage()
        {
            var subscriber = new SubscriberOne();

            MessageAggregator.Instance.Subscribe(subscriber);
            MessageAggregator.Instance.UnSubscribe(subscriber);
            MessageAggregator.Instance.UnSubscribe(subscriber);

            Assert.AreEqual(0, MessageAggregator.Instance.Subscriptions.Count);
        }

        [TestMethod]
        public void UnSubscribeNotSubscribed()
        {
            var subscriber = new SubscriberOne();

            MessageAggregator.Instance.UnSubscribe(subscriber);

            Assert.AreEqual(0, MessageAggregator.Instance.Subscriptions.Count);
        }

        [TestMethod]
        public void PublishMessage()
        {
            var subscriber = new SubscriberOne();

            MessageAggregator.Instance.Subscribe(subscriber);
            MessageAggregator.Instance.Publish(new MessageOne("hello"));

            Assert.AreEqual("hello", subscriber.Content);
            Assert.AreEqual(1, subscriber.CountOfEvents);
        }

        [TestMethod]
        public void PublishTwoMessages()
        {
            var subscriberOne1 = new SubscriberOne();
            var subscriberOne2 = new SubscriberOne();

            var subscriberTwo1 = new SubscriberTwo();
            var subscriberTwo2 = new SubscriberTwo();

            MessageAggregator.Instance.Subscribe(subscriberOne1);
            MessageAggregator.Instance.Subscribe(subscriberOne2);

            MessageAggregator.Instance.Subscribe(subscriberTwo1);
            MessageAggregator.Instance.Subscribe(subscriberTwo2);

            Assert.AreEqual(0, subscriberOne1.CountOfEvents);
            Assert.AreEqual(0, subscriberOne2.CountOfEvents);
            Assert.AreEqual(0, subscriberTwo1.CountOfEvents);
            Assert.AreEqual(0, subscriberTwo2.CountOfEvents);

            MessageAggregator.Instance.Publish(new MessageOne("hello"));
            
            Assert.AreEqual(1, subscriberOne1.CountOfEvents);
            Assert.AreEqual(1, subscriberOne2.CountOfEvents);
            Assert.AreEqual(0, subscriberTwo1.CountOfEvents);
            Assert.AreEqual(0, subscriberTwo2.CountOfEvents);

            MessageAggregator.Instance.Publish(new MessageTwo(3));

            Assert.AreEqual("hello" ,subscriberOne1.Content);
            Assert.AreEqual("hello", subscriberOne2.Content);
            Assert.AreEqual(3, subscriberTwo1.Content);
            Assert.AreEqual(3, subscriberTwo2.Content);

            Assert.AreEqual(1, subscriberOne1.CountOfEvents);
            Assert.AreEqual(1, subscriberOne2.CountOfEvents);
            Assert.AreEqual(1, subscriberTwo1.CountOfEvents);
            Assert.AreEqual(1, subscriberTwo2.CountOfEvents);
        }
        
        [TestMethod]
        public void SubscribeToBaseMessageAndReceiveInheritingMessage()
        {
            var subscriberTwo = new SubscriberTwo();
            MessageAggregator.Instance.Subscribe(subscriberTwo);
            MessageAggregator.Instance.Publish(new MessageTwoDotOne(3));
            Assert.AreEqual(3, subscriberTwo.Content);
        }
        
        [TestMethod]
        public void SubscribeToInheritingMessageAndNotReceiveBaseMessage()
        {
            var subscriberTwoDotOne = new SubscriberTwoDotOne();
            MessageAggregator.Instance.Subscribe(subscriberTwoDotOne);
            MessageAggregator.Instance.Publish(new MessageTwo(3));
            Assert.AreEqual(0, subscriberTwoDotOne.Content);
        }
    }
}
