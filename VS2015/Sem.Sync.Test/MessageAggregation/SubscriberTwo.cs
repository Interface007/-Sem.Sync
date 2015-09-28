namespace Sem.Sync.Test.Contracts
{
    using Sem.GenericHelpers.MessageAggregation;

    public class SubscriberTwo : IHandleThis<MessageTwo>
    {
        public int Content { get; set; }
        public int CountOfEvents { get; set; }
        
        public void Handle(MessageTwo message)
        {
            this.Content = message.Content;
            this.CountOfEvents++;
        }
    }
}