namespace Sem.Sync.Test.Contracts
{
    using Sem.GenericHelpers.MessageAggregation;

    public class SubscriberOne : IHandleThis<MessageOne>
    {
        public string Content { get; set; }
        public int CountOfEvents { get; set; }

        public void Handle(MessageOne message)
        {
            this.Content = message.Content;
            this.CountOfEvents++;
        }
    }
}