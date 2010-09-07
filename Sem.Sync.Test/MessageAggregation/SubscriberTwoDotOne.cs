namespace Sem.Sync.Test.MessageAggregation
{
    using Sem.GenericHelpers.MessageAggregation;

    public class SubscriberTwoDotOne : IHandleThis<MessageTwoDotOne>
    {
        public int Content { get; set; }
        public int CountOfEvents { get; set; }
        
        public void Handle(MessageTwoDotOne message)
        {
            this.Content = message.Content;
            this.CountOfEvents++;
        }
    }
}