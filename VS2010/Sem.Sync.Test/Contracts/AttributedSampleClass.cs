namespace Sem.Sync.Test.Contracts
{
    using Sem.GenericHelpers.Contracts;

    [ContractRule(typeof(Rules), "IsNotNullObject")]
    public class AttributedSampleClass
    {
        public AttributedSampleClass(string content)
        {
            this.Content = content;
        }

        public string Content { get; set; }

        public override string ToString()
        {
            return this.Content;
        }
    }
}
