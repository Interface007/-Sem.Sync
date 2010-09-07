namespace Sem.Sync.Test.MessageAggregation
{
    public class MessageOne
    {
        public MessageOne(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }
    }
}